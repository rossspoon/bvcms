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
    public class APIOrgController : CmsController
    {
        [HttpGet]
        public ActionResult OrganizationsForDiv(int id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<Organizations error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIOrganization(DbUtil.Db);
            return Content(api.OrganizationsForDiv(id), "text/xml");
        }
        [HttpGet]
        public ActionResult OrgMembers2(int id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<OrgMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIOrganization(DbUtil.Db);
            return Content(api.OrgMembersPython(id), "text/xml");
        }
        [HttpGet]
        public ActionResult OrgMembers(int id, string search)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<OrgMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIOrganization(DbUtil.Db);
            return Content(api.OrgMembers(id, search), "text/xml");
        }
        [HttpGet]
        public ActionResult ExtraValues(int id, string fields)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<ExtraValues error=\"{0}\" />".Fmt(ret.Substring(1)));
            return Content(new APIOrganization(DbUtil.Db)
                .ExtraValues(id, fields), "text/xml");
        }
        [HttpPost]
        public ActionResult AddEditExtraValue(int peopleid, string field, string value)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            return Content(new APIOrganization(DbUtil.Db)
                .AddEditExtraValue(peopleid, field, value));
        }
        [HttpPost]
        public ActionResult DeleteExtraValue(int orgid, string field)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            return Content(new APIOrganization(DbUtil.Db)
                .DeleteExtraValue(orgid, field));
        }
        [HttpPost]
        public ActionResult UpdateOrgMember(int OrgId, int PeopleId, int? type, DateTime? enrolled, string inactive)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            new APIOrganization(DbUtil.Db)
                .UpdateOrgMember(OrgId, PeopleId, type, enrolled, inactive);
            return Content("ok");
        }
    }
}