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
    public partial class ListFamilies : UserControl
    {
        public ListFamilies()
        {
            InitializeComponent();
            timer1.Interval = Program.Interval;
            timer1.Tick += new EventHandler(timer1_Tick);
        }
        List<Control> controls = new List<Control>();
        public void ShowFamilies(XDocument x)
        {
            ClearControls();
            Print.Focus();
            Print.KeyPress += new KeyPressEventHandler(FamilyKeyPress);

            var font = new Font("Verdana", 14F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            var g = this.CreateGraphics();
            var row = 0;

            var labfont = new Font("Verdana", 15F, ((FontStyle)((FontStyle.Italic | FontStyle.Underline))), GraphicsUnit.Point, ((byte)(0)));

            foreach (var e in x.Descendants("family"))
            {
                var ab = new Button();
                ab.BackColor = SystemColors.ControlLight;
                ab.Font = labfont;
                ab.Location = new Point(230, 100 + (row * 50));
                ab.Tag = int.Parse(e.Attribute("id").Value);
                ab.Size = new Size(400, 45);
                ab.TextAlign = ContentAlignment.TopCenter;
                ab.UseVisualStyleBackColor = false;
                ab.Text = e.Attribute("name").Value;
                this.Controls.Add(ab);
                ab.Click += new EventHandler(ab_Click);
                ab.KeyPress += new KeyPressEventHandler(FamilyKeyPress);
                controls.Add(ab);

                row++;
            }
            timer1.Start();
        }
        void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            Program.ClearFields();
            this.GoHome("");
        }

        void ab_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            var ab = sender as Button;
            this.Swap(Program.family);
            Program.family.ShowFamily((int)ab.Tag, 1);
        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.GoHome(string.Empty);
        }
        private void FamilyKeyPress(object sender, KeyPressEventArgs e)
        {
            timer1.Stop();
            timer1.Start();
            if (e.KeyChar == 27)
                this.GoHome(string.Empty);
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
    }
}
