using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CMSPresenter;
using CMSWeb.Models;

namespace CMSWeb.Areas.Main.Controllers
{
    public class OrgMemberDialogController : CmsStaffController
    {
        public ActionResult Index(int id, int pid, string from)
        {
            var m = DbUtil.Db.OrganizationMembers.SingleOrDefault(om => om.OrganizationId == id && om.PeopleId == pid);
            ViewData["from"] = from;
            if (m == null)
                return Content("cannot find membership: id={0} pid={1}".Fmt(id, pid));
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult CheckBoxChanged(string id, bool ck)
        {
            var a = id.Split('-');
            var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == a[2].ToInt() && m.OrganizationId == a[1].ToInt());
            if (ck)
                om.OrgMemMemTags.Add(new OrgMemMemTag { MemberTagId = a[3].ToInt() });
            else
            {
                var mt = om.OrgMemMemTags.Single(t => t.MemberTagId == a[3].ToInt());
                DbUtil.Db.OrgMemMemTags.DeleteOnSubmit(mt);
            }
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, int pid)
        {
            ViewData["MemberTypes"] = QueryModel.ConvertToSelect(CodeValueController.MemberTypeCodes(), "Id");
            var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == pid && m.OrganizationId == id);
            return View(om);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Display(int id, int pid)
        {
            var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == pid && m.OrganizationId == id);
            return View(om);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(int id, int pid)
        {
            var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == pid && m.OrganizationId == id);
            try
            {
                UpdateModel(om);
                DbUtil.Db.SubmitChanges();
            }
            catch (Exception)
            {
                ViewData["MemberTypes"] = QueryModel.ConvertToSelect(CodeValueController.MemberTypeCodes(), "Id");
                return View("Edit", om);
            }
            return View("Display", om);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Drop(string id)
        {
            var a = id.Split('-');
            var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m => m.PeopleId == a[2].ToInt() && m.OrganizationId == a[1].ToInt());
            if (om != null)
            {
                om.Drop();
                DbUtil.Db.SubmitChanges();
            }
            return Content("dropped");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Move(int id, int pid)
        {
            var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == pid && m.OrganizationId == id);
            ViewData["name"] = om.Person.Name;
            ViewData["oid"] = id;
            ViewData["pid"] = pid;
            if (om.Organization.DivisionId == null)
                return View((IEnumerable<OrgMove>)null);
            var q = from o in om.Organization.Division.Organizations
                    where o.OrganizationId != id
                    orderby o.OrganizationName
                    select new OrgMove
                    {
                         OrgName = o.OrganizationName,
                         id = "m-{0}-{1}-{2}".Fmt(id, pid, o.OrganizationId)
                    };
            return View(q.ToList());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MoveSelect(string id)
        {
            var a = id.Split('-');
            var om1 = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == a[2].ToInt() && m.OrganizationId == a[1].ToInt());
            var om2 = CmsData.OrganizationMember.InsertOrgMembers(a[3].ToInt(), om1.PeopleId, om1.MemberTypeId, DateTime.Now, om1.InactiveDate, om1.Pending ?? false);
            om2.Request = om1.Request;
            om2.Amount = om1.Amount;
            om2.UserData = om1.UserData;
            om1.Drop();
            DbUtil.Db.SubmitChanges();
            return Content("moved");
        }
        public class OrgMove
        {
            public string OrgName { get; set; }
            public string id { get; set; }
        }
    }
}
