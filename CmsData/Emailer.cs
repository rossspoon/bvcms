/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Net.Mail;
using System.Threading;
using CmsData.Codes;
using UtilityExtensions;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using HtmlAgilityPack;

namespace CmsData
{
    public partial class CMSDataContext
    {
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
        public void Email(string from, Person p, List<MailAddress> addmail, string subject, string body, bool redacted)
        {
            var From = Util.FirstAddress(from);
            if (From == null)
                From = Util.FirstAddress(Util.SysFromEmail);
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
            string addmailstr = null;
            if (addmail != null)
                addmailstr = addmail.EmailAddressListToString();
            emailqueue.EmailQueueTos.Add(new EmailQueueTo
            {
                PeopleId = p.PeopleId,
                OrgId = CurrentOrgId,
                AddEmail = addmailstr,
                Guid = Guid.NewGuid(),
            });
            SubmitChanges();
            SendPersonEmail(CmsHost, emailqueue.Id, p.PeopleId);
        }

        public List<MailAddress> PersonListToMailAddressList(IEnumerable<Person> list)
        {
            var aa = new List<MailAddress>();
            foreach (var p in list)
                aa.AddRange(GetAddressList(p));
            return aa;
        }
        public void Email(string from, IEnumerable<Person> list, string subject, string body)
        {
            if (list.Count() == 0)
                return;
            var aa = PersonListToMailAddressList(list);
            Email(from, list.First(), aa, subject, body, false);
        }
        public void EmailRedacted(string from, IEnumerable<Person> list, string subject, string body)
        {
            if (list.Count() == 0)
                return;
            var aa = PersonListToMailAddressList(list);
            Email(from, list.First(), aa, subject, body, redacted: true);
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
        public List<Person> StaffPeopleForDiv(int divid)
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
                return AdminPeople();
            return q2.ToList();
        }
        public List<Person> AdminPeople()
        {
            return (from p in CMSRoleProvider.provider.GetAdmins()
                    orderby p.Users.Any(u => u.Roles.Contains("Developer")) ascending
                    select p).ToList();
        }
        public List<Person> FinancePeople()
        {
            var q = from u in Users
                    where u.UserRoles.Any(ur => ur.Role.RoleName == "Finance")
                    select u.Person;
            return q.ToList();
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
        public List<Person> StaffPeopleForOrg(int orgid)
        {
            var org = LoadOrganizationById(orgid);
            var pids = org.NotifyIds ?? "";
            var a = pids.Split(',').Select(ss => ss.ToInt()).ToArray();
            var q2 = from p in People
                     where a.Contains(p.PeopleId)
                     orderby p.PeopleId == a.FirstOrDefault() descending
                     select p;
            //if (q2.Count() == 0)
            //    return (from p in CMSRoleProvider.provider.GetAdmins()
            //            orderby p.Users.Any(u => u.Roles.Contains("Developer")) descending
            //            select p).ToList();
            return q2.ToList();
        }
        public Person UserPersonFromEmail(string email)
        {
            var q = from u in Users
                    where u.Person.EmailAddress == email || u.Person.EmailAddress2 == email
                    select u.Person;
            var p = q.FirstOrDefault();
            if (p == null)
                p = CMSRoleProvider.provider.GetAdmins().First();
            return p;
        }
        public EmailQueue CreateQueue(MailAddress From, string subject, string body, DateTime? schedule, int tagId, bool publicViewable)
        {
            return CreateQueue(Util.UserPeopleId, From, subject, body, schedule, tagId, publicViewable);
        }
        public EmailQueue CreateQueue(int? queuedBy, MailAddress from, string subject, string body, DateTime? schedule, int tagId, bool publicViewable)
        {
            var tag = TagById(tagId);
            if (tag == null)
                return null;

            var emailqueue = new EmailQueue
            {
                Queued = DateTime.Now,
                FromAddr = from.Address,
                FromName = from.DisplayName,
                Subject = subject,
                Body = body,
                SendWhen = schedule,
                QueuedBy = queuedBy,
                Transactional = false,
                PublicX = publicViewable,
            };
            EmailQueues.InsertOnSubmit(emailqueue);

            SubmitChanges();
            if (body.Contains("http://publiclink", ignoreCase: true))
            {
                var link = Util.URLCombine(CmsHost, "/Manage/Emails/View/" + emailqueue.Id);
                var re = new Regex("http://publiclink", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
                emailqueue.Body = re.Replace(body, link);
            }

            var q = tag.People(this); 

            var q2 = from p in q.Distinct()
                     where p.EmailAddress != null
                     where p.EmailAddress != ""
                     where (p.SendEmailAddress1 ?? true) || (p.SendEmailAddress2 ?? false)
                     where p.EmailOptOuts.All(oo => oo.FromEmail != emailqueue.FromAddr)
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
            var fromname = emailqueue.FromName;
            if (!fromname.HasValue())
                fromname = emailqueue.FromAddr;
            else
                fromname = emailqueue.FromName.Replace("\"", "");
            var From = Util.FirstAddress(emailqueue.FromAddr, fromname);

            try
            {
                var p = LoadPersonById(emailqueueto.PeopleId);
                string text = emailqueue.Body;
                var aa = DoReplacements(ref text, CmsHost, p, emailqueueto);

                var qs = "OptOut/UnSubscribe/?enc=" + Util.EncryptForUrl("{0}|{1}".Fmt(emailqueueto.PeopleId, From.Address));
                var url = Util.URLCombine(CmsHost, qs);
                var link = @"<a href=""{0}"">Unsubscribe</a>".Fmt(url);
                text = text.Replace("{unsubscribe}", link);
                text = text.Replace("{Unsubscribe}", link);
                if (aa.Count > 0)
                {
                    text = text.Replace("{toemail}", aa[0].Address);
                    text = text.Replace("%7Btoemail%7D", aa[0].Address);
                }
                text = text.Replace("{fromemail}", From.Address);
                text = text.Replace("%7Bfromemail%7D", From.Address);

                if (Setting("sendemail", "true") != "false")
                {
                    if (aa.Count > 0)
                        Util.SendMsg(SysFromEmail, CmsHost, From, emailqueue.Subject, text, aa, emailqueue.Id, pid);
                    else
                        Util.SendMsg(SysFromEmail, CmsHost, From,
                            "(no email address) " + emailqueue.Subject,
                            "<p style='color:red'>You are receiving this because there is no email address for {0}({1}). You should probably contact them since they were probably expecting this information.</p>\n{2}".Fmt(p.Name, p.PeopleId, text),
                            Util.ToMailAddressList(From),
                            emailqueue.Id, pid);
                    emailqueueto.Sent = DateTime.Now;
                    emailqueue.Sent = DateTime.Now;
                    if (emailqueue.Redacted ?? false)
                        emailqueue.Body = "redacted";
                    SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Util.SendMsg(SysFromEmail, CmsHost, From,
                    "sent emails - error", ex.ToString(),
                    Util.ToMailAddressList(From),
                    emailqueue.Id, null);
                throw ex;
            }
        }
        public List<MailAddress> GetAddressList(Person p)
        {
            return GetAddressList(p, null);
        }
        public List<MailAddress> GetAddressList(Person p, string regemail)
        {
            var aa = new List<MailAddress>();
            if (p.SendEmailAddress1 ?? true)
                Util.AddGoodAddress(aa, p.FromEmail);
            if (p.SendEmailAddress2 ?? false)
                Util.AddGoodAddress(aa, p.FromEmail2);
            if (regemail.HasValue())
                foreach (var ad in regemail.SplitStr(",;"))
                    Util.AddGoodAddress(aa, ad);
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
        public void SendPeopleEmail(int queueid)
        {
            var emailqueue = EmailQueues.Single(ee => ee.Id == queueid);
            var sysFromEmail = Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
            var From = Util.FirstAddress(emailqueue.FromAddr, emailqueue.FromName);
            if (!emailqueue.Subject.HasValue() || !emailqueue.Body.HasValue())
            {
                Util.SendMsg(sysFromEmail, CmsHost, From,
                    "sent emails - error", "no subject or body, no emails sent",
                    Util.ToMailAddressList(From),
                    emailqueue.Id, null);
                return;
            }

            emailqueue.Started = DateTime.Now;
            SubmitChanges();

            var q = from To in EmailQueueTos
                    where To.Id == emailqueue.Id
                    where To.Sent == null
                    orderby To.PeopleId
                    select To;
            foreach (var To in q)
            {
                try
                {
                    var p = LoadPersonById(To.PeopleId);
                    string text = emailqueue.Body;
                    var aa = DoReplacements(ref text, CmsHost, p, To);
                    var qs = "OptOut/UnSubscribe/?enc=" + Util.EncryptForUrl("{0}|{1}".Fmt(To.PeopleId, From.Address));
                    var url = Util.URLCombine(CmsHost, qs);
                    var link = @"<a href=""{0}"">Unsubscribe</a>".Fmt(url);
                    text = text.Replace("{unsubscribe}", link);
                    text = text.Replace("{Unsubscribe}", link);
                    if (aa.Count > 0)
                    {
                        text = text.Replace("{toemail}", aa[0].Address);
                        text = text.Replace("%7Btoemail%7D", aa[0].Address);
                    }
                    text = text.Replace("{fromemail}", From.Address);
                    text = text.Replace("%7Bfromemail%7D", From.Address);

                    if (Setting("sendemail", "true") != "false")
                    {
                        Util.SendMsg(sysFromEmail, CmsHost, From,
                            emailqueue.Subject, text, aa, emailqueue.Id, To.PeopleId);
                        To.Sent = DateTime.Now;

                        SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    Util.SendMsg(sysFromEmail, CmsHost, From,
                        "sent emails - error: {0}".Fmt(CmsHost), ex.Message,
                        Util.ToMailAddressList(From),
                        emailqueue.Id, null);
                    Util.SendMsg(sysFromEmail, CmsHost, From,
                        "sent emails - error: {0}".Fmt(CmsHost), ex.Message,
                        Util.SendErrorsTo(),
                        emailqueue.Id, null);
                }
            }
            emailqueue.Sent = DateTime.Now;
            if (emailqueue.Redacted ?? false)
                emailqueue.Body = "redacted";
            else if (emailqueue.Transactional == false)
            {
                var nitems = emailqueue.EmailQueueTos.Count();
                if (nitems > 1)
                    NotifySentEmails(CmsHost, From.Address, From.DisplayName,
                        emailqueue.Subject, nitems, emailqueue.Id);
            }
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

                Util.SendMsg(SysFromEmail, CmsHost, from,
                    subj, body, Util.ToMailAddressList(from), id, null);
                var host = uri.Host;
                Util.SendMsg(SysFromEmail, CmsHost, from,
                    host + " " + subj, body,
                    Util.SendErrorsTo(), id, null);
            }
        }
    }
}
