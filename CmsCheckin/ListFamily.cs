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
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.PageUp:
                        if (pgup.Visible)
                            ShowFamily(Program.FamilyId, prev.Value);
                        return true;
                    case Keys.PageDown:
                        if (pgdn.Visible)
                            ShowFamily(Program.FamilyId, next.Value);
                        return true;
                    case Keys.Escape:
                        Program.TimerStop();
                        this.GoHome(string.Empty);
                        return true;
                    case Keys.Return:
                        Program.TimerStop();
                        PrintLabels();
                        this.GoHome(string.Empty);
                        return true;
                    case Keys.S | Keys.Alt:
                        Program.TimerReset();
                        Program.CursorShow();
                        foreach (var c in sucontrols)
                        {
                            c.Enabled = true;
                            c.BackColor = Color.Coral;
                        }
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool hasprinter;
        private int Row;
        DateTime time;
        int? next, prev;
        List<Control> controls = new List<Control>();
        List<Control> sucontrols = new List<Control>();

        public void ShowFamily(int fid, int page)
        {
            var x = this.GetDocument("Checkin/Family/" + fid + Program.QueryString + "&page=" + page);
            ShowFamily(x);
        }
        public void ShowFamily(XDocument x)
        {
            ClearControls();
            hasprinter = PrintRawHelper.HasPrinter(Program.Printer);
            this.Focus();
            time = DateTime.Now;

            Program.FamilyId = x.Root.Attribute("familyid").Value.ToInt();
            next = x.Root.Attribute("next").Value.ToInt2();
            prev = x.Root.Attribute("prev").Value.ToInt2();
            pgdn.Visible = next.HasValue;
            pgup.Visible = prev.HasValue;
            Program.MaxLabels = x.Root.Attribute("maxlabels").Value.ToInt();

            var points = 14F;
            const int sep = 10;
            const int rowheight = 50;
            int top = 50;
            const int bsize = 45;
            const int bwid = 65;

            string Verdana = "Verdana";
            var pfont = new Font(Verdana, points, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            Font font;
            Font labfont;
            string Present = "Attend";
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

            int maxheight;
            int twidab, widname, widorg, twidlb;
            int totalwid;
            while (true)
            {
                twidab = widname = widorg = twidlb = maxheight = 0;
                font = new Font(Verdana, points, FontStyle.Regular,
                    GraphicsUnit.Point, ((byte)(0)));
                labfont = new Font(Verdana, points,
                    ((FontStyle)((FontStyle.Italic | FontStyle.Underline))),
                    GraphicsUnit.Point, ((byte)(0)));
                maxheight = 0;
                foreach (var e in x.Descendants("attendee"))
                {
                    var size = g.MeasureString(Present, labfont);
                    twidab = Math.Max(twidab, (int)Math.Ceiling(size.Width));
                    twidab = Math.Max(twidab, bwid);

                    size = g.MeasureString(e.Attribute("name").Value, font);
                    widname = Math.Max(widname, (int)Math.Ceiling(size.Width));

                    size = g.MeasureString(e.Attribute("org").Value, font);
                    widorg = Math.Max(widorg, (int)Math.Ceiling(size.Width));

                    size = g.MeasureString(Labels, labfont);
                    twidlb = Math.Max(twidlb, (int)Math.Ceiling(size.Width));
                    twidlb = Math.Max(twidlb, bsize);

                    size = g.MeasureString("|", labfont);
                    maxheight = Math.Max(maxheight, (int)Math.Ceiling(size.Height));
                }

                totalwid = sep + twidab + sep + widname + sep + widorg
                                    + sep + twidlb + sep + bsize + sep + bsize + sep;
                if (totalwid > 1024)
                {
                    points -= 1F;
                    continue;
                }
                break;
            }
            var labtop = top - rowheight;
            var LeftEdge = (1024 - totalwid) / 2;

            var head = new Label();
            LeftEdge += sep;
            head.Location = new Point(LeftEdge, labtop);
            head.Size = new Size(twidab + 5, maxheight);
            head.Font = labfont;
            head.Text = Present;
            this.Controls.Add(head);
            controls.Add(head);

            head = new Label();
            LeftEdge += twidab + sep;
            head.Location = new Point(LeftEdge, labtop);
            head.Size = new Size(widname + 5, maxheight);
            head.Font = labfont;
            head.Text = "Name";
            this.Controls.Add(head);
            controls.Add(head);

            head = new Label();
            LeftEdge += widname + sep;
            head.Location = new Point(LeftEdge, labtop);
            head.Size = new Size(widorg + 5, maxheight);
            head.Font = labfont;
            head.Text = "Meeting";
            this.Controls.Add(head);
            controls.Add(head);

            head = new Label();
            LeftEdge += widorg + sep;
            head.Location = new Point(LeftEdge, labtop);
            head.Size = new Size(twidlb + 5, maxheight);
            head.Font = labfont;
            head.Text = "Labels";
            this.Controls.Add(head);
            controls.Add(head);

            Row = 0;
            foreach (var e in x.Descendants("attendee"))
            {
                LeftEdge = (1024 - totalwid) / 2;
                top += rowheight;

                var c = new AttendLabel
                {
                    cinfo = new ClassInfo
                    {
                        oid = e.Attribute("orgid").Value.ToInt(),
                        pid = e.Attribute("id").Value.ToInt(),
                        mv = e.Attribute("mv").Value,
                    },
                    name = e.Attribute("name").Value,
                    first = e.Attribute("first").Value,
                    last = e.Attribute("last").Value,
                    dob = e.Attribute("dob").Value,

                    goesby = e.Attribute("goesby").Value,
                    email = e.Attribute("email").Value,
                    addr = e.Attribute("addr").Value,
                    zip = e.Attribute("zip").Value,
                    home = e.Attribute("home").Value.FmtFone(),
                    cell = e.Attribute("cell").Value.FmtFone(),
                    gender = e.Attribute("gender").Value.ToInt(),
                    marital = e.Attribute("marital").Value.ToInt(),

                    @class = e.Attribute("org").Value,
                    NumLabels = int.Parse(e.Attribute("numlabels").Value),
                    Row = Row,
                    CheckedIn = bool.Parse(e.Attribute("checkedin").Value),
                    leadtime = double.Parse(e.Attribute("leadtime").Value)
                };

                var ab = new Button();
                LeftEdge += sep;
                ab.Location = new Point(LeftEdge, top - 5);
                ab.Size = new Size(bwid, bsize);

                ab.FlatStyle = FlatStyle.Flat;
                ab.FlatAppearance.BorderSize = 1;

                ab.BackColor = Color.CornflowerBlue;
                ab.FlatAppearance.BorderColor = Color.Black;

                double howlate  = -(Program.EarlyCheckin / 60d);
                if (c.cinfo.oid == 0
                    || c.leadtime > Program.LeadTime
                    || c.leadtime < howlate)
                {
                    ab.Enabled = false;
                    ab.BackColor = SystemColors.Control;
                    ab.FlatAppearance.BorderColor = SystemColors.ButtonShadow;
                }

                ab.ForeColor = Color.White;
                ab.Font = new Font("Wingdings", 24, FontStyle.Bold,
                    GraphicsUnit.Point, ((byte)(2)));
                ab.Name = "attend" + Row;
                ab.TextAlign = ContentAlignment.TopCenter;
                ab.UseVisualStyleBackColor = false;
                this.Controls.Add(ab);
                ab.KeyDown += new KeyEventHandler(ab_KeyDown);
                ab.Click += new EventHandler(ab_Click);
                ab.Text = c.CheckedIn ? "ü" : String.Empty;
                ab.Tag = c;
                controls.Add(ab);

                var label = new Label();
                LeftEdge += twidab + sep;
                label.Location = new Point(LeftEdge, top);
                label.Size = new Size(widname + 5, maxheight);

                label.Font = font;
                label.UseMnemonic = false;
                label.Text = c.name;
                label.TextAlign = ContentAlignment.MiddleLeft;
                if (c.cinfo.oid != 0)
                    if (c.cinfo.mv == "V")
                        label.ForeColor = Color.DarkGreen;
                    else
                        label.ForeColor = Color.Blue;
                this.Controls.Add(label);
                controls.Add(label);

                label = new Label();
                LeftEdge += widname + sep;
                label.Location = new Point(LeftEdge, top);
                label.Size = new Size(widorg + 5, maxheight);

                label.Font = font;
                label.UseMnemonic = false;
                label.Text = c.@class;
                label.TextAlign = ContentAlignment.MiddleLeft;
                if (c.cinfo.oid != 0)
                    if (c.cinfo.mv == "V")
                        label.ForeColor = Color.DarkGreen;
                    else
                        label.ForeColor = Color.Blue;
                this.Controls.Add(label);
                controls.Add(label);

                var lb = new Button();
                LeftEdge += widorg + sep;
                lb.Location = new Point(LeftEdge, top - 5);
                lb.Size = new Size(bwid, bsize);

                lb.BackColor = Color.Yellow;
                lb.Font = pfont;
                lb.Name = "print" + Row;
                lb.Tag = Row;
                lb.TextAlign = ContentAlignment.TopCenter;
                lb.UseVisualStyleBackColor = false;
                this.Controls.Add(lb);
                lb.Click += new EventHandler(lb_Click);
                controls.Add(lb);
                controls.Add(label);

                var cb = new Button();
                LeftEdge += twidlb + sep;
                cb.Location = new Point(LeftEdge, top - 5);
                cb.Size = new Size(bsize, bsize);
                cb.Text = "c";
                cb.BackColor = SystemColors.Control;
                cb.Enabled = false;
                cb.Font = pfont;
                cb.Name = "visit" + Row;
                cb.Tag = Row;
                cb.TextAlign = ContentAlignment.TopCenter;
                cb.UseVisualStyleBackColor = false;
                this.Controls.Add(cb);
                cb.Click += new EventHandler(cb_Click);
                controls.Add(cb);
                sucontrols.Add(cb);

                var eb = new Button();
                LeftEdge += bsize + sep;
                eb.Location = new Point(LeftEdge, top - 5);
                eb.Size = new Size(bsize, bsize);
                eb.Text = "e";
                eb.BackColor = SystemColors.Control;
                eb.Enabled = false;
                eb.Font = pfont;
                eb.Name = "edit" + Row;
                eb.Tag = Row;
                eb.TextAlign = ContentAlignment.TopCenter;
                eb.UseVisualStyleBackColor = false;
                this.Controls.Add(eb);
                eb.Click += new EventHandler(eb_Click);
                controls.Add(eb);
                sucontrols.Add(eb);

                Row++;
            }
            Program.TimerStart(timer1_Tick);
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            Program.TimerStop();
            Program.ClearFields();
            this.GoHome("");
        }

        void ab_KeyDown(object sender, KeyEventArgs e)
        {
            Program.TimerReset();
            if (e.KeyValue == 27)
            {
                Program.TimerStop();
                this.GoHome(string.Empty);
            }
        }

        void lb_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
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
            Program.TimerReset();
            var ab = sender as Button;
            var c = ab.Tag as AttendLabel;
            c.Clicked = true;
            var eb = this.Controls[this.Controls.IndexOfKey("print" + c.Row.ToString())] as Button;
            if (c.cinfo.oid == 0)
                return;
            Cursor.Current = Cursors.WaitCursor;
            Program.CursorShow();
            if (ab.Text == String.Empty)
            {
                ab.Text = "ü";
                eb.Text = c.NumLabels.ToString();
                var ra = new Util.RecordAttendInfo { c = c.cinfo, present = true };
                backgroundWorker1.RunWorkerAsync(ra);
            }
            else
            {
                ab.Text = String.Empty;
                eb.Text = String.Empty;
                var ra = new Util.RecordAttendInfo { c = c.cinfo, present = false };
                backgroundWorker1.RunWorkerAsync(ra);
            }
        }

        void cb_Click(object sender, EventArgs e)
        {
            var cb = sender as Button;
            var ab = this.Controls[this.Controls.IndexOfKey("attend" + cb.Tag.ToString())] as Button;
            var c = ab.Tag as AttendLabel;
            this.Swap(Program.classes);
            Program.classes.ShowResults(c.cinfo.pid, 1);
        }

        void eb_Click(object sender, EventArgs e)
        {
            var eb = sender as Button;
            var ab = this.Controls[this.Controls.IndexOfKey("attend" + eb.Tag.ToString())] as Button;
            var c = ab.Tag as AttendLabel;

            Program.PeopleId = c.cinfo.pid;
            Program.SetFields(c.last, c.email, c.addr, c.zip, c.home);
            Program.first.textBox1.Text = c.first;
            Program.goesby.textBox1.Text = c.goesby;
            Program.dob.textBox1.Text = c.dob;
            Program.cellphone.textBox1.Text = c.cell.FmtFone();
            Program.gendermarital.Marital = c.marital;
            Program.gendermarital.Gender = c.gender;

            Program.editing = true;
            this.Swap(Program.first);
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            Program.TimerStop();
            PrintLabels();
            Program.FamilyId = 0;
            this.GoHome(string.Empty);
        }
        private void PrintLabels()
        {
            var list = new List<AttendLabel>();
            for (var r = 0; r < Row; r++)
            {
                var eb = this.Controls[this.Controls.IndexOfKey("print" + r.ToString())] as Button;
                var ab = this.Controls[this.Controls.IndexOfKey("attend" + r.ToString())] as Button;
                var c = ab.Tag as AttendLabel;
                var n = 0;
                if (int.TryParse(eb.Text, out n) && n > 0)
                {
                    c.NumLabels = n;
                    list.Add(c);
                }
            }
            var q = from c in list
                    group c by new { c.cinfo.pid, c.cinfo.mv } into g
                    select new LabelInfo
                    {
                        pid = g.Key.pid,
                        mv = g.Key.mv,
                        n = g.Sum(i => i.NumLabels),
                        first = g.First().first,
                        last = g.First().last,
                    };
            foreach (var li in q)
                CmsCheckin.Print.Label(li, time);
            if (q.Sum(li => li.n) > 0)
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
            Row = 0;
            sucontrols.Clear();
            sucontrols.Add(bAddToFamily);
            bAddToFamily.BackColor = SystemColors.Control;
            bAddToFamily.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.TimerStop();
            foreach (var c in sucontrols)
            {
                c.Enabled = true;
                c.BackColor = Color.Coral;
            }
        }

        private void pgup_Click(object sender, EventArgs e)
        {
            ShowFamily(Program.FamilyId, prev.Value);
        }

        private void pgdn_Click(object sender, EventArgs e)
        {
            ShowFamily(Program.FamilyId, next.Value);
        }

        private void bAddToFamily_Click(object sender, EventArgs e)
        {
            var ab = this.Controls[this.Controls.IndexOfKey("attend0")] as Button;
            var c = ab.Tag as AttendLabel;

            Program.SetFields(c.last, c.email, c.addr, c.zip, c.home);
            Program.editing = false;
            this.Swap(Program.first);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var ra= e.Argument as Util.RecordAttendInfo;
            Util.RecordAttend(ra);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Cursor.Current = Cursors.Default;
            Program.CursorHide();
        }
    }
    public class AttendLabel
    {
        public ClassInfo cinfo { get; set; }
        public string name { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string dob { get; set; }

        public string goesby { get; set; }
        public string email { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string home { get; set; }
        public string cell { get; set; }

        public int gender { get; set; }
        public int marital { get; set; }

        public string @class { get; set; }
        public int NumLabels { get; set; }
        public int Row { get; set; }
        public bool CheckedIn { get; set; }
        public bool Clicked { get; set; }
        public double leadtime { get; set; }
    }
    public class LabelInfo
    {
        public int pid { get; set; }
        public string mv { get; set; }
        public int n { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }
}
