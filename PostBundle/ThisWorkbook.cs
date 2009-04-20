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
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            m_value = value;
        }
        private T m_value;
        public T Value
        {
            get { return m_value; }
        }
    }
    public partial class ThisWorkbook
    {
        public bool ClickToChange = false;
        private SearchPane c;
        private BundlePane b;
        private UploadPane u;
        private void ThisWorkbook_Startup(object sender, System.EventArgs e)
        {
            c = new SearchPane();
            b = new BundlePane();
            u = new UploadPane();
            Globals.ThisWorkbook.ActionsPane.AutoScroll = true;
            c.Search += new EventHandler<EventArgs<List<CmsWs.PersonResult>>>(c_Search);
            b.Search += new EventHandler<EventArgs<List<CmsWs.BundleResult>>>(b_Search);
            Globals.Sheet1.BundleId.Select();
            AddBundlePane();

            //var ws = new CmsWs.cmsSoapClient();
            //var header = new CmsWs.ServiceAuthHeader();
            //header.Username = "david";
            //header.Password = "ddrr11";
            //var a = ws.SearchPerson(header, "dav car", "", "", "");
        }

        void b_Search(object sender, EventArgs<List<CmsWs.BundleResult>> e)
        {
            AddBundlePane();
            foreach (var b in e.Value)
                Globals.ThisWorkbook.ActionsPane.Controls.Add(new BundleResultCtl(b));
        }

        public void AddSearchPane()
        {
            Globals.ThisWorkbook.ActionsPane.Controls.Clear();
            Globals.ThisWorkbook.ActionsPane.Controls.Add(c);
        }
        public void AddBundlePane()
        {
            Globals.ThisWorkbook.ActionsPane.Controls.Clear();
            Globals.ThisWorkbook.ActionsPane.Controls.Add(b);
        }
        public void AddUploadPane()
        {
            Globals.ThisWorkbook.ActionsPane.Controls.Clear();
            Globals.ThisWorkbook.ActionsPane.Controls.Add(u);
        }

        void c_Search(object sender, EventArgs<List<CmsWs.PersonResult>> e)
        {
            AddSearchPane();
            foreach (var p in e.Value)
                Globals.ThisWorkbook.ActionsPane.Controls.Add(new PersonResultCtl(p));
        }

        private void ThisWorkbook_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisWorkbook_Startup);
            this.Shutdown += new System.EventHandler(ThisWorkbook_Shutdown);
        }

        #endregion

    }
}
