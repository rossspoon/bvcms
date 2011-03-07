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
    public class WebEmailer : ITaskNotify
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

        public WebEmailer(string fromaddr, string fromname)
        {
            From = Util.FirstAddress(fromaddr, fromname);
        }
        public WebEmailer(string fromaddr)
        {
            From = Util.FirstAddress(fromaddr);
        }
        public WebEmailer()
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
                string sysfromemail = Util.SysFromEmail;
                if (sysfromemail.HasValue())
                {
                    var sysmail = new MailAddress(sysfromemail);
                    if (From.Host != sysmail.Host)
                        msg.Sender = sysmail;
                }
                msg.Subject = Subject;
                msg.Body = Message;
                msg.IsBodyHtml = true;
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
        public void EmailNotification(Person from, Person to, string subject, string message)
        {
            From = Util.FirstAddress(from.EmailAddress, from.Name);
            Addresses.Clear();
            LoadAddress(to.EmailAddress, to.Name);
            if (Addresses.Count > 0)
                NotifyEmail(subject, message);
        }
    }
}

