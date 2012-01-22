using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
