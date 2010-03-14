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
    public partial class ListNames : UserControl
    {
        public ListNames()
        {
            InitializeComponent();
        }

        int? next, prev;
        string name;
        List<Control> controls = new List<Control>();
        List<Control> sucontrols = new List<Control>();

        public void ShowResults(string match, int page)
        {
            ClearControls();
            name = match;
            var x = this.GetDocument("Checkin/NameSearch/" + name + "?page=" + page);

            var points = 14F;

            string Verdana = "Verdana";
            Font labfont;
            var g = this.CreateGraphics();

            name = x.Root.Attribute("name").Value;
            next = x.Root.Attribute("next").Value.ToInt2();
            prev = x.Root.Attribute("prev").Value.ToInt2();
            pgdn.Visible = next.HasValue;
            pgup.Visible = prev.HasValue;

            if (x.Descendants("person").Count() == 0)
            {
                var lab = new Label();
                lab.Font = new Font(Verdana, points, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                lab.Location = new Point(15, 200);
                lab.AutoSize = true;
                lab.Text = "Not Found, try another name?";
                this.Controls.Add(lab);
                GoBackButton.Text = "Try again";
                return;
            }

            const int WidName = 935;
            while (true)
            {
                var wid = 0;
                labfont = new Font(Verdana, points, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                foreach (var e in x.Descendants("person"))
                {
                    var s = e.Attribute("display").Value;
                    var size = g.MeasureString(s, labfont);
                    wid = Math.Max(wid, (int)size.Width);
                }
                if (wid > WidName)
                {
                    points -= 1F;
                    continue;
                }
                break;
            }
            var row = 0;
            foreach (var e in x.Descendants("person"))
            {
                var ab = new Button();
                ab.BackColor = SystemColors.ControlLight;
                ab.Font = labfont;
                ab.Location = new Point(10, 100 + (row * 50));
                var homephone = e.Attribute("home").Value;
                var cellphone = e.Attribute("cell").Value;
                ab.Tag = !string.IsNullOrEmpty(homephone) ? homephone : !string.IsNullOrEmpty(cellphone) ? cellphone : "";
                ab.Size = new Size(WidName, 45);
                ab.TextAlign = ContentAlignment.MiddleLeft;
                ab.UseVisualStyleBackColor = false;
                ab.Text = e.Attribute("display").Value;
                this.Controls.Add(ab);
                ab.Click += new EventHandler(ab_Click);
                ab.KeyPress += new KeyPressEventHandler(ResultKeyPress);
                controls.Add(ab);

                var an = new Button();
                an.BackColor = Color.LightGray;
                an.Text = "a";
                an.Location = new Point(10 + WidName + 10, 100 + (row * 50));
                an.Tag = new AddFamilyInfo
                {
                    FamilyId = e.Attribute("fid").Value.ToInt(),
                    home = e.Attribute("home").Value.FmtFone(),
                    addr = e.Attribute("addr").Value,
                    zip = e.Attribute("zip").Value.FmtZip(),
                    last = e.Attribute("last").Value,
                    email = e.Attribute("email").Value,
                };
                an.Size = new Size(30, 30);
                this.Controls.Add(an);
                an.Click += new EventHandler(an_Click);
                an.Enabled = false;
                controls.Add(an);
                sucontrols.Add(an);

                var ed = new Button();
                ed.BackColor = Color.LightGray;
                ed.Text = "e";
                ed.Location = new Point(10 + WidName + 10 + 30 + 10, 100 + (row * 50));
                ed.Tag = new PersonInfo
                {
                    fid = e.Attribute("fid").Value.ToInt(),
                    home = e.Attribute("home").Value.FmtFone(),
                    cell = e.Attribute("cell").Value.FmtFone(),
                    addr = e.Attribute("addr").Value,
                    zip = e.Attribute("zip").Value.FmtZip(),
                    first = e.Attribute("first").Value,
                    email = e.Attribute("email").Value,
                    last = e.Attribute("last").Value,
                    goesby = e.Attribute("goesby").Value,
                    dob = e.Attribute("dob").Value,
                    gender = e.Attribute("gender").Value.ToInt(),
                    marital = e.Attribute("marital").Value.ToInt(),
                };
                ed.Size = new Size(30, 30);
                this.Controls.Add(ed);
                ed.Click += new EventHandler(ed_Click);
                ed.Enabled = false;
                controls.Add(ed);
                sucontrols.Add(ed);

                row++;
            }
        }

        void ab_Click(object sender, EventArgs e)
        {
            var ab = sender as Button;
            this.GoHome((string)ab.Tag);
        }
        void an_Click(object sender, EventArgs e)
        {
            var an = sender as Button;
            var fi = (AddFamilyInfo)an.Tag;
            Program.FamilyId = fi.FamilyId;
            Program.SetFields(fi.last, fi.email, fi.addr, fi.zip, fi.home);
            Program.editing = false;
            this.Swap(Program.first);
        }
        void ed_Click(object sender, EventArgs e)
        {
            var ed = sender as Button;
            var pi = (PersonInfo)ed.Tag;
            Program.FamilyId = pi.fid;
            Program.SetFields(pi.last, pi.email, pi.addr, pi.zip, pi.home);
            Program.first.textBox1.Text = pi.first;
            Program.goesby.textBox1.Text = pi.goesby;
            Program.dob.textBox1.Text = pi.dob;
            Program.cellphone.textBox1.Text = pi.cell.FmtFone();
            Program.gendermarital.Marital = pi.marital;
            Program.gendermarital.Gender = pi.gender;

            Program.editing = true;
            this.Swap(Program.first);
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            this.GoHome(string.Empty);
        }
        private void ResultKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                this.GoHome(string.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.FamilyId = 0;
            Program.editing = false;
            this.Swap(Program.first);
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
        private void pgdn_Click(object sender, EventArgs e)
        {
            ClearControls();
            ShowResults(name, next.Value);
        }

        private void pgup_Click(object sender, EventArgs e)
        {
            ClearControls();
            ShowResults(name, prev.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var c in sucontrols)
            {
                c.Enabled = true;
                c.BackColor = Color.Coral;
            }
        }
    }
    public class AddFamilyInfo
    {
        public int FamilyId { get; set; }
        public string home { get; set; }
        public string addr { get; set; }
        public string zip { get; set; }
        public string last { get; set; }
        public string email { get; set; }
    }
    public class PersonInfo : AddFamilyInfo
    {
        public int fid { get; set; }
        public string first { get; set; }
        public string goesby { get; set; }
        public string dob { get; set; }
        public string cell { get; set; }
        public int gender { get; set; }
        public int marital { get; set; }
    }
}
