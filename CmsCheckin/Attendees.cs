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
        public Attendees()
        {
            InitializeComponent();
        }

        const string printer = "Datamax E-4203";
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
                Print.Text = "Return to Phone Number";
                return;
            }
            Print.Text = "Print labels and return";

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
                wids = new int[4];
                maxheight = 0;
                foreach (var e in x.Descendants("attendee").Take(MaxRows))
                {
                    var size = g.MeasureString(Present, labfont);
                    wids[0] = Math.Max(wids[0], Math.Max((int)size.Width, buttonwidth));

                    size = g.MeasureString(Labels, labfont);
                    wids[1] = Math.Max(wids[1], Math.Max((int)size.Width, buttonwidth));

                    size = g.MeasureString(e.Attribute("name").Value, font);
                    wids[2] = Math.Max(wids[2], (int)size.Width);

                    size = g.MeasureString(e.Attribute("org").Value, font);
                    wids[3] = Math.Max(wids[3], (int)g.MeasureString(e.Attribute("org").Value, font).Width);

                    size = g.MeasureString("|", font);
                    maxheight = Math.Max(maxheight, (int)size.Height);
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
            head.Text = "Labels";
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

            foreach (var e in x.Descendants("attendee").Take(MaxRows))
            {
                col = 0;

                var ab = new Button();
                ab.BackColor = SystemColors.ControlLight;
                ab.Font = new Font("Wingdings", 28, FontStyle.Regular,
                    GraphicsUnit.Point, ((byte)(2)));
                ab.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight) - 5);
                ab.Name = "attend" + Row;
                var c = new AttendLabel
                {
                    Name = e.Attribute("name").Value,
                    Birthday = e.Attribute("bday").Value,
                    Gender = e.Attribute("gender").Value,
                    PeopleId = Convert.ToInt32(e.Attribute("id").Value),
                    Class = e.Attribute("org").Value,
                    OrgId = int.Parse(e.Attribute("orgid").Value),
                    NumLabels = int.Parse(e.Attribute("numlabels").Value),
                    Row = Row,
                    CheckedIn = bool.Parse(e.Attribute("checkedin").Value)
                };
                ab.Tag = c;
                ab.Size = new Size(buttonwidth, buttonheight);
                ab.TextAlign = ContentAlignment.TopCenter;
                ab.UseVisualStyleBackColor = false;
                this.Controls.Add(ab);
                ab.KeyDown += new KeyEventHandler(ab_KeyDown);
                ab.Click += new EventHandler(ab_Click);
                ab.Text = c.CheckedIn ? "ü" : String.Empty;
                col++;

                var eb = new Button();
                eb.BackColor = Color.FloralWhite;
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
                //eb.KeyPress += new KeyPressEventHandler(KeyPressEvent);
                col++;

                var label = new Label();
                label.Font = font;
                label.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight));
                label.Size = new Size(wids[col] + sep, maxheight);
                label.Text = e.Attribute("name").Value;
                label.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(label);
                col++;

                label = new Label();
                label.Font = font;
                label.Location = new Point(LeftEdge + cols[col], top + (Row * rowheight));
                label.Size = new Size(wids[col] + sep, maxheight);
                label.Text = e.Attribute("org").Value;
                label.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(label);
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
            if (c.Clicked == false && c.CheckedIn == false && c.OrgId > 0)
                return;

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
            if (c.OrgId == 0)
                return;
            if (ab.Text == String.Empty)
            {
                ab.Text = "ü";
                eb.Text = c.NumLabels.ToString();
                RecordAttend(c, true);
            }
            else
            {
                ab.Text = String.Empty;
                eb.Text = String.Empty;
                RecordAttend(c, false);
            }
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
                if (int.TryParse(eb.Text, out n))
                    for (var i = 0; i < n; i++)
                    {
                        PrintLabel(c);
                        printed = true;
                    }
            }
            if (printed)
                PrintBlankLabel();
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
            coll.Add("thisday", Program.ThisDay.ToString());
            var url = new Uri(new Uri(Form1.ServiceUrl()), "Checkin/RecordAttend/");

            this.Cursor = Cursors.WaitCursor;
            Cursor.Show();
            var resp = wc.UploadValues(url, "POST", coll);
            if (Program.HideCursor)
                Cursor.Hide();
            this.Cursor = Cursors.Default;

            var s = Encoding.ASCII.GetString(resp);
            this.Cursor = Cursors.Default;
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
        void PrintLabel(AttendLabel c)
        {
            if (!hasprinter)
                return;
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
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public int OrgId { get; set; }
        public int NumLabels { get; set; }
        public int Row { get; set; }
        public bool CheckedIn { get; set; }
        public bool Clicked { get; set; }
    }
}
