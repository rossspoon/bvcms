using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CmsData;
using CMSPresenter;
using CmsWeb.Models;
using UtilityExtensions;
using System.Text.RegularExpressions;

namespace CmsWeb.Areas.Main.Controllers
{
    public class SearchAddController : CmsStaffController
    {
        public ActionResult Index(int? id, string type, string from)
        {
            var m = new SearchModel { typeid = id, type = type, from = from };
#if DEBUG
#endif
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Results(SearchModel m)
        {
            DbUtil.Db.SetNoLock();
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResultsFamily(SearchModel m)
        {
            DbUtil.Db.SetNoLock();
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
            m.dob = null;
            var a = m.name.SplitStr(" ");
            m.name = a[a.Length - 1];
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
            if (m.List.AsEnumerable().Any(li => li.PeopleId == id))
                return View("List", m);

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
            p.ValidateModelForNew(ModelState, true);
            if (!ModelState.IsValid)
                return View("FormFull", m);
            return View("List", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddToFamily(SearchModel m)
        {
            var p = m.List[m.List.Count - 1];
            p.ValidateModelForNew(ModelState, false);
            if (!ModelState.IsValid)
                return View("FormAbbreviated", m);
            return View("List", m);
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
            p.email = "david@bvcms.com";
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
                case "contributor":
                    return AddContributor(id.Value, m);
                case "taskdelegate":
                    if (m.List.Count > 0)
                        return Json(new { close = true, how = "addselected", url="/Task/Delegate/", pid = m.List[0].PeopleId });
                    break;
                case "taskdelegate2":
                    if (m.List.Count > 0)
                        return Json(new { close = true, how = "addselected2", url = "/Task/Action/", pid = m.List[0].PeopleId });
                    break;
                case "taskabout":
                    if (m.List.Count > 0)
                        return Json(new { close = true, how = "addselected", url = "/Task/ChangeAbout/", pid = m.List[0].PeopleId });
                    break;
                case "taskowner":
                    if (m.List.Count > 0)
                        return Json(new { close = true, how = "addselected", url = "/Task/ChangeOwner/", pid = m.List[0].PeopleId });
                    break;
            }
            return new EmptyResult();
        }

        private JsonResult AddContactees(int id, SearchModel m)
        {
            if (id > 0)
            {
                var c = DbUtil.Db.NewContacts.Single(ct => ct.ContactId == id);
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, (int)Person.OriginCode.Visit, 0);
                    var ctee = c.contactees.SingleOrDefault(ct =>
                        ct.ContactId == id && ct.PeopleId == p.person.PeopleId);
                    if (ctee == null)
                    {
                        ctee = new Contactee
                        {
                            ContactId = id,
                            PeopleId = p.person.PeopleId,
                        };
                        c.contactees.Add(ctee);
                    }
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected" });
        }
        private JsonResult AddContactors(int id, SearchModel m)
        {
            if (id > 0)
            {
                var c = DbUtil.Db.NewContacts.Single(ct => ct.ContactId == id);
                foreach (var p in m.List)
                {
                    AddPerson(p, m.List, 0, 0);
                    var ctor = c.contactsMakers.SingleOrDefault(ct =>
                        ct.ContactId == id && ct.PeopleId == p.person.PeopleId);
                    if (ctor == null)
                    {
                        ctor = new Contactor
                        {
                            ContactId = id,
                            PeopleId = p.person.PeopleId,
                        };
                        c.contactsMakers.Add(ctor);
                    }
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected" });
        }
        private JsonResult AddFamilyMembers(int id, SearchModel m, bool pending)
        {
            if (id > 0)
            {
                var f = DbUtil.Db.Families.Single(fa => fa.FamilyId == id);

                foreach (var p in m.List)
                {
                    var isnew = p.IsNew;
                    AddPerson(p, m.List, (int)Person.OriginCode.NewFamilyMember, 0);
                    if (!isnew)
                    {
                        var fm = f.People.SingleOrDefault(fa => fa.PeopleId == p.person.PeopleId);
                        if (fm != null)
                            continue; // already a member of this family

                        if (p.person.Age < 18)
                            p.person.PositionInFamilyId = (int)Family.PositionInFamily.Child;
                        else if (p.family.People.Count(per =>
                                    per.PositionInFamilyId == (int)Family.PositionInFamily.PrimaryAdult)
                                    < 2)
                            p.person.PositionInFamilyId = (int)Family.PositionInFamily.PrimaryAdult;
                        else
                            p.person.PositionInFamilyId = (int)Family.PositionInFamily.SecondaryAdult;
                        p.family.People.Add(p.person);
                    }
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
                    AddPerson(p, m.List, (int)Person.OriginCode.NewFamilyMember, 0);
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
                    AddPerson(p, m.List, (int)Person.OriginCode.Enrollment, org.EntryPointId ?? 0);
                    OrganizationMember.InsertOrgMembers(id, p.PeopleId.Value, 220, Util.Now, null, pending);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "rebindgrids" });
        }
        private JsonResult AddContributor(int id, SearchModel m)
        {
            if (id > 0)
            {
                var p = m.List[0];
                var c = DbUtil.Db.Contributions.Single(cc => cc.ContributionId == id);
                AddPerson(p, m.List, (int)Person.OriginCode.Contribution, 0);
                c.PeopleId = p.PeopleId;

                if (c.BankAccount.HasValue())
                {
                    var ci = DbUtil.Db.CardIdentifiers.SingleOrDefault(k => k.Id == c.BankAccount);
                    if (ci == null)
                    {
                        ci = new CardIdentifier
                        {
                            Id = c.BankAccount,
                            CreatedOn = Util.Now,
                        };
                        DbUtil.Db.CardIdentifiers.InsertOnSubmit(ci);
                    }
                    ci.PeopleId = p.PeopleId;
                }
                DbUtil.Db.SubmitChanges();
                return Json(new { close = true, how = "addselected", cid = id, pid = p.PeopleId, name = p.person.Name2 });
            }
            return Json(new { close = true, how = "addselected" });
        }
        private JsonResult AddVisitors(int id, SearchModel m)
        {
            var sb = new StringBuilder();
            if (id > 0)
            {
                var meeting = DbUtil.Db.Meetings.SingleOrDefault(me => me.MeetingId == id);
                foreach (var p in m.List)
                {
                    var isnew = p.IsNew;
                    AddPerson(p, m.List, (int)Person.OriginCode.Visit, meeting.Organization.EntryPointId ?? 0);
                    if (isnew)
                        p.person.CampusId = meeting.Organization.CampusId;
                    var err = Attend.RecordAttendance(p.PeopleId.Value, id, true);
                    if (err.HasValue())
                        sb.AppendLine(err);
                }
                DbUtil.Db.SubmitChanges();
            }
            return Json(new { close = true, how = "addselected", error = sb.ToString() });
        }
        private void AddPerson(SearchPersonModel p, IList<SearchPersonModel> list, int Origin, int EntryPoint)
        {
            if (p.IsNew)
                p.AddPerson(Origin, EntryPoint);
            if (p.FamilyId < 0) // fix up new family pointers
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
            var r = AddressVerify.LookupAddress(p.address, p.address2, p.city, p.state, p.zip);
            return Json(r);
        }
    }
}
