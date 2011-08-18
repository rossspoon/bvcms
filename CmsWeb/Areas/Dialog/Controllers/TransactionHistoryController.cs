using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Areas.Dialog.Models;

namespace CmsWeb.Areas.Dialog.Controllers
{
    public class TransactionHistoryController : Controller
    {
        public ActionResult Index(int id, int oid)
        {
            var m = new TransactionHistoryModel(id, oid);
            return View(m);
        }
        public ActionResult Delete(int id)
        {
            var t = DbUtil.Db.EnrollmentTransactions.Single(tt => tt.TransactionId == id);
            var m = new TransactionHistoryModel(t.PeopleId, t.OrganizationId);
            DbUtil.Db.EnrollmentTransactions.DeleteOnSubmit(t);
            DbUtil.Db.SubmitChanges();
            return View("History", m.FetchHistory());
        }
    }
}
