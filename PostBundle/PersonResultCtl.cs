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
        public PersonResultCtl(CmsWs.PersonResult p)
        {
            InitializeComponent();
            person = p;
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

        private void NameLink_Click(object sender, EventArgs e)
        {
            Globals.ThisWorkbook.ClickToChange = true;
            var c = Globals.ThisWorkbook.Application.ActiveCell;
            c.Value2 = person.PeopleId.ToString();
            AddComment(c);
            Globals.ThisWorkbook.ClickToChange = false;
        }
        public void AddComment(Excel.Range c)
        {
            c.ClearComments();
            c.AddComment(NameInfo + "\n" + DisplayInfo);
        }
    }
}
