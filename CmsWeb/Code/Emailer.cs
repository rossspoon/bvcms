/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Net.Mail;
using System.Collections.Generic;
using System.Threading;
using CmsData;
using UtilityExtensions;
using System.Web;
using System.Linq;
using CMSPresenter;
using System.IO;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Configuration;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace CmsWeb
{
    public class Emailer : ITaskNotify
    {
        private string Subject;
        private string Message;
        private MailAddress _From;
        public MailAddress From
        {
            get
            {
                if (_From == null)
                    _From = Util.FirstAddress(DbUtil.Db.Setting("AdminMail", DbUtil.SystemEmailAddress));
                return _From;
            }
            set
            {
                _From = value;
            }
        }

        private MailAddressCollection Addresses = new MailAddressCollection();
        private IEnumerable<Person> people;

        public Emailer(string fromaddr, string fromname)
        {
            From = Util.FirstAddress(fromaddr, fromname);
        }
        public Emailer(string fromaddr)
        {
            From = Util.FirstAddress(fromaddr);
        }
        public Emailer()
        {
        }
        public void LoadAddress(string Address, string Name)
        {
            var aa = Address.SplitStr(",;");
            foreach (var ad in aa)
            {
                var ma = Util.TryGetMailAddress(ad, Name);
                if (ma != null)
                    Addresses.Add(ma);
            }
        }

        private void NotifyEmail(string subject, string message)
        {
            Subject = subject;
            Message = message;
            var i = 0;
            SmtpClient smtp = null;

            foreach (var a in Addresses)
            {
                var msg = new MailMessage(From, a);
                string sysfromemail = WebConfigurationManager.AppSettings["sysfromemail"];
                if (sysfromemail.HasValue())
                {
                    var sysmail = new MailAddress(sysfromemail);
                    if (From.Host != sysmail.Host)
                        msg.Sender = sysmail;
                }
                msg.Subject = Subject;
                msg.Body = Message;
                msg.IsBodyHtml = true;
                if (i % 20 == 0)
                    smtp = Util.Smtp();
                i++;
                try
                {
                    smtp.Send(msg);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.StartsWith("The remote name could not be resolved"))
                        return;
                    throw;
                }
            }
        }
        public void SendPeopleEmail(CMSDataContext Db, string CmsHost, IEnumerable<Person> people, string subject, string message, bool IsHtml)
        {
            this.people = people;
            Subject = subject;

            var sb = new StringBuilder("<pre>\r\n");
            SmtpClient smtp = null;
            var i = 0;

            if (IsHtml)
                Message = message;
            else
                Message = Util.SafeFormat(message);
            var bhtml = Message;

            int EmailBatchCount = Db.Setting("EmailBatchCount", "500").ToInt();

            foreach (var p in people)
            {
                var em = p.EmailAddress.Trim();
                if (!p.EmailAddress.HasValue())
                    continue;

                if (i % 20 == 0)
                    smtp = Util.Smtp();
                i++;

                var text = bhtml.Replace("{name}", p.Name);
                text = text.Replace("{first}", p.PreferredName);
                if (Db.CurrentOrgId > 0 && text.Contains("{paylink}"))
                {
                    var om = Db.OrganizationMembers.SingleOrDefault(o =>
                        o.OrganizationId == Db.CurrentOrgId && o.PeopleId == p.PeopleId);
                    if (om != null)
                    {
                        if (om.PayLink.HasValue())
                            text = text.Replace("{paylink}",
                                "<a href=\"{0}\">payment link</a>".Fmt(om.PayLink));
                        text = text.Replace("{amtdue}", (om.Amount - om.AmountPaid).ToString2("c"));
                        if (om.RegisterEmail.HasValue() && string.Compare(om.RegisterEmail, em, true) == 0)
                            em += (";" + om.RegisterEmail);
                    }
                }
                text = text.Replace("{unsubscribe}",
                    "<a href=\"{0}OptOut/UnSubscribe/?enc={1}\">Unsubscribe</a>"
                    .Fmt(CmsHost, p.OptOutKey(From.Address)));

                Util.SendMsg(smtp, CmsHost, From, subject, text, p.Name, p.EmailAddress);

                sb.AppendFormat("\"{0}\" [{1}] ({2})\r\n".Fmt(p.Name, em, p.PeopleId));
                if (i % EmailBatchCount == 0)
                    NotifySentEmails(Db, CmsHost, sb, smtp, From, Subject, Message, i);
            }
            if (smtp != null)
                NotifySentEmails(Db, CmsHost, sb, smtp, From, Subject, Message, i);
        }
        public static void SendPeopleEmail(CMSDataContext Db, string CmsHost, EmailQueue emailqueue)
        {
            var emailer = new Emailer(emailqueue.FromAddr);
            emailer.Message = emailqueue.Body;
            emailqueue.Started = DateTime.Now;
            Db.SubmitChanges();

            var sb = new StringBuilder("<pre>\r\n");
            SmtpClient smtp = null;
            var i = 0;
            int EmailBatchCount = Db.Setting("EmailBatchCount", "500").ToInt();

            var q = from To in Db.EmailQueueTos
                    where To.Id == emailqueue.Id
                    where To.Sent == null
                    orderby To.PeopleId
                    select To;
            foreach (var To in q)
            {
                var p = Db.LoadPersonById(To.PeopleId);
                var text = emailqueue.Body.Replace("{name}", p.Name);
                text = text.Replace("{first}", p.PreferredName);
                var aa = p.EmailAddress.SplitStr(",;").ToList();
                if (To.OrgId.HasValue)
                {
                    var om = Db.OrganizationMembers.SingleOrDefault(o =>
                        o.OrganizationId == To.OrgId && o.PeopleId == p.PeopleId);
                    if (om != null)
                    {
                        if (om.PayLink.HasValue())
                            text = text.Replace("{paylink}", "<a href=\"{0}\">payment link</a>".Fmt(om.PayLink));
                        text = text.Replace("{amtdue}", (om.Amount - om.AmountPaid).ToString2("c"));
                        if (om.RegisterEmail.HasValue() && !aa.Contains(om.RegisterEmail, StringComparer.OrdinalIgnoreCase))
                            aa.Add(om.RegisterEmail);
                    }
                }

                foreach (var ad in aa)
                {
                    if (Util.ValidEmail(ad))
                    {
                        if (i % 20 == 0)
                            smtp = Util.Smtp();
                        i++;

                        text = text.Replace("{unsubscribe}",
                            "<a href=\"{0}OptOut/UnSubscribe/?enc={1}\">Unsubscribe</a>"
                            .Fmt(CmsHost, p.OptOutKey(emailqueue.FromAddr)));

                        Util.SendMsg(smtp, CmsHost, emailer.From, emailqueue.Subject, text, p.Name, ad, emailqueue.Id);
                        if (smtp.PickupDirectoryLocation.HasValue())
                            Thread.Sleep(50); // simulate sending
                        To.Sent = DateTime.Now;

                        sb.AppendFormat("\"{0}\" [{1}] ({2})\r\n".Fmt(p.Name, ad, p.PeopleId));
                        if (i % EmailBatchCount == 0)
                            NotifySentEmails(Db, CmsHost, sb, smtp, emailer.From, emailqueue.Subject, emailqueue.Body, emailqueue.Id);
                        Db.SubmitChanges();
                    }
                }
            }
            if (smtp != null)
                NotifySentEmails(Db, CmsHost, sb, smtp, emailer.From, emailqueue.Subject, emailqueue.Body, emailqueue.Id);
            emailqueue.Sent = DateTime.Now;
            Db.SubmitChanges();
        }
        public void EmailNotification(Person from, Person to, string subject, string message)
        {
            From = Util.FirstAddress(from.EmailAddress, from.Name);
            Addresses.Clear();
            LoadAddress(to.EmailAddress, to.Name);
            if (Addresses.Count > 0)
                NotifyEmail(subject, message);
        }
        private static void NotifySentEmails(CMSDataContext Db, string CmsHost, StringBuilder sb, SmtpClient smtp, MailAddress From, string subject, string body, int id)
        {
            sb.Append("</pre>\r\n<h2>{0}</h2>".Fmt(subject));
            sb.Append(body);
            Util.SendMsg(smtp, CmsHost, From, "sent emails", sb.ToString(), null, From.Address, id);
            Util.SendMsg(smtp, CmsHost, From, "sent emails", sb.ToString(), null,
                WebConfigurationManager.AppSettings["senderrorsto"], id);
            var notices = DbUtil.Db.Setting("NotifySentEmails", null);
            if (notices.HasValue())
                Util.SendMsg(smtp, CmsHost, From, "sent emails", sb.ToString(), null, notices, id);

            sb.Length = 0;
            sb.Append("<pre>\r\n");
        }
    }
}

