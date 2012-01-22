using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using UtilityExtensions;
using CMSPresenter;
using CmsData;
using System.Web.SessionState;

namespace CmsWeb
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ToggleOrgLeadersOnly : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            Util2.OrgLeadersOnly = !Util2.OrgLeadersOnly;
            if (Util2.OrgLeadersOnly)
                DbUtil.Db.SetOrgLeadersOnly();
            context.Response.Redirect("~/");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
