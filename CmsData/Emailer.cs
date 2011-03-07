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

namespace CmsData
{
    public partial class CMSDataContext
    {   
        public bool UseMassEmailer()
        {
            return Setting("UseMassEmailer", "false").ToBool();
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
                Redacted = redacted
            };
            EmailQueues.InsertOnSubmit(emailqueue);
            emailqueue.EmailQueueTos.Add(new EmailQueueTo
            {
                PeopleId = p.PeopleId,
                OrgId = CurrentOrgId,
                AddEmail = addemail,
            });
            SubmitChanges();
            if (UseMassEmailer())
                QueuePriorityEmail(emailqueue.Id, Util.CmsHost, Util.Host);
            else
                SendPersonEmail(Util.CmsHost, emailqueue.Id, p.PeopleId, addemail);
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
            var a = pidstring.SplitStr(",").Select(ss => ss.ToInt());
            var q = from p in People
                    where a.Contains(p.PeopleId)
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
            var a = pids.SplitStr(",").Select(ss => ss.ToInt());
            var q2 = from p in People
                     where a.Contains(p.PeopleId)
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
            var a = pids.SplitStr(",").Select(ss => ss.ToInt());
            var q2 = from p in People
                     where p.PeopleId == a.First()
                     select p.FromEmail;
            return q2.SingleOrDefault();
        }
        public IEnumerable<Person> StaffPeopleForOrg(int orgid)
        {
            var q = from o in Organizations
                    where o.OrganizationId == orgid
                    where o.NotifyIds != null && o.NotifyIds != ""
                    select o.NotifyIds;
            var pids = string.Join(",", q);
            var a = pids.SplitStr(",").Select(ss => ss.ToInt());
            var q2 = from p in People
                     where p.PeopleId == a.First()
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
        public void QueueMsg(string CmsHost, MailAddress From, string subject, string Message, Person p, string AddEmail)
        {
        }
        public EmailQueue CreateQueue(string cmshost, MailAddress From, string subject, string body, DateTime? schedule, int QBId, bool wantParents)
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
                     where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                     where !p.EmailOptOuts.Any(oo => oo.FromEmail == emailqueue.FromAddr)
                     orderby p.PeopleId
                     select p.PeopleId;

            foreach (var pid in q2)
                emailqueue.EmailQueueTos.Add(new EmailQueueTo { PeopleId = pid, OrgId = CurrentOrgId });
            SubmitChanges();
            return emailqueue;
        }
        public void SendPersonEmail(string CmsHost, int id, int pid, string addemail)
        {
            var SysFromEmail = Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
            var emailqueue = EmailQueues.Single(eq => eq.Id == id);
            var emailqueueto = EmailQueueTos.Single(eq => eq.Id == id && eq.PeopleId == pid);
            var From = Util.FirstAddress(emailqueue.FromAddr, emailqueue.FromName);
            var Message = emailqueue.Body;

            var q = from p in People
                    where p.PeopleId == emailqueueto.PeopleId
                    select new
                     {
                         p.Name,
                         p.PreferredName,
                         p.EmailAddress,
                         p.OccupationOther,
                         p.EmailAddress2,
                         Send1 = p.SendEmailAddress1 ?? true,
                         Send2 = p.SendEmailAddress2 ?? false,
                     };
            var qp = q.Single();

            string text = emailqueue.Body;

            if (qp.Name.Contains("?") || qp.Name.ToLower().Contains("unknown"))
                text = text.Replace("{name}", string.Empty);
            else
                text = text.Replace("{name}", qp.Name);

            if (qp.PreferredName.Contains("?") || qp.PreferredName.ToLower() == "unknown")
                text = text.Replace("{first}", string.Empty);
            else
                text = text.Replace("{first}", qp.PreferredName);
            text = text.Replace("{occupation}", qp.OccupationOther);

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
                var url = Util.URLCombine(CmsHost, "/OnlineReg/VoteLink/{0}?smallgroup={1}".Fmt(ot.Id, smallgroup));
                text = text.Replace(votelink, @"<a href=""{0}"">{1}</a>".Fmt(url, smallgroup));
                ma = ma.NextMatch();
            }

            var aa = new List<string>();
            if (qp.Send1)
                aa.AddRange(qp.EmailAddress.SplitStr(",;"));
            if (qp.Send2)
                aa.AddRange(qp.EmailAddress2.SplitStr(",;"));
            if (addemail.HasValue())
                aa.AddRange(addemail.SplitStr(",;"));

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
                    if (qm.RegisterEmail.HasValue() && !aa.Contains(qm.RegisterEmail, StringComparer.OrdinalIgnoreCase))
                        aa.Add(qm.RegisterEmail);
                }
            }

            foreach (var ad in aa)
            {
                if (Util.ValidEmail(ad))
                {
                    var qs = "OptOut/UnSubscribe/?enc=" + Util.EncryptForUrl("{0}|{1}".Fmt(emailqueueto.PeopleId, From.Address));
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
                        Util.SendMsg(SysFromEmail, CmsHost, From, emailqueue.Subject, text, qp.Name, ad, emailqueue.Id);
                        emailqueueto.Sent = DateTime.Now;
                        SubmitChanges();
                    }
                }
            }
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
                var qp = (from p in People
                          where p.PeopleId == To.PeopleId
                          select new
                          {
                              p.Name,
                              p.PreferredName,
                              p.EmailAddress,
                              p.OccupationOther,
                              p.EmailAddress2,
                              Send1 = p.SendEmailAddress1 ?? true,
                              Send2 = p.SendEmailAddress2 ?? false,
                          }).Single();
                string text = emailqueue.Body;

                if (qp.Name.Contains("?") || qp.Name.ToLower().Contains("unknown"))
                    text = text.Replace("{name}", string.Empty);
                else
                    text = text.Replace("{name}", qp.Name);

                if (qp.PreferredName.Contains("?") || qp.PreferredName.ToLower() == "unknown")
                    text = text.Replace("{first}", string.Empty);
                else
                    text = text.Replace("{first}", qp.PreferredName);
                text = text.Replace("{occupation}", qp.OccupationOther);

                var re = new Regex(@"\{votelink:(?<orgid>\d+),(?<sg>[^}]*)\}", RegexOptions.Singleline | RegexOptions.Multiline);
                var list = new Dictionary<string, OneTimeLink>();
                var ma = re.Match(text);
                while (ma.Success)
                {
                    var votelink = ma.Value;
                    var orgid = ma.Groups["orgid"].Value;
                    var smallgroup = ma.Groups["sg"].Value;
                    var qs = @"{0},{1}".Fmt(orgid, To.PeopleId);
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
                    var url = Util.URLCombine(CmsHost, "/OnlineReg/VoteLink/{0}?smallgroup={1}".Fmt(ot.Id, smallgroup));
                    text = text.Replace(votelink, @"<a href=""{0}"">{1}</a>".Fmt(url, smallgroup));
                    ma = ma.NextMatch();
                }

                var aa = new List<string>();
                if (qp.Send1)
                    aa.AddRange(qp.EmailAddress.SplitStr(",;"));
                if (qp.Send2)
                    aa.AddRange(qp.EmailAddress2.SplitStr(",;"));
                if (To.OrgId.HasValue)
                {
                    var qm = (from m in OrganizationMembers
                              where m.PeopleId == To.PeopleId && m.OrganizationId == To.OrgId
                              select new { m.PayLink, m.Amount, m.AmountPaid, m.RegisterEmail }).SingleOrDefault();
                    if (qm != null)
                    {
                        if (qm.PayLink.HasValue())
                            text = text.Replace("{paylink}", "<a href=\"{0}\">payment link</a>".Fmt(qm.PayLink));
                        text = text.Replace("{amtdue}", (qm.Amount - qm.AmountPaid).ToString2("c"));
                        if (qm.RegisterEmail.HasValue() && !aa.Contains(qm.RegisterEmail, StringComparer.OrdinalIgnoreCase))
                            aa.Add(qm.RegisterEmail);
                    }
                }

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
                            Util.SendMsg(sysFromEmail, CmsHost, From, emailqueue.Subject, text, qp.Name, ad, emailqueue.Id);
                            To.Sent = DateTime.Now;

                            sb.AppendFormat("\"{0}\" [{1}] ({2})\r\n".Fmt(qp.Name, ad, To.PeopleId));
                            if (i % 500 == 0)
                                NotifySentEmails(CmsHost, sb, From, emailqueue.Subject, emailqueue.Body, emailqueue.Id);
                            SubmitChanges();
                        }
                    }
                }
            }
            NotifySentEmails(CmsHost, sb, From, emailqueue.Subject, emailqueue.Body, emailqueue.Id);
            if (emailqueue.Redacted ?? false)
                emailqueue.Body = "redacted";
            emailqueue.Sent = DateTime.Now;
            SubmitChanges();
        }
        private void NotifySentEmails(string CmsHost, StringBuilder sb, MailAddress From, string subject, string body, int id)
        {
            var sysFromEmail = Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
            sb.Append("</pre>\r\n<h2>{0}</h2>".Fmt(subject));
            sb.Append(body);
            if (Setting("sendemail", "true") != "false")
            {
                string subj = "sent emails: " + subject;
                Util.SendMsg(sysFromEmail, CmsHost, From, subj, sb.ToString(), From.DisplayName, From.Address, id);
                Util.SendMsg(sysFromEmail, CmsHost, From, subj, sb.ToString(), null, ConfigurationManager.AppSettings["senderrorsto"], id);
            }
            sb.Length = 0;
            sb.Append("<pre>\r\n");
        }
    }
}

