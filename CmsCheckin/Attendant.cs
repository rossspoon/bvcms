using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CmsCheckin
{
	public partial class Attendant : Form
	{
		public Attendant()
		{
			InitializeComponent();
		}

		private void ShowCheckin_Click(object sender, EventArgs e)
		{
			if (Program.baseform == null)
				Program.baseform = new BaseForm(new Home2());
			if (Program.baseform.Visible)
			{
				Program.baseform.Hide();
				ShowCheckin.Text = "Show Checkin";
			}
			else
			{
				Program.baseform.Show();
				ShowCheckin.Text = "Hide Checkin";
			}
		}

	}
}
