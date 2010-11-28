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

namespace CmsData
{
    public static class Emailer
    {
        public static void SendPeopleEmail(CMSDataContext Db, string CmsHost, EmailQueue emailqueue)
        {
            var From = Util.FirstAddress(emailqueue.FromAddr, emailqueue.FromName);
            var Message = emailqueue.Body;
            emailqueue.Started = DateTime.Now;
            Db.SubmitChanges();

            var sb = new StringBuilder("<pre>\r\n");
            SmtpClient smtp = null;
            var i = 0;

            var q = from To in Db.EmailQueueTos
                    where To.Id == emailqueue.Id
                    where To.Sent == null
                    orderby To.PeopleId
                    select To;
            foreach (var To in q)
            {
                var qp = (from p in Db.People
                         where p.PeopleId == To.PeopleId
                         select new { p.Name, p.PreferredName, p.EmailAddress }).Single();
                var text = emailqueue.Body.Replace("{name}", qp.Name);
                text = text.Replace("{first}", qp.PreferredName);
                var aa = qp.EmailAddress.SplitStr(",;").ToList();
                if (To.OrgId.HasValue)
                {
                    var qm = (from m in Db.OrganizationMembers
                              where m.PeopleId == To.PeopleId && m.OrganizationId == To.OrgId
                              select new { m.PayLink, m.Amount, m.AmountPaid, m.RegisterEmail }).Single();
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
                        if (i % 20 == 0)
                            smtp = Util.Smtp();
                        i++;

                        text = text.Replace("{unsubscribe}",
                            "<a href=\"{0}OptOut/UnSubscribe/?enc={1}\">Unsubscribe</a>"
                            .Fmt(CmsHost,  Util.EncryptForUrl("{0}|{1}".Fmt(To.PeopleId, From.Address))));

                        Util.SendMsg(smtp, CmsHost, From, emailqueue.Subject, text, qp.Name, ad, emailqueue.Id);
                        if (smtp.PickupDirectoryLocation.HasValue())
                            Thread.Sleep(50); // simulate sending
                        To.Sent = DateTime.Now;

                        sb.AppendFormat("\"{0}\" [{1}] ({2})\r\n".Fmt(qp.Name, ad, To.PeopleId));
                        if (i % 500 == 0)
                            NotifySentEmails(Db, CmsHost, sb, smtp, From, emailqueue.Subject, emailqueue.Body, emailqueue.Id);
                        Db.SubmitChanges();
                    }
                }
            }
            if (smtp != null)
                NotifySentEmails(Db, CmsHost, sb, smtp, From, emailqueue.Subject, emailqueue.Body, emailqueue.Id);
            emailqueue.Sent = DateTime.Now;
            Db.SubmitChanges();
        }
        public static void SendPeopleEmail(CMSDataContext Db, string CmsHost, MailAddress From, IEnumerable<Person> q, string Subject, string Message)
        {
            var emailqueue = new EmailQueue
            {
                Queued = DateTime.Now,
                FromAddr = From.Address,
                FromName = From.DisplayName,
                Subject = Subject,
                Body = Message,
                SendWhen = null,
                QueuedBy = Util.UserPeopleId,
            };
            Db.EmailQueues.InsertOnSubmit(emailqueue);
            Db.SubmitChanges();

            foreach (var p in q)
            {
                if (!Util.ValidEmail(p.EmailAddress))
                    continue;
                var to = new EmailQueueTo { Id = emailqueue.Id, PeopleId = p.PeopleId, OrgId = Db.CurrentOrgId };
                Db.EmailQueueTos.InsertOnSubmit(to);
            }
            Db.SubmitChanges();
            SendPeopleEmail(Db, CmsHost, emailqueue);
        }
        private static void NotifySentEmails(CMSDataContext Db, string CmsHost, StringBuilder sb, SmtpClient smtp, MailAddress From, string subject, string body, int id)
        {
            sb.Append("</pre>\r\n<h2>{0}</h2>".Fmt(subject));
            sb.Append(body);
            Util.SendMsg(smtp, CmsHost, From, "sent emails", sb.ToString(), null, From.Address, id);
            Util.SendMsg(smtp, CmsHost, From, "sent emails", sb.ToString(), null, ConfigurationManager.AppSettings["senderrorsto"], id);
            sb.Length = 0;
            sb.Append("<pre>\r\n");
        }
    }
}

