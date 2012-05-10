using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Configuration;
using System.Xml.Linq;

namespace CmsCheckin
{
    public partial class BaseForm : Form
    {

		UserControl home;
        public BaseForm(UserControl home)
        {
			this.home = home;
            InitializeComponent();
        }

		public TextBox textbox;
        private void BaseForm_Load(object sender, EventArgs e)
        {
			Program.CursorHide();
            ControlsAdd(home);
			home.Visible = true;
			if (home is Home)
				textbox = ((Home)home).textBox1;
			else if (home is Home2)
				textbox = ((Home2)home).textBox1;
        }


        public void ControlsAdd(UserControl ctl)
        {
            ctl.Location = new Point { X = (this.Width / 2) - (ctl.Width / 2), Y = 0 };
            ctl.Visible = false;
            Controls.Add(ctl);
        }

		private void BaseForm_Resize(object sender, EventArgs e)
		{
            home.Location = new Point { X = (this.Width / 2) - (home.Width / 2), Y = 0 };
		}
    }
}
