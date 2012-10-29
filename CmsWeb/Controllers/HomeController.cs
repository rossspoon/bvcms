using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Diagnostics;
using UtilityExtensions;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.SqlClient;
using System.Net.Mail;
using CmsWeb.Models;

namespace CmsWeb.Controllers
{
    public class HomeController : CmsStaffController
    {
        public ActionResult Index()
        {
            if (!Util2.OrgMembersOnly && User.IsInRole("OrgMembersOnly"))
            {
                Util2.OrgMembersOnly = true;
                DbUtil.Db.SetOrgMembersOnly();
            }
            else if (!Util2.OrgLeadersOnly && User.IsInRole("OrgLeadersOnly"))
            {
                Util2.OrgLeadersOnly = true;
                DbUtil.Db.SetOrgLeadersOnly();
            }
            var m = new HomeModel();
            return View(m);
        }
        public ActionResult About()
        {
            ViewData["build"] = BuildDate();
            return View();
        }
        public string BuildDate()
        {
			return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        [ValidateInput(false)]
        public ActionResult ShowError(string error, string url)
        {
            ViewData["error"] = Server.UrlDecode(error);
            ViewData["url"] = url;
            return View();
        }
        public ActionResult NewQuery()
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            return Redirect("/QueryBuilder/Main");
        }
		public ActionResult Test()
		{
			var api = new CmsData.API.APIOrganization(DbUtil.Db);
			var xml = api.ParentOrgs(214, null, null);
			return Content(xml, "text/xml");
		}
		public ActionResult Test2()
		{
			var api = new CmsData.API.APIOrganization(DbUtil.Db);
			var xml = api.ChildOrgs(81470, null, null);
			return Content(xml, "text/xml");
		}
		public ActionResult Test3()
		{
			var wc = new WebClient();
			var b = wc.DownloadString("http://localhost:888/Public/APIContribution/StatementYearToDate?PeopleId=828612");
			return Content(b, "application/pdf");
		}
		public ActionResult Test4()
		{
			var api = new CmsData.API.APIContribution(DbUtil.Db);
			var xml = api.Contributions(24562, DateTime.Now.Year);
			return Content(xml, "text/xml");
		}
    }
}

