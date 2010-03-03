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
            //var org = DbUtil.Db.LoadOrganizationById(id);
            //if (org == null)
            //    return Content("invalid organization");
            var list = new List<SearchAddModel>();
//#if DEBUG
//            list.Add(new SearchAddModel
//            {
//                first = "David",
//                last = "Carroll",
//                dob = "5/30/52",
//                email = "david@davidcarroll.name",
//                phone = "9017581862".FmtFone(),
//                homecell = "h",
//            });
//#else
//            list.Add(new SearchAddModel());
//#endif
            switch (type)
            {
                case "family":
                    break;
                case "organization":
                    break;
                case "meeting":
                    break;
                case "contactee":
                    break;
                case "contactor":
                    break;
            }
            if (id.HasValue)
            list[0].evtype = type;
            SetViewData(id);
            return View(list);
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
        public ActionResult PersonFind(int id, IList<SearchAddModel> list)
        {
            list[id].ValidateModelForFind(ModelState);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(int id, IList<SearchAddModel> list)
        {
#if DEBUG
            var m = list[id];
            m.address = "235 Riveredge Cv.";
            m.city = "Cordova";
            m.state = "TN";
            m.zip = "38018";
            m.gender = 1;
            m.married = 20;
#endif
            list[id].ShowAddress = true;
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitNew(int id, IList<SearchAddModel> list)
        {
            list[id].ValidateModelForNew(ModelState);
            if (ModelState.IsValid)
                list[id].IsNew = true;
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cancel(int id, IList<SearchAddModel> list)
        {
            list.RemoveAt(id);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAnotherPerson(IList<SearchAddModel> list)
        {
            list[list.Count - 1].ValidateModelForComplete(ModelState);
            if (!ModelState.IsValid)
                return View("list", list);
#if DEBUG
            list.Add(new SearchAddModel
            {
                first = "Delaine",
                last = "Carroll",
                dob = "9/29/46",
                email = "davcar@pobox.com",
                phone = "9017581862".FmtFone(),
                homecell = "h"
            });
#else
            list.Add(new SearchAddModel());
#endif
            list[list.Count - 1].evtype = list[0].evtype;
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IsValid(IList<SearchAddModel> list)
        {
            list[list.Count - 1].ValidateModelForComplete(ModelState);
            if (!ModelState.IsValid)
                return View("list", list);
            return Content("OK");
        }
        public static decimal ComputeFee(IList<SearchAddModel> list)
        {
            return list[0].evtype == "ChildCare" ?
                            list.Max(i => i.ComputeFee()) :
                            list.Sum(i => i.ComputeFee());
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(int? id, IList<SearchAddModel> list)
        {
            var org = DbUtil.Db.LoadOrganizationById(id.Value);
            var m = list[list.Count - 1];
            m.ValidateModelForComplete(ModelState);
            if (!ModelState.IsValid)
            {
                SetViewData(id);
                return View("Index", list);
            }

            var p = list[0];
            if (!id.HasValue)
                return View("Unknown");
            //var orgid = Misc3.ToInt();
            //var org = DbUtil.Db.LoadOrganizationById(orgid);

            for (var i = 0; i < list.Count; i++ )
            {
                m = list[i];
                if (m.IsNew)
                    m.AddPerson(i == 0 ? null : list[0].person, org.EntryPointId ?? 0);
                var om = OrganizationMember.InsertOrgMembers(id.Value,
                    m.person.PeopleId, (int)OrganizationMember.MemberTypeCode.Member, 
                    DateTime.Now, null, false);
                if (om.UserData.HasValue())
                    om.UserData += "<br />\n";
                switch(m.evtype)
                {
                    case "childcare":
                        om.AddToGroup(m.person.PositionInFamilyId == 30 ? "EV: Child" : "EV: Adult");
                        break;
                    case "5kfunrun":
                        om.AddToGroup(m.option == 1 ? "EV: 5K" : "EV: FunRun");
                        break;
                }
                if (!m.person.HomePhone.HasValue() && m.homecell == "h")
                    m.person.Family.HomePhone = m.phone;
                if (!m.person.CellPhone.HasValue() && m.homecell == "c")
                    m.person.CellPhone = m.phone;

                var reg = m.person.RecRegs.SingleOrDefault();

                if (reg == null)
                {
                    reg = new RecReg();
                    m.person.RecRegs.Add(reg);
                }
                reg.ShirtSize = m.ShirtSize;
            }
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
