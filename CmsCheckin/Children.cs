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

namespace CmsCheckin
{
    public partial class Children : UserControl
    {
        public event EventHandler GoBack;
        public event EventHandler<EventArgs<List<ChildLabel>>> Go;
        public Children()
        {
            InitializeComponent();
        }
        private void ReturnMatch_Click(object sender, EventArgs e)
        {
            GoBack(sender, e);
        }

        public void FindChildren(int FamilyId)
        {
            var wc = new WebClient();
            var url = new Uri(new Uri(ConfigurationSettings.AppSettings["ServiceUrl"]), 
                "Checkin/Children/" + FamilyId);
            string str = wc.DownloadString(url);
            var x = XDocument.Parse(str);
            var font = new Font("Verdana", 16F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            var g = this.CreateGraphics();
            var wids = new int[6];
            wids[0] = 15;
            wids[1] = 65;
            wids[2] = 65;
            foreach (var e in x.Descendants("child").Take(10))
            {
                var n = 3;
                wids[n] = Math.Max(wids[n], (int)g.MeasureString(e.Attribute("name").Value, font).Width); n++;
                wids[n] = Math.Max(wids[n], (int)g.MeasureString(e.Attribute("org").Value, font).Width); n++;
                wids[n] = Math.Max(wids[n], (int)g.MeasureString(e.Attribute("bday").Value, font).Width); n++;
            }
            var cols = new int[5];
            cols[0] = 15;
            for (var i = 1; i < 5; i++)
                cols[i] = cols[i - 1] + 15 + wids[i];
            var top = 120;
            var row = 0;
            var col = 0;

            var labfont = new Font("Verdana", 15.75F, ((FontStyle)((FontStyle.Italic | FontStyle.Underline))), GraphicsUnit.Point, ((byte)(0)));
            var labtop = top - 50;

            var hpresent = new Label();
            hpresent.AutoSize = true;
            hpresent.Font = labfont;
            hpresent.Location = new Point(cols[col++], labtop);
            hpresent.Text = "Here";
            this.Controls.Add(hpresent);

            var hextra = new Label();
            hextra.AutoSize = true;
            hextra.Font = labfont;
            hextra.Location = new Point(cols[col++], labtop);
            hextra.Text = "Extra";
            this.Controls.Add(hextra);

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

            var hbday = new Label();
            hbday.AutoSize = true;
            hbday.Font = labfont;
            hbday.Location = new Point(cols[col++], labtop);
            hbday.Text = "Birthday";
            this.Controls.Add(hbday);

            foreach (var e in x.Descendants("child").Take(10))
            {
                col = 0;

                var ab = new Button();
                ab.BackColor = SystemColors.ControlLight;
                ab.Font = new Font("Wingdings", 32.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(2)));
                ab.Location = new Point(cols[col++], top + (row * 50) - 5);
                ab.Name = "attend" + row;
                var c = new ChildLabel
                {
                    Name = e.Attribute("name").Value,
                    Age = Convert.ToInt32(e.Attribute("age").Value),
                    Birthday = e.Attribute("bday").Value,
                    Gender = e.Attribute("gender").Value,
                    PeopleId = Convert.ToInt32(e.Attribute("id").Value),
                    Class = e.Attribute("org").Value,
                    OrgId = Convert.ToInt32(e.Attribute("orgid").Value)
                };
                ab.Tag = c;
                ab.Size = new Size(65, 45);
                ab.TextAlign = ContentAlignment.TopCenter;
                ab.UseVisualStyleBackColor = false;
                this.Controls.Add(ab);
                ab.Click += new EventHandler(ab_Click);

                var eb = new Button();
                eb.BackColor = SystemColors.ControlLight;
                eb.Font = new Font("Wingdings", 32.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(2)));
                eb.Location = new Point(cols[col++], top + (row * 50) - 5);
                eb.Name = "extra" + row;
                eb.Tag = row;
                eb.Size = new Size(65, 45);
                eb.TextAlign = ContentAlignment.TopCenter;
                eb.UseVisualStyleBackColor = false;
                this.Controls.Add(eb);
                eb.Click += new EventHandler(eb_Click);

                var name = new Label();
                name.Font = font;
                name.Location = new Point(cols[col++], top + (row * 50));
                name.AutoSize = true;
                name.Text = e.Attribute("name").Value;
                name.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(name);

                var org = new Label();
                org.Font = font;
                org.Location = new Point(cols[col++], top + (row * 50));
                org.AutoSize = true;
                org.Text = e.Attribute("org").Value;
                org.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(org);

                var bday = new Label();
                bday.Font = font;
                bday.Location = new Point(cols[col++], top + (row * 50));
                bday.AutoSize = true;
                bday.Text = e.Attribute("bday").Value;
                bday.TextAlign = ContentAlignment.MiddleLeft;
                this.Controls.Add(bday);

                row++;
            }
        }

        void eb_Click(object sender, EventArgs e)
        {
            var eb = sender as Button;
            var ab = this.Controls[this.Controls.IndexOfKey("attend" + eb.Tag.ToString())] as Button;
            var c = ab.Tag as ChildLabel;
            if (eb.Text == String.Empty)
            {
                eb.Text = "ü";
                if (ab.Text != eb.Text)
                    ab_Click(ab, e);
                c.Extra = true;
            }
            else
            {
                eb.Text = String.Empty;
                c.Extra = false;
            }
        }

        List<ChildLabel> list = new List<ChildLabel>();

        void ab_Click(object sender, EventArgs e)
        {
            var ab = sender as Button;
            if (ab.Text == String.Empty)
            {
                ab.Text = "ü";
                list.Add((ChildLabel)ab.Tag);
            }
            else
            {
                ab.Text = String.Empty;
                list.Remove((ChildLabel)ab.Tag);
            }
        }

        private void Print_Click(object sender, EventArgs e)
        {
            Go(sender, new EventArgs<List<ChildLabel>>(list));
        }
    }
    public class ChildLabel
    {
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public string Birthday { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public int OrgId { get; set; }
        public bool Extra { get; set; }
    }
}
