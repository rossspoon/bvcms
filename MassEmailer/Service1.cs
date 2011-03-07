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

        public MassEmailer()
        {
            InitializeComponent();

#if !DEBUG
            var sSource = "MassEmailer";
            var sLog = "Application";
            if (!EventLog.SourceExists(sSource))
                EventLog.CreateEventSource(sSource, sLog);
            eventLog1.Source = "MassEmailer";
#endif
            ConnStr = ConfigurationManager.ConnectionStrings["CMSEmailQueue"].ConnectionString;

            if (ConfigurationManager.AppSettings["awscreds"].HasValue())
            {
                var a = ConfigurationManager.AppSettings["awscreds"].Split(',');
                Util.InsertCacheNotRemovable("awscreds", a);
            }
        }
        internal class WorkData
        {
            public DateTime dailyrun = new DateTime(10, 10, 1, 4, 0, 0);// 4:00 AM
            public string connstr { get; set; }
            public DateTime lastrun { get; set; }
            public DateTime lastSES { get; set; }
        }

        protected override void OnStart(string[] args)
        {
            WriteLog("MassEmailer service started");
            StartListening();

        }
        public void StartListening()
        {
            listener = new Thread(Listen);
#if !DEBUG
            listener.IsBackground = true;
#endif
            serviceStarted = true;
            listener.Start();
        }
        private void Listen()
        {
            var data = new WorkData { connstr = ConnStr };
            while (serviceStarted)
            {
                using (var cn = new SqlConnection(data.connstr))
                {
                    cn.Open();
                    var todaysrun = DateTime.Today + data.dailyrun.TimeOfDay;
                    if (DateTime.Now > todaysrun && data.lastrun.Date != DateTime.Today)
                    {
                        WriteLog("Check for scheduled emails {0}".Fmt(todaysrun));
                        var t = DateTime.Now;
                        using (var cmd = new SqlCommand("QueueScheduledEmails", cn))
                            cmd.ExecuteNonQuery();
                        var s = DateTime.Now - t;
                        WriteLog("Check Complete for scheduled emails {0:n1} sec".Fmt(s.TotalSeconds));
                        data.lastrun = DateTime.Now;
                    }
                    const string sql = @"
WAITFOR(
    RECEIVE TOP(20) CONVERT(VARCHAR(max), message_body) AS message 
    FROM EmailPriorityReceiveQueue
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
                            try
                            {
                                var a = s.Split('|');
                                var Host = a[1];
                                var CmsHost = a[2];
                                var id = a[3].ToInt();
                                using (var Db = new CMSDataContext(GetConnectionString(Host, data.connstr)))
                                {
                                    Db.Host = Host;
                                    var equeue = Db.EmailQueues.Single(ee => ee.Id == id);
                                    switch (a[0])
                                    {
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
                                            Db.SubmitChanges();
                                            Db.EndQueue(Guid.Parse(a[4]));
                                            break;
                                        case "SEND":
                                            SendPersonEmail(Db, data, CmsHost, id, a[4].ToInt());
                                            break;
                                        default:
                                            continue;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                WriteLog("Error sending emails ", EventLogEntryType.Error);
                                var smtp = Util.Smtp();
                                var msg = new MailMessage("david@bvcms.com", "david@bvcms.com", "Mass Emailer Error " + cn.DataSource, Util.SafeFormat(ex.Message + "\n\n" + ex.StackTrace));
                                smtp.Send(msg);
                            }
                        }
                    }
                }
            }
            Thread.CurrentThread.Abort();
        }
        private void SendPersonEmail(CMSDataContext Db, WorkData data, string CmsHost, int id, int pid)
        {
            var useSES = HttpRuntime.Cache["awscreds"] != null;
            
            var SysFromEmail = Db.Setting("SysFromEmail", ConfigurationManager.AppSettings["sysfromemail"]);
            var emailqueue = Db.EmailQueues.Single(eq => eq.Id == id);
            var emailqueueto = Db.EmailQueueTos.Single(eq => eq.Id == id && eq.PeopleId == pid);
            var From = Util.FirstAddress(emailqueue.FromAddr, emailqueue.FromName);
            var Message = emailqueue.Body;

            var q = from p in Db.People
                    where p.PeopleId == emailqueueto.PeopleId
                    select new
                     {
                         p.Name,
                         p.PreferredName,
                         p.EmailAddress,
                         p.OccupationOther,
                         p.EmailAddress2,
                         Send1 = p.SendEmailAddress1 ?? true,
                         Send2 = p.SendEmailAddress2 ?? false,
                     };
            var qp = q.Single();

            string text = emailqueue.Body;

            if (qp.Name.Contains("?") || qp.Name.ToLower().Contains("unknown"))
                text = text.Replace("{name}", string.Empty);
            else
                text = text.Replace("{name}", qp.Name);

            if (qp.PreferredName.Contains("?") || qp.PreferredName.ToLower() == "unknown")
                text = text.Replace("{first}", string.Empty);
            else
                text = text.Replace("{first}", qp.PreferredName);
            text = text.Replace("{occupation}", qp.OccupationOther);

            var re = new Regex(@"\{votelink:(?<orgid>\d+),(?<sg>[^}]*)\}", RegexOptions.Singleline | RegexOptions.Multiline);
            var list = new Dictionary<string, OneTimeLink>();
            var ma = re.Match(text);
            while (ma.Success)
            {
                var votelink = ma.Value;
                var orgid = ma.Groups["orgid"].Value;
                var smallgroup = ma.Groups["sg"].Value;
                var qs = @"{0},{1}".Fmt(orgid, emailqueueto.PeopleId);
                OneTimeLink ot;
                if (list.ContainsKey(qs))
                    ot = list[qs];
                else
                {
                    ot = new OneTimeLink
                    {
                        Id = Guid.NewGuid(),
                        Querystring = qs
                    };
                    Db.OneTimeLinks.InsertOnSubmit(ot);
                    Db.SubmitChanges();
                    list.Add(qs, ot);
                }
                var url = Util.URLCombine(CmsHost, "/OnlineReg/VoteLink/{0}?smallgroup={1}".Fmt(ot.Id, smallgroup));
                text = text.Replace(votelink, @"<a href=""{0}"">{1}</a>".Fmt(url, smallgroup));
                ma = ma.NextMatch();
            }

            var aa = new List<string>();
            if (qp.Send1)
                aa.AddRange(qp.EmailAddress.SplitStr(",;"));
            if (qp.Send2)
                aa.AddRange(qp.EmailAddress2.SplitStr(",;"));
            if (emailqueue.Addemail.HasValue())
                aa.AddRange(emailqueue.Addemail.SplitStr(",;"));

            if (emailqueueto.OrgId.HasValue)
            {
                var qm = (from m in Db.OrganizationMembers
                          where m.PeopleId == emailqueueto.PeopleId && m.OrganizationId == emailqueueto.OrgId
                          select new { m.PayLink, m.Amount, m.AmountPaid, m.RegisterEmail }).SingleOrDefault();
                if (qm != null)
                {
                    if (qm.PayLink.HasValue())
                        text = text.Replace("{paylink}", "<a href=\"{0}\">payment link</a>".Fmt(qm.PayLink));
                    text = text.Replace("{amtdue}", (qm.Amount - qm.AmountPaid).ToString2("c"));
                    if (qm.RegisterEmail.HasValue() && !aa.Contains(qm.RegisterEmail, StringComparer.OrdinalIgnoreCase))
                        aa.Add(qm.RegisterEmail);
                }
            }

            foreach (var ad in aa)
            {
                if (Util.ValidEmail(ad))
                {
                    var qs = "OptOut/UnSubscribe/?enc=" + Util.EncryptForUrl("{0}|{1}".Fmt(emailqueueto.PeopleId, From.Address));
                    var url = Util.URLCombine(CmsHost, qs);
                    var link = @"<a href=""{0}"">Unsubscribe</a>".Fmt(url);
                    text = text.Replace("{unsubscribe}", link);
                    text = text.Replace("{Unsubscribe}", link);
                    text = text.Replace("{toemail}", ad);
                    text = text.Replace("%7Btoemail%7D", ad);
                    text = text.Replace("{fromemail}", From.Address);
                    text = text.Replace("%7Bfromemail%7D", From.Address);

                    if (Db.Setting("sendemail", "true") != "false")
                    {
                        if (useSES && SESCanSend(data))
                        {
                                SendAmazonSESRawEmail(CmsHost, From, ad, qp.Name, emailqueue.Subject, text, CmsHost, emailqueue.Id);
                                data.lastSES = DateTime.Now;
                        }
                        else
                            Util.SendMsg(SysFromEmail, CmsHost, From, emailqueue.Subject, text, qp.Name, ad, emailqueue.Id);
                        emailqueueto.Sent = DateTime.Now;
                        Db.SubmitChanges();
                    }
                }
            }
        }
        private Boolean SESCanSend(WorkData data)
        {
            var cansend = false;
            var sendrate = 0D;

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
                }
                sendrate = resp.GetSendQuotaResult.MaxSendRate.ToInt();
                if (resp.GetSendQuotaResult.SentLast24Hours < (resp.GetSendQuotaResult.Max24HourSend * 90 / 100))
                    cansend = true;
            }
            if (cansend)
            {
                var ms = 1000 / sendrate;
                var elapsed = DateTime.Now.Subtract(data.lastSES).TotalMilliseconds;
                if (elapsed >= ms)
                    return true;
            }
            return false;
        }
        public Boolean SendAmazonSESRawEmail(string CmsHost,
            MailAddress from, string to, string nameto, string Subject, string body, string host, int id)
        {
            Util.RecordEmailSent(CmsHost, from, Subject, nameto, to, id, true);
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
#if DEBUG2
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
                    return SendAmazonSESRawEmail(CmsHost, from, ConfigurationManager.AppSettings["senderrorsto"], nameto, "(sending error) " + Subject, "<p>(to: {0})</p><pre>{1}</pre>{2}".Fmt(to, ex.Message, body), host, id);
                return false;
            }
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

        private void WriteLog(string message)
        {
            WriteLog(message, EventLogEntryType.Information);
        }
        private void WriteLog(string message, EventLogEntryType err)
        {
#if !DEBUG
            eventLog1.WriteEntry(message, err);
#endif
        }
        protected override void OnStop()
        {
            WriteLog("MassEmailer service stopped");
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
