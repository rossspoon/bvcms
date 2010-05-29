using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CMSWeb.Models;
using CMSPresenter;

namespace CMSWeb.Areas.Main.Controllers
{
    public class OrgGroupsController : CmsStaffController
    {
        public ActionResult Index(int id, bool? inactives, bool? pendings, int? sg, int? groupid)
        {
            var m = new OrgGroupsModel
            {
                orgid = id,
                inactives = inactives ?? false,
                pendings = pendings ?? false,
                Pending = pendings ?? false,
                sg = sg,
                groupid = groupid,
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
        public ActionResult Update(OrgGroupsModel m)
        {
            var a = m.List.ToArray();
            if (m.groupid.HasValue)
            {
                var sgname = DbUtil.Db.MemberTags.Single(mt => mt.Id == m.groupid).Name;
                var q1 = from om in m.OrgMembers()
                        where !a.Contains(om.PeopleId)
                        select om;
                foreach(var om in q1)
                    om.RemoveFromGroup(sgname);
                var q2 = from om in m.OrgMembers()
                        where a.Contains(om.PeopleId)
                        select om;
                foreach (var om in q2)
                    om.AddToGroup(sgname);
            }
            else
            {
                var q = from om in m.OrgMembers()
                        where a.Contains(om.PeopleId)
                        select om;
                foreach (var om in q)
                {
                    if (m.MemberType == (int)OrganizationMember.MemberTypeCode.Drop)
                        om.Drop();
                    else
                    {
                        if (m.MemberType > 0)
                            om.MemberTypeId = m.MemberType;
                        if (m.InactiveDate.HasValue)
                            om.InactiveDate = m.InactiveDate;
                        om.Pending = m.Pending;
                    }
                }
                DbUtil.Db.SubmitChanges();
            }
            return View();
        }
    }
}
