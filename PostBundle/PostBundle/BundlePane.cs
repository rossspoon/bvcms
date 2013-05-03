using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using System.ServiceModel;

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
            var ws = Globals.ThisWorkbook.ws;
            var header = Globals.ThisWorkbook.header;
            this.Cursor = Cursors.WaitCursor;
            var a = ws.RecentBundleList(header);
            this.Cursor = Cursors.Default;
            Search(this, new EventArgs<List<CmsWs.BundleResult>>(a.ToList()));
        }
    }
}
