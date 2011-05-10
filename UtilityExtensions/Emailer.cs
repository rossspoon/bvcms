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
        public static void SendMsg(string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, MailAddress to, int id)
        {
            SendMsg(SysFromEmail, CmsHost, From, subject, Message, Util.ToMailAddressList(to), id, Record: true);
        }
        public static void SendMsg(string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, List<MailAddress> to, int id, bool Record)
        {
            if (WebConfigurationManager.AppSettings["sendemail"] == "false")
                return;

            if (Record)
                RecordEmailSent(CmsHost, From, subject, to, id, false);
            var msg = new MailMessage();
            if (From == null)
                From = Util.FirstAddress(WebConfigurationManager.AppSettings["senderrorsto"]);
            msg.From = From;
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
                msg.Subject += "-- bad addr:" + addrs;
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

            if (SysFromEmail.HasValue())
            {
                var sysmail = new MailAddress(SysFromEmail);
                if (From.Host != sysmail.Host)
                    msg.Sender = sysmail;
            }
            msg.Headers.Add("bvcms-host", CmsHost);
            msg.Headers.Add("bvcms-mail-id", id.ToString());

            try
            {
                var smtp = Util.Smtp();
                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                if (!msg.Subject.StartsWith("(smtp error)"))
                    SendMsg(SysFromEmail, CmsHost, From, 
                        "(smtp error) " + subject, 
                        "<p>(to: {0})</p><pre>{1}</pre>{2}".Fmt(addrs, ex.Message, Message), 
                        Util.SendErrorsTo(), id, Record:true);
            }
            htmlView.Dispose();
            htmlStream.Dispose();
        }
        public static void RecordEmailSent(string CmsHost, MailAddress From, string subject, List<MailAddress> to, int id, bool cansend)
        {
            var sescn = WebConfigurationManager.ConnectionStrings["ses"];
            if (sescn != null)
            {
                using (var cn = new SqlConnection(sescn.ConnectionString))
                {
                    try { cn.Open(); }
                    catch (Exception) { return; }
                    var cmd = new SqlCommand("insert dbo.ses (host, fromemail, toemail, subject, qid, useSES) values(@host,@fromemail,@toemail,@subject,@qid,@useSES)", cn);
                    cmd.Parameters.AddWithValue("@host", CmsHost);
                    cmd.Parameters.AddWithValue("@fromemail", From.ToString());
                    cmd.Parameters.AddWithValue("@toemail", to.EmailAddressListToString());
                    cmd.Parameters.AddWithValue("@subject", subject ?? "");
                    cmd.Parameters.AddWithValue("@qid", id);
                    cmd.Parameters.AddWithValue("@useSES", cansend);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception )
                    {

                    }

                }
            }
        }
    }
}
