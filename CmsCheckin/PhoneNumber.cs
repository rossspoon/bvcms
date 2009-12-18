using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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
            var d = b.Name[6];
            KeyStroke(d);
        }
        public string GetDigits(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            var digits = new StringBuilder();
            foreach (var c in s.ToCharArray())
                if (Char.IsDigit(c))
                    digits.Append(c);
            return digits.ToString();
        }
        public bool AllDigits(string str)
        {
            Regex patt = new Regex("[^0-9]");
            return !(patt.IsMatch(str));
        }
        public string FmtFone(string phone)
        {
            var ph = GetDigits(phone);
            if (string.IsNullOrEmpty(ph))
                return "";
            var t = new StringBuilder(ph);

            if (ph.Length >= 4)
                t.Insert(3, "-");
            if (ph.Length >= 8)
                t.Insert(7, "-");
            return t.ToString();
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
            button10.Click += new EventHandler(button10_Click);
            button11.Click += new EventHandler(button10_Click);
            button12.Click += new EventHandler(button10_Click);
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        private Button lastbutton;
        private string lastnumber;
        void button10_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (button10 == b)
                lastbutton = b;
            else if (lastbutton == button10 && button11 == b)
                lastbutton = b;
            else if (lastbutton == button11 && button12 == b)
            {
                textBox1.Text = lastnumber;
                lastbutton = b;
            }
        }
        public event EventHandler<EventArgs<string>> Go;
        private void buttongo_Click(object sender, EventArgs e)
        {
            lastbutton = null;
            var d = GetDigits(textBox1.Text).Length;
            if (d != 10 && d != 7)
                return;
            lastnumber = textBox1.Text;
            Go(sender, new EventArgs<string>(textBox1.Text));
        }

        private void buttonbs_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            BackSpace();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\b')
                BackSpace();
            else if(e.KeyChar >= '0' && e.KeyChar <= '9')
                KeyStroke(e.KeyChar);
            else if(e.KeyChar == '\r')
            {
                var d = GetDigits(textBox1.Text).Length;
                if (d != 10 && d != 7)
                    return;
                lastnumber = textBox1.Text;
                Go(sender, new EventArgs<string>(textBox1.Text));
            }
            e.Handled = true;
        }
        private void KeyStroke(char d)
        {
            lastbutton = null;
            var t = GetDigits(textBox1.Text);
            if (t.Length < 10)
                t += d;
            textBox1.Text = FmtFone(t);
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
        private void BackSpace()
        {
            lastbutton = null;
            var t = GetDigits(textBox1.Text);
            var len = t.Length - 1;
            if (len < 0)
                len = 0;
            textBox1.Text = FmtFone(t.Substring(0, len));
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
    }
}
