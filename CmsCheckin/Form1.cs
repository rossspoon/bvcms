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
            var url = new Uri(new Uri(ConfigurationSettings.AppSettings["ServiceUrl"]),
                "Checkin/Match/" + GetDigits(e.Value));
            this.Cursor = Cursors.WaitCursor;
            var str = wc.DownloadString(url);
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
            var url = new Uri(new Uri(ConfigurationSettings.AppSettings["ServiceUrl"]),
                "Checkin/Family/" + e.Value);
            this.Cursor = Cursors.WaitCursor;
            var str = wc.DownloadString(url);
            this.Cursor = Cursors.Default;

            var x = XDocument.Parse(str);

            attendees = new Attendees();
            this.Controls.Add(attendees);
            attendees.Left = (this.Width / 2) - (attendees.Width / 2);
            attendees.Top = 0;
            attendees.FindAttendees(x);
            attendees.GoBack += new EventHandler(attendees_GoBack);
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
        }

        void attendees_GoBack(object sender, EventArgs e)
        {
            this.Controls.Remove(attendees);
            attendees = null;
            phone.Visible = true;
            phone.textBox1.Text = String.Empty;
        }
        void families_GoBack(object sender, EventArgs e)
        {
            this.Controls.Remove(families);
            families = null;
            phone.Visible = true;
            phone.textBox1.Text = String.Empty;
        }


            //try
            //{
            //    pfont = new Font("Arial", 10);
            //    var pd = new PrintDocument();
            //    pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
            //    var ps = new PageSettings();
            //    ps.PaperSize = new PaperSize("label", 300, 100);
            //    ps.Margins = new Margins(13, 0, 0, 0);
            //    pd.DefaultPageSettings = ps;
            //    foreach (var c in labels)
            //    {
            //        this.c = c;
            //        pd.Print();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        //private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        //{
        //    int row = 0;
        //    float left = ev.MarginBounds.Left;
        //    float top = ev.MarginBounds.Top;
        //    var h = pfont.GetHeight(ev.Graphics);
        //    var sf = new StringFormat();

        //    ev.Graphics.DrawString(c.Name + " (" + c.PeopleId + ") " + c.Gender,
        //        pfont, Brushes.Black, left, top + (row * h), sf);
        //    row++;
        //    ev.Graphics.DrawString(c.Birthday + " (" + c.Age + ")",
        //        pfont, Brushes.Black, left, top + (row * h), sf);
        //    row++;
        //    ev.Graphics.DrawString(c.Class,
        //        pfont, Brushes.Black, left, top + (row * h), sf);
        //    row++;
        //    ev.HasMorePages = false;
        //}
        

        //private void button1_Click(object sender, EventArgs e)
        //{

        //    var memStrm = new MemoryStream();
        //    var sw = new StreamWriter(memStrm);
        //    sw.WriteLine("\x02L");
        //    sw.WriteLine("H07");
        //    sw.WriteLine("D11");
        //    sw.WriteLine("19110080200002510K Ny linje");
        //    sw.WriteLine("19110080100002510K OHM 1/4 WATT");
        //    sw.Flush();
        //    sw.WriteLine("1a6210000000050590PCS");
        //    sw.WriteLine("E");
        //    sw.WriteLine("");
        //    sw.Flush();

        //    memStrm.Position = 0;
        //    RawPrinterHelper.SendDocToPrinter(printer, memStrm);
        //    sw.Close();
        //}
    }
}
