using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    public class CheckinController : Controller
    {
        //
        // GET: /Checkin/

        IEnumerable<Fam> t;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Match(string id)
        {
            var ph = Util.GetDigits(id);
            var q = from p in DbUtil.Db.People
                    where p.Family.HomePhone.EndsWith(ph)
                    || p.CellPhone.EndsWith(id)
                    where p.Family.HeadOfHouseholdId == p.PeopleId
                    where p.Family.People.Any(c => c.Grade < 3)
                    select new Fam
                    {
                        FamId = p.FamilyId,
                        Last = p.LastName,
                        Addr = p.PrimaryAddress,
                        City = p.PrimaryCity,
                        St = p.PrimaryState,
                        Zip = p.PrimaryZip,
                        NumGrade2 = p.Family.People.Count(c => c.Grade < 3)
                    };
            return View(q);
        }
        public ActionResult Children(int id)
        {
            var q = from p in DbUtil.Db.People
                    where p.FamilyId == id
                    where p.PositionInFamilyId == 30
                    where p.Grade < 3
                    let att = p.Attends.Where(a => a.AttendanceFlag && a.Organization.DivOrgs.Any(dor => dor.Division.ProgId == DbUtil.BFClassOrgTagId)).OrderByDescending(a => a.MeetingDate).FirstOrDefault()
                    select new
                    {
                        p.PeopleId,
                        p.Name,
                        p.BirthYear, 
                        p.BirthMonth, 
                        p.BirthDay,
                        att,
                    };
            var list = q.ToList();
            var q2 = from p in list
                     select new Child
                     {
                         Id = p.PeopleId,
                         Name = p.Name,
                         Birthday = Util.FormatBirthday(p.BirthYear, p.BirthMonth, p.BirthDay),
                         Class = p.att == null ? "(first visit)" : p.att.Organization.FullName,
                     };
            return View(q2);
        }

    }
    public class Child
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Birthday { get; set; }
        public string Class { get; set; }
    }
    public class Fam
    {
        public int FamId { get; set; }
        public string Last {get; set;}
        public string Addr {get; set;}
        public string City {get; set;}
        public string St {get; set;}
        public string Zip {get; set;}
        public int NumGrade2 { get; set; }
        public override string ToString()
        {
            return "{0} Family, {1}, {2}, {3} {4} ({5})".Fmt(Last, Addr, City, St, Zip, NumGrade2);
        }
    }
}
