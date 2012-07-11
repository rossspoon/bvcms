using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace CmsWeb.Areas.Finance.Controllers
{
	[Authorize(Roles = "Admin")]
    public class VaultController : Controller
    {
        //
        // GET: /Finance/Vault/

        public ActionResult Index()
        {
            return View();
        }
		public ActionResult DeleteVaultData(int id)
		{
			var sage = new SagePayments(DbUtil.Db, testing: true);
			sage.deleteVaultData(id);
			var p = DbUtil.Db.LoadPersonById(id);
			p.ManagedGiving();
			DbUtil.Db.RecurringAmounts.DeleteAllOnSubmit(p.RecurringAmounts);
			DbUtil.Db.ManagedGivings.DeleteOnSubmit(p.ManagedGiving());
			DbUtil.Db.PaymentInfos.DeleteOnSubmit(p.PaymentInfo());
			DbUtil.Db.SubmitChanges();
			return Content("ok");
		}

    }
}
