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

namespace MassEmailer
{
    public partial class MassEmailer : ServiceBase
    {

        private Timer timer;
        private bool isTimerStarted;
        internal WorkData data;

        public MassEmailer()
        {
            InitializeComponent();
#if (!DEBUG)
            if (!EventLog.SourceExists("MassEmailer"))
                EventLog.CreateEventSource("MassEmailer", "Application");
#endif
            eventLog1.Source = "MassEmailer";
            data = new WorkData 
            {
                connstr = ConfigurationManager.ConnectionStrings["CMSEmailQueue"].ConnectionString
            };
        }
        internal class WorkData
        {
            public DateTime dailyrun = new DateTime(10, 10, 1, 4, 0, 0);// 4:00 AM
            public string connstr { get; set; }
            public DateTime lastrun { get; set; }
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("MassEmailer service started");
            var cb = new TimerCallback(timer_Elapsed);
            timer = new Timer(cb, data, 3000, 15000);
        }

        void timer_Elapsed(object sender)
        {
            var data = sender as WorkData;
            CheckQueue(data);
        }

        internal void CheckQueue(WorkData data)
        {
            using (var cn = new SqlConnection(data.connstr))
            {
                try
                {
                    cn.Open();

                    var todaysrun = DateTime.Today + data.dailyrun.TimeOfDay;
                    if (DateTime.Now > todaysrun && data.lastrun.Date != DateTime.Today)
                    {
                        eventLog1.WriteEntry("Check for scheduled emails {0}".Fmt(todaysrun));
                        var t = DateTime.Now;
                        using (var cmd = new SqlCommand("QueueScheduledEmails", cn))
                            cmd.ExecuteNonQuery();
                        var s = DateTime.Now - t;
                        eventLog1.WriteEntry("Check Complete for scheduled emails {0:n1} sec".Fmt(s.TotalSeconds));
                        data.lastrun = DateTime.Now;
                    }
                    using (var cmdr = new SqlCommand("RECEIVE TOP(1) CONVERT(VARCHAR(max), message_body) AS message FROM EmailReceiveQueue", cn))
                        while (true)
                        {
                            var s = (string)cmdr.ExecuteScalar();
                            if (!s.HasValue())
                                break;

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
                                eventLog1.WriteEntry("Sending {0} Emails for {1}, id={2}".Fmt(nt, Host, emailqueue.Id));
                                var t = DateTime.Now;
                                Emailer.SendPeopleEmail(Db, SysFromEmail, CmsHost, emailqueue);
                                var dur = DateTime.Now - t;
                                eventLog1.WriteEntry("Finished {0} Emails for {1}, id={2}, duration={3:mm\\:ss}".Fmt(nt, Host, emailqueue.Id, dur));
                            }
                        }
                }
                catch (Exception ex)
                {
                    eventLog1.WriteEntry("Error sending emails ", EventLogEntryType.Error);
                    Util.SendMsg(Util.Smtp(), ConfigurationManager.AppSettings["sysfromemail"], "http://bvcms.com", 
                        new MailAddress("david@bvcms.com", "David Carroll"), "Mass Emailer Error " + cn.DataSource, Util.SafeFormat(ex.Message + "\n\n" + ex.StackTrace), "David Carroll", "david@bvcms.com");
                }
            }
        }
        protected override void OnStop()
        {
            eventLog1.WriteEntry("MassEmailer service stopped");
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
