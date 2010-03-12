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
        public event EventHandler GoBack;
        public event EventHandler<EventArgs<int>> GoClasses;

        public Attendees()
        {
            InitializeComponent();
        }

        private string printer = ConfigurationSettings.AppSettings["PrinterName"];
        private bool hasprinter;
        private int Row;
        DateTime time;

        public void FindAttendees(XDocument x)
        {
            hasprinter = RawPrinterHelper.HasPrinter(printer);
            Print.Focus();
            time = DateTime.Now;

            var points = 14F;
            const int sep = 15;
            const int MaxRows = 13;
            const int rowheight = 50;
            const int top = 50;
            const int buttonheight = 45;
            const int buttonwidth = 65;

            string Verdana = "Verdana";
            var pfont = new Font(Verdana, points, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            //var font = new Font(Verdana, points, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            //var labfont = new Font(Verdana, points, ((FontStyle)((FontStyle.Italic | FontStyle.Underline))), GraphicsUnit.Point, ((byte)(0)));
            Font font;
            Font labfont;
            string Present = "Present";
            string Labels = "Labels";
            var g = this.CreateGraphics();
            if (x.Descendants("attendee").Count() == 0)
            {
                var lab = new Label();
                lab.Font = pfont;
                lab.Location = new Point(15, 200);
                lab.AutoSize = true;
                lab.Text = "Not Found, try another phone number?";
                this.Controls.Add(lab);
                Print.Text = "Try again";
                return;
            }

            var cols = new int[5];

            int[] wids;
            int maxheight;
            while (true)
            {
                font = new Font(Verdana, points, FontStyle.Regular,
                    GraphicsUnit.Point, ((byte)(0)));
                labfont = new Font(Verdana, points,
                    ((FontStyle)((FontStyle.Italic | FontStyle.Underline))),
                    GraphicsUnit.Point, ((byte)(0)));
                wids = new int[5];
                maxheight = 0;
                foreach (var e in x.Descendants("attendee").Take(MaxRows))
                {
                    var n = 0;

                    var size = g.MeasureString(Present, labfont);
                    wids[n] = Math.Max(wids[n], Math.Max((int)size.Width, buttonwidth));
                    n++;

                    size = g.MeasureString(e.Attribute("name").Value, font);
                    wids[n] = Math.Max(wids[n], (int)size.Width);
                    n++;

                    size = g.MeasureString(e.Attribute("org").Value, font);
                    wids[n] = Math.Max(wids[n], (int)g.MeasureString(e.Attribute("org").Value, font).Width);
                    n++;

                    size = g.MeasureString(Labels, labfont);
                    wids[n] = Math.Max(wids[n], Math.Max((int)size.Width, buttonwidth));
                    n++;

                    size = g.MeasureString("|", font);
                    maxheight = Math.Max(maxheight, (int)size.Height);
                    wids[n] = Math.Max(wids[n], Math.Max((int)size.Width, buttonwidth/2));
                    n++;

                    size = g.MeasureString("|", font);
                    maxheight = Math.Max(maxheight, (int)size.Height);
                    n++;
                }
                for (var i = 1; i < 5; i++)
                    cols[i] = cols[i - 1] + wids[i - 1] + sep;
                cols[4] -= sep;
                if (cols[4] > 1024)
                {
                    points -= 1F;
                    continue;
                }
                break;
            }
            Row = 0;
            var col = 0;
            var LeftEdge = (1024 - cols[4]) / 2;
            var labtop = top - rowheight;

            var head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = Present;
            this.Controls.Add(head);
            col++;

            head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = "Name";
            this.Controls.Add(head);
            col++;

            head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = "Meeting";
            this.Controls.Add(head);
            col++;

            head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = "Labels";
            this.Controls.Add(head);
            col++;

            foreach (var e in x.Descendants("attendee").Take(MaxRows))
            {
                col = 0;

                var c = new AttendLabel
                {
                    cinfo = new ClassInfo
                    {
                         OrgId = e.Attribute("orgid").Value.ToInt(),
                         PeopleId = e.Attribute("id").Value.ToInt()
                    },
                    Name = e.Attribute("name").Value,
                    First = e.Attribute("first").Value,
                    Last = e.Attribute("last").Value,
                    Birthday = e.Attribute("bday").Value,
                    Gender = e.Attribute("gender").Value,
                    Class = e.Attribute("org").Value,
                    NumLabels = int.Parse(e.Attribute("numlabels").Value),
                    Row = Row,
                    CheckedIn = bool.Parse(e.Attribute("checkedin").Value),
                    leadtime = double.Parse(e.Attribute("leadtime").Value)
                };

                var ab = new Button();
                ab.FlatStyle = FlatStyle.Flat;
                ab.FlatAppearance.BorderSize = 1;
                if (c.cinfo.OrgId != 0 && c.leadtime <= Program.LeadTime)
                {
                    ab.BackColor = Color.CornflowerBlue;
                    ab.FlatAppearance.BorderColor = Color.Black;
                }
                else
                {
                    ab.Enabled = false;
                    ab.FlatAppearance.BorderColor = SystemColors.ButtonShadow;
                }
                ab.ForeColor = Color.White;
                ab.Font = new Font("Wingdings", 24, FontStyle.Bold,
                    GraphicsUnit.Point, ((byte)(2)));
                ab.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight) - 5);
                ab.Name = "attend" + Row;
                ab.Size = new Size(buttonwidth, buttonheight);
                ab.TextAlign = ContentAlignment.TopCenter;
                ab.UseVisualStyleBackColor = false;
                this.Controls.Add(ab);
                ab.KeyDown += new KeyEventHandler(ab_KeyDown);
                ab.Click += new EventHandler(ab_Click);
                ab.Text = c.CheckedIn ? "ü" : String.Empty;
                ab.Tag = c;
                col++;

                var label = new Label();
                label.Font = font;
                label.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight));
                label.Size = new Size(wids[col] + sep, maxheight);
                label.UseMnemonic = false;
                label.Text = c.Name;
                label.TextAlign = ContentAlignment.MiddleLeft;
                if (c.cinfo.OrgId != 0)
                    label.ForeColor = Color.Blue;
                this.Controls.Add(label);
                col++;

                label = new Label();
                label.Font = font;
                label.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight));
                label.Size = new Size(wids[col] + sep, maxheight);
                label.UseMnemonic = false;
                label.Text = c.Class;
                label.TextAlign = ContentAlignment.MiddleLeft;
                if (c.cinfo.OrgId != 0)
                    label.ForeColor = Color.Blue;
                this.Controls.Add(label);
                col++;

                var eb = new Button();
                eb.BackColor = Color.Yellow;
                eb.Font = pfont;
                eb.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight) - 5);
                eb.Name = "print" + Row;
                eb.Tag = Row;
                eb.Size = new Size(buttonwidth, buttonheight);
                eb.TextAlign = ContentAlignment.TopCenter;
                eb.UseVisualStyleBackColor = false;
                this.Controls.Add(eb);
                eb.KeyPress += new KeyPressEventHandler(AttendeeKeyPress);
                eb.Click += new EventHandler(eb_Click);
                col++;

                var cb = new Button();
                cb.BackColor = Color.Coral;
                cb.Font = pfont;
                cb.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight) - 5);
                cb.Name = "visit" + Row;
                cb.Tag = Row;
                cb.Size = new Size(buttonwidth/2, buttonheight/2);
                cb.TextAlign = ContentAlignment.TopCenter;
                cb.UseVisualStyleBackColor = false;
                this.Controls.Add(cb);
                cb.Click += new EventHandler(cb_Click);
                col++;

                Row++;
            }
        }

        void ab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
                GoBack(sender, e);
        }

        void eb_Click(object sender, EventArgs e)
        {
            var eb = sender as Button;
            var ab = this.Controls[this.Controls.IndexOfKey("attend" + eb.Tag.ToString())] as Button;
            var c = ab.Tag as AttendLabel;

            if (ab.Enabled == true && !c.Clicked && ab.Text == String.Empty)
            {
                ab_Click(ab, e);
                return;
            }

            var n = 0;
            if (int.TryParse(eb.Text, out n))
                n += 1;
            else
                n = 1;
            if (n == 6)
                n = 0;
            eb.Text = n.ToString();
        }

        void ab_Click(object sender, EventArgs e)
        {
            var ab = sender as Button;
            var c = ab.Tag as AttendLabel;
            c.Clicked = true;
            var eb = this.Controls[this.Controls.IndexOfKey("print" + c.Row.ToString())] as Button;
            if (c.cinfo.OrgId == 0)
                return;
            if (ab.Text == String.Empty)
            {
                ab.Text = "ü";
                eb.Text = c.NumLabels.ToString();
                Refresh();
                this.RecordAttend(c.cinfo, true);
            }
            else
            {
                ab.Text = String.Empty;
                eb.Text = String.Empty;
                Refresh();
                this.RecordAttend(c.cinfo, false);
            }
        }

        void cb_Click(object sender, EventArgs e)
        {
            var cb = sender as Button;
            var ab = this.Controls[this.Controls.IndexOfKey("attend" + cb.Tag.ToString())] as Button;
            var c = ab.Tag as AttendLabel;
            GoClasses(sender, new EventArgs<int>(c.cinfo.PeopleId));
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            PrintLabels();
            GoBack(sender, e);
        }
        private void PrintLabels()
        {
            var printed = false;
            for (var r = 0; r < Row; r++)
            {
                var eb = this.Controls[this.Controls.IndexOfKey("print" + r.ToString())] as Button;
                var ab = this.Controls[this.Controls.IndexOfKey("attend" + r.ToString())] as Button;
                var c = ab.Tag as AttendLabel;
                var n = 0;
                if (int.TryParse(eb.Text, out n) && n > 0)
                {
                    PrintLabel(c, n);
                    printed = true;
                }
            }
            if (printed)
                PrintBlankLabel();
        }

        private void PrintBlankLabel()
        {
            if (!hasprinter)
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
        void PrintLabel(AttendLabel c, int n)
        {
            if (!hasprinter || n == 0)
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
            sw.WriteLine("1911A3000450009" + c.First);
            sw.WriteLine("1911A1000300011" + c.Last);
            sw.WriteLine("1911A1000060008" + " (" + c.cinfo.PeopleId + " " + c.Gender + ")" + time.ToString("  M/d/yy"));
            sw.WriteLine("1911A2400040179" + time.ToString("HHmmss"));
            sw.WriteLine("Q" + n.ToString("0000"));
            sw.WriteLine("E");
            sw.Flush();

            memStrm.Position = 0;
            RawPrinterHelper.SendDocToPrinter(printer, memStrm);
            sw.Close();
        }


        private void AttendeeKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                GoBack(sender, e);
            if (e.KeyChar == 13)
            {
                PrintLabels();
                GoBack(sender, e);
            }
        }
    }
    public class AttendLabel
    {
        public ClassInfo cinfo { get; set; }
        public string Name { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public int NumLabels { get; set; }
        public int Row { get; set; }
        public bool CheckedIn { get; set; }
        public bool Clicked { get; set; }
        public double leadtime { get; set; }
    }
}
