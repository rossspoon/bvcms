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
        private void SetViewData(int? id)
        {
            var rv = new RouteValueDictionary();
            rv.Add("id", id);
            ViewData["rv"] = rv;
        }
        public ActionResult Index(int? id, string type)
        {
#if DEBUG
            var m = new SearchModel { name = "David Carr" };
#else
            var m = new SearchModel();
#endif
            m.from = type;
            //switch (type)
            //{
            //    case "family":
            //        break;
            //    case "organization":
            //        break;
            //    case "meeting":
            //        break;
            //    case "contactee":
            //        break;
            //    case "contactor":
            //        break;
            //}
            if (id.HasValue)
                SetViewData(id);
            return View(m);
        }
        public ActionResult Results(SearchModel m)
        {
            return View(m);
        }
        public ActionResult ResultsFamily(SearchModel m)
        {
            m.dob = "";
            return View(m);
        }
        public ActionResult SearchPerson(SearchModel m)
        {
#if DEBUG
            m.name = "d c";
#endif
            return View(m);
        }
        public ActionResult SearchFamily(SearchModel m)
        {
#if DEBUG
            m.name = "f m";
#endif
            return View(m);
        }
        public ActionResult SearchCancel(SearchModel m)
        {
            if (m.List.Count > 0)
                return View("List", m);
            return Content("close");
        }
        public ActionResult SearchFamilyCancel(SearchModel m)
        {
#if DEBUG
            m.name = "d c";
#endif
            return View("SearchPerson", m);
        }
        public ActionResult PersonCancel(int id, SearchModel m)
        {
            m.List.RemoveAt(id);
            if (m.List.Count > 0)
                return View("List", m);
#if DEBUG
            m.name = "d c";
#endif
            return View("SearchForm", m);
        }
        public ActionResult Select(int id, SearchModel m)
        {
            var p = DbUtil.Db.LoadPersonById(id);
            var s = new SearchPersonModel
            {
                first = p.FirstName,
                goesby = p.NickName,
                last = p.LastName,
                marital = p.MaritalStatusId,
                email = p.EmailAddress,
                suffix = p.SuffixCode,
                title = p.TitleCode,
                dob = p.DOB,
                address = p.PrimaryAddress,
                city = p.PrimaryCity,
                state = p.PrimaryState,
                zip = p.PrimaryZip,
                gender = p.GenderId,
                phone = p.CellPhone,

            };
            m.List.Add(s);
            ViewData["mode"] = "search";
            return View("List", m);
        }

        public ActionResult Childcare(int? id, bool? testing)
        {
            if (!id.HasValue)
                return Content("must specify an organizationid");
            var org = DbUtil.Db.LoadOrganizationById(id.Value);
            if (org == null)
                return Content("must specify the correct organizationid");
            return RedirectToAction("Index", new { id = id.Value, testing = testing });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(int id, SearchModel m)
        {
            //#if DEBUG
            //            var m = list[id];
            //            m.address = "235 Riveredge Cv.";
            //            m.city = "Cordova";
            //            m.state = "TN";
            //            m.zip = "38018";
            //            m.gender = 1;
            //            m.married = 20;
            //#endif
            m.List[id].ShowAddress = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitNew(int id, SearchModel m)
        {
            m.List[id].ValidateModelForNew(ModelState);
            if (ModelState.IsValid)
                m.List[id].IsNew = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cancel(int id, SearchModel m)
        {
            m.List.RemoveAt(id);
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAnotherPerson(IList<SearchPersonModel> list)
        {
            //list[list.Count - 1].ValidateModelForComplete(ModelState);
            if (!ModelState.IsValid)
                return View("list", list);
            //#if DEBUG
            //            list.Add(new SearchPersonModel
            //            {
            //                first = "Delaine",
            //                last = "Carroll",
            //                dob = "9/29/46",
            //                email = "davcar@pobox.com",
            //                phone = "9017581862".FmtFone(),
            //                homecell = "h"
            //            });
            //#else
            //            list.Add(new SearchAddModel());
            //#endif
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(int? id, SearchModel m)
        {
            var org = DbUtil.Db.LoadOrganizationById(id.Value);
            //m.ValidateModelForComplete(ModelState);
            if (!ModelState.IsValid)
            {
                SetViewData(id);
                return View("Index", m);
            }

            if (!id.HasValue)
                return View("Unknown");
            //var orgid = Misc3.ToInt();
            //var org = DbUtil.Db.LoadOrganizationById(orgid);

            foreach(var p in m.List)
            {
                if (p.IsNew)
                    p.AddPerson(null, org.EntryPointId ?? 0);
                var om = OrganizationMember.InsertOrgMembers(id.Value,
                    p.person.PeopleId, (int)OrganizationMember.MemberTypeCode.Member,
                    DateTime.Now, null, false);
                if (om.UserData.HasValue())
                    om.UserData += "<br />\n";

                if (!p.person.HomePhone.HasValue() && p.homecell == "h")
                    p.person.Family.HomePhone = m.phone;
                if (!p.person.CellPhone.HasValue() && p.homecell == "c")
                    p.person.CellPhone = m.phone;

            }
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
