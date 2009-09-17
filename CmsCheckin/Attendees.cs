using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;

namespace CmsCheckin
{
    public partial class Attendees : UserControl
    {
        public event EventHandler Go;
        public Attendees()
        {
            InitializeComponent();
        }

        string printer = "Datamax E-4203";
        DateTime time;
        bool printed;

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
        public void FindAttendees(string phone)
        {
            time = DateTime.Now;

            var wc = new WebClient();
            var url = new Uri(new Uri(ConfigurationSettings.AppSettings["ServiceUrl"]), 
                "Checkin/Match/" + GetDigits(phone));
            this.Cursor = Cursors.WaitCursor;

            //var coll = new NameValueCollection();
            //var resp = wc.UploadValues(url, "POST", coll);
            //var str = Encoding.ASCII.GetString(resp);
            var str = wc.DownloadString(url);

            this.Cursor = Cursors.Default;
            var x = XDocument.Parse(str);
            var font = new Font("Verdana", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            var g = this.CreateGraphics();
            if (x.Descendants("attendee").Count() == 0)
            {
                var lab = new Label();
                lab.Font = new Font("Verdana", 24F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                lab.Location = new Point(15, 200);
                lab.AutoSize = true;
                lab.Text = "Not Found";
                this.Controls.Add(lab);
                return;
            }
            var wids = new int[6];
            wids[0] = 65;
            wids[1] = 65;
            foreach (var e in x.Descendants("attendee").Take(10))
            {
                var name = string.Format("{0} ({1})", e.Attribute("name").Value, e.Attribute("age").Value);
                var n = 2;
                wids[n] = Math.Max(wids[n], (int)g.MeasureString(name, font).Width); n++;
                wids[n] = Math.Max(wids[n], (int)g.MeasureString(e.Attribute("org").Value, font).Width); n++;
                wids[n] = 15;
            }
            var cols = new int[5];
            cols[0] = 15;
            for (var i = 1; i < 4; i++)
                cols[i] = cols[i - 1] + 15 + wids[i];
            var top = 60;
            var row = 0;
            var col = 0;

            var labfont = new Font("Verdana", 15F, ((FontStyle)((FontStyle.Italic | FontStyle.Underline))), GraphicsUnit.Point, ((byte)(0)));
            var labtop = top - 50;

            var hpresent = new Label();
            hpresent.AutoSize = true;
            hpresent.Font = labfont;
            hpresent.Location = new Point(cols[col++], labtop);
            hpresent.Text = "Present";
            this.Controls.Add(hpresent);

            var hname = new Label();
            hname.AutoSize = true;
            hname.Font = labfont;
            hname.Location = new Point(cols[col++], labtop);
            hname.Text = "Name";
            this.Controls.Add(hname);

            var hclass = new Label();
            hclass.AutoSize = true;
            hclass.Font = labfont;
            hclass.Location = new Point(cols[col++], labtop);
            hclass.Text = "Class";
            this.Controls.Add(hclass);

            var hextra = new Label();
            hextra.AutoSize = true;
            hextra.Font = labfont;
            hextra.Location = new Point(cols[col++], labtop);
            hextra.Text = "Labels";
            this.Controls.Add(hextra);

            foreach (var e in x.Descendants("attendee").Take(13))
            {
                col = 0;

                var ab = new Button();
                ab.BackColor = SystemColors.ControlLight;
                ab.Font = new Font("Wingdings", 32.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(2)));
                ab.Location = new Point(cols[col++], top + (row * 50) - 5);
                ab.Name = "attend" + row;
                var c = new AttendLabel
                {
                    Name = e.Attribute("name").Value,
                    Age = Convert.ToInt32(e.Attribute("age").Value),
                    Birthday = e.Attribute("bday").Value,
                    Gender = e.Attribute("gender").Value,
                    PeopleId = Convert.ToInt32(e.Attribute("id").Value),
                    Class = e.Attribute("org").Value,
                    OrgId = Convert.ToInt32(e.Attribute("orgid").Value),
                    NumLabels = Convert.ToInt32(e.Attribute("numlabels").Value)
                };
                ab.Tag = c;
                ab.Size = new Size(65, 45);
                ab.TextAlign = ContentAlignment.TopCenter;
                ab.UseVisualStyleBackColor = false;
                this.Controls.Add(ab);
                ab.Click += new EventHandler(ab_Click);

                var name = new Label();
                name.Font = font;
                name.Location = new Point(cols[col++], top + (row * 50));
                name.AutoSize = true;
                name.Text = string.Format("{0} ({1})", e.Attribute("name").Value, e.Attribute("age").Value);
                name.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(name);

                var org = new Label();
                org.Font = font;
                org.Location = new Point(cols[col++], top + (row * 50));
                org.AutoSize = true;
                org.Text = e.Attribute("org").Value;
                org.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(org);

                var eb = new Button();
                eb.BackColor = Color.FloralWhite;
                eb.Font = font;
                eb.Location = new Point(cols[col++], top + (row * 50) - 5);
                eb.Name = "print" + row;
                eb.Text = "Print";
                eb.Tag = row;
                eb.Size = new Size(65, 45);
                eb.TextAlign = ContentAlignment.TopCenter;
                eb.UseVisualStyleBackColor = false;
                this.Controls.Add(eb);
                eb.Click += new EventHandler(eb_Click);

                row++;
            }
        }

        void eb_Click(object sender, EventArgs e)
        {
            var eb = sender as Button;
            var ab = this.Controls[this.Controls.IndexOfKey("attend" + eb.Tag.ToString())] as Button;
            var c = ab.Tag as AttendLabel;
            for (var i = 0; i < c.NumLabels; i++ )
                PrintLabel(c);
            c.NumLabels = 1;
            printed = true;
        }

        void ab_Click(object sender, EventArgs e)
        {
            var ab = sender as Button;
            var a = ab.Tag as AttendLabel;
            if (a.OrgId == 0)
                return;
            if (ab.Text == String.Empty)
            {
                ab.Text = "ü";
                RecordAttend(a, true);
            }
            else
            {
                ab.Text = String.Empty;
                RecordAttend(a, false);
            }
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            if (printed)
            {
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
            Go(sender, e);
        }
        private void RecordAttend(AttendLabel c, bool present)
        {
            if (c.OrgId == 0)
                return;
            this.Cursor = Cursors.WaitCursor;
            var wc = new WebClient();
            var coll = new NameValueCollection();
            coll.Add("PeopleId", c.PeopleId.ToString());
            coll.Add("OrgId", c.OrgId.ToString());
            coll.Add("Present", present.ToString());
            var url = new Uri(new Uri(ConfigurationSettings.AppSettings["ServiceUrl"]),
                "Checkin/RecordAttend/");
            var resp = wc.UploadValues(url, "POST", coll);
            var s = Encoding.ASCII.GetString(resp);
            this.Cursor = Cursors.Default;
        }
        

        void PrintLabel(AttendLabel c)
        {
            var memStrm = new MemoryStream();
            var sw = new StreamWriter(memStrm);
            sw.WriteLine("\x02O0130");
            sw.WriteLine("\x02L");
            sw.WriteLine("H07");
            sw.WriteLine("D11");
            sw.WriteLine("191100500400015" + c.Name);
            sw.WriteLine("191100300200015" + " (" + c.PeopleId + " " + c.Gender + ")  " + time.ToString("M/d/yy HHmmss"));
            //sw.WriteLine("191100300100015" + c.Class);
            sw.WriteLine("E");
            sw.Flush();

            memStrm.Position = 0;
            RawPrinterHelper.SendDocToPrinter(printer, memStrm);
            sw.Close();
        }

    }
    public class AttendLabel
    {
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public string Birthday { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public int OrgId { get; set; }
        public int NumLabels { get; set; }
    }
}
