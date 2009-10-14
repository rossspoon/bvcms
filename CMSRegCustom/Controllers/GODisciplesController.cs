using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using CMSRegCustom.Models;
using UtilityExtensions;
using System.Net.Mail;

namespace CMSRegCustom.Controllers
{
    [HandleError]
    public class GODisciplesController : Controller
    {
        public GODisciplesController()
        {
            ViewData["logoimg"] = DbUtil.Settings("GoDisciplesLogo", "/Content/Crosses.png");
        }
        public ActionResult Leader()
        {
            ViewData["title"] = DbUtil.Settings("GODisciplesTitle", "GO Disciples") + " Leader Registration";
            var m = new Models.GODisciplesModel("Leader");
            if (Request.HttpMethod.ToUpper() == "GET")
                return View("Signup", m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View("Signup", m);

            m.PerformLeaderSetup();
            m.EmailLeaderNotices();

            TempData["orgid"] = m.neworgid;
            return RedirectToAction("Confirm");
        }
        public ActionResult Disciple(int id)
        {
            ViewData["title"] = DbUtil.Settings("GODisciplesTitle", "GO Disciples") + " Registration";
            var m = new Models.GODisciplesModel("Disciple", id);
            if (Request.HttpMethod.ToUpper() == "GET")
                return View("Signup", m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View("Signup", m);

            m.PerformMemberSetup();
            m.EmailMemberNotices();

            TempData["orgid"] = m.neworgid;
            return RedirectToAction("Confirm");
        }
        public ActionResult Confirm()
        {
            var id = TempData["orgid"].ToInt();
            var m = new GODisciplesModel("Confirm", id);
            ViewData["title"] = DbUtil.Settings("GODisciplesTitle", "GO Disciples") + " Registration Successful";
            return View(m);
        }
    }
}
