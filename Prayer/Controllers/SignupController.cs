using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using DiscData;
using Prayer.Models;

namespace Prayer.Controllers
{
    public class SignupController : Controller
    {
        private const string STR_PrayerPartners = "Prayer Partners";

        public ActionResult Index(int? id)
        {
            User u = Util.CurrentUser;
            var g = Group.LoadByName(STR_PrayerPartners);
            if (g.IsAdmin && id.HasValue)
                u = Util.GetUser(id);
            return View(new SignupModel(u));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ToggleSlot(int? id, string slot, bool ck)
        {
            User u = Util.CurrentUser;
            var g = Group.LoadByName(STR_PrayerPartners);
            if (g.IsAdmin && id.HasValue)
                u = Util.GetUser(id);
            var m = new SignupModel(u);
            var ret = m.ToggleSlot(slot, ck);
            return Json(ret);
        }
        public ActionResult Report()
        {
            return View();
        }
        public ActionResult SearchPeople()
        {
            var g = Group.LoadByName(STR_PrayerPartners);
            if (!g.IsAdmin)
                return RedirectToAction("Index");
            var m = new SearchPeopleModel();
            UpdateModel<ISearchPeopleFormBindable>(m);
            return View(m);
        }
        public ActionResult Notify()
        {
            PsUtil.SendNotifications();
            return new EmptyResult();
        }
    }
}
