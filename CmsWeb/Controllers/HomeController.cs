using System;
using System.Collections.Generic;
using System.Linq;
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
            var m = new HomeModel();
            return View(m);
        }
        public ActionResult About()
        {
            ViewData["build"] = BuildDate();
            return View();
        }
        public DateTime BuildDate()
        {
            return System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
        public ActionResult BatchTag(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            var q2 = from s in text.Split('\n')
                     where s.HasValue()
                     select s.ToInt();

            var Db = DbUtil.Db;
            var tag = Db.TagCurrent();
            
            foreach (var PeopleId in q2)
            {
                var tagp = Db.TagPeople.SingleOrDefault(tp => tp.PeopleId == PeopleId && tp.Id == tag.Id);
                if (tagp == null)
                    tag.PersonTags.Add(new TagPerson { PeopleId = PeopleId });
                Db.SubmitChanges();
            }
            return Redirect("/MyTags.aspx");
        }
        public ActionResult DoDrops(string text)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            var q2 = from s in text.Split('\n')
                     where s.HasValue()
                     let a = s.Split('\t')
                     select new { pid = a[0].ToInt(), oid = a[1].ToInt() };
            int n = 0;
            var list = q2.ToList();
            foreach (var i in list)
            {
                var om = DbUtil.Db.OrganizationMembers.SingleOrDefault(m => m.OrganizationId == i.oid && m.PeopleId == i.pid);
                if (om == null)
                    continue;
                om.Person.Comments = "InactiveDrop: {0}({1})\n{2}".Fmt(om.Organization.OrganizationName, om.Organization.OrganizationId, om.Person.Comments);
                om.Drop(DbUtil.Db);

                DbUtil.Db.SubmitChanges();
                n++;
                if (n % 50 == 0)
                    DbUtil.DbDispose();
            }
            return Content("done");
        }
    }
}

