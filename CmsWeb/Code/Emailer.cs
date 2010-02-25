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

namespace CMSWeb
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
                    _From = Util.FirstAddress(DbUtil.Settings("AdminMail", DbUtil.SystemEmailAddress));
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
        public void SendPeopleEmail(IEnumerable<Person> people,
            string subject,
            string message,
            FileUpload attach,
            bool IsHtml)
        {
            this.people = people;

            Attachment a = null;

            if (attach.FileName.HasValue())
                a = new Attachment(attach.FileContent, attach.FileName);

            var sb = new StringBuilder("<pre>\r\n");
            SmtpClient smtp = null;
            var i = 0;

            if (IsHtml)
                Message = message;
            else
                Message = Util.SafeFormat(message);
            var bhtml = Message;

            int EmailBatchCount = DbUtil.Settings("EmailBatchCount", "500").ToInt();

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
                text = text.Replace("{unsubscribe}",
                    "<a href=\"{0}OptOut/UnSubscribe/{1}\">Unsubscribe</a>"
                    .Fmt(Util.CmsHost, p.OptOutKey(From.Address)));

                Util.SendMsg(smtp, From, subject, text, p.Name, p.EmailAddress, a);

                DbUtil.Db.EmailLogs.InsertOnSubmit(
                    new EmailLog
                    {
                        Fromaddr = From.Address,
                        Toaddr = p.EmailAddress,
                        Subject = Subject,
                        Time = Util.Now
                    });
                DbUtil.Db.SubmitChanges();

                //System.Threading.Thread.Sleep(100);
                sb.AppendFormat("\"{0}\" [{1}] ({2})\r\n".Fmt(p.Name, em, p.PeopleId));
                if (i % EmailBatchCount == 0)
                    NotifySentEmails(sb, smtp, String.Empty);
            }
            NotifySentEmails(sb, smtp, String.Empty);
        }
        public void EmailNotification(Person from, Person to, string subject, string message)
        {
            From = new MailAddress(from.EmailAddress, from.Name);
            Addresses.Clear();
            LoadAddress(to.EmailAddress, to.Name);
            if (Addresses.Count > 0)
                NotifyEmail(subject, message);
        }
        private void NotifySentEmails(StringBuilder sb, SmtpClient smtp, string message)
        {
            sb.Append("</pre>\r\n");
            sb.Append(Message);

            Util.SendMsg(smtp, From, "sent emails", sb.ToString(), null, From.Address, null);
            Util.SendMsg(smtp, From, "sent emails", sb.ToString(), null, 
                WebConfigurationManager.AppSettings["senderrorsto"], null);

            sb.Length = 0;
            sb.Append("<pre>\r\n");
        }
    }
}

