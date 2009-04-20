using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using UtilityExtensions;

namespace CMSWeb
{
    public partial class SavedQueries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GridPager.SetPageSize(this.GridView1);
        }
        public bool CanEdit(object user)
        {
            var isdev = Roles.IsUserInRole("Developer");
            if (isdev)
                return true;
            return string.Compare(Util.UserName,(string)user, true) == 0;
        }
    }
}
