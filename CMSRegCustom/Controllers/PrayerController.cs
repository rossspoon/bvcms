using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSRegCustom.Models;
using UtilityExtensions;

namespace CMSRegCustom.Controllers
{
    public class PrayerController : CMSWebCommon.Controllers.CmsController
    {
        private const string STR_PrayerPartners = "Prayer Partners";

        public PrayerController()
        {
            ViewData["head"] = HeaderHtml("Prayer",
                "Prayer Registration", "/Content/Crosses.png");
        }
        public ActionResult Index()
        {
            var m = new Models.PrayerModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count > 1)
                    ModelState.AddModelError("find", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("find", "Cannot find your church record");
            }
            if (!ModelState.IsValid)
                return View(m);

            TempData["PeopleId"] = m.person.PeopleId;
            return RedirectToAction("PickTimes");
        }
        public ActionResult PickTimes()
        {
            var id = (int?)TempData["PeopleId"];
            if (!id.HasValue)
                return Content("no person");
            var m = new PrayerModel(id.Value);
            return View(m); 
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ToggleSlot(int id, string slot, bool ck)
        {
            var g = Group.LoadByName(STR_PrayerPartners);
            var m = new PrayerModel(id);
            var ret = m.ToggleSlot(slot, ck);
            return Json(ret);
        }
        public ActionResult Report()
        {
            return View();
        }
        public ActionResult Notify()
        {
            PsUtil.SendNotifications();
            return new EmptyResult();
        }
    }
}
