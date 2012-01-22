using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Deployment.Application;
using System.Web;
using System.Collections.Specialized;

namespace CmsCheckin
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var login = new Login();
            login.password.Focus();
            var r = login.ShowDialog();
            if (r == DialogResult.Cancel)
                return;
            PrintMode = login.PrintMode.Text;
            PrintKiosks = login.PrintKiosks.Text;

            var f = new StartUp { campuses = login.campuses };
            var ret = f.ShowDialog();
            if (ret == DialogResult.Cancel)
                return;

            AdminPassword = f.AdminPassword;
            CampusId = f.CampusId;
            ThisDay = f.DayOfWeek;
            HideCursor = f.HideCursor.Checked;
            Printer = f.Printer.Text;
            AskGrade = f.AskGrade.Checked;
            AskLabels = f.AskLabels.Checked;
            LeadTime = int.Parse(f.LeadHours.Text);
            EarlyCheckin = int.Parse(f.LateMinutes.Text);
            AskEmFriend = f.AskEmFriend.Checked;
            KioskName = f.KioskName.Text;
            AskChurch = f.AskChurch.Checked;
            AskChurchName = f.AskChurchName.Checked;
            EnableTimer = f.EnableTimer.Checked;
            TwoInchLabel = f.TwoInchLabel.Checked;
            DisableJoin = f.DisableJoin.Checked;

            f.Dispose();


            if (PrintMode == "Print From Server")
            {
                var p = new PrintingServer();
                Application.Run(p);
                return;
            }

            var b = new BaseForm();
            Program.baseform = b;

            if (f.FullScreen.Checked)
            {
                b.WindowState = FormWindowState.Maximized;
                b.FormBorderStyle = FormBorderStyle.None;
            }

            Application.Run(b);
        }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string URL { get; set; }
        public static string Printer { get; set; }
        public static string PrintKiosks { get; set; }
        public static string AdminPassword { get; set; }
        public static int FamilyId { get; set; }
        public static int PeopleId { get; set; }
        public static int CampusId { get; set; }
        public static string SecurityCode { get; set; }
        public static int ThisDay { get; set; }
        public static int LeadTime { get; set; }
        public static int EarlyCheckin { get; set; }
        public static int? Grade { get; set; }
        public static bool HideCursor { get; set; }
        public static bool editing { get; set; }
        public static bool EnableTimer { get; set; }
        public static bool DisableJoin { get; set; }
        public static string KioskName { get; set; }
        public static bool AskEmFriend { get; set; }
        public static bool AskGrade { get; set; }
        public static bool AskChurch { get; set; }
        public static bool AskChurchName { get; set; }
        public static bool AskLabels { get; set; }
        public static bool TwoInchLabel { get; set; }
        public static string PrintMode { get; set; }

        public static string QueryString
        {
            get { return string.Format("?campus={0}&thisday={1}&kioskmode={2}&kiosk={3}", CampusId, ThisDay, false, KioskName); }
        }
        public static int MaxLabels { get; set; }
        public static Timer timer1;
        public static Timer timer2;
        public static Home home;
        public static ListFamilies families;
        public static ListFamily family;
        public static ListClasses classes;
        public static EnterText namesearch;
        public static ListNames names;

        public static EnterText allergy;
        public static EnterText church;
        public static EnterText emfriend;
        public static EnterNumber grade;
        public static EnterPhone emphone;
        public static EnterText parent;

        public static EnterText first;
        public static EnterText goesby;
        public static EnterText last;
        public static EnterText email;
        public static EnterText addr;
        public static EnterText zip;
        public static EnterDate dob;
        public static EnterPhone cellphone;
        public static EnterPhone homephone;
        public static EnterGenderMarital gendermarital;
        public static BaseForm baseform;
        public static void ClearFields()
        {
            SecurityCode = null;
            first.textBox1.Text = null;
            goesby.textBox1.Text = null;
            last.textBox1.Text = null;
            email.textBox1.Text = null;
            addr.textBox1.Text = null;
            zip.textBox1.Text = null;
            dob.textBox1.Text = null;
            if (AskGrade)
                grade.textBox1.Text = null;
            allergy.textBox1.Text = null;
            if (AskEmFriend)
            {
                emfriend.textBox1.Text = null;
                emphone.textBox1.Text = null;
                parent.textBox1.Text = null;
            }
            cellphone.textBox1.Text = null;
            homephone.textBox1.Text = null;
            if (AskChurchName)
                church.textBox1.Text = null;
            gendermarital.Gender = 0;
            gendermarital.Marital = 0;
            if (AskChurch)
                gendermarital.ActiveOther.CheckState = CheckState.Indeterminate;
        }
        public static CheckState ActiveOther(string s)
        {
            return s == bool.TrueString || s == "Checked" ? CheckState.Checked :
            s == bool.FalseString || s == "Unchecked" ? CheckState.Unchecked : CheckState.Indeterminate;
        }
        public static void SetFields(string Last, string Email, string Addr, string Zip, string Home, string Parent, string EmFriend, string EmPhone, string AnotherChurch, string ChurchName)
        {
            last.textBox1.Text = Last;
            email.textBox1.Text = Email;
            addr.textBox1.Text = Addr;
            zip.textBox1.Text = Zip;
            homephone.textBox1.Text = Home;
            if (AskEmFriend)
            {
                emfriend.textBox1.Text = EmFriend;
                parent.textBox1.Text = Parent;
                emphone.textBox1.Text = EmPhone;
            }
            if (AskChurchName)
                church.textBox1.Text = ChurchName;
            gendermarital.ActiveOther.CheckState = ActiveOther(AnotherChurch);
        }
        public static void Timer2Reset()
        {
            if (!EnableTimer)
                return;
            if (timer2 == null)
                return;
            timer2.Stop();
            timer2.Start();
        }
        public static void Timer2Start(EventHandler t)
        {
            if (!EnableTimer)
                return;
            if (timer2 != null)
                Timer2Stop();
            timer2 = new Timer();
            timer2.Interval = 60000;
            timer2.Tick += t;
            timer2.Start();
        }
        public static void Timer2Stop()
        {
            if (timer2 == null)
                return;
            timer2.Stop();
            timer2.Dispose();
            timer2 = null;
        }
        public static void TimerReset()
        {
            if (!EnableTimer)
                return;
            if (timer1 == null)
                return;
            timer1.Stop();
            timer1.Start();
        }
        public static void TimerStart(EventHandler t)
        {
            if (!EnableTimer)
                return;
            if (timer1 != null)
                TimerStop();
            timer1 = new Timer();
            timer1.Interval = 60000;
            timer1.Tick += t;
            timer1.Start();
        }
        public static void TimerStop()
        {
            if (!EnableTimer)
                return;
            if (timer1 == null)
                return;
            timer1.Stop();
            timer1.Dispose();
            timer1 = null;
        }
        private static bool showing = true;
        public static void CursorShow()
        {
            if (showing)
                return;
            Cursor.Show();
            showing = true;
        }
        public static void CursorHide()
        {
            if (!Program.HideCursor)
                return;
            if (!showing)
                return;
            Cursor.Hide();
            showing = false;
        }

        //public static NameValueCollection GetQueryStringParameters()
        //{
        //    NameValueCollection col = new NameValueCollection();

        //    if (ApplicationDeployment.IsNetworkDeployed)
        //    {
        //        string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
        //        col = HttpUtility.ParseQueryString(queryString);
        //    }
        //    return col;
        //}
    }
}
