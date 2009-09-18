using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

namespace CMSWeb.DynamicData.FieldTemplates
{
    public partial class DateCalendar : System.Web.DynamicData.FieldTemplateUserControl
    {
        public override Control DataControl
        {
            get
            {
                return Literal1;
            }
        }
        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            string s = string.Empty;
            if (FieldValue != null)
            {
                var dt = (DateTime)FieldValue;
                s = dt.ToShortDateString();
            }
            Literal1.Text = s;
        }
    }
}
