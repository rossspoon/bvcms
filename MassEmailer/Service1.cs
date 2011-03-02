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
                    try
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
    FROM EmailReceiveQueue
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
                                var id = a[0].ToInt();
                                var CmsHost = a[1];
                                var Host = a[2];

                                using (var Db = new CMSDataContext(GetConnectionString(Host, data.connstr)))
                                {
                                    Db.Host = Host;
                                    var SysFromEmail = Db.Setting("SysFromEmail",
                                        ConfigurationManager.AppSettings["sysfromemail"]);
                                    var emailqueue = Db.EmailQueues.Single(eq => eq.Id == id);
                                    var nt = Db.EmailQueueTos.Count(et => et.Id == id);
                                    WriteLog("Sending {0} Emails for {1}, id={2}".Fmt(nt, Host, emailqueue.Id));
                                    var t = DateTime.Now;
                                    Emailer.SendPeopleEmail(Db, SysFromEmail, CmsHost, emailqueue);
                                    var dur = DateTime.Now - t;
                                    WriteLog("Finished {0} Emails for {1}, id={2}, duration={3:mm\\:ss}".Fmt(nt, Host, emailqueue.Id, dur));
                                }
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
            Thread.CurrentThread.Abort();
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
