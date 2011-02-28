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
using System.Reflection;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Web;
using System.Data.SqlClient;
using System.Web.Caching;

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
            if (!(WebConfigurationManager.AppSettings["sendemail"] != "false" && !String.IsNullOrEmpty(smtp.Host)))
                return;

            bool sesCanSend = SESCanSend(CmsHost, From, subject, Message, Name, addr, id);
            if (sesCanSend)
            {
                SendAmazonSESRawEmail(From, addr, Name, subject, Message, CmsHost, id);
                return;
            }
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
            htmlView.Dispose();
            htmlStream.Dispose();
        }
        public static MemoryStream ConvertMailMessageToMemoryStream(MailMessage message)
        {
            var assembly = typeof(SmtpClient).Assembly;
            var mailWriterType = assembly.GetType("System.Net.Mail.MailWriter");
            var fileStream = new MemoryStream();
            var mailWriterContructor = mailWriterType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(Stream) }, null);
            var mailWriter = mailWriterContructor.Invoke(new object[] { fileStream });
            var sendMethod = typeof(MailMessage).GetMethod("Send", BindingFlags.Instance | BindingFlags.NonPublic);
            sendMethod.Invoke(message, BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { mailWriter, true }, null);
            var closeMethod = mailWriter.GetType().GetMethod("Close", BindingFlags.Instance | BindingFlags.NonPublic);
            closeMethod.Invoke(mailWriter, BindingFlags.Instance | BindingFlags.NonPublic, null, new object[] { }, null);
            return fileStream;
        }
        public static Boolean SendAmazonSESRawEmail(
            MailAddress from, string to, string nameto, string Subject, string body, string host, int id)
        {
            var awsfrom = ConfigurationManager.AppSettings["awsfromemail"];
            var fromname = from.DisplayName;
            if (!fromname.HasValue())
                fromname = from.Address;
            var msg = new MailMessage();
            msg.From = new MailAddress(awsfrom, fromname);
#if DEBUG2
            msg.To.Add(new MailAddress("davcar@pobox.com", nameto));
#else
            var aa = to.SplitStr(",;");
            foreach (var ad in aa)
            {
                if (nameto.HasValue() && nameto.Contains("?"))
                    nameto = null;
                var ma = Util.TryGetMailAddress(ad, nameto);
                if (ma != null)
                    msg.To.Add(ma);
            }
            if (msg.To.Count == 0)
            {
                msg.To.Add(msg.From);
                msg.Subject = "(bad addr:{0}) {1}".Fmt(to, Subject);
            }
#endif
            if (Subject.HasValue())
                msg.Subject = Subject;
            else
                msg.Subject = "no subject";
            msg.ReplyToList.Add(from);
            msg.Headers.Add("X-bvcms-host", host);
            msg.Headers.Add("X-bvcms-mail-id", id.ToString());

            var regex = new Regex("</?([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var text = regex.Replace(body, string.Empty);

            var bytes1 = Encoding.UTF8.GetBytes(text);
            var htmlStream1 = new MemoryStream(bytes1);
            var htmlView1 = new AlternateView(htmlStream1, MediaTypeNames.Text.Plain);
            htmlView1.TransferEncoding = TransferEncoding.QuotedPrintable;
            msg.AlternateViews.Add(htmlView1);

            var bytes = Encoding.UTF8.GetBytes(body);
            var htmlStream = new MemoryStream(bytes);
            var htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
            htmlView.TransferEncoding = TransferEncoding.QuotedPrintable;
            msg.AlternateViews.Add(htmlView);

            var rawMessage = new RawMessage();
            using (var memoryStream = ConvertMailMessageToMemoryStream(msg))
                rawMessage.WithData(memoryStream);

            var request = new SendRawEmailRequest();
            request.WithRawMessage(rawMessage);
#if DEBUG
            var fullto = new MailAddress("davcar@pobox.com", nameto);
            request.WithDestinations(fullto.ToString());
#else
            var toarray = msg.To.Select(tt => tt.ToString()).ToArray();
            request.WithDestinations(toarray);
#endif
            var fullsys = new MailAddress(awsfrom, fromname);
            request.WithSource(fullsys.ToString());

            string[] a = (string[])HttpRuntime.Cache["awscreds"];
            try
            {
                var cfg = new AmazonSimpleEmailServiceConfig();
                cfg.UseSecureStringForAwsSecretKey = false;
                var ses = new AmazonSimpleEmailServiceClient(a[0], a[1], cfg);
                var response = ses.SendRawEmail(request);
                var result = response.SendRawEmailResult;
                return true;
            }
            catch (Exception ex)
            {
                if (!msg.Subject.StartsWith("(sending error)"))
                    return SendAmazonSESRawEmail(from, ConfigurationManager.AppSettings["senderrorsto"], nameto, "(sending error) " + Subject, "<p>(to: {0})</p><pre>{1}</pre>{2}".Fmt(to, ex.Message, body), host, id);
                return false;
            }
        }
        //public static bool UseAmazonSES(string host)
        //{
        //    var o = HttpRuntime.Cache["awscreds"];
        //    if (o == null)
        //        return false;
        //    var cs = ConfigurationManager.ConnectionStrings["ses"];
        //    var cn = new SqlConnection(cs.ConnectionString);
        //    try
        //    {
        //        cn.Open();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    var cmd = new SqlCommand("select max(tm) from ses", cn);
        //    var dt = (DateTime?)cmd.ExecuteScalar();
        //    if (dt.HasValue)
        //    {
        //        if (HttpRuntime.Cache["ses_speed"] == null)
        //        {
        //            cmd = new SqlCommand("select top 1 speed, max24 from ses_setting", cn);
        //            using (var dr = cmd.ExecuteReader())
        //            {
        //                dr.Read();
        //                HttpRuntime.Cache.Insert("ses_speed", dr.GetInt32(0), null,
        //                    DateTime.Now.AddSeconds(60), Cache.NoSlidingExpiration);
        //                HttpRuntime.Cache.Insert("ses_max24", dr.GetInt32(1), null,
        //                    DateTime.Now.AddSeconds(60), Cache.NoSlidingExpiration);
        //            }
        //        }

        //        var speed = (int)HttpRuntime.Cache["ses_speed"];
        //        var max24 = (int)HttpRuntime.Cache["ses_max24"];

        //        try
        //        {
        //            cmd = new SqlCommand("select count(*) from dbo.ses where tm > @time", cn);
        //            cmd.Parameters.AddWithValue("@time", dt.Value.AddHours(-22));
        //            var cnt = (int)cmd.ExecuteScalar();
        //            if (cnt > (max24 * 90 / 100))
        //                return false;
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        var elapsed = DateTime.Now.Subtract(dt.Value).TotalMilliseconds.ToInt();
        //        if (elapsed < speed)
        //        {
        //            var sleeptime = (speed - elapsed);
        //            System.Threading.Thread.Sleep(sleeptime);
        //        }
        //    }
        //    cmd = new SqlCommand("insert dbo.ses (host) values(@host)", cn);
        //    cmd.Parameters.AddWithValue("@host", host);
        //    cmd.ExecuteNonQuery();

        //    cn.Close();
        //    return true;
        //}
        public static Boolean SESCanSend(string CmsHost, MailAddress From, string subject, string Message, string Name, string addr, int id)
        {
            var cansend = false;
            var sendrate = 0D;

            var o = HttpRuntime.Cache["awscreds"];
            if (o != null)
            {
                string[] a = (string[])o;

                var cfg = new AmazonSimpleEmailServiceConfig();
                cfg.UseSecureStringForAwsSecretKey = false;
                var ses = new AmazonSimpleEmailServiceClient(a[0], a[1], cfg);
                var req = new GetSendQuotaRequest();
                var resp = ses.GetSendQuota(req);
                sendrate = resp.GetSendQuotaResult.MaxSendRate.ToInt();
                if (resp.GetSendQuotaResult.SentLast24Hours < (resp.GetSendQuotaResult.Max24HourSend * 90 / 100))
                    cansend = true;
            }

            DateTime dt;
            var sescn = WebConfigurationManager.ConnectionStrings["ses"];
            if (sescn != null)
            {
                using (var cn = new SqlConnection(sescn.ConnectionString))
                {
                    cn.Open();
                    var cmd = new SqlCommand("insert dbo.ses (host, fromemail, name, toemail, subject, message, qid, useSES) values(@host,@fromemail,@name,@toemail,@subject,@message,@qid,@useSES)", cn);
                    cmd.Parameters.AddWithValue("@host", CmsHost);
                    cmd.Parameters.AddWithValue("@fromemail", From.ToString());
                    cmd.Parameters.AddWithValue("@name", Name ?? "");
                    cmd.Parameters.AddWithValue("@toemail", addr);
                    cmd.Parameters.AddWithValue("@subject", subject ?? "");
                    cmd.Parameters.AddWithValue("@message", Message);
                    cmd.Parameters.AddWithValue("@qid", id);
                    cmd.Parameters.AddWithValue("@useSES", cansend);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        throw;
                    }
                    if (cansend)
                    {
                        cmd = new SqlCommand("select MAX(tm) from dbo.ses", cn);
                        var ms = 1000 / sendrate;
                        dt = (DateTime)cmd.ExecuteScalar();
                        var elapsed = DateTime.Now.Subtract(dt).TotalMilliseconds;
                        if (elapsed < ms)
                        {
                            var sleeptime = (ms - elapsed);
                            System.Threading.Thread.Sleep(sleeptime.ToInt());
                        }
                    }
                }
            }
            return cansend;
        }
    }
}
