using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using System.Text;
using CmsWeb.Models.OrganizationPage;
using System.Diagnostics;
using CmsData.Codes;

namespace CmsWeb.Areas.Main.Controllers
{
    [ValidateInput(false)]
    public class RegSettingController : CmsStaffController
    {
        [HttpGet]
        public ActionResult Index(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            var regsetting = (string)TempData["regsetting"];
            if (!regsetting.HasValue())
                regsetting = org.RegSetting;

            ViewData["lines"] = RegSettings.SplitLines(regsetting);
            ViewData["regsetting"] = regsetting;
            ViewData["OrganizationId"] = id;
            ViewData["orgname"] = org.OrganizationName;
            return View();
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult Edit(int id, string regsetting)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            ViewData["OrganizationId"] = id;
            ViewData["orgname"] = org.OrganizationName;
            if (regsetting.HasValue())
                ViewData["text"] = regsetting;
            else
                ViewData["text"] = org.RegSetting;
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult Update(int id, string text)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            try
            {
                var os = new RegSettings(text, DbUtil.Db, id);
                org.RegSetting = text;
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                TempData["regsetting"] = text;
                return Redirect("/RegSetting/Index/" + id);
            }
            DbUtil.Db.SubmitChanges();
            return Redirect("/RegSetting/Index/" + id);
        }
        public ActionResult EditGui(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            ViewData["OrganizationId"] = id;
            ViewData["orgname"] = org.OrganizationName;
            RegSettings m = new RegSettings(org.RegSetting, DbUtil.Db, id);
            return View("EditGui", m);
        }
        [HttpPost]
        [Authorize(Roles="Edit")]
        public ActionResult UpdateGui(int id, RegSettings m)
        {
            string text = "";
            try
            {
                var org = DbUtil.Db.LoadOrganizationById(id);
                m.OrgId = id;
                m.Db = DbUtil.Db;
                text = m.ToString();
                var os = new RegSettings(text, DbUtil.Db, id);
                org.RegSetting = text;
                DbUtil.Db.SubmitChanges();
                return Content("ok");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult NewMenuItem()
        {
            return View("MenuItemEditor", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewDropdown1Item()
        {
            return View("Dropdown1Editor", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewDropdown2Item()
        {
            return View("Dropdown2Editor", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewDropdown3Item()
        {
            return View("Dropdown3Editor", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewCheckbox()
        {
            return View("CheckboxEditor", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewCheckbox2()
        {
            return View("Checkbox2Editor", new RegSettings.MenuItem());
        }
        [HttpPost]
        public ActionResult NewAgeGroup()
        {
            return View("AgeGroupEditor", new RegSettings.AgeGroup());
        }
        [HttpPost]
        public ActionResult NewGradeOption()
        {
            return View("GradeOptionEditor", new RegSettings.GradeOption());
        }
        [HttpPost]
        public ActionResult NewYesNoQuestion()
        {
            return View("YesNoQuestionEditor", new RegSettings.YesNoQuestion());
        }
        [HttpPost]
        public ActionResult NewShirtSize()
        {
            return View("ShirtSizeEditor", new RegSettings.ShirtSize());
        }
        [HttpPost]
        public ActionResult NewExtraQuestion()
        {
            return View("ExtraQuestionEditor", new RegSettings.ExtraQuestion());
        }
        [HttpPost]
        public ActionResult NewVoteTag()
        {
            return View("VoteTagEditor", new RegSettings.VoteTag());
        }
        public ActionResult VoteTag(int id)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            RegSettings m = new RegSettings(org.RegSetting, DbUtil.Db, id);
            Response.ContentType = "text/plain";
            return Content(@"Copy and paste these directly into your email text, 
no need to put these into the ""Source"" view of the editor anymore.

" + m.VoteTagsLinks());
        }
    }
}