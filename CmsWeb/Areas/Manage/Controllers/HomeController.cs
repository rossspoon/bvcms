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

namespace CmsWeb.Areas.Manage.Controllers
{
    public class HomeController : CmsStaffController
    {
        public ActionResult Index()
        {
            return Redirect("/default.aspx");
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult ShowError(string id, string url)
        {
            ViewData["error"] = Server.UrlDecode(id);
            ViewData["url"] = url;
            return View();
        }
        public ActionResult NewQuery()
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate();
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
        //     public ActionResult DoBirthDays()
   //     {
			//var offset = new int[] { 20, 25, 30, 35, 40, 45, 50, 55, 60, 65,
			//						-20, -25, -30, -35, -40, -45, -50, -55, -60, -65 };
   //         var q = from p in DbUtil.Db.People
   //                 where DbUtil.Db.Birthday(p.PeopleId) != null
   //                 select p;
			//var r = new Random();
			//int n = 0;
			//foreach (var p in q)
			//{
			//	var bd = new DateTime(p.BirthYear.Value, p.BirthMonth.Value, p.BirthDay.Value);
			//	bd = bd.AddDays(offset[r.Next(20)]);
			//	p.BirthDay = bd.Day;
			//	p.BirthMonth = bd.Month;
			//	p.BirthYear = bd.Year;
			//	DbUtil.Db.SubmitChanges();
			//	n++;
   //             if (n % 100 == 0)
   //                Debug.WriteLine(n);
			//}
   //         var res = new ContentResult();
   //         res.Content = "<html><body>done</body></html>";
   //         return res;
   //     }
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
                om.Drop();

                DbUtil.Db.SubmitChanges();
                n++;
                if (n % 50 == 0)
                    DbUtil.DbDispose();
            }
            return Content("done");
        }
    }
}

