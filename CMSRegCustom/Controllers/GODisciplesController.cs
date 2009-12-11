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
    public class GODisciplesController : CMSWebCommon.Controllers.CmsController
    {
        public GODisciplesController()
        {
            ViewData["head"] = HeaderHtml("GODisciplesHeader",
                Header,
                DbUtil.Settings("GoDisciplesLogo", "/Content/Crosses.png"));
        }
        public ActionResult Index()
        {
            if (bool.Parse(DbUtil.Settings("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            var c = DbUtil.Content("GODisciplesIndex");
            if (c == null)
                return Content("Sorry, this is not a valid registration URL");
            ViewData["content"] = c.Body;
            return View();
        }
        public ActionResult Leader(int? id)
        {
            if (bool.Parse(DbUtil.Settings("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            var m = new Models.GODisciplesModel("Leader");
            if (id.HasValue)
                m.campus = id;
            else
                m.campus = DbUtil.Settings("DefaultCampusId", "").ToInt2();

            if (Request.HttpMethod.ToUpper() == "GET")
                return View("Signup", m);

            UpdateModel(m);

            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View("Signup", m);

            m.PerformLeaderSetup();
            m.EmailLeaderNotices();

            return RedirectToAction("Confirm", new { id = m.neworgid });
        }
        public ActionResult Disciple(int id)
        {
            if (bool.Parse(DbUtil.Settings("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            var m = new Models.GODisciplesModel("Disciple", id);
            if (Request.HttpMethod.ToUpper() == "GET")
                return View("Signup", m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View("Signup", m);

            m.PerformMemberSetup();
            m.EmailMemberNotices();

            return RedirectToAction("Confirm", new { id = m.neworgid });
        }
        public ActionResult Individual(string id)
        {
            if (bool.Parse(DbUtil.Settings("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            var m = new Models.GODisciplesModel("Individual");
            if (Request.HttpMethod.ToUpper() == "GET")
                return View("Signup", m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (!ModelState.IsValid)
                return View("Signup", m);

            m.PerformIndividualSetup(id);
            m.EmailIndividualNotices(id);

            return RedirectToAction("Confirm2");
        }
        [Authorize(Roles = "Edit")]
        public ActionResult RenameGroup(string oldname, string newname)
        {
            if (bool.Parse(DbUtil.Settings("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!(oldname.HasValue() && newname.HasValue() && newname != oldname))
                return View();
            Models.GODisciplesModel.RenameGroups(oldname, newname);
            return Content(Util.EndShowMessage("rename successful", "/", "home"));
        }
        public ActionResult Confirm(int id)
        {
            var m = new GODisciplesModel("Confirm", id);
            ViewData["header"] = Header + " Successful";
            return View(m);
        }
        public ActionResult Confirm2()
        {
            var m = new GODisciplesModel("Confirm");
            ViewData["header"] = Header + " Successful";
            return View("Confirm", m);
        }

        private string Header
        {
            get { return DbUtil.Settings("GODisciplesTitle", "GO Disciples") + " Registration"; }
        }
        //public ActionResult FixPW()
        //{
        //    var q = from u in DbUtil.Db.Users
        //            where u.TempPassword != null && u.TempPassword != ""
        //            select u;
        //    foreach(var u in q)
        //        MembershipService.ChangePassword(u.Username, u.TempPassword);
        //    return Content("done");
        //}
    }
}
