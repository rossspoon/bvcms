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
    [Authorize(Roles="Edit")]
    public class TransactionsController : CmsStaffController
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            var m = new TransactionsModel();
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult List(TransactionsModel m)
        {
            UpdateModel(m.Pager);
            return View(m);
        }
    }
}
