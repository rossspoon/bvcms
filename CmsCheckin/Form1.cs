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
            var wc = new WebClient();
            Uri url;
            string str;
            XDocument x;
            if (e.Value.StartsWith("0"))
            {
                url = new Uri(new Uri(ServiceUrl()),
                    string.Format("Checkin/Class/{0}", GetDigits(e.Value.Substring(1))));
                str = wc.DownloadString(url + Program.QueryString);
                x = XDocument.Parse(str);
                var list = x.Root.Descendants("Name").Select(m => m.Value).ToList();
                var n = list.Count / 5;
                if (n % 5 > 0)
                    n++;
                n = n * 5 - 1;
                for(;n > 0;n -= 5)
                PrintLabel5(list, n);

                list = new List<string>();
                list.Add(x.Root.Attribute("Name").Value);
                list.Add(x.Root.Attribute("Teacher").Value);
                list.Add(x.Root.Attribute("Date").Value);
                list.Add(x.Root.Attribute("Time").Value);
                list.Add("Count: " + x.Root.Attribute("Count").Value);
                PrintLabel5(list, 4);

                PrintBlankLabel();
                return;
            }
            phone.Visible = false;

            url = new Uri(new Uri(ServiceUrl()),
                string.Format("Checkin/Match/{0}", GetDigits(e.Value)));

            this.Cursor = Cursors.WaitCursor;
            Cursor.Show();
            //var str = wc.UploadString(url, Program.CampusArg);
            str = wc.DownloadString(url + Program.QueryString);
            if (Program.HideCursor)
                Cursor.Hide();
            this.Cursor = Cursors.Default;

            x = XDocument.Parse(str);

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
        void PrintLabel5(List<string> list, int n)
        {
            string printer = ConfigurationSettings.AppSettings["PrinterName"];
            if (!RawPrinterHelper.HasPrinter(printer))
                return;
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            sw.WriteLine("\x02n");
            sw.WriteLine("\x02M0500");
            sw.WriteLine("\x02O0220");
            sw.WriteLine("\x02V0");
            sw.WriteLine("\x02SG");
            sw.WriteLine("\x02d");
            sw.WriteLine("\x01D");
            sw.WriteLine("\x02L");
            sw.WriteLine("D11");
            sw.WriteLine("PG");
            sw.WriteLine("pC");
            sw.WriteLine("SG");
            sw.WriteLine("ySPM");
            sw.WriteLine("A2");
            if (list.Count > n)
                sw.WriteLine("1911A1000040010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000210010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000370010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000540010" + list[n]);
            n--;
            if (list.Count > n)
                sw.WriteLine("1911A1000700010" + list[n]);
            n--;
            sw.WriteLine("Q0001");
            sw.WriteLine("E");
            sw.Flush();

            memStrm.Position = 0;
            RawPrinterHelper.SendDocToPrinter(printer, memStrm);
            sw.Close();
        }
        private void PrintBlankLabel()
        {
            string printer = ConfigurationSettings.AppSettings["PrinterName"];
            if (!RawPrinterHelper.HasPrinter(printer))
                return;
            var ms = new MemoryStream();
            var st = new StreamWriter(ms);
            st.WriteLine("\x02L");
            st.WriteLine("H07");
            st.WriteLine("D11");
            st.WriteLine("E");
            st.Flush();
            ms.Position = 0;
            RawPrinterHelper.SendDocToPrinter(printer, ms);
            st.Close();
        }

    }
}
