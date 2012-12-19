using System;
using System.IO;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsData.API;

namespace CmsWeb.Areas.Public.Controllers
{
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
        public ActionResult AddEditExtraValue(int orgid, string field, string value)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
			DbUtil.LogActivity("APIOrg AddEditExtraValue {0}, {1}, {2}".Fmt(orgid, field, value));
            return Content(new APIOrganization(DbUtil.Db)
                .AddEditExtraValue(orgid, field, value));
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
        public ActionResult UpdateOrgMember(int OrgId, int PeopleId, string type, DateTime? enrolled, string inactive, bool? pending)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            new APIOrganization(DbUtil.Db)
                .UpdateOrgMember(OrgId, PeopleId, type, enrolled, inactive, pending);
			DbUtil.LogActivity("APIOrg UpdateOrgMember {0}, {1}".Fmt(OrgId, PeopleId));
            return Content("ok");
        }
		[HttpPost]
		public ActionResult NewOrganization(int id, string name, string location, int? ParentOrgId)
		{
			var ret = AuthenticateDeveloper();
			if (ret.StartsWith("!"))
				return Content(@"<NewOrganization status=""error"">" + ret.Substring(1) + "</NewOrganization>");
			DbUtil.LogActivity("APIOrganization NewOrganization");
			return Content(new APIOrganization(DbUtil.Db).NewOrganization(id, name, location, ParentOrgId), "text/xml");
		}
        [HttpPost]
        public ActionResult UpdateOrganization(int orgId, string name, string campusid, string active, string location, string description)
        {
            var ret = AuthenticateDeveloper();
            if (ret.StartsWith("!"))
                return Content(ret.Substring(1));
            new APIOrganization(DbUtil.Db)
                .UpdateOrganization(orgId, name, campusid, active, location, description);
			DbUtil.LogActivity("APIOrg UpdateOrganization {0}".Fmt(orgId));
            return Content("ok");
        }
		[HttpPost]
		public ActionResult AddOrgMember(int OrgId, int PeopleId, string MemberType, bool? pending)
		{
			var ret = AuthenticateDeveloper();
			if (ret.StartsWith("!"))
				return Content(@"<AddOrgMember status=""error"">" + ret.Substring(1) + "</AddOrgMember>");
			DbUtil.LogActivity("APIOrganization AddOrgMember");
			return Content(new APIOrganization(DbUtil.Db).AddOrgMember(OrgId, PeopleId, MemberType, pending), "text/xml");
		}
		[HttpPost]
		public ActionResult DropOrgMember(int OrgId, int PeopleId, string MemberType)
		{
			var ret = AuthenticateDeveloper();
			if (ret.StartsWith("!"))
				return Content(@"<DropOrgMember status=""error"">" + ret.Substring(1) + "</DropOrgMember>");
			DbUtil.LogActivity("APIOrganization DropOrgMember");
			return Content(new APIOrganization(DbUtil.Db).DropOrgMember(OrgId, PeopleId), "text/xml");
		}
		public ActionResult ParentOrgs(int id, string extravalue1, string extravalue2)
		{
			var ret = AuthenticateDeveloper();
			if (ret.StartsWith("!"))
				return Content(@"<ParentOrgs status=""error"">" + ret.Substring(1) + "</ParentOrgs>");
			DbUtil.LogActivity("APIOrganization ParentOrgs");
			return Content(new APIOrganization(DbUtil.Db).ParentOrgs(id, extravalue1, extravalue2), "text/xml");
		}
		public ActionResult ChildOrgs(int id, string extravalue1, string extravalue2)
		{
			var ret = AuthenticateDeveloper();
			if (ret.StartsWith("!"))
				return Content(@"<ChildOrgs status=""error"">" + ret.Substring(1) + "</ChildOrgs>");
			DbUtil.LogActivity("APIOrganization ChildOrgs");
			return Content(new APIOrganization(DbUtil.Db).ChildOrgs(id, extravalue1, extravalue2), "text/xml");
		}
    }
}