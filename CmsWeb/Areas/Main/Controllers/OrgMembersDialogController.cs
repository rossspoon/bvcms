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
            var a = m.List.ToArray();
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
            return View();
        }
        public string HelpLink(string page)
        {
            return Util.HelpLink("SearchAdd_{0}".Fmt(page));
        }
    }
}
