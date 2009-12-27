using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web.Configuration;

namespace UtilityExtensions
{
    public static partial class Util
    {
        public static void Email(string from, string name, string addr, string subject, string message)
        {
            var smtp = new SmtpClient();
            Email(smtp, from, name, addr, subject, message);
        }
        public static void Email(SmtpClient smtp, string from, string name, string addr, string subject, string message)
        {
            var fr = FirstAddress(from);
            var ma = Util.TryGetMailAddress(addr, name);
            if (ma == null)
                return;
            var msg = new MailMessage(fr, ma);
            //msg.From = new MailAddress("");
            //msg.Sender = new MailAddress("");
            msg.Subject = subject;
            msg.Body = "<html>\r\n" + message + "\r\n</html>\r\n";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            TrySend(smtp, msg);
        }
        public static void Email2(SmtpClient smtp, string from, string addrs, string subject, string message)
        {
            var fr = FirstAddress(from);
            var msg = new MailMessage();
            msg.From = fr;
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            msg.To.Add(addrs);
            msg.Subject = subject;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = false;
            msg.Body = message;
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            TrySend(smtp, msg);
        }
        public static void EmailHtml2(SmtpClient smtp, string from, string addrs, string subject, string message)
        {
            var fr = FirstAddress(from);
            var msg = new MailMessage();
            msg.From = fr;
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            msg.To.Add(addrs);
            msg.Subject = subject;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Body = "<html>\r\n" + message + "\r\n</html>\r\n";
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            TrySend(smtp, msg);
        }
        public static MailAddress FirstAddress(string addrs)
        {
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            var a = addrs.SplitStr(",");
            return new MailAddress(a[0]);
        }
        public static void Email3(SmtpClient smtp, string from, string name, string addr, string subject, string message)
        {
            var fr = FirstAddress(from);
            var ma = Util.TryGetMailAddress(addr, name);
            if (ma == null)
                return;
            var msg = new MailMessage(fr, ma);
            msg.From = fr;
            msg.Sender = new MailAddress("bbcms01@bellevue.org");
            msg.Subject = subject;
            msg.Body = "<html>\r\n" + message + "\r\n</html>\r\n";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            TrySend(smtp, msg);
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
    }
}
