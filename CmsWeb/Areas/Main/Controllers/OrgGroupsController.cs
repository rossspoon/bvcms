using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;

namespace CmsWeb.Areas.Main.Controllers
{
    public class OrgGroupsController : CmsStaffController
    {
        public ActionResult Index(int id)
        {
            var m = new OrgGroupsModel( id );
            return View(m);
        }
        [HttpPost]
        public ActionResult Filter(OrgGroupsModel m)
        {
            return View("Rows", m);
        }
        [HttpPost]
        public ActionResult Display(int id, int pid)
        {
            var om = DbUtil.Db.OrganizationMembers.Single(m => m.PeopleId == pid && m.OrganizationId == id);
            return View(om);
        }
        [HttpPost]
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
        [HttpPost]
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
        [HttpPost]
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
        [HttpPost]
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
        [HttpPost]
        public ActionResult DeleteGroup(OrgGroupsModel m)
        {
            var group = DbUtil.Db.MemberTags.SingleOrDefault(g => g.Id == m.groupid);
            if (group != null)
            {
                DbUtil.Db.OrgMemMemTags.DeleteAllOnSubmit(group.OrgMemMemTags);
                DbUtil.Db.MemberTags.DeleteOnSubmit(group);
                DbUtil.Db.SubmitChanges();
                m.groupid = (from v in m.Groups()
                             where v.Value != "0"
                             select v.Value).FirstOrDefault().ToInt();
                ViewData["groupid"] = m.groupid.ToString();
            }
            return View("Form", m);
        }

        public ActionResult UpdateScore(string id, int value)
        {
            string[] split = id.Split('-');
            int orgID = split[0].ToInt();
            int peopleID = split[1].ToInt();

            var member = (from e in DbUtil.Db.OrganizationMembers
                          where e.OrganizationId == orgID
                          where e.PeopleId == peopleID
                          select e).SingleOrDefault();

            member.Score = value;
            DbUtil.Db.SubmitChanges();

            return Content(value.ToString());
        }

        public ActionResult SwapPlayers(string pOne, string pTwo)
        {
            string[] splitOne = pOne.Split('-');
            int orgIDOne = splitOne[0].ToInt();
            int peopleIDOne = splitOne[1].ToInt();

            string[] splitTwo = pTwo.Split('-');
            int orgIDTwo = splitTwo[0].ToInt();
            int peopleIDTwo = splitTwo[1].ToInt();

            var playerOne = (from e in DbUtil.Db.OrganizationMembers
                          where e.OrganizationId == orgIDOne
                          where e.PeopleId == peopleIDOne
                          select e).SingleOrDefault();

            var playerTwo = (from e in DbUtil.Db.OrganizationMembers
                             where e.OrganizationId == orgIDTwo
                             where e.PeopleId == peopleIDTwo
                             select e).SingleOrDefault();


            var pOneTag = playerOne.OrgMemMemTags.FirstOrDefault();
            var pTwoTag = playerTwo.OrgMemMemTags.FirstOrDefault();

            var pOneNew = new OrgMemMemTag();
            var pTwoNew = new OrgMemMemTag();

            pOneNew.PeopleId = pOneTag.PeopleId;
            pOneNew.OrgId = pTwoTag.OrgId;
            pOneNew.MemberTagId = pTwoTag.MemberTagId;

            pTwoNew.PeopleId = pTwoTag.PeopleId;
            pTwoNew.OrgId = pOneTag.OrgId;
            pTwoNew.MemberTagId = pOneTag.MemberTagId;

            DbUtil.Db.OrgMemMemTags.DeleteOnSubmit(pOneTag);
            DbUtil.Db.OrgMemMemTags.DeleteOnSubmit(pTwoTag);
            DbUtil.Db.SubmitChanges();

            DbUtil.Db.OrgMemMemTags.InsertOnSubmit(pOneNew);
            DbUtil.Db.OrgMemMemTags.InsertOnSubmit(pTwoNew);
            DbUtil.Db.SubmitChanges();

            return Content("Complete");
        }
    }
}
