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
        public static void Email2(string from, string addr, string subject, string message)
        {
            var smtp = new SmtpClient();
            Email2(smtp, from, addr, subject, message);
        }
        public static void Email(SmtpClient smtp, string from, string name, string addr, string subject, string message)
        {
            var fr = FirstAddress(from);
            var ma = Util.TryGetMailAddress(addr, name);
            if (ma == null)
                return;
            var msg = new MailMessage(fr, ma);
            msg.Subject = subject;
            msg.Body = "<html><body>\n" + message + "\n</body></html>\n";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            smtp.Send(msg);
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
            msg.Body = "<html><body>\n" + message + "\n</body></html>\n";
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            var InDebug = false;
#if DEBUG
            InDebug = true;
#endif
            if (InDebug)
                return;
            smtp.Send(msg);
        }
        public static MailAddress FirstAddress(string addrs)
        {
            if (!addrs.HasValue())
                addrs = WebConfigurationManager.AppSettings["senderrorsto"];
            var a = addrs.SplitStr(",");
            return new MailAddress(a[0]);
        }
    }
}
