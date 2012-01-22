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
    public partial class EnterText : UserControl
    {
        public event EventHandler GoBack;
        public event EventHandler GoNext;
        private bool isname;

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Tab:
                        Program.TimerStop();
                        GoNext(this, new EventArgs());
                        return true;
                    case Keys.Shift | Keys.Tab:
                        Program.TimerStop();
                        GoBack(this, new EventArgs());
                        return true;
                    case Keys.S | Keys.Alt:
                        Program.TimerReset();
                        Program.CursorShow();
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public EnterText(string Header)
            : this(Header, false)
        {
        }
        public EnterText(string Header, bool isname)
        {
            InitializeComponent();
            label2.Text = Header;
            this.isname = isname;
            Program.TimerStart(timer1_Tick);
        }
        public void SetBackNext(UserControl c1, UserControl c2)
        {
            this.c1 = c1;
            this.c2 = c2;
            if (c1 != null)
                GoBack += new EventHandler(EnterPhone_GoBack);
            if (c2 != null)
                GoNext += new EventHandler(EnterPhone_GoNext);
        }
        private UserControl c1, c2;
        void EnterPhone_GoBack(object sender, EventArgs e)
        {
            this.Swap(c1);
        }
        void EnterPhone_GoNext(object sender, EventArgs e)
        {
            this.Swap(c2);
        }
        void buttonclick(object sender, EventArgs e)
        {
            Program.TimerReset();
            var b = sender as Button;
            var d = b.Name[1];
            KeyStroke(d);
        }
        private void GoBack_Click(object sender, EventArgs e)
        {
            Program.TimerStop();
            GoBack(sender, e);
        }

        private void Name_Load(object sender, EventArgs e)
        {
            b1.Click += new EventHandler(buttonclick);
            b2.Click += new EventHandler(buttonclick);
            b3.Click += new EventHandler(buttonclick);
            b4.Click += new EventHandler(buttonclick);
            b5.Click += new EventHandler(buttonclick);
            b6.Click += new EventHandler(buttonclick);
            b7.Click += new EventHandler(buttonclick);
            b8.Click += new EventHandler(buttonclick);
            b9.Click += new EventHandler(buttonclick);
            b0.Click += new EventHandler(buttonclick);

            bq.Click += new EventHandler(buttonclick);
            bw.Click += new EventHandler(buttonclick);
            be.Click += new EventHandler(buttonclick);
            br.Click += new EventHandler(buttonclick);
            bt.Click += new EventHandler(buttonclick);
            by.Click += new EventHandler(buttonclick);
            bu.Click += new EventHandler(buttonclick);
            bi.Click += new EventHandler(buttonclick);
            bo.Click += new EventHandler(buttonclick);
            bp.Click += new EventHandler(buttonclick);

            ba.Click += new EventHandler(buttonclick);
            bs.Click += new EventHandler(buttonclick);
            bd.Click += new EventHandler(buttonclick);
            bf.Click += new EventHandler(buttonclick);
            bg.Click += new EventHandler(buttonclick);
            bh.Click += new EventHandler(buttonclick);
            bj.Click += new EventHandler(buttonclick);
            bk.Click += new EventHandler(buttonclick);
            bl.Click += new EventHandler(buttonclick);
            
            bz.Click += new EventHandler(buttonclick);
            bx.Click += new EventHandler(buttonclick);
            bc.Click += new EventHandler(buttonclick);
            bv.Click += new EventHandler(buttonclick);
            bb.Click += new EventHandler(buttonclick);
            bn.Click += new EventHandler(buttonclick);
            bm.Click += new EventHandler(buttonclick);
            b_.Click += new EventHandler(buttonclick);

            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            Program.TimerStop();
            Util.UnLockFamily();
            Program.ClearFields();
            this.GoHome("");
        }

        private void buttongo_Click(object sender, EventArgs e)
        {
            Program.TimerStop();
            if (string.IsNullOrEmpty(textBox1.Text))
                GoNext(this, new EventArgs<string>("xyz"));
            else
                GoNext(this, new EventArgs<string>(textBox1.Text));
        }

        private void buttonbs_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            var b = sender as Button;
            BackSpace();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 27)
            {
                Program.TimerStop();
                GoBack(sender, e);
            }
            else if (e.KeyChar == '\b')
                BackSpace();
            else if (e.KeyChar == '\r' || e.KeyChar == '\t')
                buttongo_Click(sender, null);
            else
                KeyStroke(e.KeyChar);
            Program.TimerReset();
            e.Handled = true;
        }
        private void KeyStroke(char d)
        {
            textBox1.Text += d;
            if (isname)
                textBox1.Text = textBox1.Text.ToTitleCase();
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
        private void BackSpace()
        {
            var t = textBox1.Text;
            var len = t.Length - 1;
            if (len < 0)
                len = 0;
            textBox1.Text = t.Substring(0, len);
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        private void bspace_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            KeyStroke(' ');
        }

        private void GoBackButton_Click(object sender, EventArgs e)
        {
            Program.TimerStop();
            GoBack(sender, e);
        }

        private void bat_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            KeyStroke('@');
        }

        private void bdot_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            KeyStroke('.');
        }

        private void bdash_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            KeyStroke('-');
        }

        private void clear_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            textBox1.Text = string.Empty;
            textBox1.Focus();
        }

        private void EnterText_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                Program.TimerStart(timer1_Tick);
                textBox1.Focus();
                textBox1.Select(textBox1.Text.Length, 0);
            }
        }

        private void bsharp_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            KeyStroke('#');
        }

        private void bcomma_Click(object sender, EventArgs e)
        {
            Program.TimerReset();
            KeyStroke(',');
        }
    }
}
