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
    public partial class Match : UserControl
    {
        public event EventHandler GoBack;
        public event EventHandler<EventArgs<int>> Go;
        public Match()
        {
            InitializeComponent();
        }

        private void ReturnPhone_Click(object sender, EventArgs e)
        {
           GoBack(sender, e);
        }

        public void Search(string phone)
        {
            var wc = new WebClient();
            string Url = ConfigurationSettings.AppSettings["ServiceUrl"];
            
            var url = new Uri(new Uri(ConfigurationSettings.AppSettings["ServiceUrl"]), 
                "Checkin/Match/" + phone);

            string str = wc.DownloadString(url);
            var x = XDocument.Parse(str);
            var font = new Font("Verdana", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            var size = new Size(650, 44);
            var top = 75;
            var row = 0;
            foreach (var e in x.Descendants("fam").Take(12))
            {
                var b = new Button();
                b.BackColor = SystemColors.ControlLight;
                b.Font = font;
                b.Location = new Point(30, top + (row * 50) );
                b.Name = "rb" + row;
                b.Tag = Convert.ToInt32(e.Attribute("id").Value);
                b.Size = size;
                b.Text = e.Value.Trim();
                b.TextAlign = ContentAlignment.MiddleLeft;
                b.UseVisualStyleBackColor = false;
                row++;
                this.Controls.Add(b);
                b.Click += new EventHandler(b_Click);
            }
        }

        void b_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            Go(sender, new EventArgs<int>((int)b.Tag));
        }
    }
}
