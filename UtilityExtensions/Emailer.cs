using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Configuration;

namespace UtilityExtensions
{
    public static partial class Util
    {
        public static void Email(SmtpClient smtp, string from, string name, string addrs, string subject, string message)
        {
            var fr = FirstAddress(from);
            if (fr == null)
                fr = FirstAddress(WebConfigurationManager.AppSettings["senderrorsto"]);
            if (!addrs.HasValue())
            {
                addrs = fr.Address + "," + WebConfigurationManager.AppSettings["senderrorsto"];
                message = "<p style=\"color:red\">you are receiving this email because the recipient had no email</p>" + message;
            }
            SendMsg(smtp, Util.SysFromEmail, Util.CmsHost, fr, subject, message, name, addrs);
        }
        public static void Email(SmtpClient smtp, string from, string addrs, string subject, string message)
        {
            Email(smtp, from, null, addrs, subject, message);
        }
        public static void SendMsg(SmtpClient smtp, string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, string Name, string addr)
        {
            SendMsg(smtp, SysFromEmail, CmsHost, From, subject, Message, Name, addr, 0);
        }
        public static void SendMsg(SmtpClient smtp, string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, string Name, string addr, int id)
        {
            var msg = new MailMessage();
            if (From == null)
                From = Util.FirstAddress(WebConfigurationManager.AppSettings["senderrorsto"]);
            msg.From = From;
            var aa = addr.SplitStr(",;");
            foreach (var ad in aa)
            {
                if (Name.HasValue() && Name.Contains("?"))
                    Name = null;
                var ma = Util.TryGetMailAddress(ad, Name);
                if (ma != null)
                    msg.To.Add(ma);
            }
            msg.Subject = subject;
            if (msg.To.Count == 0)
            {
                msg.To.Add(msg.From);
                msg.Subject = "(bad addr:{0}) {1}".Fmt(addr, subject);
            }

            var regex = new Regex("</?([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var text = regex.Replace(Message, string.Empty);
            var bytes1 = Encoding.UTF8.GetBytes(text);
            var htmlStream1 = new MemoryStream(bytes1);
            var htmlView1 = new AlternateView(htmlStream1, MediaTypeNames.Text.Plain);
            htmlView1.TransferEncoding = TransferEncoding.QuotedPrintable;
            msg.AlternateViews.Add(htmlView1);

            var html = Message;
            var bytes = Encoding.UTF8.GetBytes(html);
            var htmlStream = new MemoryStream(bytes);
            var htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
            htmlView.TransferEncoding = TransferEncoding.QuotedPrintable;
            msg.AlternateViews.Add(htmlView);

            if (SysFromEmail.HasValue())
            {
                var sysmail = new MailAddress(SysFromEmail);
                if (From.Host != sysmail.Host)
                    msg.Sender = sysmail;
            }
            msg.Headers.Add("bvcms-host", CmsHost);
            msg.Headers.Add("bvcms-mail-id", id.ToString());

            if (WebConfigurationManager.AppSettings["sendemail"] != "false" && smtp.Host != null)
            {
                try
                {
                    smtp.Send(msg);
                }
                catch (Exception ex)
                {
                    if (!msg.Subject.StartsWith("(smtp error)"))
                        SendMsg(smtp, SysFromEmail, CmsHost, From,
                            "(smtp error) " + subject,
                            "<p>(to: {0})</p><pre>{1}</pre>{2}".Fmt(addr, ex.Message, Message),
                            Name,
                            WebConfigurationManager.AppSettings["senderrorsto"]);
                }
            }
            htmlView.Dispose();
            htmlStream.Dispose();
        }
        public static void SendIfEmailDifferent(
            SmtpClient smtp, string staff, string to,
            int peopleid, string name, string emailonrecord,
            string subject, string message)
        {
            if (string.Compare(to, emailonrecord, true) == 0)
                return;

            if (emailonrecord.HasValue())
                Email(smtp, staff, name, emailonrecord, subject, message);

            Email(smtp, emailonrecord, staff, "different email address than one on record",
                "<p>{0}({1}) registered  with '{2}' but has '{3}' in record.</p>".Fmt(
                name, peopleid, to, emailonrecord));
        }
    }
}
