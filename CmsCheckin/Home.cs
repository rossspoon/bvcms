using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace CmsCheckin
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.S | Keys.Alt:
                        this.Swap(Program.namesearch);
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
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

        private void buttongo_Click(object sender, EventArgs e)
        {
            lastbutton = null;
            var d = textBox1.Text.GetDigits().Length;
            lastnumber = textBox1.Text;
            Go();
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
                lastnumber = textBox1.Text;
                Go();
            }
            e.Handled = true;
        }
        private void KeyStroke(char d)
        {
            lastbutton = null;
            var t = textBox1.Text.GetDigits();
            if (t.Length < 10)
                t += d;
            textBox1.Text = t.FmtFone();
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
        private void BackSpace()
        {
            lastbutton = null;
            var t = textBox1.Text.GetDigits();
            var len = t.Length - 1;
            if (len < 0)
                len = 0;
            textBox1.Text = t.Substring(0, len).FmtFone();
            textBox1.Focus();
            textBox1.Select(textBox1.Text.Length, 0);
        }
        private PleaseWait PleaseWaitForm;
        private void Go()
        {
            if (textBox1.Text == "010")
            {
                Application.Exit();
                return;
            }
            if (textBox1.Text == "411")
                this.Swap(Program.namesearch);
            else if (textBox1.Text.StartsWith("0") && textBox1.Text.Length > 1)
                Print.MemberList(textBox1.Text.Substring(1));
            else
            {
                PleaseWaitForm = new PleaseWait();
                PleaseWaitForm.Show();
                buttongo.Enabled = false;
                var bw = new BackgroundWorker();
                bw.DoWork += backgroundWorker1_DoWork;
                bw.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
                bw.RunWorkerAsync(textBox1.Text);
            }
            textBox1.Text = string.Empty;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var ph = e.Argument as string;
            var x = this.GetDocument("Checkin2/Match/" + ph.GetDigits() 
                + Program.QueryString + "&page=1");
            e.Result = x;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PleaseWaitForm.Hide();
            PleaseWaitForm.Dispose();
            PleaseWaitForm = null;
            buttongo.Enabled = true;
            var x = e.Result as XDocument;
            if (x.Document.Root.Name == "Families")
            {
                this.Swap(Program.families);
                Program.families.ShowFamilies(x);
            }
            else
            {
                var locked = bool.Parse(x.Document.Root.Attribute("waslocked").Value);
                if (locked)
                    MessageBox.Show("Family is already being viewed");
                else
                {
                    this.Swap(Program.family);
                    Program.family.ShowFamily(x);
                }
            }
        }

        private void MagicButton_Click(object sender, EventArgs e)
        {
            this.Swap(Program.namesearch);
        }
    }
}
