using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsWeb.Areas.Dialog.Models;
using UtilityExtensions;

namespace CmsWeb.Areas.Dialog.Controllers
{
    public class TransactionHistoryController : CmsStaffController
    {
        public ActionResult Index(int id, int oid)
        {
            var m = new TransactionHistoryModel(id, oid);
            ViewBag.orgid = oid;
            ViewBag.PeopleId = id;
            ViewBag.IsMember = DbUtil.Db.OrganizationMembers.Any(mm => mm.OrganizationId == oid && mm.PeopleId == id);
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
        public ActionResult DeleteAll(int orgid, int peopleid)
        {
            var q = DbUtil.Db.EnrollmentTransactions.Where(tt => tt.OrganizationId == orgid && tt.PeopleId == peopleid);
            DbUtil.Db.EnrollmentTransactions.DeleteAllOnSubmit(q);
            DbUtil.Db.SubmitChanges();
            return Content("ok");
        }
        [HttpPost]
        public ActionResult Edit(string id, DateTime value)
        {
            var iid = id.Substring(2).ToInt();
            var t = DbUtil.Db.EnrollmentTransactions.Single(tt => tt.TransactionId == iid);
            t.TransactionDate = value;
            DbUtil.Db.SubmitChanges();
            return Content(value.ToString("g"));
        }
    }
}
