using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using UtilityExtensions;
using CmsData;
using System.Threading;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web;
using System.Text.RegularExpressions;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using System.Web.Caching;
using System.IO;
using System.Net.Mime;
using System.Reflection;

namespace MassEmailer
{
    public partial class MassEmailer : ServiceBase
    {

        private bool serviceStarted = false;
        private Thread listener;
        private string ConnStr;
        private int SleepTime;
        DateTime lastSES;
        public DateTime lastRun { get; set; }
        public DateTime lastThrottle { get; set; }
        public int sentSinceQuotaCheck { get; set; }

        public MassEmailer()
        {
            InitializeComponent();

            var sSource = "MassEmailer2";
            var sLog = "Application";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            eventLog1.Source = "MassEmailer2";

            ConnStr = ConfigurationManager.ConnectionStrings["CMSEmailQueue"].ConnectionString;

            string awscreds = Path.Combine(GetApplicationPath(), "awscreds.txt");
            if (File.Exists(awscreds))
            {
                var a = File.ReadAllText(awscreds).Split(',');
                Util.InsertCacheNotRemovable("awscreds", a);
            }
            else if (ConfigurationManager.AppSettings["awscreds"].HasValue())
            {
                var a = ConfigurationManager.AppSettings["awscreds"].Split(',');
                Util.InsertCacheNotRemovable("awscreds", a);
            }
            SleepTime = ConfigurationManager.AppSettings["SleepTime"].ToInt();
        }
        private static string GetApplicationPath()
        {
            return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);
        }
        public DateTime[] dailyruns = new DateTime[] {
                new DateTime(10, 10, 1, 4, 0, 0), // 4:00 AM
                new DateTime(10, 10, 1, 6, 0, 0), // 6:00 AM
                new DateTime(10, 10, 1, 8, 0, 0), // 8:00 AM
                new DateTime(10, 10, 1, 10, 0, 0), // 10:00 AM
                new DateTime(10, 10, 1, 12, 0, 0), // 12:00 AM
                new DateTime(10, 10, 1, 14, 0, 0), // 2:00 PM
                new DateTime(10, 10, 1, 16, 0, 0), // 4:00 PM
                new DateTime(10, 10, 1, 18, 0, 0), // 6:00 PM
                new DateTime(10, 10, 1, 20, 0, 0), // 8:00 PM
                new DateTime(10, 10, 1, 22, 0, 0), // 10:00 PM
                new DateTime(10, 10, 1, 21, 55, 0), // 11:55 PM
            };

        protected override void OnStart(string[] args)
        {
            WriteLog("MassEmailer service started");
            StartListening();
        }
        public void StartListening()
        {
            listener = new Thread(Listen);
            serviceStarted = true;
            listener.Start();
        }
        private void Listen()
        {
            string Host = "";
            string CmsHost = "";
            int id = 0;
            int pid = 0;
            string lasterror = null;
            while (serviceStarted)
            {
                try
                {
                    foreach (var run in dailyruns)
                    {
                        var scheduledtime = DateTime.Today + run.TimeOfDay;
                        if (DateTime.Now >= scheduledtime // at or past the scheduled time
                            && lastRun < scheduledtime) // have not run for this time
                        {
                            WriteLog("Check for scheduled emails {0}".Fmt(scheduledtime));
                            var t = DateTime.Now;
                            var cb = new SqlConnectionStringBuilder(ConnStr);
                            cb.InitialCatalog = "BlogData";
                            using (var cn2 = new SqlConnection(cb.ConnectionString))
                            {
                                cn2.Open();
                                using (var cmd = new SqlCommand("SendScheduledEmails", cn2))
                                    cmd.ExecuteNonQuery();
                                lastRun = DateTime.Now;
                            }
                        }
                    }
                    using (var cn = new SqlConnection(ConnStr))
                    {
                        cn.Open();
                        const string sql = @"
WAITFOR(
    RECEIVE TOP(20) CONVERT(VARCHAR(max), message_body) AS message 
    FROM EmailQueue
), TIMEOUT 10000";
                        using (var cmdr = new SqlCommand(sql, cn))
                        {
                            cmdr.CommandTimeout = 0;
                            var reader = cmdr.ExecuteReader();
                            while (reader.Read())
                            {
                                var s = reader.GetString(0);
                                if (!s.HasValue())
                                    continue;
                                var a = s.Split('|');
                                Host = a[1];
                                CmsHost = a[2];
                                id = a[3].ToInt();
                                using (var Db = new CMSDataContext(GetConnectionString(Host, ConnStr)))
                                {
                                    Db.Host = Host;
                                    var equeue = Db.EmailQueues.Single(ee => ee.Id == id);
                                    switch (a[0])
                                    {
                                        case "SEND":
                                            pid = a[4].ToInt();
                                            SendPersonEmail(Db, CmsHost, id, pid);
                                            break;
                                        case "START":
                                            equeue.Started = DateTime.Now;
                                            Db.SubmitChanges();
                                            var nitems = equeue.EmailQueueTos.Count();
                                            if (nitems > 5)
                                                WriteLog("Processing Queue for " + nitems);
                                            break;
                                        case "END":
                                            equeue.Sent = DateTime.Now;
                                            if (equeue.Redacted ?? false)
                                                equeue.Body = "redacted";
                                            else
                                            {
                                                nitems = equeue.EmailQueueTos.Count();
                                                if (nitems > 1)
                                                    NotifySentEmails(Db, CmsHost, equeue.FromAddr, equeue.FromName, equeue.Subject, nitems, id);
                                            }
                                            Db.SubmitChanges();
                                            EndQueue(new Guid(a[4]));
                                            break;
                                        default:
                                            continue;
                                    }
                                }
                            }
                        }
                    }
                    lasterror = null;
                }
                catch (Exception ex)
                {
                    if (lasterror != ex.Message)
                    {
                        WriteLog("Error sending emails ", EventLogEntryType.Error);
                        var SysFromEmail = ConfigurationManager.AppSettings["sysfromemail"];
                        var senderrorsto = Util.SendErrorsTo();
                        Util.SendMsg(SysFromEmail, CmsHost, senderrorsto[0],
                            "Mass Emailer Error " + Host + " id:" + id + " pid:" + pid,
                            Util.SafeFormat(ex.Message + "\n\n" + ex.StackTrace),
                            senderrorsto, 0, Record: false);
                        lasterror = ex.Message;
                    }
                    Thread.Sleep(5000);
                }
            }
            Thread.CurrentThread.Abort();
        }
        private void SendPersonEmail(CMSDataContext Db, string CmsHost, int id, int pid)
        {
            var useSES = HttpRuntime.Cache["awscreds"] != null;

            var SysFromEmail = Db.Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
            var emailqueue = Db.EmailQueues.Single(eq => eq.Id == id);
            var emailqueueto = Db.EmailQueueTos.Single(eq => eq.Id == id && eq.PeopleId == pid);
            var From = Util.FirstAddress(emailqueue.FromAddr, emailqueue.FromName);
            var Message = emailqueue.Body;

            var q = from p in Db.People
                    where p.PeopleId == emailqueueto.PeopleId
                    select p;
            var qp = q.Single();
            string text = emailqueue.Body;
            var aa = Db.DoReplacements(ref text, CmsHost, qp, emailqueueto);

            var qs = "OptOut/UnSubscribe/?enc=" + Util.EncryptForUrl("{0}|{1}".Fmt(emailqueueto.PeopleId, From.Address));
            var url = Util.URLCombine(CmsHost, qs);
            var link = @"<a href=""{0}"">Unsubscribe</a>".Fmt(url);
            text = text.Replace("{unsubscribe}", link);
            text = text.Replace("{Unsubscribe}", link);
            if (aa.Count > 0)
            {
                text = text.Replace("{toemail}", aa[0].Address);
                text = text.Replace("%7Btoemail%7D", aa[0].Address);
            }
            text = text.Replace("{fromemail}", From.Address);
            text = text.Replace("%7Bfromemail%7D", From.Address);

            emailqueueto.Messageid = EmailRoute(
                SysFromEmail, From.DisplayName, From.Address,
                aa, emailqueue.Subject, text, CmsHost, id, pid);
            emailqueueto.Sent = DateTime.Now;
            Db.SubmitChanges();
        }
        private Boolean SESCanSend()
        {
            var cansend = false;
            var sendrate = 0D;

            if (lastThrottle > DateTime.MinValue)
                if (DateTime.Now.Subtract(lastThrottle).TotalSeconds < 30)
                    return false;
                else
                    lastThrottle = DateTime.MinValue;

            var o = HttpRuntime.Cache["awscreds"];
            if (o != null)
            {
                var resp = (GetSendQuotaResponse)HttpRuntime.Cache["ses_quota"];
                if (resp == null)
                {
                    string[] a = (string[])o;
                    var cfg = new AmazonSimpleEmailServiceConfig();
                    cfg.UseSecureStringForAwsSecretKey = false;
                    var ses = new AmazonSimpleEmailServiceClient(a[0], a[1], cfg);
                    var req = new GetSendQuotaRequest();
                    resp = ses.GetSendQuota(req);
                    HttpRuntime.Cache.Insert("ses_quota", resp, null,
                                DateTime.Now.AddMinutes(15), Cache.NoSlidingExpiration);
                    sentSinceQuotaCheck = 0;
                }
                sendrate = resp.GetSendQuotaResult.MaxSendRate.ToInt();
                cansend = (resp.GetSendQuotaResult.SentLast24Hours + sentSinceQuotaCheck)
                    < (resp.GetSendQuotaResult.Max24HourSend);
            }
            if (cansend)
            {
                var ms = 1000 / sendrate;
                var elapsed = DateTime.Now.Subtract(lastSES).TotalMilliseconds;
                if (elapsed >= ms)
                {
                    lastSES = DateTime.Now;
                    sentSinceQuotaCheck++;
                    return true;
                }
            }
            return false;
        }
        private string SendAmazonSESRawEmail(string SysEmailFrom,
            string fromname, string fromaddress, List<MailAddress> to, string Subject, string body, string host, int id, int pid)
        {
            var awsfrom = ConfigurationManager.AppSettings["awsfromemail"];
            if (!fromname.HasValue())
                fromname = fromaddress;
            else
                fromname = fromname.Replace("\"", "");
            var from = new MailAddress(fromaddress, fromname);
            var msg = new MailMessage();
            msg.From = new MailAddress(awsfrom, fromname);
            Util.RecordEmailSent(host, from, Subject, to, id, true);

            foreach (var t in to)
            {
                if (t.Host != "nowhere.name")
                    msg.To.Add(t);
            }
            if (msg.To.Count == 0)
            {
                msg.To.Add(msg.From);
                Subject += "-- NO GOOD EMAIL({0},{1})".Fmt(host, pid);
            }
            if (Subject.HasValue())
                msg.Subject = Subject;
            else
                msg.Subject = "no subject";
            msg.ReplyToList.Add(from);
            msg.Headers.Add("X-bvcms-host", host);
            msg.Headers.Add("X-bvcms-mail-id", id.ToString());
            msg.Headers.Add("X-bvcms-peopleid", pid.ToString());
            var addrs = to.EmailAddressListToString();
            if (addrs.HasValue())
                msg.Headers.Add("X-bvcms-addresses", addrs);

            var regex = new Regex("</?([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var text = regex.Replace(body, string.Empty);

            var bytes1 = Encoding.UTF8.GetBytes(text);
            var textStream1 = new MemoryStream(bytes1);
            var textView1 = new AlternateView(textStream1, MediaTypeNames.Text.Plain);
            var lines = Regex.Split(text, @"\r?\n|\r");
            if (lines.Any(li => li.Length > 990))
                textView1.TransferEncoding = TransferEncoding.QuotedPrintable;
            else
                textView1.TransferEncoding = TransferEncoding.SevenBit;
            msg.AlternateViews.Add(textView1);

            var bytes = Encoding.UTF8.GetBytes(body);
            var htmlStream = new MemoryStream(bytes);
            var htmlView = new AlternateView(htmlStream, MediaTypeNames.Text.Html);
            htmlView.TransferEncoding = TransferEncoding.Base64;
            msg.AlternateViews.Add(htmlView);

            var rawMessage = new RawMessage();
            using (var memoryStream = ConvertMailMessageToMemoryStream(msg))
                rawMessage.WithData(memoryStream);

            var request = new SendRawEmailRequest();
            request.WithRawMessage(rawMessage);
            var tolist = msg.To.Select(tt => tt.ToString());
            request.WithDestinations(tolist);
            var fullsys = new MailAddress(awsfrom, fromname);
            request.WithSource(fullsys.ToString());

            SendRawEmailResponse response = null;
            SendRawEmailResult result = null;
            string[] a = (string[])HttpRuntime.Cache["awscreds"];
            try
            {
                var cfg = new AmazonSimpleEmailServiceConfig();
                cfg.UseSecureStringForAwsSecretKey = false;
                var ses = new AmazonSimpleEmailServiceClient(a[0], a[1], cfg);
                response = ses.SendRawEmail(request);
                result = response.SendRawEmailResult;
                return result.MessageId;
            }
            catch (AmazonSimpleEmailServiceException ex)
            {
                if (ex.ErrorCode == "Throttling")
                {
                    lastThrottle = DateTime.Now;
                    return EmailRoute(SysEmailFrom, fromname, fromaddress, to, Subject, body, host, id, pid);
                }
                string resp = "no response";
                if (response.IsNotNull())
                    resp = response.SendRawEmailResult.ToString();
                Util.SendMsg(SysEmailFrom, host, from,
                    "({0}) {1}".Fmt(ex.ErrorCode, Subject),
                    "<p>to: {0}<br>host:{1} id:{2} pid:{3}</p><pre>{4}</pre>{5}<br><br>{6}".Fmt(
                        addrs, host, id, pid, ex.Message, body, resp),
                        Util.SendErrorsTo(), id, Record: true);
            }
            catch (Exception ex)
            {
                if (!Subject.EndsWith(" .rs") &&
                       (ex.Message.StartsWith("The underlying connection was closed")
                        || ex.Message.StartsWith("Root element is missing")
                        || ex.Message.StartsWith("The operation has timed out")
                        || ex.Message.StartsWith("The remote name could not be resolved")
                        ))
                {
                    return EmailRoute(SysEmailFrom, fromname, fromaddress, to,
                        Subject + " .rs", body, host, id, pid);
                }
                else if (Subject.EndsWith(" .rs"))
                {
                    // resort to SMTP instead of Amazon
                    Util.SendMsg(SysEmailFrom, host, from, Subject, body,
                        to, id, Record: true);
                }
                else if (!msg.Subject.StartsWith("(sending error)"))
                {
                    string resp = "no response";
                    if (response.IsNotNull())
                        resp = response.SendRawEmailResult.ToString();
                    Util.SendMsg(SysEmailFrom, host, from,
                        "(sending error) " + Subject,
                        "<p>to: {0}<br>host:{1} id:{2} pid:{3}</p><pre>{4}</pre>{5}<br><br>{6}".Fmt(
                            addrs, host, id, pid, ex.Message, body, resp),
                            Util.SendErrorsTo(), id, Record: true);
                }
            }
            return "no messageid";
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
        private string EmailRoute(string SysFrom, string fromname, string fromaddress, List<MailAddress> to, string subject, string body, string CmsHost, int id, int pid)
        {
            var useSES = HttpRuntime.Cache["awscreds"] != null;
            if (useSES && SESCanSend())
                return SendAmazonSESRawEmail(SysFrom, fromname, fromaddress, to, subject, body, CmsHost, id, pid);
            else
                Util.SendMsg(SysFrom, CmsHost, new MailAddress(fromaddress, fromname), subject, body, to, id, Record: true);
            return "no messageid";
        }
        private void NotifySentEmails(CMSDataContext Db, string CmsHost, string FromAddress, string FromName, string subject, int count, int id)
        {
            if (Db.Setting("sendemail", "true") != "false")
            {
                string subj = "sent emails: " + subject;
                var uri = new Uri(new Uri(CmsHost), "/Manage/Emails/Details/" + id);
                string body = @"<a href=""{0}"">{1} emails sent</a>".Fmt(uri, count);
                var SysFromEmail = Db.Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
                var SendErrorsTo = Util.SendErrorsTo();
                var to = Util.ToMailAddressList(FromAddress, FromName);
                EmailRoute(SysFromEmail, FromName, FromAddress, to, subj, body, CmsHost, id, 0);
                var host = uri.Host;
                EmailRoute(SysFromEmail, FromName, FromAddress,
                    SendErrorsTo, host + " " + subj, body, CmsHost, id, 0);
            }
        }
        private void EndQueue(Guid guid)
        {
            using (var cn = new SqlConnection(ConnStr))
            {
                cn.Open();
                using (var cmd = new SqlCommand("EndConversation", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var conv = new SqlParameter("@conversationID", SqlDbType.UniqueIdentifier);
                    conv.Value = guid;
                    cmd.Parameters.Add(conv);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void WriteLog(string message)
        {
            WriteLog(message, EventLogEntryType.Information);
        }
        private void WriteLog(string message, EventLogEntryType err)
        {
            eventLog1.WriteEntry(message, err);
        }
        protected override void OnStop()
        {
            WriteLog("MassEmailer2 service stopped");
            serviceStarted = false;
            // give it a little time to finish any pending work
            listener.Join(new TimeSpan(0, 0, 20));
        }
        protected string GetConnectionString(string Host, string cs)
        {
            var cb = new SqlConnectionStringBuilder(cs);
            var a = Host.SplitStr(".:");
            cb.InitialCatalog = "CMS_{0}".Fmt(a[0]);
            return cb.ConnectionString;
        }
    }
}
