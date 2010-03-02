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
    public partial class SearchResults : UserControl
    {
        public event EventHandler<EventArgs<string>> GoBack;

        public SearchResults()
        {
            InitializeComponent();
        }

        DateTime time;

        public void ShowResults(XDocument x)
        {
            time = DateTime.Now;

            var points = 14F;

            string Verdana = "Verdana";
            Font labfont;
            var g = this.CreateGraphics();
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
                if (wid > 1000)
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
                var homephone = e.Attribute("homephone").Value;
                var cellphone = e.Attribute("cellphone").Value;
                ab.Tag = !string.IsNullOrEmpty(homephone) ? homephone : !string.IsNullOrEmpty(cellphone) ? cellphone : "";
                ab.Size = new Size(1000, 45);
                ab.TextAlign = ContentAlignment.MiddleLeft;
                ab.UseVisualStyleBackColor = false;
                ab.Text = e.Attribute("display").Value;
                this.Controls.Add(ab);
                ab.Click += new EventHandler(ab_Click);
                ab.KeyPress += new KeyPressEventHandler(ResultKeyPress);
                row++;
            }
        }

        void ab_Click(object sender, EventArgs e)
        {
            var ab = sender as Button;
            GoBack(sender, new EventArgs<string>((string)ab.Tag));
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            GoBack(sender, new EventArgs<string>(""));
        }
        private void ResultKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                GoBack(sender, new EventArgs<string>(""));
        }
    }
    public class PeopleLabel
    {
        public string Name { get; set; }
        public int PeopleId { get; set; }
        public int FamilyId { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public string HomePhone { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public int Row { get; set; }
    }
}
