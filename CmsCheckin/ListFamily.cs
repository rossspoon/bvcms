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
    public partial class ListFamily : UserControl
    {
        public ListFamily()
        {
            InitializeComponent();
        }

        private string printer = ConfigurationSettings.AppSettings["PrinterName"];
        private bool hasprinter;
        private int Row;
        DateTime time;
        List<Control> controls = new List<Control>();
        List<Control> sucontrols = new List<Control>();

        public void ShowFamily(int fid)
        {
            var x = this.GetDocument("Checkin/Family/" + fid + Program.QueryString);
            ShowFamily(x);
        }
        public void ShowFamily(XDocument x)
        {
            ClearControls();
            hasprinter = PrintRawHelper.HasPrinter(printer);
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
            Print.Text = "Print Labels, Return";
            if (x.Descendants("attendee").Count() == 0)
            {
                var lab = new Label();
                lab.Font = pfont;
                lab.Location = new Point(15, 200);
                lab.AutoSize = true;
                lab.Text = "Not Found, try another phone number?";
                this.Controls.Add(lab);
                Print.Text = "Try again";
                controls.Add(lab);
                return;
            }

            var cols = new int[6];

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
                    wids[n] = Math.Max(wids[n], buttonwidth/2);
                    n++;

                    maxheight = Math.Max(maxheight, (int)size.Height);
                }
                for (var i = 1; i < 6; i++)
                    cols[i] = cols[i - 1] + wids[i - 1] + sep;
                cols[5] -= sep;
                if (cols[5] > 1024)
                {
                    points -= 1F;
                    continue;
                }
                break;
            }
            Row = 0;
            var col = 0;
            var LeftEdge = (1024 - cols[5]) / 2;
            var labtop = top - rowheight;

            var head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = Present;
            this.Controls.Add(head);
            controls.Add(head);
            col++;

            head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = "Name";
            this.Controls.Add(head);
            controls.Add(head);
            col++;

            head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = "Meeting";
            this.Controls.Add(head);
            controls.Add(head);
            col++;

            head = new Label();
            head.Size = new Size(wids[col] + sep, maxheight);
            head.Font = labfont;
            head.Location = new Point(LeftEdge + cols[col], labtop);
            head.Text = "Labels";
            this.Controls.Add(head);
            controls.Add(head);
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
                controls.Add(ab);

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
                controls.Add(label);
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
                controls.Add(label);
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
                controls.Add(eb);
                col++;

                var cb = new Button();
                cb.BackColor = Color.LightGray;
                cb.Enabled = false;
                cb.Font = pfont;
                cb.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight) - 5);
                cb.Name = "visit" + Row;
                cb.Tag = Row;
                cb.Size = new Size(buttonwidth/2, buttonheight/2);
                cb.TextAlign = ContentAlignment.TopCenter;
                cb.UseVisualStyleBackColor = false;
                this.Controls.Add(cb);
                cb.Click += new EventHandler(cb_Click);
                controls.Add(cb);
                sucontrols.Add(cb);
                col++;

                Row++;
            }
        }

        void ab_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 27)
                this.GoHome(string.Empty);
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
            this.Swap(Program.classes);
            Program.classes.ShowResults(c.cinfo.PeopleId, 1);
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            PrintLabels();
            this.GoHome(string.Empty);
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
                    CmsCheckin.Print.Label(c, n, time);
                    printed = true;
                }
            }
            if (printed)
                CmsCheckin.Print.BlankLabel();
        }
        private void ClearControls()
        {
            foreach (var c in controls)
            {
                this.Controls.Remove(c);
                c.Dispose();
            }
            controls.Clear();
        }



        private void AttendeeKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                this.GoHome(string.Empty);
            if (e.KeyChar == 13)
            {
                PrintLabels();
                this.GoHome(string.Empty);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var c in sucontrols)
            {
                c.Enabled = true;
                c.BackColor = Color.Coral;
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
