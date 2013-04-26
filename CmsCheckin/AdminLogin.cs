using System;
using System.Windows.Forms;

namespace CmsCheckin
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();

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

            password.KeyPress += textBox_KeyPress;

            password.Enter += textbox_Enter;
            password.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        TextBox current = null;
        private void Login_Load(object sender, EventArgs e)
        {
            password.Focus();
        }
        void buttonclick(object sender, EventArgs e)
        {
            var b = sender as Button;
            var d = b.Name[1];
            KeyStroke(d);
        }
        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (current == null)
                return;
            if (e.KeyChar == '\b')
                BackSpace();
            else
                KeyStroke(e.KeyChar);
            e.Handled = true;
        }
        private void KeyStroke(char d)
        {
            if (current == null)
                return;
            current.Text += d;
            current.Focus();
            current.Select(current.Text.Length, 0);
        }
        private void BackSpace()
        {
            if (current == null)
                return;
            var t = current.Text;
            var len = t.Length - 1;
            if (len < 0)
                len = 0;
            current.Text = t.Substring(0, len);
            current.Focus();
            current.Select(current.Text.Length, 0);
        }

        private void bdot_Click(object sender, EventArgs e)
        {
            KeyStroke('.');
        }

        private void textbox_Enter(object sender, EventArgs e)
        {
            current = (TextBox)sender;
        }

        private void bbs_Click(object sender, EventArgs e)
        {
            BackSpace();
        }

        private void bang_Click(object sender, EventArgs e)
        {
            KeyStroke('!');
        }

        private void bslash_Click(object sender, EventArgs e)
        {
            KeyStroke('/');
        }

        private void bcolon_Click(object sender, EventArgs e)
        {
            KeyStroke(':');
        }

        private void AdminLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (password.Text != Program.AdminPassword)
                e.Cancel = true;
        }
   }
}
