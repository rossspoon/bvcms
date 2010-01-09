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

        private List<MailAddress> Addresses = new List<MailAddress>();
        private IEnumerable<Person> people;

        public Emailer(string fromaddr, string fromname)
        {
            //ReplyTo = new MailAddress(fromaddr, fromname);
            From = new MailAddress(fromaddr, fromname);
            //if (From.Host == ReplyTo.Host)
            //    From = ReplyTo;
        }
        public Emailer(string fromaddr)
        {
            //ReplyTo = new MailAddress(fromaddr);
            From = new MailAddress(fromaddr);
            //if (From.Host == ReplyTo.Host)
            //    From = ReplyTo;
        }
        public Emailer()
        {
        }
        public void LoadAddresses(IEnumerable<Person> q)
        {
            foreach (var p in q)
                Addresses.Add(new MailAddress(p.EmailAddress, p.Name));
        }
        public MailAddress LoadAddress(string Address, string Name)
        {
            if (Addresses.SingleOrDefault(m => m.Address == Address) == null)
            {
                var ma = new MailAddress(Address, Name);
                Addresses.Add(ma);
                return ma;
            }
            return null;
        }

        public void NotifyEmail(string subject, string message)
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
                msg.Body = "<html>\r\n" + Message + "\r\n</html>";
                msg.IsBodyHtml = true;
                if (i % 20 == 0)
                    smtp = new SmtpClient();
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
            Subject = subject;
            Message = message;
            this.people = people;

            Attachment a = null;

            if (attach.FileName.HasValue())
                a = new Attachment(attach.FileContent, attach.FileName);

            var sb = new StringBuilder();
            SmtpClient smtp = null;
            var i = 0;

            string bhtml = null;
            if (IsHtml)
                bhtml = message;
            else
                bhtml = Util.SafeFormat(Message);
            int EmailBatchCount = DbUtil.Settings("EmailBatchCount", "500").ToInt();

            foreach (var p in people)
            {
                var em = p.EmailAddress.Trim();
                if (!p.EmailAddress.HasValue())
                    continue;
                if (Util.ValidEmail(em))
                {
                    var to = new MailAddress(em, p.Name);
                    var msg = new MailMessage(From, to);
                    string sysfromemail = WebConfigurationManager.AppSettings["sysfromemail"];
                    if (sysfromemail.HasValue())
                    {
                        var sysmail = new MailAddress(sysfromemail);
                        if (From.Host != sysmail.Host)
                            msg.Sender = sysmail;
                    }
                    msg.Subject = Subject;

                    var text = Message;
                    text = text.Replace("{name}", p.Name);
                    text = text.Replace("{first}", p.PreferredName);
                    text = text.Replace("{firstname}", p.PreferredName);
                    msg.Body = text;

                    var html = bhtml;
                    html = html.Replace("{name}", p.Name);
                    html = html.Replace("{first}", p.PreferredName);
                    html = html.Replace("{firstname}", p.PreferredName);

                    var bytes = Encoding.UTF8.GetBytes(html);
                    var htmlStream = new MemoryStream(bytes);
                    var htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
                    htmlView.TransferEncoding = TransferEncoding.SevenBit;
                    msg.AlternateViews.Add(htmlView);

                    if (a != null)
                        msg.Attachments.Add(a);

                    if (i % 20 == 0)
                        smtp = new SmtpClient();
                    i++;
                    smtp.Send(msg);
                    //System.Threading.Thread.Sleep(100);
                    htmlView.Dispose();
                    htmlStream.Dispose();
                    sb.AppendFormat("\"{0}\" <{1}> ({2})\r\n".Fmt(p.Name, em, p.PeopleId));
                    if (i % EmailBatchCount == 0)
                        NotifySentEmails(sb, smtp);
                }
                else
                {
                    var msg = new MailMessage(From, From);
                    msg.Subject = "not a valid email address";
                    string sysfromemail = WebConfigurationManager.AppSettings["sysfromemail"];
                    if (sysfromemail.HasValue())
                    {
                        var sysmail = new MailAddress(sysfromemail);
                        if (From.Host != sysmail.Host)
                            msg.Sender = sysmail;
                    }
                    msg.Body = "Addressed to: " + em + "\r\n"
                        + "Name: " + p.Name + "\r\n\r\n"
                        + Message.Replace("{name}", p.Name).Replace("{first}", p.PreferredName);
                    msg.IsBodyHtml = false;
                    if (i % 20 == 0)
                        smtp = new SmtpClient();
                    i++;
                    smtp.Send(msg);
                }
            }
            NotifySentEmails(sb, smtp);
        }
        public void EmailNotification(Person from, Person to, string subject, string message)
        {
            try
            {
                From = new MailAddress(from.EmailAddress, from.Name);
                Addresses.Clear();
                var To = LoadAddress(to.EmailAddress, to.Name);
                if (To == null)
                    return;
                NotifyEmail(subject, message);
            }
            catch
            {
                //var s = "failure sending email to {0}".Fmt(to.Name);
                //throw new Exception(s);
            }
        }
        private void NotifySentEmails(StringBuilder sb, SmtpClient smtp)
        {
            sb.Append("\r\n");
            sb.Append(Message);
            smtp = new SmtpClient();

            var msg = new MailMessage(From, From);
            string sysfromemail = WebConfigurationManager.AppSettings["sysfromemail"];
            if (sysfromemail.HasValue())
            {
                var sysmail = new MailAddress(sysfromemail);
                if (From.Host != sysmail.Host)
                    msg.Sender = sysmail;
            }
            msg.Subject = "sent emails";
            msg.Body = sb.ToString();
            msg.IsBodyHtml = false;
            smtp.Send(msg);

            msg = new MailMessage();
            msg.From = From;
            if (sysfromemail.HasValue())
            {
                var sysmail = new MailAddress(sysfromemail);
                if (From.Host != sysmail.Host)
                    msg.Sender = sysmail;
            }
            msg.To.Add(WebConfigurationManager.AppSettings["senderrorsto"]);
            msg.Subject = "sent emails";
            msg.Body = sb.ToString();
            msg.IsBodyHtml = false;
            smtp.Send(msg);

            sb.Length = 0;
        }
    }
}

