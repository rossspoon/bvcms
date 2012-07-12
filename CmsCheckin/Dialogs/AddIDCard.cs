using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CmsCheckin.Dialogs
{
	public partial class AddIDCard : Form
	{
		private int ActivePersonID = 0;
		private string ActivePersonName = "";

		public AddIDCard()
		{
			InitializeComponent();
			ScanID.Focus();
			ScanForName.Text = "Unknown";
		}

		public AddIDCard(int personID, string personName)
		{
			ActivePersonID = personID;
			ActivePersonName = personName;

			InitializeComponent();
			ScanForName.Text = ActivePersonName;
			ScanID.Focus();
		}

		private void ScanID_TextChanged(object sender, EventArgs e)
		{
			var ID = ScanID.Text;
		}

		private void ScanID_Leave(object sender, EventArgs e)
		{
			ScanID.Focus();
		}

		private void ScanID_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) OK.PerformClick();
		}

		private void OK_Click(object sender, EventArgs e)
		{
			if (ActivePersonID > 0)
			{
				if (Util.AddIDCard(ScanID.Text, ActivePersonID))
					Program.attendant.AddHistoryString( DateTime.Now.ToString("hh:mm tt") + " ID Card added for " + ActivePersonName);
				else
					Program.attendant.AddHistoryString("-- " + DateTime.Now.ToString("hh:mm tt") + " ID Card NOT added for " + ActivePersonName);
			}

			this.Close();
			this.Dispose();
		}
	}
}
