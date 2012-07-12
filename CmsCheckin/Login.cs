using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Xml.Linq;
using System.Drawing.Printing;
using System.Xml.Serialization;
using System.IO;

namespace CmsCheckin
{
	public partial class Login : Form
	{
		public Login()
		{
			InitializeComponent();

			b1.Click += buttonclick;
			b2.Click += buttonclick;
			b3.Click += buttonclick;
			b4.Click += buttonclick;
			b5.Click += buttonclick;
			b6.Click += buttonclick;
			b7.Click += buttonclick;
			b8.Click += buttonclick;
			b9.Click += buttonclick;
			b0.Click += buttonclick;
			bdash.Click += buttonclick;
			bequal.Click += buttonclick;

			bq.Click += buttonclick;
			bw.Click += buttonclick;
			be.Click += buttonclick;
			br.Click += buttonclick;
			bt.Click += buttonclick;
			by.Click += buttonclick;
			bu.Click += buttonclick;
			bi.Click += buttonclick;
			bo.Click += buttonclick;
			bp.Click += buttonclick;
			blbrace.Click += buttonclick;
			brbrace.Click += buttonclick;

			ba.Click += buttonclick;
			bs.Click += buttonclick;
			bd.Click += buttonclick;
			bf.Click += buttonclick;
			bg.Click += buttonclick;
			bh.Click += buttonclick;
			bj.Click += buttonclick;
			bk.Click += buttonclick;
			bl.Click += buttonclick;

			bz.Click += buttonclick;
			bx.Click += buttonclick;
			bc.Click += buttonclick;
			bv.Click += buttonclick;
			bb.Click += buttonclick;
			bn.Click += buttonclick;
			bm.Click += buttonclick;

			bcomma.Click += buttonclick;
			bdot.Click += buttonclick;
			bslash.Click += buttonclick;

			username.KeyPress += textBox_KeyPress;
			password.KeyPress += textBox_KeyPress;
			URL.KeyPress += textBox_KeyPress;

			username.Enter += textbox_Enter;
			password.Enter += textbox_Enter;
			URL.Enter += textbox_Enter;
			password.Focus();
		}

		public XDocument campuses { get; set; }
		private void button1_Click(object sender, EventArgs e)
		{
			Settings1.Default.URL = URL.Text;
			Settings1.Default.username = username.Text;
			Settings1.Default.Kiosks = PrintKiosks.Text;
			Settings1.Default.PrintMode = PrintMode.Text;
			Settings1.Default.Printer = Printer.Text;
			Settings1.Default.TwoInchLabel = TwoInchLabel.Checked;
			Settings1.Default.BuildingMode = BuildingAccessMode.Checked;
			Settings1.Default.Building = building.Text;
			Settings1.Default.Save();

			if(URL.Text.StartsWith("localhost"))
                Program.URL = "http://" + URL.Text;
            else if (Settings1.Default.UseSSL)
                Program.URL = "https://" + URL.Text;
            else
                Program.URL = "http://" + URL.Text;

			Program.Username = username.Text;
			Program.Password = password.Text;
			if (BuildingAccessMode.Checked == true)
			{
				try
				{
					Program.Building = building.Text;
					Program.BuildingInfo = Util.FetchBuildingInfo();
					if (Program.BuildingInfo.Activities.Count == 0)
					{
						CancelClose = true;
						return;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("cannot find " + Program.URL);
					CancelClose = true;
					throw;
				}
			}
			var wc = Util.CreateWebClient();
			try
			{
				var url = new Uri(new Uri(Program.URL), "Checkin2/Campuses");
				var str = wc.DownloadString(url);
				if (str == "not authorized")
				{
					MessageBox.Show(str);
					CancelClose = true;
					return;
				}
				campuses = XDocument.Parse(str);
				this.Hide();
			}
			catch (WebException ex)
			{
				MessageBox.Show("cannot find " + Program.URL);
				CancelClose = true;
			}
		}

		TextBox current = null;
		private void Login_Load(object sender, EventArgs e)
		{
			password.Focus();
			var prtdoc = new PrintDocument();
			var defp = prtdoc.PrinterSettings.PrinterName;
			foreach (var s in PrinterSettings.InstalledPrinters)
				Printer.Items.Add(s);

			if (Util.IsDebug())
			{
				URL.Text = "localhost:888/";
				username.Text = "David";
				var name = @"..\..\secrets.xml";
				if (File.Exists(name))
				{
					var xd = XDocument.Load(name);
					var pa = xd.Descendants("password").FirstOrDefault();
					if (pa != null)
						password.Text = pa.Value;
				}
				PrintKiosks.Text = "";
				PrintMode.Text = "Print to Printer";
				Printer.SelectedIndex = Printer.FindStringExact("Godex EZ-DT-4");
				BuildingAccessMode.Checked = true;
				building.Text = "recreation";
			}
			else
			{
				Printer.SelectedIndex = Printer.FindStringExact(defp);
				if (Settings1.Default.Printer.HasValue())
					Printer.SelectedIndex = Printer.FindStringExact(Settings1.Default.Printer);
				TwoInchLabel.Checked = Settings1.Default.TwoInchLabel;
				BuildingAccessMode.Checked = Settings1.Default.BuildingMode;
				URL.Text = Settings1.Default.URL;
				username.Text = Settings1.Default.username;
				PrintKiosks.Text = Settings1.Default.Kiosks;
				PrintMode.Text = Settings1.Default.PrintMode;
				building.Text = Settings1.Default.Building;
			}
		}
		void buttonclick(object sender, EventArgs e)
		{
			var b = sender as Button;
			var d = b.Text[0];
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


		private void textbox_Enter(object sender, EventArgs e)
		{
			current = (TextBox)sender;
		}

		private void bbs_Click(object sender, EventArgs e)
		{
			BackSpace();
		}


		public bool CancelClose { get; set; }
		private void Login_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = CancelClose;
			CancelClose = false;
		}

		private void bshift_Click(object sender, EventArgs e)
		{
			if (ba.Text == "A")
			{
				ba.Text = "a";
				bb.Text = "b";
				bc.Text = "c";
				bd.Text = "d";
				be.Text = "e";
				bf.Text = "f";
				bg.Text = "g";
				bh.Text = "h";
				bi.Text = "i";
				bj.Text = "j";
				bk.Text = "k";
				bl.Text = "l";
				bm.Text = "m";
				bn.Text = "n";
				bo.Text = "o";
				bp.Text = "p";
				bq.Text = "q";
				br.Text = "r";
				bs.Text = "s";
				bt.Text = "t";
				bu.Text = "u";
				bv.Text = "v";
				bw.Text = "w";
				bx.Text = "x";
				by.Text = "y";
				bz.Text = "z";

				b1.Text = "1";
				b2.Text = "2";
				b3.Text = "3";
				b4.Text = "4";
				b5.Text = "5";
				b6.Text = "6";
				b7.Text = "7";
				b8.Text = "8";
				b9.Text = "9";
				b0.Text = "0";
				bdash.Text = "-";
				bequal.Text = "=";
				blbrace.Text = "[";
				brbrace.Text = "]";
				bcolon.Text = ":";
				bcomma.Text = ",";
				bdot.Text = ".";
				bslash.Text = "/";

			}
			else
			{
				ba.Text = "A";
				bb.Text = "B";
				bc.Text = "C";
				bd.Text = "D";
				be.Text = "E";
				bf.Text = "F";
				bg.Text = "G";
				bh.Text = "H";
				bi.Text = "I";
				bj.Text = "J";
				bk.Text = "K";
				bl.Text = "L";
				bm.Text = "M";
				bn.Text = "N";
				bo.Text = "O";
				bp.Text = "P";
				bq.Text = "Q";
				br.Text = "R";
				bs.Text = "S";
				bt.Text = "T";
				bu.Text = "U";
				bv.Text = "V";
				bw.Text = "W";
				bx.Text = "X";
				by.Text = "Y";
				bz.Text = "Z";

				b1.Text = "!";
				b2.Text = "@";
				b3.Text = "#";
				b4.Text = "$";
				b5.Text = "%";
				b6.Text = "^";
				b7.Text = "&&";
				b8.Text = "*";
				b9.Text = "(";
				b0.Text = ")";
				bdash.Text = "_";
				bequal.Text = "+";
				blbrace.Text = "{";
				brbrace.Text = "}";
				bcolon.Text = ";";
				bcomma.Text = "<";
				bdot.Text = ">";
				bslash.Text = "?";
			}
		}
	}
}
