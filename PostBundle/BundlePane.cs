using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;

namespace PostBundle
{
    partial class BundlePane : UserControl
    {
        public event EventHandler<EventArgs<List<CmsWs.BundleResult>>> Search;
          
        public BundlePane()
        {
            InitializeComponent();
        }
        private void Search_Click(object sender, EventArgs e)
        {
            var ws = new CmsWs.cmsSoapClient();
            var header = new CmsWs.ServiceAuthHeader();
            header.Username = "david";
            header.Password = "ddrr11";
            this.Cursor = Cursors.WaitCursor;
            var a = ws.RecentBundleList(header, dateTimePicker1.Value.Date);
            this.Cursor = Cursors.Default;
            Search(this, new EventArgs<List<CmsWs.BundleResult>>(a.ToList()));
        }

        private void TB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                Search_Click(button1, null);
        }

        private void ActionsPaneControl2_Load(object sender, EventArgs e)
        {
            dateTimePicker1.CustomFormat = "M/d/yy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
        }
    }
}
