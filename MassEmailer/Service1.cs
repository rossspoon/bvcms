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
using Elmah;

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
        public Elmah.SqlErrorLog ErrorLog;

        public MassEmailer()
        {
            InitializeComponent();


            var sSource = "MassEmailer2";
            var sLog = "Application";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            eventLog1.Source = "MassEmailer2";

            ConnStr = ConfigurationManager.ConnectionStrings["CMSEmailQueue"].ConnectionString;
            var cb = new SqlConnectionStringBuilder(ConnStr);
            cb.InitialCatalog = "ELMAH";
            ErrorLog = new SqlErrorLog(cb.ConnectionString);
            ErrorLog.ApplicationName = "BVCMS";

            string awscreds = Path.Combine(GetApplicationPath(), "awscreds.txt");
            if (File.Exists(awscreds))
            {
                var lines = File.ReadAllLines(awscreds);
                foreach (var line in lines)
                {
                    var a = line.Split(':');
                    Util.InsertCacheNotRemovable(a[0], a[1]);
                }
            }
            else if (ConfigurationManager.AppSettings["AccessId"].HasValue())
            {
                Util.InsertCacheNotRemovable("AccessId",
                    ConfigurationManager.AppSettings["AccessId"]);
                Util.InsertCacheNotRemovable("SecretKey",
                    ConfigurationManager.AppSettings["SecretKey"]);
                Util.InsertCacheNotRemovable("ChilkatMailKey",
                    ConfigurationManager.AppSettings["ChilkatMailKey"]);
                Util.InsertCacheNotRemovable("ChilkatMimeKey",
                    ConfigurationManager.AppSettings["ChilkatMimeKey"]);
            }
            // You should create your own privatekey.txt file
            string privatekey = Path.Combine(GetApplicationPath(), "privatekey.txt");
            if (File.Exists(privatekey))
            {
                var s = File.ReadAllText(privatekey);
                Util.InsertCacheNotRemovable("privatekey", s);
            }
            else if (ConfigurationManager.AppSettings["privatekey"].HasValue())
            {
                var s = ConfigurationManager.AppSettings["privatekey"];
                Util.InsertCacheNotRemovable("privatekey", s);
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
            WriteLog2("MassEmailer service started");
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
            //var currentDomain = AppDomain.CurrentDomain;
            //currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

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
                            WriteLog2("Check for scheduled emails {0}".Fmt(scheduledtime));
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
                }
                catch (Exception ex)
                {
                    WriteLog2("Error checking for scheduled emails ", EventLogEntryType.Error);
                    ErrorLog.Log(new Error(ex));
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
                            try
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
                                                WriteLog2("Processing Queue for " + nitems);
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
                            catch (Exception ex)
                            {
                                if (lasterror != ex.Message)
                                {
                                    WriteLog2("Error sending emails ", EventLogEntryType.Error);
                                    ErrorLog.Log(new Error(ex));

                                    var SysFromEmail = ConfigurationManager.AppSettings["sysfromemail"];
                                    var senderrorsto = Util.SendErrorsTo();
                                    Util.SendMsg(SysFromEmail, CmsHost, senderrorsto[0],
                                        "Mass Emailer Error " + Host,
                                        Util.SafeFormat(ex.Message) +
            @"
<div><a href='{0}Manage/Emails/Details/{1}'>message {1}</a></div>
<div><a href='{0}Person/Index/{2}'>to person {2}</a></div>
<div><a href='{0}elmah.axd'>elmah.axd</a></div>
".Fmt(CmsHost, id, pid),
                                        senderrorsto, 0, pid, Record: false);
                                    lasterror = ex.Message;
                                }
                                Thread.Sleep(5000);
                            }
                            lasterror = null;
                        }
                    }
                }
            }
            Thread.CurrentThread.Abort();
        }
        private void SendPersonEmail(CMSDataContext Db, string CmsHost, int id, int pid)
        {
            var useSES = HttpRuntime.Cache["AccessId"] != null;
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

            var AccessId = HttpRuntime.Cache["AccessId"];
            if (AccessId != null)
            {
                var resp = (GetSendQuotaResponse)HttpRuntime.Cache["ses_quota"];
                if (resp == null)
                {
                    //var cfg = new AmazonSimpleEmailServiceConfig();
                    //cfg.UseSecureStringForAwsSecretKey = false;
                    var ses = new AmazonSimpleEmailServiceClient(
                        (string)AccessId,
                        (string)HttpRuntime.Cache["SecretKey"]);
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

            var mailman = new Chilkat.MailMan();
            mailman.UnlockComponent((string)HttpRuntime.Cache["ChilkatMailKey"]);

            var email = new Chilkat.Email();

            var ma = new MailAddress(awsfrom, fromname);
            email.From = ma.ToString();

            Util.RecordEmailSent(host, from, Subject, to, id, true);

            var tolist = new List<string>();
            foreach (var t in to)
            {
                if (t.Host != "nowhere.name")
                    tolist.Add(t.ToString());
            }
            if (tolist.Count == 0)
            {
                tolist.Add(from.ToString());
                Subject += "-- NO GOOD EMAIL({0},{1})".Fmt(host, pid);
            }
            email.AddMultipleTo(string.Join(",", tolist));

            if (Subject.HasValue())
                email.Subject = Subject;
            else
                email.Subject = "no subject";
            email.ReplyTo = from.ToString();
            email.AddHeaderField("X-bvcms-host", host);
            email.AddHeaderField("X-bvcms-mail-id", id.ToString());
            email.AddHeaderField("X-bvcms-peopleid", pid.ToString());
            var addrs = to.EmailAddressListToString();
            if (addrs.HasValue())
                email.AddHeaderField("X-bvcms-addresses", addrs);

            var regex = new Regex("</?([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var text = regex.Replace(body, string.Empty);

            email.AddPlainTextAlternativeBody(text);

            email.AddHtmlAlternativeBody(body);

            var dkim = new Chilkat.Dkim();
            dkim.UnlockComponent((string)HttpRuntime.Cache["ChilkatMimeKey"]);

            byte[] mimeData = null;
            mimeData = mailman.RenderToMimeBytes(email);

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            var str = enc.GetString(mimeData);

            dkim.DkimDomain = ConfigurationManager.AppSettings["domain"];
            dkim.DkimSelector = ConfigurationManager.AppSettings["dkimselector"];
            dkim.LoadDkimPk((string)HttpRuntime.Cache["privatekey"], null);
            dkim.DkimHeaders = "mime-version:subject:from:to:content-type";
            var dkimSignedMime = dkim.AddDkimSignature(mimeData);
            var str2 = enc.GetString(dkimSignedMime);

            var rawMessage = new RawMessage();

            var fileStream = new MemoryStream(dkimSignedMime);
            rawMessage.WithData(fileStream);

            var request = new SendRawEmailRequest();
            request.WithRawMessage(rawMessage);
            request.WithDestinations(tolist);
            var fullsys = new MailAddress(awsfrom, fromname);
            request.WithSource(fullsys.ToString());

            SendRawEmailResponse response = null;

            //var cfg = new AmazonSimpleEmailServiceConfig();
            //cfg.UseSecureStringForAwsSecretKey = false;
            var ses = new AmazonSimpleEmailServiceClient(
                (string)HttpRuntime.Cache["AccessId"],
                (string)HttpRuntime.Cache["SecretKey"]);
            try
            {
                response = ses.SendRawEmail(request);
                var result = response.SendRawEmailResult;
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
                throw;
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
                    ErrorLog.Log(new Error(ex));
                    return EmailRoute(SysEmailFrom, fromname, fromaddress, to,
                        Subject + " .rs", body, host, id, pid);
                }
                else if (Subject.EndsWith(" .rs"))
                {
                    // resort to SMTP instead of Amazon
                    Util.SendMsg(SysEmailFrom, host, from, Subject, body,
                        to, id, pid, Record: true);
                }
                else if (!email.Subject.StartsWith("(sending error)"))
                {
                    string resp = "no response";
                    if (response.IsNotNull())
                        resp = response.SendRawEmailResult.ToString();
                    throw;
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
            var useSES = HttpRuntime.Cache["AccessId"] != null;
            if (useSES && SESCanSend())
                return SendAmazonSESRawEmail(SysFrom, fromname, fromaddress, to, subject, body, CmsHost, id, pid);
            else
                Util.SendMsg(SysFrom, CmsHost, new MailAddress(fromaddress, fromname), subject, body, to, id, pid, Record: true);
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

        private void WriteLog2(string message)
        {
            WriteLog2(message, EventLogEntryType.Information);
        }
        private void WriteLog2(string message, EventLogEntryType err)
        {
            eventLog1.WriteEntry(message, err);
        }
        protected override void OnStop()
        {
            WriteLog2("MassEmailer2 service stopped");
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
	
        //Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    //This handler is called only when the common language runtime tries to bind to the assembly and fails.
	
        //    //Retrieve the list of referenced assemblies in an array of AssemblyName.
        //    Assembly MyAssembly, objExecutingAssemblies;
        //    string strTempAssmbPath = "";
	
        //    objExecutingAssemblies = Assembly.GetExecutingAssembly();
        //    AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();
	
        //    //Loop through the array of referenced assembly names.
        //    foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
        //    {
        //        //Check for the assembly names that have raised the "AssemblyResolve" event.
        //        if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
        //        {
        //            //Build the path of the assembly from where it has to be loaded.
        //            //The following line is probably the only line of code in this method you may need to modify:
        //            strTempAssmbPath = "c:\\Program Files (x86)\\Usage Defined Software\\BVCMS MassEmailer2\\";
        //            if (!strTempAssmbPath.EndsWith("\\")) strTempAssmbPath += "\\";
        //            strTempAssmbPath += args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
        //            break;
        //        }
        //    }
        //    //Load the assembly from the specified path.
        //    MyAssembly = Assembly.LoadFrom(strTempAssmbPath);
	
        //    //Return the loaded assembly.
        //    return MyAssembly;
        //}
	
    }
}
