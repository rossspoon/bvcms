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
using CmsData.API;
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
        [RequireBasicAuthentication]
        public ActionResult Login(string user, string password)
        {
            Response.ContentType = "text/xml";
            var ret = Authenticate(log: true);
            if (ret.StartsWith("!"))
                return Content("<Login error=\"{0}\" />".Fmt(ret.Substring(1)));
            var valid = CMSMembershipProvider.provider.ValidateUser(user, password);
            if (!valid)
                return Content("<Login error=\"{0} not valid\" />".Fmt(user ?? "(null)"));
            var u = DbUtil.Db.Users.Single(uu => uu.Username == user);
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.Login(u.Person));
        }
        [HttpGet]
        [RequireBasicAuthentication]
        public ActionResult LoginInfo(int id)
        {
            Response.ContentType = "text/xml";
            var ret = Authenticate(log: true);
            if (ret.StartsWith("!"))
                return Content("<LoginInfo error=\"{0}\" />".Fmt(ret.Substring(1)));
            var p = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.Login(p));
        }
        [RequireBasicAuthentication]
        public ActionResult Organizations(int id)
        {
            Response.ContentType = "text/xml";
            var ret = Authenticate();
            if (ret.StartsWith("!"))
                return Content("<Organizations error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.Organizations(id));
        }
        [RequireBasicAuthentication]
        public ActionResult Lookups(string id)
        {
            Response.ContentType = "text/xml";
            var ret = Authenticate();
            if (ret.StartsWith("!"))
                return Content("<Lookups error=\"{0}\" />".Fmt(ret.Substring(1)));
            if (!id.HasValue())
                return Content("Lookups error=\"not found\">");
            ViewData["name"] = id;
            var q = DbUtil.Db.ExecuteQuery<CmsWeb.Areas.Setup.Controllers.LookupController.Row>("select * from lookup." + id);
            return View(q);
        }
        [RequireBasicAuthentication]
        public ActionResult OrgMembers(int id)
        {
            Response.ContentType = "text/xml";
            var ret = Authenticate();
            if (ret.StartsWith("!"))
                return Content("<OrgMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.OrgMembers(id));
        }
        [HttpGet]
        [RequireBasicAuthentication]
        public ActionResult ExtraValues(int id, string fields, string value)
        {
            Response.ContentType = "text/xml";
            var ret = Authenticate();
            if (ret.StartsWith("!"))
                return Content("<ExtraValues error=\"{0}\" />".Fmt(ret.Substring(1)));
            return Content(new APIFunctions().ExtraValues(id, fields));
        }
        [HttpPost]
        public ActionResult AddEditExtraValue(int id, string field, string value)
        {
            var ret = Authenticate();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            return Content(APIFunctions.AddEditExtraValue(id, field, value));
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
        [HttpPost]
        public ActionResult MarkRegistered(int id, int PeopleId, bool Present)
        {
            var ret = Authenticate();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            Attend.MarkRegistered(PeopleId, id, Present);
            return Content("ok");
        }
    }
}