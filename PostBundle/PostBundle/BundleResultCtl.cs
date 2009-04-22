using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;
using Microsoft.Office.Tools.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace PostBundle
{
    public partial class BundleResultCtl : UserControl
    {
        public BundleResultCtl(CmsWs.BundleResult b)
        {
            InitializeComponent();
            bundle = b;
        }
        private string DisplayInfo
        {
            get
            {
                return string.Format("{0:d}\n{1:C} ({2})", _bundle.Date, _bundle.Total, _bundle.Count);
            }
        }
        private CmsWs.BundleResult _bundle;
        public CmsWs.BundleResult bundle
        {
            get { return _bundle; }
            set
            {
                _bundle = value;
                linkLabel1.Text = value.BundleId.ToString();
                label1.Text = DisplayInfo;
            }
        }

        private static void FillRow(Excel.Range r, CmsWs.BundleDetail d)
        {
            if (d == null)
            {
                r[1, 1] = "";
                r[1, 2] = "";
                r[1, 3] = "";
                r[1, 4] = "";
            }
            else
            {
                r[1, 1] = d.PeopleId;
                r[1, 2] = d.Amount;
                r[1, 3] = d.Fund;
                r[1, 4] = d.Name;
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.Sheet1.BundleId.Value = bundle.BundleId;
            Globals.Sheet1.BundleFund.Value = bundle.Fund;
            Globals.Sheet1.BundleTotal.Value = bundle.Total;
            Globals.Sheet1.BundleDate.Value = bundle.Date;
            var t = Globals.Sheet1.Table1;
            while (t.ListRows.Count > 1)
                t.ListRows[1].Delete();
            FillRow(t.ListRows[1].Range, null); 

            this.Cursor = Cursors.WaitCursor;
            var ws = Globals.ThisWorkbook.ws;
            var header = Globals.ThisWorkbook.header;
            var a = ws.BundleDetails(header, bundle.BundleId);
            this.Cursor = Cursors.Default;
            bool pledgefound = false;
            int i = 1;
            Globals.Sheet1.ignoreChanges = true;
            foreach (var d in a)
            {
                if (i > 1)
                    t.ListRows.AddEx(i, 1);
                FillRow(t.ListRows[i].Range, d);
                if (d.Pledge)
                    pledgefound = true;
                i++;
            }
            Globals.Sheet1.ignoreChanges = false;
            if (pledgefound)
                Globals.Sheet1.Pledge.Value = 1;
            else
                Globals.Sheet1.Pledge.Value = 0;
        }
    }
}
