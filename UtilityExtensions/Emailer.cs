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
        public static void Email(string from, string name, string addrs, string subject, string message)
        {
            Email(new SmtpClient(), from, name, addrs, subject, message);
        }
        public static void Email(SmtpClient smtp, string from, string name, string addrs, string subject, string message)
        {
            var fr = FirstAddress(from);
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            SendMsg(smtp, fr, subject, message, name, addrs, null);
        }
        public static void EmailAlways(SmtpClient smtp, string from, string name, string addrs, string subject, string message)
        {
            var fr = FirstAddress(from);
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            SendMsg(smtp, fr, subject, message, name, addrs, null);
        }
        public static void Email2(SmtpClient smtp, string from, string addrs, string subject, string message)
        {
            Email(smtp, from, null, addrs, subject, message);
        }
        public static MailAddress FirstAddress(string addrs)
        {
            return FirstAddress(addrs, null);
        }
        public static MailAddress FirstAddress(string addrs, string name)
        {
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            var a = addrs.SplitStr(",;");
            try
            {
                return new MailAddress(a[0], name);
            }
            catch (Exception)
            {
                return null;
            }
        }
        private static void TrySend(SmtpClient smtp, MailMessage msg)
        {
            try
            {
                smtp.Send(msg);
            }
            catch (System.Net.Mail.SmtpException)
            {
                // try one more time
                System.Threading.Thread.Sleep(2000);
                smtp.Send(msg);
            }
        }
        public static void SendMsg(SmtpClient smtp, MailAddress From, string subject, string Message, string Name, string addr, Attachment attach)
        {
            var msg = new MailMessage();
            msg.From = From;
            var aa = addr.SplitStr(",;");
            foreach (var ad in aa)
            {
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

            string sysfromemail = WebConfigurationManager.AppSettings["sysfromemail"];
            if (sysfromemail.HasValue())
            {
                var sysmail = new MailAddress(sysfromemail);
                if (From.Host != sysmail.Host)
                    msg.Sender = sysmail;
            }
            if (attach != null)
                msg.Attachments.Add(attach);

#if DEBUG
#else
            try
            {
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                if (!msg.Subject.StartsWith("(smtp error)"))
                    SendMsg(smtp, From, 
                        "(smtp error) " + subject, 
                        "<p>(to: {0})</p><pre>{1}</pre>{2}".Fmt(addr, ex.Message, Message),
                        Name,
                        WebConfigurationManager.AppSettings["senderrorsto"], 
                        attach);
            }
#endif
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
                Util.Email(smtp, staff, name, emailonrecord, subject, message);

            Util.Email2(smtp, emailonrecord, staff, "different email address than one on record",
                "<p>{0}({1}) registered  with '{2}' but has '{3}' in record.</p>".Fmt(
                name, peopleid, to, emailonrecord));
        }
    }
}
