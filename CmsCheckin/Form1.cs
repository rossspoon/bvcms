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

namespace CmsCheckin
{
    public partial class Form1 : Form
    {
        Match match;
        PhoneNumber phone;
        Children children;
        public Form1()
        {
            InitializeComponent();
        }

        void phone_Go(object sender, EventArgs<string> e)
        {
            phone.Visible = false;
            match = new Match();
            this.Controls.Add(match);
            match.Left = (this.Width / 2) - (match.Width / 2);
            match.Top = 0;
            match.Search(e.Value);
            match.GoBack += new EventHandler(m_Done);
            match.Go += new EventHandler<EventArgs<int>>(match_Go);
        }

        void match_Go(object sender, EventArgs<int> e)
        {
            match.Visible = false;
            children = new Children();
            this.Controls.Add(children);
            children.Left = (this.Width / 2) - (children.Width / 2);
            children.Top = 0;
            children.FindChildren(e.Value);
            children.GoBack += new EventHandler(children_GoBack);
            children.Go += new EventHandler<EventArgs<List<ChildLabel>>>(children_Go);
        }

        void children_GoBack(object sender, EventArgs e)
        {
            this.Controls.Remove(children);
            children = null;
            match.Visible = true;
        }

        void m_Done(object sender, EventArgs e)
        {
            this.Controls.Remove(match);
            match = null;
            phone.Visible = true;
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

        string printer = "Datamax E-4203";

        List<ChildLabel> labels;
        DateTime time;

        void children_Go(object sender, EventArgs<List<ChildLabel>> e)
        {
            labels = e.Value;
            time = DateTime.Now;

            foreach (var c in labels)
            {
                PrintLabel(c);
                PrintLabel(c);
                if (c.Extra)
                    PrintLabel(c);
            }
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

            foreach (var c in labels)
                RecordAttend(c);

            this.Controls.Remove(children);
            children = null;
            phone.Visible = true;
            phone.textBox1.Text = String.Empty;
        }

        private void PrintLabel(ChildLabel c)
        {
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            sw.WriteLine("\x02O0130");
            sw.WriteLine("\x02L");
            sw.WriteLine("H07");
            sw.WriteLine("D11");
            sw.WriteLine("191100300500015" + c.Name + " (" + c.PeopleId + " " + c.Gender + ")");
            sw.WriteLine("191100300300015" + c.Birthday + " (" + c.Age + ")    " + time.ToString("MMddHHmm-ss"));
            sw.WriteLine("191100300100015" + c.Class);
            sw.WriteLine("E");
            sw.Flush();

            memStrm.Position = 0;
            RawPrinterHelper.SendDocToPrinter(printer, memStrm);
            sw.Close();
        }
        private void RecordAttend(ChildLabel c)
        {
            if (c.OrgId == 0)
                return;
            var wc = new WebClient();
            var coll = new NameValueCollection();
            coll.Add("PeopleId", c.PeopleId.ToString());
            coll.Add("OrgId", c.OrgId.ToString());
            var url = new Uri(new Uri(ConfigurationSettings.AppSettings["ServiceUrl"]), 
                "Checkin/RecordAttend/");
            var resp = wc.UploadValues(url, "POST", coll);
            var s = Encoding.ASCII.GetString(resp);
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
