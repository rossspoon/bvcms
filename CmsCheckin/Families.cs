using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CmsCheckin
{
    public partial class Families : UserControl
    {
        public event EventHandler GoBack;
        public event EventHandler<EventArgs<int>> Go;

        public Families()
        {
            InitializeComponent();
        }
        public void ShowFamilies(XDocument x)
        {
            var font = new Font("Verdana", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            var g = this.CreateGraphics();
            var row = 0;

            var labfont = new Font("Verdana", 15F, ((FontStyle)((FontStyle.Italic | FontStyle.Underline))), GraphicsUnit.Point, ((byte)(0)));

            foreach (var e in x.Descendants("family"))
            {
                var ab = new Button();
                ab.BackColor = SystemColors.ControlLight;
                ab.Font = labfont;
                ab.Location = new Point(130, 60 + (row * 50));
                ab.Tag = int.Parse(e.Attribute("id").Value);
                ab.Size = new Size(400, 45);
                ab.TextAlign = ContentAlignment.TopCenter;
                ab.UseVisualStyleBackColor = false;
                ab.Text = e.Attribute("name").Value;
                this.Controls.Add(ab);
                ab.Click += new EventHandler(ab_Click);

                row++;
            }
        }
        void ab_Click(object sender, EventArgs e)
        {
            var ab = sender as Button;
            Go(sender, new EventArgs<int>((int)ab.Tag));
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            GoBack(sender, e);
        }
    }
}
