using System;
using System.Linq;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;
using CmsData;
using UtilityExtensions;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Mvc;

namespace CmsWeb
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/Home");
        }
    }
}
