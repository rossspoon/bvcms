using System;
using System.Text;
using System.Linq;
using System.Net.Mail;
using System.Web.Configuration;
using System.IO;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace UtilityExtensions
{
    public static partial class Util
    {
        //public static void Email(string from, string name, string addrs, string subject, string message)
        //{
        //    var fr = FirstAddress(from);
        //    if (fr == null)
        //        fr = FirstAddress(WebConfigurationManager.AppSettings["senderrorsto"]);
        //    if (!addrs.HasValue())
        //    {
        //        addrs = fr.Address + "," + WebConfigurationManager.AppSettings["senderrorsto"];
        //        message = "<p style=\"color:red\">you are receiving this email because the recipient had no email</p>" + message;
        //    }
        //    SendMsg(Util.SysFromEmail, Util.CmsHost, fr, subject, message, name, addrs);
        //}
        //public static void SendMsg(string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, string Name, string addr)
        //{
        //    SendMsg(SysFromEmail, CmsHost, From, subject, Message, Name, addr, 0);
        //}
        public static void SendMsg(string SysFromEmail, string CmsHost, MailAddress From, string subject, string Message, string Name, string addr, int id)
        {
            if (WebConfigurationManager.AppSettings["sendemail"] == "false")
                return;

            RecordEmailSent(CmsHost, From, subject, Name, addr, id, false);
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
                if (ma != null && !ma.Address.ToLower().Contains("nowhere.name"))
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
                    SendMsg(SysFromEmail, CmsHost, From, "(smtp error) " + subject, "<p>(to: {0})</p><pre>{1}</pre>{2}".Fmt(addr, ex.Message, Message), Name, WebConfigurationManager.AppSettings["senderrorsto"], id);
            }
            htmlView.Dispose();
            htmlStream.Dispose();
        }
        public static void RecordEmailSent(string CmsHost, MailAddress From, string subject, string Name, string addr, int id, bool cansend)
        {
            var sescn = WebConfigurationManager.ConnectionStrings["ses"];
            if (sescn != null)
            {
                using (var cn = new SqlConnection(sescn.ConnectionString))
                {
                    cn.Open();
                    var cmd = new SqlCommand("insert dbo.ses (host, fromemail, name, toemail, subject, qid, useSES) values(@host,@fromemail,@name,@toemail,@subject,@qid,@useSES)", cn);
                    cmd.Parameters.AddWithValue("@host", CmsHost);
                    cmd.Parameters.AddWithValue("@fromemail", From.ToString());
                    cmd.Parameters.AddWithValue("@name", Name ?? "");
                    cmd.Parameters.AddWithValue("@toemail", addr);
                    cmd.Parameters.AddWithValue("@subject", subject ?? "");
                    cmd.Parameters.AddWithValue("@qid", id);
                    cmd.Parameters.AddWithValue("@useSES", cansend);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }
    }
}
