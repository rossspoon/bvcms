using System;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsData.API;

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
			DbUtil.LogActivity("APIOrg Organizations for Div " + id);
            return Content(api.OrganizationsForDiv(id), "text/xml");
        }
        [HttpGet]
        public ActionResult OrgMembers2(int id)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<OrgMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIOrganization(DbUtil.Db);
			DbUtil.LogActivity("APIOrg OrgMembers2 " + id);
            return Content(api.OrgMembersPython(id), "text/xml");
        }
        [HttpGet]
        public ActionResult OrgMembers(int id, string search)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<OrgMembers error=\"{0}\" />".Fmt(ret.Substring(1)));
            var api = new APIOrganization(DbUtil.Db);
			DbUtil.LogActivity("APIOrg OrgMembers " + id);
            return Content(api.OrgMembers(id, search), "text/xml");
        }
        [HttpGet]
        public ActionResult ExtraValues(int id, string fields)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content("<ExtraValues error=\"{0}\" />".Fmt(ret.Substring(1)));
			DbUtil.LogActivity("APIOrg ExtraValues {0}, {1}".Fmt(id, fields));
            return Content(new APIOrganization(DbUtil.Db)
                .ExtraValues(id, fields), "text/xml");
        }
        [HttpPost]
        public ActionResult AddEditExtraValue(int peopleid, string field, string value)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
			DbUtil.LogActivity("APIOrg AddEditExtraValue {0}, {1}, {2}".Fmt(peopleid, field, value));
            return Content(new APIOrganization(DbUtil.Db)
                .AddEditExtraValue(peopleid, field, value));
        }
        [HttpPost]
        public ActionResult DeleteExtraValue(int orgid, string field)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
			DbUtil.LogActivity("APIOrg DeleteExtraValue {0}, {1}".Fmt(orgid, field));
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
			DbUtil.LogActivity("APIOrg UpdateOrgMember {0}, {1}".Fmt(OrgId, PeopleId));
            return Content("ok");
        }
    }
}