using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
            var ws = Globals.ThisWorkbook.ws;
            var header = Globals.ThisWorkbook.header;
            var bundleid = int.Parse(Globals.Sheet1.BundleId.Text.ToString());
            var list = new List<CmsWs.BundleDetail>();
            var t = Globals.Sheet1.Table1.Range;
            var dt = DateTime.Parse(Globals.Sheet1.BundleDate.Text.ToString());

            for (int r = 2; r < t.Rows.Count; r++)
            {
                var c = t.Cells[r, 1] as Excel.Range;
                int pid;
                if (!int.TryParse(c.Value2.ToString(), out pid))
                    continue;
                c = t.Cells[r, 2] as Excel.Range;
                decimal amt;
                if (!decimal.TryParse(c.Value2.ToString(), out amt))
                    continue;
                c = t.Cells[r, 3] as Excel.Range;
                int fund;
                if (!int.TryParse(c.Value2.ToString(), out fund))
                    continue;
                list.Add(new CmsWs.BundleDetail
                {
                    PeopleId = pid,
                    Amount = amt,
                    Date = dt,
                    Fund = fund,
                    Pledge = ((double)Globals.Sheet1.Pledge.Value) == 1.0,
                });
            }
            try
            {
                ws.UploadBundle(header, bundleid, list.ToArray());
                MessageBox.Show("Succeeded", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                if (ex.Message.Contains("Totals do not match"))
                    err = "Totals do not match";
                else if (ex.Message.Contains("Unknown Fund"))
                {
                    var fund = Regex.Match(ex.Message, @"Unknown Fund (\d+)", RegexOptions.Singleline).Groups[1].Value;
                    err = "Unknown Fund: " + fund;
                }
                MessageBox.Show(err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        Random rnd = new Random();
        private void CreateTotalsByFund_Click(object sender, EventArgs e)
        {
            var t1 = Globals.Sheet1.Table1;
            var list = new List<object>();
            foreach (Excel.ListRow r in t1.ListRows)
            {
                var v = r.Range[1, 3] as Excel.Range;
                list.Add(v.Value2);
            }
            var t = Globals.Sheet2.Table2;
            while (t.ListRows.Count > 1)
                t.ListRows[1].Delete();
            int i = 1;
            foreach(var f in list.Distinct())
            {
                if (i > 1)
                    t.ListRows.AddEx(i, 1);
                t.ListRows[i].Range[1, 1] = f;
                i++;
            }
        }
    }
}
