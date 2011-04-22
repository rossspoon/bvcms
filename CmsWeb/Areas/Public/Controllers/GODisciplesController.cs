using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using UtilityExtensions;
using System.Net.Mail;
using CmsWeb.Models;

namespace CmsWeb.Areas.Public.Controllers
{
    [ValidateInput(false)]
    public class GODisciplesController : CmsController
    {
        public GODisciplesController()
        {
            ViewData["head"] = HeaderHtml("GODisciplesHeader",
                Header,
                DbUtil.Db.Setting("GoDisciplesLogo", "/Content/Crosses.png"));
        }
        public ActionResult Index()
        {
            if (bool.Parse(DbUtil.Db.Setting("GODisciplesDisabled", "false")))
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
            if (bool.Parse(DbUtil.Db.Setting("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            var m = new GODisciplesModel("Leader");
            if (id.HasValue)
                m.campus = id;
            else
                m.campus = DbUtil.Db.Setting("DefaultCampusId", "").ToInt2();

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
        public ActionResult Disciple(int? id)
        {
            if (!id.HasValue)
                return Content("group not found");
            if (bool.Parse(DbUtil.Db.Setting("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            var m = new GODisciplesModel("Disciple", id.Value);
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
            if (bool.Parse(DbUtil.Db.Setting("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            var m = new GODisciplesModel("Individual");
            if (Request.HttpMethod.ToUpper() == "GET")
            {
#if DEBUG
                m.first = "David";
                m.last = "Carroll";
                m.dob = "5/30/52";
                m.phone = "901.758.1862";
                m.email = "david@bvcms.com";
#endif
                return View("Signup", m);
            }

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
            if (bool.Parse(DbUtil.Db.Setting("GODisciplesDisabled", "false")))
                return Content(DbUtil.Content("GoDisciplesDisabled").Body);
            ViewData["header"] = Header;
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!(oldname.HasValue() && newname.HasValue() && newname != oldname))
                return View();
            GODisciplesModel.RenameGroups(oldname, newname);
            return Content(Util.EndShowMessage("rename successful", "/Home", "home"));
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
            get { return DbUtil.Db.Setting("GODisciplesTitle", "GO Disciples") + " Registration"; }
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
