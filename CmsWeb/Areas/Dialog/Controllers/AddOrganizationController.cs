using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CmsData;
using CMSPresenter;
using CmsWeb.Models;
using UtilityExtensions;
using System.Text.RegularExpressions;
using System.Data.Linq;
using CmsData.Codes;

namespace CmsWeb.Areas.Dialog.Controllers
{
    public class AddOrganizationController : CmsStaffController
    {
        public ActionResult Index()
        {
			var id = Util2.CurrentOrgId;
			if (!id.HasValue)
				id = 1;
			var m = new NewOrganizationModel(id.Value);
			m.org.OrganizationName = "";
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Submit(int id, NewOrganizationModel m)
        {
            m.org.CreatedDate = Util.Now;
            m.org.CreatedBy = Util.UserId1;
            m.org.EntryPointId = null;
			if (m.org.CampusId == 0)
				m.org.CampusId = null;
            m.org.OrganizationStatusId = 30;

            DbUtil.Db.Organizations.InsertOnSubmit(m.org);
            DbUtil.Db.SubmitChanges();
			var org = DbUtil.Db.LoadOrganizationById(id);
			foreach (var div in org.DivOrgs)
				m.org.DivOrgs.Add(new DivOrg { Organization = m.org, DivId = div.DivId });
			if (m.copysettings)
			{
				foreach (var sc in org.OrgSchedules)
					m.org.OrgSchedules.Add(new OrgSchedule
					{
						OrganizationId = m.org.OrganizationId,
						AttendCreditId = sc.AttendCreditId,
						SchedDay = sc.SchedDay,
						SchedTime = sc.SchedTime,
						Id = sc.Id
					});
				m.org.CopySettings(DbUtil.Db, id);
			}
	        DbUtil.Db.SubmitChanges();
            DbUtil.LogActivity("Add new org {0}".Fmt(m.org.OrganizationName));
			return Content("<script>parent.CloseAddOrgDialog({0});</script>".Fmt(m.org.OrganizationId));
        }
    }
}
