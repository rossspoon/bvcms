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
            sendmsg(SysFromEmail, CmsHost, From, subject, Message, to, id, pid);
            return;
        }
    	public static void sendmsg(string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, List<MailAddress> to, int id, int? pid, List<LinkedResource> attachments = null)
        {
			var senderrorsto = WebConfigurationManager.AppSettings["senderrorsto"];
            var msg = new MailMessage();
            if (From == null)
                From = Util.FirstAddress(senderrorsto);

            msg.From = From;
            if (SysFromEmail.HasValue())
            {
                var sysmail = new MailAddress(SysFromEmail);
                if (From.Host != sysmail.Host)
                    msg.Sender = sysmail;
            }
			msg.Headers.Add("X-SMTPAPI", 
				"{{\"unique_args\":{{\"host\":\"{0}\",\"mailid\":\"{1}\",\"pid\":\"{2}\"}}}}"
				.Fmt(CmsHost, id, pid));
			msg.Headers.Add("X-BVCMS", "host:{0}, mailid:{1}, pid:{2}".Fmt(CmsHost, id, pid));

            foreach (var ma in to)
            {
                if (ma.Host != "nowhere.name")
                    msg.To.Add(ma);
            }
            msg.Subject = subject;
            var addrs = string.Join(", ", to.Select(tt => tt.ToString()));
			var BadEmailLink = "";
            if (msg.To.Count == 0)
            {
                msg.To.Add(msg.From);
                msg.To.Add(Util.FirstAddress(senderrorsto));
                msg.Subject += "-- bad addr for {0}({1})".Fmt(CmsHost, pid);
				BadEmailLink = "<p><a href='{0}Person/Index/{1}'>bad addr for</a></p>\n".Fmt(CmsHost, pid);
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

            var html = BadEmailLink + Message;
            var bytes = Encoding.UTF8.GetBytes(html);
            var htmlStream = new MemoryStream(bytes);
            var htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
            lines = Regex.Split(html, @"\r?\n|\r");
            if (lines.Any(li => li.Length > 990))
                htmlView.TransferEncoding = TransferEncoding.QuotedPrintable;
            else
                htmlView.TransferEncoding = TransferEncoding.SevenBit;
			if (attachments != null)
				foreach(var a in attachments)
					htmlView.LinkedResources.Add(a);
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