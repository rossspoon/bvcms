using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PostBundle
{
    public partial class PersonResultCtl : UserControl
    {
        public SearchPane pane;
        public PersonResultCtl(CmsWs.PersonResult p, SearchPane pane)
        {
            InitializeComponent();
            person = p;
            this.pane = pane;
        }
        private string NameInfo
        {
            get { return string.Format("{0} ({1})", _person.Name, _person.Age); ; }
        }

        private string DisplayInfo
        {
            get
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrEmpty(_person.Address))
                    sb.AppendFormat("{0}\n{1}",
                        _person.Address, _person.CSZ);
                if (!string.IsNullOrEmpty(_person.Phone))
                {
                    if (sb.Length > 0)
                        sb.Append("\n");
                    sb.Append(_person.Phone);
                }
                if (!string.IsNullOrEmpty(_person.Birthday))
                {
                    if (sb.Length > 0)
                        sb.Append("\n");
                    sb.Append(_person.Birthday);
                }
                return sb.ToString();
            }
        }
        public CmsWs.PersonResult _person;
        public CmsWs.PersonResult person
        {
            get { return _person; }
            set
            {
                _person = value;
                NameLink.Text = NameInfo;
                label1.Text = DisplayInfo;
            }
        }

        public void AddComment(Excel.Range c)
        {
            c.ClearComments();
            c.AddComment(NameInfo + "\n" + DisplayInfo);
        }

        private void NameLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Globals.ThisWorkbook.ClickToChange = true;
            var c = Globals.ThisWorkbook.Application.ActiveCell;
            c.Value2 = person.PeopleId.ToString();
            var n = c.Cells[1, 4] as Excel.Range;
            n.Value2 = person.Name;
            AddComment(n);
            Globals.ThisWorkbook.ClickToChange = false;
            if (pane != null)
            {
                pane.ClearIt();
                Globals.ThisWorkbook.Application.SendKeys("+{F6}", true);
            }
        }
    }
}
