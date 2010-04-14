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
    public class OrgMembersDialogController : CmsStaffController
    {
        public ActionResult Index(int id, bool? inactives, bool? pendings, int? sg)
        {
            var m = new OrgMembersDialogModel 
            { 
                orgid = id, 
                inactives = inactives ?? false, 
                pendings = pendings ?? false,
                Pending = pendings ?? false,
                sg = sg,
            };
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Filter(OrgMembersDialogModel m)
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
        public ActionResult Update(OrgMembersDialogModel m)
        {
            foreach(var pid in m.List)
            {
                var om = DbUtil.Db.OrganizationMembers.Single(M => M.PeopleId == pid && M.OrganizationId == m.orgid);
                if (m.MemberType == (int)OrganizationMember.MemberTypeCode.Drop)
                    om.Drop();
                else if (m.MemberType > 0)
                    om.MemberTypeId = m.MemberType;

                if (m.InactiveDate.HasValue)
                    om.InactiveDate = m.InactiveDate;

                om.Pending = m.Pending;
            }
            DbUtil.Db.SubmitChanges();
            return View();
        }
    }
}
