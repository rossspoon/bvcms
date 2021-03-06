using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CmsData;
using CmsData.Registration;
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

            ViewData["lines"] = CmsData.Registration.Parser.SplitLines(regsetting);
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
                var os = new Settings(text, DbUtil.Db, id);
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
        public ActionResult ConvertFromMdy(int id)
        {
            var cul = "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cul);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cul);
            var org = DbUtil.Db.LoadOrganizationById(id);
            var m = new Settings(org.RegSetting, DbUtil.Db, id);
            var os = new Settings(m.ToString(), DbUtil.Db, id);
            m.org.RegSetting = os.ToString();
            DbUtil.Db.SubmitChanges();
            return Redirect("/RegSetting/Index/" + id);
        }
        public ActionResult ConvertFromDmy(int id)
        {
            var cul = "en-GB";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cul);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cul);
            var org = DbUtil.Db.LoadOrganizationById(id);
            var m = new Settings(org.RegSetting, DbUtil.Db, id);
            var os = new Settings(m.ToString(), DbUtil.Db, id);
            m.org.RegSetting = os.ToString();
            DbUtil.Db.SubmitChanges();
            return Redirect("/RegSetting/Index/" + id);
        }
    }
}