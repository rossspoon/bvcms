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
    public partial class EnterPhone : UserControl
    {
        public event EventHandler GoNext;
        public event EventHandler GoBack;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Tab:
                        GoNext(this, new EventArgs());
                        return true;
                    case Keys.Shift | Keys.Tab:
                        GoBack(this, new EventArgs());
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public EnterPhone(string title)
        {
            InitializeComponent();
            label2.Text = title;
        }
        void buttonclick(object sender, EventArgs e)
        {
            var b = sender as Button;
            var d = b.Name[6];
            KeyStroke(d);
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

        private void buttongo_Click(object sender, EventArgs e)
        {
            var d = textBox1.Text.GetDigits().Length;
            GoNext(sender, e);
        }

        private void buttonbs_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            BackSpace();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
                GoBack(sender, e);
            else if (e.KeyChar == '\b')
                BackSpace();
            else if(e.KeyChar >= '0' && e.KeyChar <= '9')
                KeyStroke(e.KeyChar);
            else if(e.KeyChar == '\r')
            {
                var d = textBox1.Text.GetDigits().Length;
                if (d == 0 || d == 10 || d == 7)
                    GoNext(sender, e);
                return;
            }
            e.Handled = true;
        }
        private void KeyStroke(char d)
        {
            var t = textBox1.Text.GetDigits();
            if (t.Length < 10)
                t += d;
            textBox1.Text = t.FmtFone();
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
        private void BackSpace()
        {
            var t = textBox1.Text.GetDigits();
            var len = t.Length - 1;
            if (len < 0)
                len = 0;
            textBox1.Text = t.Substring(0, len).FmtFone();
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        private void bslash_Click(object sender, EventArgs e)
        {
            KeyStroke('/');
        }

        private void button_goback_Click(object sender, EventArgs e)
        {
            GoBack(sender, e);
        }
        private void EnterText_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                textBox1.Focus();
                textBox1.Select(textBox1.Text.Length, 0);
            }
        }

    }
}
