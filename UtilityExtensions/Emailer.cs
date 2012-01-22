using System;
using System.Text;
using System.Linq;
using System.Net.Mail;
using System.Web.Configuration;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace UtilityExtensions
{
    public static partial class Util
    {
        public static void SendMsg(string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, List<MailAddress> to, int id, int? pid)
        {
            if (WebConfigurationManager.AppSettings["sendemail"] == "false")
                return;
            var error = "";
            for(var n = 0;n < 4; n++)
            {
                try
                {
                    sendmsg(SysFromEmail, CmsHost, From, subject, Message, to, id, pid);
                    return;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(35);
                    error = ex.Message;
                }
            }
            System.Threading.Thread.Sleep(35);
            sendmsg(SysFromEmail, CmsHost, From, "(smtp error) " + subject, "<p>(to: {0})</p><pre>{1}</pre>{2}".Fmt(to[0].Address, error, Message), 
                Util.SendErrorsTo(), id, pid);
        }
        private static void sendmsg(string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, List<MailAddress> to, int id, int? pid)
        {
            var msg = new MailMessage();
            if (From == null)
                From = Util.FirstAddress(WebConfigurationManager.AppSettings["senderrorsto"]);

            msg.From = From;
            if (SysFromEmail.HasValue())
            {
                var sysmail = new MailAddress(SysFromEmail);
                if (From.Host != sysmail.Host)
                    msg.Sender = sysmail;
            }
            msg.Headers.Add("X-bvcms-host", CmsHost);
            msg.Headers.Add("X-bvcms-mail-id", id.ToString());
            if (pid.HasValue)
                msg.Headers.Add("X-bvcms-peopleid", pid.ToString());

            foreach (var ma in to)
            {
                if (ma.Host != "nowhere.name")
                    msg.To.Add(ma);
            }
            msg.Subject = subject;
            var addrs = string.Join(", ", to.Select(tt => tt.ToString()));
            if (msg.To.Count == 0)
            {
                msg.To.Add(msg.From);
                msg.Subject += "-- bad addr for " + pid;
            }

            var regex = new Regex("</?([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var text = regex.Replace(Message, string.Empty);
            var bytes1 = Encoding.UTF8.GetBytes(text);
            var htmlStream1 = new MemoryStream(bytes1);
            var htmlView1 = new AlternateView(htmlStream1, MediaTypeNames.Text.Plain);
            var lines = Regex.Split(text, @"\r?\n|\r");
            if (lines.Any(li => li.Length > 990))
                htmlView1.TransferEncoding = TransferEncoding.QuotedPrintable;
            else
                htmlView1.TransferEncoding = TransferEncoding.SevenBit;
            msg.AlternateViews.Add(htmlView1);

            var html = Message;
            var bytes = Encoding.UTF8.GetBytes(html);
            var htmlStream = new MemoryStream(bytes);
            var htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
            lines = Regex.Split(html, @"\r?\n|\r");
            if (lines.Any(li => li.Length > 990))
                htmlView.TransferEncoding = TransferEncoding.QuotedPrintable;
            else
                htmlView.TransferEncoding = TransferEncoding.SevenBit;
            msg.AlternateViews.Add(htmlView);

            try
            {
                var smtp = Util.Smtp();
                smtp.Send(msg);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                htmlView.Dispose();
                htmlStream.Dispose();
            }
        }
    }
}