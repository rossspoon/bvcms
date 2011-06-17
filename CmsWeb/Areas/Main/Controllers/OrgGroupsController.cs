using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using CMSPresenter;

namespace CmsWeb.Areas.Main.Controllers
{
    public class OrgGroupsController : CmsStaffController
    {
        public ActionResult Index(int id)
        {
            var m = new OrgGroupsModel
            {
                orgid = id
            };
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Filter(OrgGroupsModel m)
        {
            return View("Rows", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Display(int id, int pid)
        {
            var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == pid && m.OrganizationId == id);
            return View(om);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AssignSelectedToTargetGroup(OrgGroupsModel m)
        {
            var a = m.List.ToArray();
            var sgname = DbUtil.Db.MemberTags.Single(mt => mt.Id == m.groupid).Name;
            var q2 = from om in m.OrgMembers()
                     where !om.OrgMemMemTags.Any(mt => mt.MemberTag.Name == sgname)
                     where a.Contains(om.PeopleId)
                     select om;
            foreach (var om in q2)
                om.AddToGroup(DbUtil.Db, sgname);
            DbUtil.Db.SubmitChanges();
            return View("Rows", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RemoveSelectedFromTargetGroup(OrgGroupsModel m)
        {
            var a = m.List.ToArray();
            var sgname = DbUtil.Db.MemberTags.Single(mt => mt.Id == m.groupid).Name;
            var q1 = from omt in DbUtil.Db.OrgMemMemTags
                     where omt.OrgId == m.orgid
                     where omt.MemberTag.Name == sgname
                     where a.Contains(omt.PeopleId)
                     select omt;
            DbUtil.Db.OrgMemMemTags.DeleteAllOnSubmit(q1);
            DbUtil.Db.SubmitChanges();
            return View("Rows", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MakeNewGroup(OrgGroupsModel m)
        {
            if (!m.GroupName.HasValue())
                return Content("error: no group name");
            var Db = DbUtil.Db;
            var group = Db.MemberTags.SingleOrDefault(g =>
                g.Name == m.GroupName && g.OrgId == m.orgid);
            if (group == null)
            {
                group = new MemberTag
                {
                    Name = m.GroupName,
                    OrgId = m.orgid
                };
                Db.MemberTags.InsertOnSubmit(group);
                Db.SubmitChanges();
            }
            m.groupid = group.Id;
            ViewData["newgid"] = group.Id;
            return View("Form", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RenameGroup(OrgGroupsModel m)
        {
            if (!m.GroupName.HasValue() || m.groupid == 0)
                return Content("error: no group name");
            var group = DbUtil.Db.MemberTags.SingleOrDefault(d => d.Id == m.groupid);
            group.Name = m.GroupName;
            DbUtil.Db.SubmitChanges();
            m.GroupName = null;
            return View("Form", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteGroup(OrgGroupsModel m)
        {
            var group = DbUtil.Db.MemberTags.SingleOrDefault(g => g.Id == m.groupid);
            if (group != null)
            {
                DbUtil.Db.OrgMemMemTags.DeleteAllOnSubmit(group.OrgMemMemTags);
                DbUtil.Db.MemberTags.DeleteOnSubmit(group);
                DbUtil.Db.SubmitChanges();
            }
            return View("Form", m);
        }
    }
}
