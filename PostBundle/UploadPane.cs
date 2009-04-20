using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace PostBundle
{
    partial class UploadPane : UserControl
    {
        public UploadPane()
        {
            InitializeComponent();
        }
        private void Upload_Click(object sender, EventArgs e)
        {
            var ws = new CmsWs.cmsSoapClient();
            var header = new CmsWs.ServiceAuthHeader();
            header.Username = "david";
            header.Password = "ddrr11";
            var bundleid = int.Parse(Globals.Sheet1.BundleId.Text.ToString());
            var list = new List<CmsWs.BundleDetail>();
            var t = Globals.Sheet1.Table1.Range;
            for (int r = 2; r < t.Rows.Count; r++)
            {
                var c = t.Cells[r, 1] as Excel.Range;
                var pid = int.Parse(c.Value2.ToString());
                c = t.Cells[r, 2] as Excel.Range;
                var amt = decimal.Parse(c.Value2.ToString());
                c = t.Cells[r, 3] as Excel.Range;
                Debug.WriteLine(c.Text.ToString());
                var dt = DateTime.Parse(c.Text.ToString());
                c = t.Cells[r, 4] as Excel.Range;
                var fund = int.Parse(c.Value2.ToString());
                list.Add(new CmsWs.BundleDetail
                {
                    PeopleId = pid,
                    Amount = amt,
                    Date = dt,
                    Fund = fund,
                });
            }
            ws.UploadBundle(header, bundleid, list.ToArray());
            MessageBox.Show("Succeeded");
        }
    }
}
