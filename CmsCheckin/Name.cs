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
    public partial class Name : UserControl
    {
        public event EventHandler GoBack;
        public event EventHandler<EventArgs<string>> Go;

        public Name()
        {
            InitializeComponent();
        }
        void buttonclick(object sender, EventArgs e)
        {
            var b = sender as Button;
            var d = b.Name[1];
            KeyStroke(d);
        }
        private void GoBack_Click(object sender, EventArgs e)
        {
            GoBack(sender, e);
        }

        private void Name_Load(object sender, EventArgs e)
        {
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

            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }

        private void buttongo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
                GoBack(this, e);
            Go(this, new EventArgs<string>(textBox1.Text));
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
            else if(e.KeyChar >= 'a' && e.KeyChar <= 'z')
                KeyStroke(e.KeyChar);
            else if (e.KeyChar == '\r')
                buttongo_Click(sender, null);
            e.Handled = true;
        }
        private void KeyStroke(char d)
        {
            textBox1.Text += d;
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
            KeyStroke(' ');
        }

    }
}
