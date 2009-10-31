using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Disciples
{
    public partial class SideBarArea : System.Web.UI.UserControl
    {
        private string _ContentName;
        public string ContentName
        {
            get { return _ContentName; }
            set { _ContentName = value; }
        }
        private bool _ListType;
        public bool ListType
        {
            get { return _ListType; }
            set { _ListType = value; }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Paragraph1.ContentName = ContentName;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            header.Text = Paragraph1.HeaderText;
            //        if (ListType)
            //        {
            //            Panel div = new Panel();
            //            div.CssClass = "CommonListArea";
            //            div.Controls.Add(paragraph);
            //        }
            //        else
            //            content.Controls.Add(paragraph);
        }
    }
}