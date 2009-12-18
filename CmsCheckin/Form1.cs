using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Linq;

namespace CmsCheckin
{
    public partial class Form1 : Form
    {
        PhoneNumber phone;
        Attendees attendees;
        Families families;
        public Form1()
        {
            InitializeComponent();
        }

        public string GetDigits(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            var digits = new StringBuilder();
            foreach (var c in s.ToCharArray())
                if (Char.IsDigit(c))
                    digits.Append(c);
            return digits.ToString();
        }
        void phone_Go(object sender, EventArgs<string> e)
        {
            phone.Visible = false;

            var wc = new WebClient();
            var url = new Uri(new Uri(ServiceUrl()),
                string.Format("Checkin/Match/{0}", GetDigits(e.Value)));

            this.Cursor = Cursors.WaitCursor;
            Cursor.Show();
            //var str = wc.UploadString(url, Program.CampusArg);
            var str = wc.DownloadString(url + Program.QueryString);
            if (Program.HideCursor)
                Cursor.Hide();
            this.Cursor = Cursors.Default;

            var x = XDocument.Parse(str);

            if (x.Document.Root.Name == "Families")
            {
                //ChooseFamily(x);
                families = new Families();
                this.Controls.Add(families);
                families.Left = (this.Width / 2) - (families.Width / 2);
                families.Top = 0;
                families.ShowFamilies(x);
                families.GoBack += new EventHandler(families_GoBack);
                families.Go += new EventHandler<EventArgs<int>>(families_Go);
            }
            else
            {
                attendees = new Attendees();
                this.Controls.Add(attendees);
                attendees.Left = (this.Width / 2) - (attendees.Width / 2);
                attendees.Top = 0;
                attendees.FindAttendees(x);
                attendees.GoBack += new EventHandler(attendees_GoBack);
            }
        }
        void families_Go(object sender, EventArgs<int> e)
        {
            this.Controls.Remove(families);
            families = null;

            var wc = new WebClient();
            var url = new Uri(new Uri(ServiceUrl()),
                string.Format("Checkin/Family/{0}", e.Value));

            this.Cursor = Cursors.WaitCursor;
            Cursor.Show();
            //var str = wc.UploadString(url, Program.CampusArg);
            var str = wc.DownloadString(url + Program.QueryString);
            if (Program.HideCursor)
                Cursor.Hide();
            this.Cursor = Cursors.Default;

            var x = XDocument.Parse(str);

            attendees = new Attendees();
            this.Controls.Add(attendees);
            attendees.Left = (this.Width / 2) - (attendees.Width / 2);
            attendees.Top = 0;
            attendees.FindAttendees(x);
            attendees.GoBack += new EventHandler(attendees_GoBack);
        }
        public static string ServiceUrl()
        {
            string serviceurl = ConfigurationSettings.AppSettings["ServiceUrl"];
            if (Program.TestMode)
                serviceurl = ConfigurationSettings.AppSettings["ServiceUrlTest"];
            return serviceurl;
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            phone.Left = (this.Width / 2) - (phone.Width / 2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            phone = new PhoneNumber();
            this.Controls.Add(phone);
            phone.Go += new EventHandler<EventArgs<string>>(phone_Go);
            phone.Left = (this.Width / 2) - (phone.Width / 2);
            phone.Top = 0;
            this.Resize += new EventHandler(Form1_Resize);
            if (Program.HideCursor)
                Cursor.Hide();
        }

        void attendees_GoBack(object sender, EventArgs e)
        {
            this.Controls.Remove(attendees);
            attendees = null;
            phone.Visible = true;
            phone.textBox1.Text = String.Empty;
            phone.textBox1.Focus();
        }
        void families_GoBack(object sender, EventArgs e)
        {
            this.Controls.Remove(families);
            families = null;
            phone.Visible = true;
            phone.textBox1.Text = String.Empty;
            phone.textBox1.Focus();
        }
    }
}
