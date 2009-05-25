using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsData;
using UtilityExtensions;
using System.Diagnostics;

namespace CMSWeb.Admin
{
    public partial class DeleteSpecialTags : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbUtil.Db.DeleteSpecialTags(null);
        }
    }
}
