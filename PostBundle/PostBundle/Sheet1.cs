using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace PostBundle
{
    public partial class Sheet1
    {
        public bool ignoreChanges = false;

        private void Sheet1_Startup(object sender, System.EventArgs e)
        {
            PeopleId.Selected += new Excel.DocEvents_SelectionChangeEventHandler(PeopleId_Selected);
            BundleId.Selected += new Excel.DocEvents_SelectionChangeEventHandler(BundleId_Selected);
            BundleTotal.Selected += new Microsoft.Office.Interop.Excel.DocEvents_SelectionChangeEventHandler(BundleTotal_Selected);
            PeopleId.Change += new Microsoft.Office.Interop.Excel.DocEvents_ChangeEventHandler(PeopleId_Change);
        }

        void BundleTotal_Selected(Microsoft.Office.Interop.Excel.Range Target)
        {
            Globals.ThisWorkbook.AddUploadPane();
        }

        void PeopleId_Change(Microsoft.Office.Interop.Excel.Range Target)
        {
            if (ignoreChanges)
                return;
            var c = Target as Excel.Range;

            var d = c.Cells[1, 3] as Excel.Range;
            d.Value2 = BundleDate.Value;
            var f = c.Cells[1, 4] as Excel.Range;
            f.Value2 = BundleFund.Value;
            var n = c.Cells[1, 5] as Excel.Range;

            if (Globals.ThisWorkbook.ClickToChange)
                return;
            if (c.Text.ToString() != "")
            {
                var ws = Globals.ThisWorkbook.ws;
                var header = Globals.ThisWorkbook.header;
                var a = ws.SearchPerson(header, c.Text.ToString(), "", "", "");
                if (a.Length == 1)
                {
                    n.Value2 = a[0].Name;
                    var ctl = new PersonResultCtl(a[0], null);
                    ctl.AddComment(n);
                }
            }
        }

        void BundleId_Selected(Excel.Range Target)
        {
            Globals.ThisWorkbook.AddBundlePane();
        }

        void PeopleId_Selected(Excel.Range Target)
        {
            Globals.ThisWorkbook.AddSearchPane();
        }

        private void Sheet1_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(Sheet1_Startup);
            this.Shutdown += new System.EventHandler(Sheet1_Shutdown);
        }

        #endregion

    }
}
