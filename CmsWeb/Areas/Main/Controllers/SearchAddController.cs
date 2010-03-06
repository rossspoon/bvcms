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

namespace CMSWeb.Areas.Main.Controllers
{
    public class SearchAddController : Controller
    {
        public ActionResult Index(int? id, string type, string from)
        {
            var m = new SearchModel { typeid = id, type = type, from = from };
#if DEBUG
            m.name = "Da Car";
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
            m.dob = "";
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
            return Content("close");
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
                FamilyId = p.FamilyId,
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
#if DEBUG
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
#if DEBUG
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
                case "org":
                    AddOrgMembers(id.Value, m, false);
                    break;
                case "pending":
                    AddOrgMembers(id.Value, m, true);
                    break;
                case "meeting":
                    AddVisitors(id.Value, m);
                    break;
                case "contactee":
                    AddContactees(id.Value, m);
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return Content("close");
        }

        private void AddContactees(int id, SearchModel m)
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
        }
        private void AddOrgMembers(int id, SearchModel m, bool pending)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return;
            foreach (var p in m.List)
            {
                AddPerson(p, m.List, org.EntryPointId ?? 0);
                OrganizationMember.InsertOrgMembers(id, p.PeopleId.Value, 220, Util.Now, null, pending);
            }
        }
        private void AddVisitors(int id, SearchModel m)
        {
            var meeting = DbUtil.Db.Meetings.SingleOrDefault(me => me.MeetingId == id);
            if (meeting == null)
                return;
            foreach (var p in m.List)
            {
                AddPerson(p, m.List, meeting.Organization.EntryPointId ?? 0);
                Attend.RecordAttendance(p.PeopleId.Value, id, true);
            }
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
    }
}
