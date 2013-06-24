using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class CheckinLabelsController : Controller
    {
        public ActionResult Index(int id = 0)
        {
            ViewBag.ID = id;
            return View();
        }

        public ActionResult Save(int id = 0, string labelFormat = "")
        {
            if( id == 0 || labelFormat.Length == 0 )
                return new RedirectResult("/Manage/CheckinLabels");

            var label = (from e in DbUtil.Db.LabelFormats
                         where e.Id == id
                         select e).FirstOrDefault();

            if (label != null)
            {
                label.Format = labelFormat.Replace("\n", "").Replace("\r", "").Replace(" ", "");
                DbUtil.Db.SubmitChanges();
            }

            return new RedirectResult("/Manage/CheckinLabels/Index/" + id);
        }
    }
}
