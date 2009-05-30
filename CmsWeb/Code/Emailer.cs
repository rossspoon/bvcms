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
                    _From = new MailAddress(DbUtil.SystemEmailAddress);
                return _From;
            }
            set
            {
                _From = value;
            }
        }

        private MailAddress ReplyTo;
        private List<MailAddress> Addresses = new List<MailAddress>();
        private IEnumerable<Person> people;

        public Emailer(string fromaddr, string fromname)
        {
            ReplyTo = new MailAddress(fromaddr, fromname);
            From = new MailAddress(DbUtil.SystemEmailAddress, fromname);
            if (From.Host == ReplyTo.Host)
            {
                From = ReplyTo;
                ReplyTo = null;
            }
        }
        public Emailer(string fromaddr)
        {
            ReplyTo = new MailAddress(fromaddr);
            From = new MailAddress(DbUtil.SystemEmailAddress);
            if (From.Host == ReplyTo.Host)
            {
                From = ReplyTo;
                ReplyTo = null;
            }
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
                msg.ReplyTo = ReplyTo;
                msg.Subject = Subject;
                msg.Body = Message;
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
        public void SendPeopleEmail(IEnumerable<Person> people, string subject, string message, FileUpload attach)
        {
            Subject = subject;
            Message = message;
            this.people = people;

            Attachment a = null;

            if (attach.FileName.HasValue())
                a = new Attachment(attach.FileContent, attach.FileName);

            SmtpClient smtp = null;
            var i = 0;
            foreach (var p in people)
            {
                if (!p.EmailAddress.HasValue())
                    continue;
                try
                {
                    var to = new MailAddress(p.EmailAddress, p.Name);
                    var msg = new MailMessage(From, to);
                    msg.ReplyTo = ReplyTo;
                    msg.Subject = Subject;
                    var b = Message.Replace("{name}", p.Name);
                    b = b.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
                    b = b.Replace("{firstname}", p.NickName.HasValue() ? p.NickName : p.FirstName);
                    msg.Body = b;
                    if (a != null)
                        msg.Attachments.Add(a);

                    msg.IsBodyHtml = true;
                    if (i % 20 == 0)
                        smtp = new SmtpClient();
                    i++;
                    smtp.Send(msg);
                }
                catch (Exception ex)
                {
                    var f = new MailAddress(DbUtil.SystemEmailAddress);
                    var msg = new MailMessage(f, From);
                    msg.Subject = ex.Message;
                    msg.Body = "Addressed to: \"" + p.EmailAddress + "\"<br/>" + "Name: " + p.Name + "<br/><br/>" + Message.Replace("{name}", p.Name).Replace("{firstname}", p.NickName ?? p.FirstName);
                    msg.IsBodyHtml = true;
                    if (i % 20 == 0)
                        smtp = new SmtpClient();
                    i++;
                    smtp.Send(msg);
                }
            }
        }
        public void SendPersonEmail(Person p, string subject, string message)
        {
            Subject = subject;
            Message = message;

            var smtp = new SmtpClient();
            try
            {
                var to = new MailAddress(p.EmailAddress, p.Name);
                var msg = new MailMessage(From, to);
                msg.ReplyTo = ReplyTo;
                msg.Subject = Subject;
                var b = Message.Replace("{name}", p.Name);
                b = b.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
                b = b.Replace("{firstname}", p.NickName.HasValue() ? p.NickName : p.FirstName);
                msg.Body = b;
                msg.IsBodyHtml = true;
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                var f = new MailAddress(DbUtil.SystemEmailAddress);
                var msg = new MailMessage(f, From);
                msg.Subject = ex.Message;
                msg.Body = "Addressed to: \"" + p.EmailAddress + "\"<br/>" + "Name: " + p.Name + "<br/><br/>" + Message.Replace("{name}", p.Name).Replace("{firstname}", p.NickName ?? p.FirstName);
                msg.IsBodyHtml = true;
                smtp.Send(msg);
            }
        }
        public void EmailNotification(Person from, Person to, string subject, string message)
        {
            try
            {
                ReplyTo = new MailAddress(from.Users.First().EmailAddress, from.Name);
                From = new MailAddress(DbUtil.SystemEmailAddress, from.Name);
                Addresses.Clear();
                var To = LoadAddress(to.Users.First().EmailAddress, to.Name);
                if (To == null)
                    return;
                if (From.Host == ReplyTo.Host || From.Host == To.Host)
                {
                    From = ReplyTo;
                    ReplyTo = null;
                }
                NotifyEmail(subject, message);
            }
            catch
            {
                //var s = "failure sending email to {0}".Fmt(to.Name);
                //throw new Exception(s);
            }
        }
    }
}

