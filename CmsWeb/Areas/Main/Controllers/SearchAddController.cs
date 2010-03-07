using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CMSWeb.Models;
using CmsData;
using UtilityExtensions;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Net.Mail;
using System.Web.Routing;
using CMSPresenter;

namespace CMSWeb.Areas.Main.Controllers
{
    public class SearchAddController : Controller
    {
        public ActionResult Index(int? id, string type, string from)
        {
            var m = new SearchModel { typeid = id, type = type, from = from };
#if DEBUG2
            m.name = "Da Car";
            m.address = "";
#endif
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Results(SearchModel m)
        {
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResultsFamily(SearchModel m)
        {
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchPerson(SearchModel m)
        {
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchFamily(SearchModel m)
        {
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchCancel(SearchModel m)
        {
            if (m.List.Count > 0)
                return View("List", m);
            return Complete(0, m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SearchFamilyCancel(SearchModel m)
        {
            return View("SearchPerson", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonCancel(int id, SearchModel m)
        {
            m.List.RemoveAt(id);
            if (m.List.Count > 0)
                return View("List", m);
            return View("SearchPerson", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Select(int id, SearchModel m)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            var s = new SearchPersonModel
            {
                PeopleId = id,
                FamilyId = m.type == "family" ? m.typeid.Value : p.FamilyId,
                first = p.FirstName,
                goesby = p.NickName,
                last = p.LastName,
                marital = p.MaritalStatusId,
                email = p.EmailAddress,
                suffix = p.SuffixCode,
                title = p.TitleCode,
                dob = p.DOB,
                gender = p.GenderId,
                phone = p.CellPhone,
            };
            s.LoadFamily();
            m.List.Add(s);
            return View("List", m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddNewFamily(SearchModel m)
        {
            var p = m.List[m.List.Count - 1];
            p.ValidateModelForNew(ModelState);
            if (!ModelState.IsValid)
                return View("FormFull", m);
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddToFamily(SearchModel m)
        {
            var p = m.List[m.List.Count - 1];
            p.ValidateModelForNew(ModelState);
            if (!ModelState.IsValid)
                return View("FormAbbreviated", m);
            return View("list", m);
        }
        private SearchPersonModel NewPerson(int FamilyId, SearchModel m)
        {
            var p = new SearchPersonModel
            {
                FamilyId = FamilyId,
                index = m.List.Count,
                gender = 99,
                marital = 99,
                state = "na"
            };
#if DEBUG2
            p.title = "Mr.";
            p.first = "David5";
            p.last = "Carroll";
            p.gender = 1;
            p.marital = 20;
            p.dob = "5/30/52";
            p.email = "david@davidcarroll.name";
            p.phone = "9014890611";
#endif
            m.List.Add(p);
            return p;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FormAbbreviated(int id, SearchModel m)
        {
            var p = NewPerson(id, m);
            if (id < 0)
            {
                var f = m.List.FirstOrDefault(fm => fm.FamilyId == id);
                p.address = f.address;
                p.city = f.city;
                p.state = f.state;
                p.zip = f.zip;
                p.homephone = f.homephone;
            }
            else
                p.LoadFamily();
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FormFull(SearchModel m)
        {
            int id = 0;
            if (m.List.Count > 0)
                id = m.List.Min(i => i.FamilyId) - 1;
            if (id >= 0)
                id = -1;
            var p = NewPerson(id, m);
#if DEBUG2
            p.address = "235 Riveredge Cv";
            p.city = "Cordova";
            p.state = "TN";
            p.zip = "38018";
            p.homephone = "9017581862";
#endif
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Complete(int? id, SearchModel m)
        {
            switch (m.type)
            {
                case "family":
                    return AddFamilyMembers(id.Value, m, false);
                case "relatedfamily":
                    return AddRelatedFamilys(id.Value, m, false);
                case "org":
                    return AddOrgMembers(id.Value, m, false);
                case "pending":
                    return AddOrgMembers(id.Value, m, true);
                case "visitor":
                    return AddVisitors(id.Value, m);
                case "contactee":
                    return AddContactees(id.Value, m);
                case "contactor":
                    return AddContactors(id.Value, m);
            }
            return new EmptyResult();
        }

        private JsonResult AddContactees(int id, SearchModel m)
        {
            if (id > 0)
            {
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, 0);
                    var ce = new Contactee
                    {
                        ContactId = id,
                        PeopleId = p.person.PeopleId,
                    };
                    DbUtil.Db.Contactees.InsertOnSubmit(ce);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected" });
        }
        private JsonResult AddContactors(int id, SearchModel m)
        {
            if (id > 0)
            {
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, 0);
                    var ce = new Contactor
                    {
                        ContactId = id,
                        PeopleId = p.person.PeopleId,
                    };
                    DbUtil.Db.Contactors.InsertOnSubmit(ce);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected" });
        }
        private JsonResult AddFamilyMembers(int id, SearchModel m, bool pending)
        {
            if (id > 0)
            {
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, 0);
                    if (p.person.Age < 18)
                        p.person.PositionInFamilyId = (int)Family.PositionInFamily.Child;
                    else if (p.family.People.Count(per =>
                        per.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult) < 2)
                        p.person.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
                    else
                        p.person.PositionInFamilyId = (int)Family.PositionInFamily.SecondaryAdult;
                    p.family.People.Add(p.person);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected" });
        }
        private JsonResult AddRelatedFamilys(int id, SearchModel m, bool pending)
        {
            if (id > 0)
            {
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, 0);
                    FamilyController.AddRelatedFamily(id, p.PeopleId.Value);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected" });
        }
        private JsonResult AddOrgMembers(int id, SearchModel m, bool pending)
        {
            if (id > 0)
            {
                var org = DbUtil.Db.LoadOrganizationById(id);
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, org.EntryPointId ?? 0);
                    OrganizationMember.InsertOrgMembers(id, p.PeopleId.Value, 220, Util.Now, null, pending);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "rebindgrids" });
        }
        private JsonResult AddVisitors(int id, SearchModel m)
        {
            var sb = new StringBuilder();
            if (id > 0)
            {
                var meeting = DbUtil.Db.Meetings.SingleOrDefault(me => me.MeetingId == id);
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, meeting.Organization.EntryPointId ?? 0);
                    var err = Attend.RecordAttendance(p.PeopleId.Value, id, true);
                    if (err.HasValue())
                        sb.AppendLine(err);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected", error = sb.ToString() });
        }
        private void AddPerson(SearchPersonModel p, IList<SearchPersonModel> list, int EntryPoint)
        {
            if (p.IsNew)
                p.AddPerson(EntryPoint);
            if (p.FamilyId < 0)
            {
                var q = from m in list
                        where m.FamilyId == p.FamilyId
                        select m;
                var list2 = q.ToList();
                foreach (var m in list2)
                    m.FamilyId = p.person.FamilyId;
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult VerifyAddress(SearchModel m)
        {
            var p = m.List[m.List.Count - 1];
            var r = CMSPresenter.PersonController.LookupAddress(p.address, p.address2, p.city, p.state, p.zip);
            return Json(r);
        }
    }
}
