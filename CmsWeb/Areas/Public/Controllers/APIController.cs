using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using System.Xml;
using System.IO;
using System.Net.Mail;
using CmsData.Codes;
using System.Text;
using System.Net;
using CmsWeb.Areas.Manage.Controllers;

namespace CmsWeb.Areas.Public.Controllers
{
#if DEBUG
#else
    [RequireHttps]
#endif
    public class APIController : CmsController
    {
        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            Response.ContentType = "text/xml";
            //var user = Request.Headers["user"];
            //var password = Request.Headers["password"];
            var ret = Authenticate(log: true);
            if (ret.StartsWith("!"))
                return Content("<Login error=\"{0}\" />".Fmt(ret.Substring(1)));
            var valid = CMSMembershipProvider.provider.ValidateUser(user, password);
            if (!valid)
                return Content("<Login error=\"{0} not valid\" />".Fmt(user ?? "(null)"));
            var u = DbUtil.Db.Users.Single(uu => uu.Username == user);
            return Content(APIFunctions.Login(DbUtil.Db, u));
        }
#if DEBUG
    [RequireBasicAuthentication]
#endif
        public ActionResult Organizations(int id)
        {
            Response.ContentType = "text/xml";
            var ret = Authenticate(log: true);
            if (ret.StartsWith("!"))
                return Content("<Organizations error=\"{0}\" />".Fmt(ret.Substring(1)));
            return new OrganizationsResult(id);
        }
        private string Authenticate(bool log = false)
        {
            var auth = Request.Headers["Authorization"];
            if (auth.HasValue())
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(auth.Substring(6))).Split(':');
                var username = cred[0];
                var password = cred[1];

                string ret = null;
                var valid = CMSMembershipProvider.provider.ValidateUser(username, password);
                if (valid)
                {
                    var roles = CMSRoleProvider.provider;
                    var u = AccountController.SetUserInfo(username, Session);
                    if (!roles.IsUserInRole(username, "Developer"))
                        valid = false;
                }
                if (valid)
                    ret = " API {0} authenticated".Fmt(username);
                else
                    ret = "!API {0} not authenticated".Fmt(username);
                if (log)
                    DbUtil.LogActivity(ret.Substring(1));
                return ret;
            }
            return "!API no Authorization Header";
        }
    }
}
