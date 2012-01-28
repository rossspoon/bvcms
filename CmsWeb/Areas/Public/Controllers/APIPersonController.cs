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
    public class APIPersonController : CmsController
    {
        [HttpPost]
        public ActionResult Login(string user, string password)
        {
            var ret = AuthenticateDeveloper(log: true);
            if (ret.StartsWith("!"))
                return Content("<Login error=\"{0}\" />".Fmt(ret.Substring(1)));
            var valid = CMSMembershipProvider.provider.ValidateUser(user, password);
            if (!valid)
                return Content("<Login error=\"{0} not valid\" />".Fmt(user ?? "(null)"));
            var u = DbUtil.Db.Users.Single(uu => uu.Username == user);
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.Login(u.Person),"text/xml");
        }
        [HttpGet]
        public ActionResult LoginInfo(int id)
        {
            var ret = AuthenticateDeveloper(log: true);
            if (ret.StartsWith("!"))
                return Content("<LoginInfo error=\"{0}\" />".Fmt(ret.Substring(1)));
            var p = DbUtil.Db.People.Single(pp => pp.PeopleId == id);
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.Login(p), "text/xml");
        }
        [HttpPost]
        public ActionResult GetOneTimeLoginLink(string url, string user)
        {
            var ret = AuthenticateDeveloper(log: true);
            if (ret.StartsWith("!"))
                return Content(url);
            return Content(GetOTLoginLink(url, user));
        }
        public static string GetOTLoginLink(string url, string user)
        {
            var ot = new OneTimeLink
            {
                Id = Guid.NewGuid(),
                Querystring = user,
                Expires = DateTime.Now.AddHours(24)
            };
            DbUtil.Db.OneTimeLinks.InsertOnSubmit(ot);
            DbUtil.Db.SubmitChanges();
            return "{0}Logon?ReturnUrl={1}&otltoken={2}".Fmt(
                Util.CmsHost2, 
                HttpUtility.UrlEncode(url), 
                ot.Id.ToCode());
        }
        [HttpPost]
        public ActionResult GetOneTimeRegisterLink(int OrgId, int PeopleId)
        {
            var ret = AuthenticateDeveloper(log: true);
            if (ret.StartsWith("!"))
                return Content("/");
            var ot = new OneTimeLink
            {
                Id = Guid.NewGuid(),
                Querystring = "{0},{1},0".Fmt(OrgId, PeopleId),
                Expires = DateTime.Now.AddMinutes(10),
            };
            DbUtil.Db.OneTimeLinks.InsertOnSubmit(ot);
            DbUtil.Db.SubmitChanges();
            return Content(Util.CmsHost2 + "OnlineReg/RegisterLink/" + ot.Id.ToCode());
        }
        [HttpGet]
        public ActionResult ExtraValues(int id, string fields)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<ExtraValues error=\"{0}\" />".Fmt(ret.Substring(1)));
            return Content(new APIFunctions(DbUtil.Db).ExtraValues(id, fields), "text/xml");
        }
        [HttpPost]
        public ActionResult AddEditExtraValue(int peopleid, string field, string value, string type = "data")
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            return Content(new APIFunctions(DbUtil.Db).AddEditExtraValue(peopleid, field, value, type));
        }
        [HttpPost]
        public ActionResult DeleteExtraValue(int peopleid, string field)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            new APIFunctions(DbUtil.Db).DeleteExtraValue(peopleid, field);
            return Content("ok");
        }
        [HttpPost]
        public ActionResult ChangePassword(string username, string current, string password)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            var m = new MembershipService();
            try
            {
                var ok = m.ChangePassword(username, current, password);
                if (ok)
                    return Content("ok");
                else
                    return Content("<ChangePassword error=\"invalid password\" />");
            }
            catch (Exception ex)
            {
                return Content("<ChangePassword error=\"{0}\" />".Fmt(ex.Message));
            }
        }
        [HttpGet]
        public ActionResult FamilyMembers(int id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<FamilyMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            return Content(new APIFunctions(DbUtil.Db).FamilyMembers(id), "text/xml");
        }
        [HttpGet]
        public ActionResult AccessUsers()
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<AccessUsers error=\"{0}\" />".Fmt(ret.Substring(1)));
            return Content(new APIFunctions(DbUtil.Db).AccessUsersXml(), "text/xml");
        }
    }
}