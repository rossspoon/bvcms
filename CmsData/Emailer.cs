/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Net.Mail;
using System.Threading;
using UtilityExtensions;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace CmsData
{
    public partial class CMSDataContext
    {
        public bool UseMassEmailer
        {
            get
            {
                return Setting("UseMassEmailer", "false").ToBool();
            }
        }
        public string CmsHost
        {
            get
            {
                var h = ConfigurationManager.AppSettings["cmshost"];
                return h.Replace("{church}", Host);
            }
        }
        public void Email(string from, Person p, string subject, string body)
        {
            Email(from, p, null, subject, body, false);
        }
        public void EmailRedacted(string from, Person p, string subject, string body)
        {
            Email(from, p, null, subject, body, true);
        }
        public void Email(string from, Person p, string addemail, string subject, string body, bool redacted)
        {
            var From = Util.FirstAddress(from);
            var emailqueue = new EmailQueue
            {
                Queued = DateTime.Now,
                FromAddr = From.Address,
                FromName = From.DisplayName,
                Subject = subject,
                Body = body,
                QueuedBy = Util.UserPeopleId,
                Redacted = redacted,
                Transactional = true
            };
            EmailQueues.InsertOnSubmit(emailqueue);
            emailqueue.EmailQueueTos.Add(new EmailQueueTo
            {
                PeopleId = p.PeopleId,
                OrgId = CurrentOrgId,
                AddEmail = addemail,
                Guid = Guid.NewGuid(),
            });
            SubmitChanges();
            if (UseMassEmailer)
                QueuePriorityEmail(emailqueue.Id, CmsHost, Host);
            else
                SendPersonEmail(CmsHost, emailqueue.Id, p.PeopleId);
        }
        public void Email(string from, IEnumerable<Person> list, string subject, string body)
        {
            foreach (var p in list)
                Email(from, p, subject, body);
        }
        public void EmailRedacted(string from, IEnumerable<Person> list, string subject, string body)
        {
            foreach (var p in list)
                EmailRedacted(from, p, subject, body);
        }
        public void Email(string from, IEnumerable<Person> list, string addemail, string subject, string body)
        {
            var a = list.ToArray();
            if (a.Length == 0)
                return;
            Email(from, a[0], addemail, subject, body, false);
            for (var n = 1; n < a.Length; n++)
                Email(from, a[n], subject, body);
        }
        public IEnumerable<Person> PeopleFromPidString(string pidstring)
        {
            var a = pidstring.SplitStr(",").Select(ss => ss.ToInt()).ToArray();
            var q = from p in People
                    where a.Contains(p.PeopleId)
                    orderby p.PeopleId == a[0] descending
                    select p;
            return q;
        }
        public IEnumerable<Person> StaffPeopleForDiv(int divid)
        {
            var q = from o in Organizations
                    where o.DivOrgs.Any(dd => dd.DivId == divid)
                    where o.NotifyIds != null && o.NotifyIds != ""
                    select o.NotifyIds;
            var pids = string.Join(",", q);
            var a = pids.SplitStr(",").Select(ss => ss.ToInt()).ToArray();
            var q2 = from p in People
                     where a.Contains(p.PeopleId)
                     orderby p.PeopleId == a[0] descending
                     select p;
            if (q2.Count() == 0)
                return from p in CMSRoleProvider.provider.GetAdmins()
                       orderby p.Users.Any(u => u.Roles.Contains("Developer")) descending
                       select p;
            return q2;
        }
        public string StaffEmailForOrg(int orgid)
        {
            var q = from o in Organizations
                    where o.OrganizationId == orgid
                    where o.NotifyIds != null && o.NotifyIds != ""
                    select o.NotifyIds;
            var pids = string.Join(",", q);
            var a = pids.SplitStr(",").Select(ss => ss.ToInt()).ToArray();
            var q2 = from p in People
                     where p.PeopleId == a[0]
                     select p.FromEmail;
            if (q2.Count() == 0)
                return (from p in CMSRoleProvider.provider.GetAdmins()
                        orderby p.Users.Any(u => u.Roles.Contains("Developer")) descending
                        select p.FromEmail).First();
            return q2.SingleOrDefault();
        }
        public IEnumerable<Person> StaffPeopleForOrg(int orgid)
        {
            var q = from o in Organizations
                    where o.OrganizationId == orgid
                    where o.NotifyIds != null && o.NotifyIds != ""
                    select o.NotifyIds;
            var pids = string.Join(",", q);
            var a = pids.SplitStr(",").Select(ss => ss.ToInt()).ToArray();
            var q2 = from p in People
                     where a.Contains(p.PeopleId)
                     orderby p.PeopleId == a[0] descending
                     select p;
            if (q2.Count() == 0)
                return from p in CMSRoleProvider.provider.GetAdmins()
                       orderby p.Users.Any(u => u.Roles.Contains("Developer")) descending
                       select p;
            return q2;
        }
        public Person UserPersonFromEmail(string email)
        {
            var q = from u in Users
                    where u.Person.EmailAddress == email || u.Person.EmailAddress2 == email
                    select u.Person;
            var p = q.SingleOrDefault();
            if (p == null)
                p = UserPersonFromEmail(DbUtil.SystemEmailAddress);
            return p;
        }
        public EmailQueue CreateQueue(MailAddress From, string subject, string body, DateTime? schedule, int QBId, bool wantParents, bool PublicViewable)
        {
            var emailqueue = new EmailQueue
            {
                Queued = DateTime.Now,
                FromAddr = From.Address,
                FromName = From.DisplayName,
                Subject = subject,
                Body = body,
                SendWhen = schedule,
                QueuedBy = Util.UserPeopleId,
                Transactional = false,
                PublicX = PublicViewable,
            };
            EmailQueues.InsertOnSubmit(emailqueue);

            var Qb = LoadQueryById(QBId);
            var q = People.Where(Qb.Predicate(this));

            if (wantParents || Qb.ParentsOf)
                q = from p in q
                    from fm in People.Where(ff => ff.FamilyId == p.FamilyId)
                    where (fm.PositionInFamilyId == 10 && p.PositionInFamilyId != 10)
                    || (fm.PeopleId == p.PeopleId && p.PositionInFamilyId == 10)
                    select fm;

            var q2 = from p in q.Distinct()
                     where p.EmailAddress != null
                     where p.EmailAddress != ""
                     where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                     where !p.EmailOptOuts.Any(oo => oo.FromEmail == emailqueue.FromAddr)
                     orderby p.PeopleId
                     select p.PeopleId;

            var i = 0;
            foreach (var pid in q2)
            {
                i++;
                emailqueue.EmailQueueTos.Add(new EmailQueueTo
                {
                    PeopleId = pid,
                    OrgId = CurrentOrgId,
                    Guid = Guid.NewGuid()
                });
            }
            SubmitChanges();
            return emailqueue;
        }
        public void SendPersonEmail(string CmsHost, int id, int pid)
        {
            var SysFromEmail = Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
            var emailqueue = EmailQueues.Single(eq => eq.Id == id);
            var emailqueueto = EmailQueueTos.Single(eq => eq.Id == id && eq.PeopleId == pid);
            var From = Util.FirstAddress(emailqueue.FromAddr, emailqueue.FromName);

            var p = LoadPersonById(emailqueueto.PeopleId);
            string text = emailqueue.Body;
            var aa = DoReplacements(ref text, CmsHost, p, emailqueueto);

            foreach (var ad in aa)
            {
                if (Util.ValidEmail(ad))
                {
                    var ma = new MailAddress(ad);
                    var qs = "OptOut/UnSubscribe/?enc=" + Util.EncryptForUrl("{0}|{1}".Fmt(emailqueueto.PeopleId, From.Address));
                    var url = Util.URLCombine(CmsHost, qs);
                    var link = @"<a href=""{0}"">Unsubscribe</a>".Fmt(url);
                    text = text.Replace("{unsubscribe}", link);
                    text = text.Replace("{Unsubscribe}", link);
                    text = text.Replace("{toemail}", ma.Address);
                    text = text.Replace("%7Btoemail%7D", ma.Address);
                    text = text.Replace("{fromemail}", From.Address);
                    text = text.Replace("%7Bfromemail%7D", From.Address);

                    if (Setting("sendemail", "true") != "false")
                    {
                        Util.SendMsg(SysFromEmail, CmsHost, From, emailqueue.Subject, text, p.Name, ad, emailqueue.Id);
                        emailqueueto.Sent = DateTime.Now;
                        SubmitChanges();
                    }
                }
            }
        }
        public List<string> DoReplacements(ref string text, string CmsHost, Person p, EmailQueueTo emailqueueto)
        {
            if (text == null)
                text = "(no content)";
            if (p.Name.Contains("?") || p.Name.Contains("unknown", true))
                text = text.Replace("{name}", string.Empty);
            else
                text = text.Replace("{name}", p.Name);

            if (p.PreferredName.Contains("?", true) || (p.PreferredName.Contains("unknown", true)))
                text = text.Replace("{first}", string.Empty);
            else
                text = text.Replace("{first}", p.PreferredName);
            text = text.Replace("{occupation}", p.OccupationOther);

            var re = new Regex(@"\{votelink:(?<orgid>\d+),(?<sg>[^}]*)\}", RegexOptions.Singleline | RegexOptions.Multiline);
            var list = new Dictionary<string, OneTimeLink>();
            var ma = re.Match(text);
            while (ma.Success)
            {
                var votelink = ma.Value;
                var orgid = ma.Groups["orgid"].Value;
                var smallgroup = ma.Groups["sg"].Value;
                var qs = @"{0},{1}".Fmt(orgid, emailqueueto.PeopleId);
                OneTimeLink ot;
                if (list.ContainsKey(qs))
                    ot = list[qs];
                else
                {
                    ot = new OneTimeLink
                    {
                        Id = Guid.NewGuid(),
                        Querystring = qs
                    };
                    OneTimeLinks.InsertOnSubmit(ot);
                    SubmitChanges();
                    list.Add(qs, ot);
                }
                var url = Util.URLCombine(CmsHost, "/OnlineReg/VoteLink/{0}?smallgroup={1}".Fmt(ot.Id.ToCode(), smallgroup));
                text = text.Replace(votelink, @"<a href=""{0}"">{1}</a>".Fmt(url, smallgroup));
                ma = ma.NextMatch();
            }
            if (emailqueueto.Guid.HasValue)
            {
                var turl = Util.URLCombine(CmsHost, "/Track/Index/" + emailqueueto.Guid.Value.ToCode());
                text = text.Replace("{track}", "<img src=\"{0}\" />".Fmt(turl));
            }

            var aa = new List<string>();
            if (p.SendEmailAddress1 ?? true)
                aa.Add(p.FromEmail);
            if (p.SendEmailAddress2 ?? false)
                aa.Add(p.FromEmail2);
            if (emailqueueto.AddEmail.HasValue())
                foreach (var ad in emailqueueto.AddEmail.SplitStr(","))
                    if (Util.ValidEmail(ad))
                        if (!aa.Any(mm => EmailMatch(mm, ad)))
                            aa.Add(ad);

            if (emailqueueto.OrgId.HasValue)
            {
                var qm = (from m in OrganizationMembers
                          where m.PeopleId == emailqueueto.PeopleId && m.OrganizationId == emailqueueto.OrgId
                          select new { m.PayLink, m.Amount, m.AmountPaid, m.RegisterEmail }).SingleOrDefault();
                if (qm != null)
                {
                    if (qm.PayLink.HasValue())
                        text = text.Replace("{paylink}", "<a href=\"{0}\">payment link</a>".Fmt(qm.PayLink));
                    text = text.Replace("{amtdue}", (qm.Amount - qm.AmountPaid).ToString2("c"));
                    if (qm.RegisterEmail.HasValue())
                        if (!aa.Any(mm => EmailMatch(mm, qm.RegisterEmail)))
                            aa.Add(qm.RegisterEmail);
                }
            }
            return aa;
        }
        bool EmailMatch(string existing, string addemail)
        {
            var exist = Util.TryGetMailAddress(existing, null);
            var add = Util.TryGetMailAddress(addemail, null);
            if (add == null || exist == null)
                return false;
            var r = string.Compare(exist.Address, add.Address, true);
            return r == 0;
        }
        public void SendPeopleEmail(string CmsHost, EmailQueue emailqueue)
        {
            var sysFromEmail = Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
            var From = Util.FirstAddress(emailqueue.FromAddr, emailqueue.FromName);
            if (!emailqueue.Subject.HasValue() || !emailqueue.Body.HasValue())
            {
                Util.SendMsg(sysFromEmail, CmsHost, From, "sent emails - error", "no subject or body, no emails sent", null, From.Address, emailqueue.Id);
                return;
            }

            emailqueue.Started = DateTime.Now;
            SubmitChanges();

            var sb = new StringBuilder("<pre>\r\n");
            var i = 0;

            var q = from To in EmailQueueTos
                    where To.Id == emailqueue.Id
                    where To.Sent == null
                    orderby To.PeopleId
                    select To;
            foreach (var To in q)
            {
                var p = LoadPersonById(To.PeopleId);
                string text = emailqueue.Body;
                var aa = DoReplacements(ref text, CmsHost, p, To);
                foreach (var ad in aa)
                {
                    if (Util.ValidEmail(ad))
                    {
                        i++;

                        var qs = "OptOut/UnSubscribe/?enc=" + Util.EncryptForUrl("{0}|{1}".Fmt(To.PeopleId, From.Address));
                        var url = Util.URLCombine(CmsHost, qs);
                        var link = @"<a href=""{0}"">Unsubscribe</a>".Fmt(url);
                        text = text.Replace("{unsubscribe}", link);
                        text = text.Replace("{Unsubscribe}", link);
                        text = text.Replace("{toemail}", ad);
                        text = text.Replace("%7Btoemail%7D", ad);
                        text = text.Replace("{fromemail}", From.Address);
                        text = text.Replace("%7Bfromemail%7D", From.Address);

                        if (Setting("sendemail", "true") != "false")
                        {
                            Util.SendMsg(sysFromEmail, CmsHost, From, emailqueue.Subject, text, p.Name, ad, emailqueue.Id);
                            To.Sent = DateTime.Now;

                            sb.AppendFormat("\"{0}\" [{1}] ({2})\r\n".Fmt(p.Name, ad, To.PeopleId));
                            SubmitChanges();
                        }
                    }
                }
            }
            NotifySentEmails(CmsHost, From.Address, From.DisplayName, emailqueue.Subject, i, emailqueue.Id);
            if (emailqueue.Redacted ?? false)
                emailqueue.Body = "redacted";
            emailqueue.Sent = DateTime.Now;
            SubmitChanges();
        }

        private void NotifySentEmails(string CmsHost, string From, string FromName, string subject, int count, int id)
        {
            if (Setting("sendemail", "true") != "false")
            {
                var from = new MailAddress(From, FromName);
                string subj = "sent emails: " + subject;
                var uri = new Uri(new Uri(CmsHost), "/Manage/Emails/Details/" + id);
                string body = @"<a href=""{0}"">{1} emails sent</a>".Fmt(uri, count);
                var SysFromEmail = Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
                var SendErrorsTo = ConfigurationManager.AppSettings["senderrorsto"];
                SendErrorsTo = SendErrorsTo.Replace(';', ',');

                Util.SendMsg(SysFromEmail, CmsHost, from, subj, body, from.DisplayName, from.Address, id);
                var host = uri.Host;
                Util.SendMsg(SysFromEmail, CmsHost, from, host + " " + subj, body, null, ConfigurationManager.AppSettings["senderrorsto"], id);
            }
        }
    }
}
