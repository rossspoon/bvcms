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
		public ActionResult NthTimeVisitors(int id)
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
            DbUtil.Db.SetUserPreference("newlook", "false");
            DbUtil.Db.SubmitChanges();
            return Redirect(Request.UrlReferrer.OriginalString);
        }
        public ActionResult UseNewLook()
        {
            DbUtil.Db.SetUserPreference("newlook", "true");
            DbUtil.Db.SubmitChanges();
            return Redirect(Request.UrlReferrer.OriginalString);
        }
        public ActionResult UseAdvancedSearch()
        {
            DbUtil.Db.SetUserPreference("advancedsearch", "true");
            DbUtil.Db.SubmitChanges();
            return Redirect(Request.UrlReferrer.OriginalString);
        }
        public ActionResult UseSearchBuilder()
        {
            DbUtil.Db.SetUserPreference("advancedsearch", "false");
            DbUtil.Db.SubmitChanges();
            return Redirect(Request.UrlReferrer.OriginalString);
        }
    }
}

