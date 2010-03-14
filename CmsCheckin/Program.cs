using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CmsCheckin
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var f = new StartUp();
            var r = f.ShowDialog();
            if (r == DialogResult.Cancel)
                return;
            CampusId = f.CampusId;
            ThisDay = f.DayOfWeek;
            HideCursor = f.HideCursor.Checked;
            TestMode = f.TestMode.Checked;
            LeadTime = int.Parse(f.LeadTime.Text);
            f.Dispose();

            Application.Run(new BaseForm());
        }
        public static int FamilyId { get; set; }
        public static int CampusId { get; set; }
        public static int ThisDay { get; set; }
        public static int LeadTime { get; set; }
        public static bool HideCursor { get; set; }
        public static bool TestMode { get; set; }
        public static bool editing { get; set; }
        public static string QueryString
        {
            get
            {
                return string.Format("?campus={0}&thisday={1}", CampusId, ThisDay);
            }
        }

        public static Home home;
        public static ListFamilies families;
        public static ListFamily family;
        public static ListClasses classes;
        public static EnterText namesearch;
        public static ListNames names;

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
        public static void ClearFields()
        {
            first.textBox1.Text = null;
            goesby.textBox1.Text = null;
            last.textBox1.Text = null;
            email.textBox1.Text = null;
            addr.textBox1.Text = null;
            zip.textBox1.Text = null;
            dob.textBox1.Text = null;
            cellphone.textBox1.Text = null;
            homephone.textBox1.Text = null;
            gendermarital.Gender = 0;
            gendermarital.Marital = 0;
        }
        public static void SetFields(string Last, string Email, string Addr, string Zip, string Home)
        {
            last.textBox1.Text = Last;
            email.textBox1.Text = Email;
            addr.textBox1.Text = Addr;
            zip.textBox1.Text = Zip;
            homephone.textBox1.Text = Home;
        }
    }
}
