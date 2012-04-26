using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles="Edit, ManageTransactions")]
    public class TransactionsController : CmsStaffController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var m = new TransactionsModel();
            return View(m);
        }

        [HttpPost]
        public ActionResult List(TransactionsModel m)
        {
            UpdateModel(m.Pager);
            return View(m);
        }
		[HttpPost]
		public ActionResult Export(TransactionsModel m)
        {
            return new TransactionsExcelResult(m);
        }
		[HttpGet]
		[Authorize(Roles = "Finance")]
		public ActionResult RunRecurringGiving()
		{
			var count = RecurringGiving.DoAllGiving(DbUtil.Db);
			return Content(count.ToString());
		}
		[HttpPost]
		[Authorize(Roles = "Finance")]
		public ActionResult CreditVoid(int id, string type, TransactionsModel m)
		{
			var t = DbUtil.Db.Transactions.SingleOrDefault(tt => tt.Id == id);
			if (t == null)
				return Content("notran");
			var sage = new SagePayments(DbUtil.Db, false);
			TransactionResponse resp;
			if (type == "Void")
			{
				resp = sage.voidTransactionRequest(t.TransactionId);
				if (resp.Approved)
					t.Voided = true;
			}
			else
			{
				resp = sage.creditTransactionRequest(t.TransactionId, t.Amt ?? 0);
				if (resp.Approved)
					t.Credited = true;
			}
			if(!resp.Approved)
			{
				return Content("error: " + resp.Message);
			}
			else // approved
			{
				var tt = new Transaction
				{
					TransactionId = resp.TransactionId,
					Name = t.Name,
					Amt = -t.Amt,
					Approved = true,
					AuthCode = t.AuthCode,
					Message = t.Message,
					Donate = -t.Donate,
					Regfees = -t.Regfees,
					TransactionDate = DateTime.Now,
					TransactionGateway = t.TransactionGateway,
					Testing = t.Testing,
					Description = t.Description,
					OrgId = t.OrgId,
					OriginalId = t.Id,
					Participants = t.Participants,
					Financeonly = t.Financeonly,
				};
				DbUtil.Db.Transactions.InsertOnSubmit(tt);
				DbUtil.Db.SubmitChanges(); 
				Util.SendMsg(Util.SysFromEmail, Util.Host, 
					Util.TryGetMailAddress(DbUtil.Db.StaffEmailForOrg(tt.OrgId ?? 0)),
					"Void/Credit Transaction Type: " + type,
@"<table>
<tr><td>Name</td><td>{0}</td></tr>
<tr><td>Email</td><td>{1}</td></tr>
<tr><td>Address</td><td>{2}</td></tr>
<tr><td>Phone</td><td>{3}</td></tr>
<tr><th colspan=""2"">Transaction Info</th></tr>
<tr><td>Description</td><td>{4}</td></tr>
<tr><td>Amount</td><td>{5:N2}</td></tr>
<tr><td>Date</td><td>{6}</td></tr>
<tr><td>TranIds</td><td>Org: {7} {8}, Curr: {9} {10}</td></tr>
</table>".Fmt(t.Name, t.Emails, t.Address, t.Phone,
		 t.Description, 
		 t.Amt,
		 t.TransactionDate.Value.FormatDateTm(),
		 t.Id, t.TransactionId, tt.Id, tt.TransactionId
		 ), Util.EmailAddressListFromString(DbUtil.Db.StaffEmailForOrg(tt.OrgId ?? 0)),
					0, 0);
			}
			return View("List", m);
		}
    }
}
