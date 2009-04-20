using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;

namespace PostBundle
{
    partial class SearchPane : UserControl
    {
        public event EventHandler<EventArgs<List<CmsWs.PersonResult>>> Search;
 
        public SearchPane()
        {
            InitializeComponent();
        }

        private void TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                button1_Click(button1, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ws = new CmsWs.cmsSoapClient();
            var header = new CmsWs.ServiceAuthHeader();
            header.Username = "david";
            header.Password = "ddrr11";
            string pid = Globals.Sheet1.Application.ActiveCell.Text.ToString();
            if (!string.IsNullOrEmpty(pid))
            {
                NameTB.Text = pid;
                CommTB.Text = string.Empty;
                AddrTB.Text = string.Empty;
                BirthdayTB.Text = string.Empty;
            }
            var a = ws.SearchPerson(header, NameTB.Text, CommTB.Text, AddrTB.Text, BirthdayTB.Text);
            Search(this, new EventArgs<List<CmsWs.PersonResult>>(a.ToList()));
        }
    }
}
