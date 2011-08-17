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
using System.Xml.Linq;
using Sgml;

namespace CmsData
{
    public partial class CMSDataContext
    {
        public bool UseMassEmailer
        {
            get
            {
#if DEBUG
                return false;
#else
                return Setting("UseMassEmailer", "false").ToBool();
#endif
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
            if (UseMassEmailer)
                QueuePriorityEmail(emailqueue.Id, CmsHost, Host);
            else
                SendPersonEmail(CmsHost, emailqueue.Id, p.PeopleId);
        }
        private List<MailAddress> PersonListToMailAddressList(IEnumerable<Person> list)
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
                return (from p in CMSRoleProvider.provider.GetAdmins()
                        orderby p.Users.Any(u => u.Roles.Contains("Developer")) descending
                        select p).ToList();
            return q2.ToList();
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
                return (from p in CMSRoleProvider.provider.GetAdmins()
                        orderby p.Users.Any(u => u.Roles.Contains("Developer")) descending
                        select p).ToList();
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
                        Util.SendMsg(SysFromEmail, CmsHost, From, emailqueue.Subject, text, aa, emailqueue.Id, pid, Record: true);
                    else
                        Util.SendMsg(SysFromEmail, CmsHost, From, 
                            "(no email address) " + emailqueue.Subject, 
                            "<p>No email address for {0}({1})</p>\n{2}".Fmt(p.Name, p.PeopleId, text), 
                            Util.ToMailAddressList(From), 
                            emailqueue.Id, pid, Record: true);
                    emailqueueto.Sent = DateTime.Now;
                    emailqueue.Sent = DateTime.Now;
                    SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                Util.SendMsg(SysFromEmail, CmsHost, From,
                    "sent emails - error", ex.ToString(),
                    Util.ToMailAddressList(From),
                    emailqueue.Id, null, Record: true);
                throw ex;
            }
        }
        public List<MailAddress> DoReplacements(ref string text, string CmsHost, Person p, EmailQueueTo emailqueueto)
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

            text = DoVoteLinkAnchorStyle(text, CmsHost, emailqueueto);
            text = DoVoteTag(text, CmsHost, emailqueueto);
            text = DoVoteTag2(text, CmsHost, emailqueueto);

            if (emailqueueto.Guid.HasValue)
            {
                var turl = Util.URLCombine(CmsHost, "/Track/Key/" + emailqueueto.Guid.Value.GuidToQuerystring());
                text = text.Replace("{track}", "<img src=\"{0}\" />".Fmt(turl));
            }

            var aa = GetAddressList(p);

            if (emailqueueto.AddEmail.HasValue())
                foreach (var ad in emailqueueto.AddEmail.SplitStr(","))
                    Util.AddGoodAddress(aa, ad);

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
                    Util.AddGoodAddress(aa, Util.FullEmail(qm.RegisterEmail, p.Name));
                }
            }
            return aa.DistinctEmails();
        }
        private string DoVoteLinkAnchorStyle(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"{votelink(?<inside>[^}]*)}";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var votelink = match.Value;
                var anchor = "<a " + match.Groups["inside"].Value + ">text</a>";
                anchor = anchor.Replace("&quot;", "\"");
                anchor = anchor.Replace("&rdquo;", "\"");
                anchor = anchor.Replace("&ldquo;", "\"");
                var rd = new SgmlReader();
                rd.DocType = "HTML";
                rd.InputStream = new StringReader(anchor);
                var e = XDocument.Load(rd).Descendants("a").First();
                var txt = "click here";
                var d = e.Attributes().ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);
                if (d.ContainsKey("text"))
                    txt = d["text"];

                string msg = "Thank you for responding.";
                if (d.ContainsKey("message"))
                    msg = d["message"];

                string confirm = "false";
                if (d.ContainsKey("confirm"))
                    confirm = d["confirm"];

                if (!d.ContainsKey("smallgroup"))
                    throw new Exception("Votelink: no smallgroup attribute");
                var smallgroup = d["smallgroup"];
                var pre = "";
                var a = smallgroup.SplitStr(":");
                if (a.Length > 1)
                    pre = a[0];

                if (!d.ContainsKey("id"))
                    throw new Exception("Votelink: no id attribute");
                var id = d["id"];

                var url = VoteLinkUrl(text, CmsHost, emailqueueto, list, votelink, id, msg, confirm, smallgroup, pre);
                text = text.Replace(votelink, @"<a href=""{0}"">{1}</a>".Fmt(url, txt));

                match = match.NextMatch();
            }
            return text;
        }//&lt;votetag .*?&gt;(?<inside>.+?)&lt;/votetag&gt;
        private string DoVoteTag2(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"&lt;votetag .*?&gt;(?<inside>.+?)&lt;/votetag&gt;";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var inside = HttpUtility.HtmlDecode(match.Groups["inside"].Value);
                var rd = new SgmlReader();
                rd.DocType = "HTML";
                rd.InputStream = new StringReader(HttpUtility.HtmlDecode(tag));
                var e = XDocument.Load(rd).Descendants("votetag").First();
                var d = e.Attributes().ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                string msg = "Thank you for responding.";
                if (d.ContainsKey("message"))
                    msg = d["message"];

                string confirm = "false";
                if (d.ContainsKey("confirm"))
                    confirm = d["confirm"];

                if (!d.ContainsKey("smallgroup"))
                    throw new Exception("Votelink: no smallgroup attribute");
                var smallgroup = d["smallgroup"];
                var pre = "";
                var a = smallgroup.SplitStr(":");
                if (a.Length > 1)
                    pre = a[0];

                if (!d.ContainsKey("id"))
                    throw new Exception("Votelink: no id attribute");
                var id = d["id"];

                var url = VoteLinkUrl(text, CmsHost, emailqueueto, list, tag, id, msg, confirm, smallgroup, pre);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string DoVoteTag(string text, string CmsHost, EmailQueueTo emailqueueto)
        {
            var list = new Dictionary<string, OneTimeLink>();
            const string VoteLinkRE = @"<votetag[^>]*>(?<inside>.+?)</votetag>";
            var re = new Regex(VoteLinkRE, RegexOptions.Singleline | RegexOptions.Multiline);
            var match = re.Match(text);
            while (match.Success)
            {
                var tag = match.Value;
                var inside = match.Groups["inside"].Value;
                var rd = new SgmlReader();
                rd.DocType = "HTML";
                rd.InputStream = new StringReader(tag);
                var e = XDocument.Load(rd).Descendants("votetag").First();
                var d = e.Attributes().ToDictionary(aa => aa.Name.ToString(), aa => aa.Value);

                string msg = "Thank you for responding.";
                if (d.ContainsKey("message"))
                    msg = d["message"];

                string confirm = "false";
                if (d.ContainsKey("confirm"))
                    confirm = d["confirm"];

                if (!d.ContainsKey("smallgroup"))
                    throw new Exception("Votelink: no smallgroup attribute");
                var smallgroup = d["smallgroup"];
                var pre = "";
                var a = smallgroup.SplitStr(":");
                if (a.Length > 1)
                    pre = a[0];

                if (!d.ContainsKey("id"))
                    throw new Exception("Votelink: no id attribute");
                var id = d["id"];

                var url = VoteLinkUrl(text, CmsHost, emailqueueto, list, tag, id, msg, confirm, smallgroup, pre);
                text = text.Replace(tag, @"<a href=""{0}"">{1}</a>".Fmt(url, inside));
                match = match.NextMatch();
            }
            return text;
        }
        private string VoteLinkUrl(string text,
            string CmsHost,
            EmailQueueTo emailqueueto,
            Dictionary<string, OneTimeLink> list,
            string votelink,
            string id,
            string msg,
            string confirm,
            string smallgroup,
            string pre)
        {
            var qs = "{0},{1},{2},{3}".Fmt(id, emailqueueto.PeopleId, emailqueueto.Id, pre);
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
            var url = Util.URLCombine(CmsHost, "/OnlineReg/VoteLink/{0}?smallgroup={1}&confirm={2}&message={3}"
                .Fmt(ot.Id.ToCode(), HttpUtility.UrlEncode(smallgroup), confirm, HttpUtility.UrlEncode(msg)));
            return url;
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
                    emailqueue.Id, null, Record: true);
                return;
            }

            emailqueue.Started = DateTime.Now;
            SubmitChanges();

            var sb = new StringBuilder("<pre>\r\n");

            var q = from To in EmailQueueTos
                    where To.Id == emailqueue.Id
                    where To.Sent == null
                    orderby To.PeopleId
                    select To;
            try
            {
                foreach (var To in q)
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
                            emailqueue.Subject, text, aa, emailqueue.Id, To.PeopleId, Record: true);
                        To.Sent = DateTime.Now;

                        foreach (var ma in aa)
                            sb.AppendFormat("{0} ({1})\r\n".Fmt(ma.ToString(), To.PeopleId));
                        SubmitChanges();
                    }
                }
                emailqueue.Sent = DateTime.Now;
                if (emailqueue.Redacted ?? false)
                    emailqueue.Body = "redacted";
                else
                {
                    var nitems = emailqueue.EmailQueueTos.Count();
                    if (nitems > 1)
                        NotifySentEmails(CmsHost, From.Address, From.DisplayName,
                            emailqueue.Subject, nitems, emailqueue.Id);
                }
                SubmitChanges();
            }
            catch (Exception ex)
            {
                Util.SendMsg(sysFromEmail, CmsHost, From,
                    "sent emails - error", ex.Message,
                    Util.ToMailAddressList(From),
                    emailqueue.Id, null, Record: true);
                throw ex;
            }
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
                    subj, body, Util.ToMailAddressList(from), id, null, Record: true);
                var host = uri.Host;
                Util.SendMsg(SysFromEmail, CmsHost, from,
                    host + " " + subj, body,
                    Util.SendErrorsTo(), id, null, Record: true);
            }
        }
    }
}
