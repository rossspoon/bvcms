using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Diagnostics;
using CmsData.Registration;
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
            return View();
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
            string test = null;
            var x = test.Replace('3','4');
		    return Content("done");
		}
        public ActionResult RecordTest(int id, string v)
        {
            var o = DbUtil.Db.LoadOrganizationById(id);
            o.AddEditExtra(DbUtil.Db, "tested", v);
            DbUtil.Db.SubmitChanges();
            return Content(v);
        }
		public ActionResult NthTimeAttenders(int id)
		{
		    var name = "VisitNumber-" + id;
		    var qb = DbUtil.Db.QueryBuilderClauses.FirstOrDefault(c => c.IsPublic && c.Description == name && c.SavedBy == "public");
		    if (qb == null)
		    {
			    qb = DbUtil.Db.QueryBuilderScratchPad();
                qb.CleanSlate(DbUtil.Db);

		        var comp = CompareType.Equal;
		        QueryBuilderClause clause = null;
		        switch (id)
		        {
		            case 1:
		                clause = qb.AddNewClause(QueryType.RecentVisitNumber, comp, "1,T");
		                clause.Quarters = "1";
		                clause.Days = 7;
		                break;
		            case 2:
		                clause = qb.AddNewClause(QueryType.RecentVisitNumber, comp, "1,T");
		                clause.Quarters = "2";
		                clause.Days = 7;
		                clause = qb.AddNewClause(QueryType.RecentVisitNumber, comp, "0,F");
		                clause.Quarters = "1";
		                clause.Days = 7;
		                break;
		            case 3:
		                clause = qb.AddNewClause(QueryType.RecentVisitNumber, comp, "1,T");
		                clause.Quarters = "3";
		                clause.Days = 7;
		                clause = qb.AddNewClause(QueryType.RecentVisitNumber, comp, "0,F");
		                clause.Quarters = "2";
		                clause.Days = 7;
		                break;
		        }
		        qb = qb.SaveTo(DbUtil.Db, name, "public", true);
		    }
		    TempData["autorun"] = true;
			return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
		}
        [Authorize(Roles = "Admin")]
		public ActionResult ActiveRecords()
        {
		    var name = "ActiveRecords";
		    var qb = DbUtil.Db.QueryBuilderClauses.FirstOrDefault(c => c.IsPublic && c.Description == name && c.SavedBy == "public");
		    if (qb == null)
		    {
			    qb = DbUtil.Db.QueryBuilderScratchPad();
                qb.CleanSlate(DbUtil.Db);
		        var anygroup = qb.AddNewGroupClause(CompareType.AnyTrue);

                var clause = anygroup.AddNewClause(QueryType.RecentAttendCount, CompareType.GreaterEqual, "1");
                clause.Days = 365;
		        clause = anygroup.AddNewClause(QueryType.RecentHasIndContributions, CompareType.Equal, "1,T");
		        clause.Days = 365;
		        qb.AddNewClause(QueryType.IncludeDeceased, CompareType.Equal, "1,T");
		        qb.SaveTo(DbUtil.Db, name, "public", true);
		    }
            qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate(DbUtil.Db);
            qb.AddNewClause(QueryType.ActiveRecords, CompareType.Equal, "1,T");
            var count = DbUtil.Db.PeopleQuery(qb.QueryId).Count();
            TempData["ActiveRecords"] = count;
            return View("About");
		}
        public ActionResult UseOldLook()
        {
            DbUtil.Db.SetUserPreference("newlook3", "false");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }
        public ActionResult UseNewLook()
        {
            DbUtil.Db.SetUserPreference("newlook3", "true");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }
        public ActionResult UseAdvancedSearch()
        {
            DbUtil.Db.SetUserPreference("advancedsearch", "true");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }
        public ActionResult UseSearchBuilder()
        {
            DbUtil.Db.SetUserPreference("advancedsearch", "false");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }
        public ActionResult UseNewPeoplePage()
        {
            DbUtil.Db.SetUserPreference("newpeoplepage", "true");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }
        public ActionResult UseOldPersonPage()
        {
            DbUtil.Db.SetUserPreference("newpeoplepage", "false");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }
        public ActionResult TargetPerson(bool id)
        {
            DbUtil.Db.SetUserPreference("TargetLinkPeople", id ? "false" : "true");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }

        public ActionResult NewQuickSearch(string id)
        {
            var b = id.ToBool();
            DbUtil.Db.SetUserPreference("NewQuickSearch", b ? "false" : "true");
            DbUtil.Db.SubmitChanges();
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.OriginalString);
            return Redirect("/");
        }
        public ActionResult Names(string term)
        {
            var q = HomeModel.Names(term).ToList();
            return Json(q, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TestRegs()
        {
            foreach (var o in DbUtil.Db.Organizations)
            {
                try
                {
                    var rs = new Settings(o.RegSetting, DbUtil.Db, o.OrganizationId);
                }
                catch (Exception ex)
                {
                    return Content("bad org <a href=\"{0}{1}\">{2}</a>\n{3}".Fmt(Util.ServerLink("/RegSetting/Index/"), o.OrganizationId, o.OrganizationName, ex.Message));
                }
            }
            return Content("ok");
        }

    }
}

