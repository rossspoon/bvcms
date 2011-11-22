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
        [HttpGet]
        public ActionResult OrganizationsForDiv(int id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<Organizations error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.OrganizationsForDiv(id), "text/xml");
        }
        [HttpGet]
        public ActionResult Lookups(string id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<Lookups error=\"{0}\" />".Fmt(ret.Substring(1)));
            if (!id.HasValue())
                return Content("Lookups error=\"not found\">");
            var q = DbUtil.Db.ExecuteQuery<CmsWeb.Areas.Setup.Controllers.LookupController.Row>("select * from lookup." + id);
            var w = new CmsData.API.APIWriter();
            w.Start("Lookups");
            w.Attr("name", id);
            foreach(var i in q)
            {
                w.Start("Lookup");
                w.Attr("Id", i.Id);
                w.AddText(i.Description);
                w.End();
            }
            w.End();
            return Content(w.ToString(), "text/xml");
        }
        [HttpGet]
        public ActionResult OrgMembers2(int id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<OrgMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.OrgMembersPython(id), "text/xml");
        }
        [HttpGet]
        public ActionResult OrgMembers(int id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<OrgMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIFunctions(DbUtil.Db);
            return Content(api.OrgMembers(id), "text/xml");
        }
        [HttpGet]
        public ActionResult ExtraValues(int id, string fields)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<ExtraValues error=\"{0}\" />".Fmt(ret.Substring(1)));
            return Content(new APIFunctions().ExtraValues(id, fields), "text/xml");
        }
        [HttpPost]
        public ActionResult AddEditExtraValue(int peopleid, string field, string value)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            return Content(new APIFunctions().AddEditExtraValue(peopleid, field, value));
        }
        [HttpPost]
        public ActionResult DeleteExtraValue(int peopleid, string field)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            new APIFunctions().DeleteExtraValue(peopleid, field);
            return Content("ok");
        }
        [HttpPost]
        public ActionResult MarkRegistered(int meetingid, int peopleid, bool present)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            Attend.MarkRegistered(peopleid, meetingid, present);
            return Content("ok");
        }
        [HttpPost]
        public ActionResult GetOneTimeLoginLink(string url, string user)
        {
            var ret = AuthenticateDeveloper(log: true);
            if (ret.StartsWith("!"))
                return Content(url);
            var ot = new OneTimeLink
            {
                Id = Guid.NewGuid(),
                Querystring = user,
                Expires = DateTime.Now.AddMinutes(10),
            };
            DbUtil.Db.OneTimeLinks.InsertOnSubmit(ot);
            DbUtil.Db.SubmitChanges();
            return Content("{0}Logon?ReturnUrl={1}&otltoken={2}".Fmt(
                Util.CmsHost2, 
                HttpUtility.UrlEncode(url), 
                ot.Id.ToCode()));
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
        [HttpPost]
        public ActionResult UpdateOrgMember(int OrgId, int PeopleId, int? type, DateTime? enrolled, string inactive)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            new APIFunctions().UpdateOrgMember(OrgId, PeopleId, type, enrolled, inactive);
            return Content("ok");
        }
    }
}