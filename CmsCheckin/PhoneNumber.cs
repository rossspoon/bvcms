using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CmsCheckin
{
    public partial class PhoneNumber : UserControl
    {
        public PhoneNumber()
        {
            InitializeComponent();
        }
        void buttonclick(object sender, EventArgs e)
        {
            var b = sender as Button;
            var d = b.Name.Substring(6);
            var t = textBox1.Text;
            if (t.Length < 8)
                if (t.Length == 3)
                    textBox1.Text = t + '-' + d;
                else
                    textBox1.Text = t + d;
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        private void PhoneNumber_Load(object sender, EventArgs e)
        {
            button1.Click += new EventHandler(buttonclick);
            button2.Click += new EventHandler(buttonclick);
            button3.Click += new EventHandler(buttonclick);
            button4.Click += new EventHandler(buttonclick);
            button5.Click += new EventHandler(buttonclick);
            button6.Click += new EventHandler(buttonclick);
            button7.Click += new EventHandler(buttonclick);
            button8.Click += new EventHandler(buttonclick);
            button9.Click += new EventHandler(buttonclick);
            button0.Click += new EventHandler(buttonclick);
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
        public event EventHandler<EventArgs<string>> Go;
        private void buttongo_Click(object sender, EventArgs e)
        {
            Go(sender, new EventArgs<string>(textBox1.Text));
        }

        private void buttonbs_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            var t = textBox1.Text;
            var len = t.Length - 1;
            if (len == 4)
                len--;
            if (len < 0)
                len = 0;
            textBox1.Text = t.Substring(0, len);
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
    }
}
