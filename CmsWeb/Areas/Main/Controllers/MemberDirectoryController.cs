using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Models;

namespace CmsWeb.Areas.Main.Controllers
{
    public class MemberDirectoryController : CmsController
    {
        public ActionResult Index(int id)
        {
            if (DbUtil.Db.Organizations.Any(oo => oo.OrganizationId == id && oo.PublishDirectory == true) 
                && (User.IsInRole("Admin") || DbUtil.Db.OrganizationMembers.Any(
                        mm => mm.OrganizationId == id && mm.PeopleId == UtilityExtensions.Util.UserPeopleId)))
                return View(new MemberDirectoryModel(id));
            return RedirectToAction("NoAccess");
        }
        [HttpPost]
        public ActionResult Results(MemberDirectoryModel m)
        {
            if (User.IsInRole("Admin") ||
                DbUtil.Db.OrganizationMembers.Any(
                    mm => mm.OrganizationId == m.OrgId && mm.PeopleId == UtilityExtensions.Util.UserPeopleId))
                return View(m);
            return Content("unauthorized");
        }
        public ActionResult NoAccess()
        {
            return View("NoAccess");
        }
    }
}
