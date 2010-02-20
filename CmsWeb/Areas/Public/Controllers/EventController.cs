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

namespace CMSWeb.Areas.Public.Controllers
{
    public class EventController : Controller
    {
        public ActionResult Index(int id, bool? testing)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return Content("invalid organization");
            var rv = new RouteValueDictionary();
            rv.Add("id", id);
            if (testing == true)
                rv.Add("testing", "true");
            ViewData["rv"] = rv;
            ViewData["EventName"] = org.OrganizationName + " Registration";
            ViewData["Instructions"] = org.Instructions;
            var list = new List<EventModel>();
#if DEBUG
            list.Add(new EventModel
            {
                first = "David",
                last = "Carroll",
                dob = "5/30/52",
                email = "david@davidcarroll.name",
                phone = "9017581862".FmtFone(),
                homecell = "h",
            });
#else
            list.Add(new EventModel());
#endif
            list[0].evtype = org.RegType;
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
        private decimal ComputeFee(IList<EventModel> list)
        {
            decimal fee = 0;
            switch (list[0].evtype)
            {
                case "childcare":
                    fee = list.Count(p => p.age < 15) > 0 ? 6M : 0M;
                    break;
                case "5kfunrun":
                    foreach (var m in list)
                        fee += m.age >= 10 ? 25 : 15;
                    break;
            }
            list[list.Count - 1].CanPay = fee > 0;
            ViewData["fee"] = fee.ToString("c");
            return fee;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonFind(int id, IList<EventModel> list)
        {
            list[id].ValidateModelForFind(ModelState);
            ComputeFee(list);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(int id, IList<EventModel> list)
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
        public ActionResult SubmitNew(int id, IList<EventModel> list)
        {
            list[id].ValidateModelForNew(ModelState);
            if (ModelState.IsValid)
                list[id].IsNew = true;
            ComputeFee(list);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cancel(int id, IList<EventModel> list)
        {
            list.RemoveAt(id);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAnotherPerson(IList<EventModel> list)
        {
#if DEBUG
            list.Add(new EventModel
            {
                first = "Delaine",
                last = "Carroll",
                dob = "9/29/46",
                email = "davcar@pobox.com",
                phone = "9017581862".FmtFone(),
                homecell = "h"
            });
#else
            list.Add(new EventModel());
#endif
            list[list.Count - 1].evtype = list[0].evtype;
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(int id, bool? testing, IList<EventModel> list)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);

            var s = Util.Serialize<IList<EventModel>>(list);
            var d = new ExtraDatum { Data = s, Stamp = Util.Now };
            DbUtil.Db.ExtraDatas.InsertOnSubmit(d);
            DbUtil.Db.SubmitChanges();

            var p = list[0];
            var m = new PaymentModel
            {
                NameOnAccount = p.first + " " + p.last,
                Address = p.address,
                Amount = ComputeFee(list),
                City = p.city,
                Email = p.email,
                Phone = p.phone.FmtFone(),
                State = p.state,
                PostalCode = p.zip,
                testing = testing ?? false,
                PostbackURL = Request.Url.Scheme + "://" + Request.Url.Authority + "/Event/Confirm/" + d.Id,
                Misc1 = p.first + " " + p.last,
                Misc2 = org.OrganizationName,
                Misc3 = id.ToString(),
            };
            return View("Payment", m);
        }
        public ActionResult Confirm(int? id, string TransactionID, string Misc3)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");
            var orgid = Misc3.ToInt();
            var org = DbUtil.Db.LoadOrganizationById(orgid);

            var s = DbUtil.Db.ExtraDatas.Single(e => e.Id == id).Data;
            var list = Util.DeSerialize<IList<EventModel>>(s);

            for (var i = 0; i < list.Count; i++ )
            {
                var m = list[i];
                if (m.IsNew)
                    m.AddPerson(i == 0 ? null : list[0].person, org.EntryPointId ?? 0);
                var om = OrganizationMember.InsertOrgMembers(orgid,
                    m.person.PeopleId, (int)OrganizationMember.MemberTypeCode.Member, 
                    DateTime.Now, null, false);
                if (om.UserData.HasValue())
                    om.UserData += "<br />\n";
                om.UserData += "TransactionId: " + TransactionID;
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
            }
            DbUtil.Db.SubmitChanges();

            var sb = new StringBuilder();
            foreach (var m in list)
                sb.AppendLine(m.ToString());

            var p = list[0].person;
            var msg = org.EmailMessage;
            msg = msg.Replace("{first}", p.PreferredName);
            msg = msg.Replace("{number}", list.Count.ToString());
            msg = msg.Replace("{amount}", ComputeFee(list).ToString("C"));
            msg = msg.Replace("{participants}", sb.ToString());

            var smtp = new SmtpClient();
            Util.Email2(smtp, list[0].email, org.EmailAddresses,
                org.EmailSubject,
                "<p>{0}({1}) has registered for {2}</p>\n<p>Participants<br />\n{3}".Fmt(
                p.Name, p.PeopleId, org.OrganizationName, sb.ToString()));
            Util.Email(smtp, org.EmailAddresses, p.Name, list[0].email, org.EmailSubject, msg);
            Util.SendIfEmailDifferent(smtp, org.EmailAddresses, list[0].email, 
                p.PeopleId, p.Name, p.EmailAddress, org.EmailSubject, msg);

            ViewData["orgname"] = org.OrganizationName;
            ViewData["email"] = list[0].email;
            return View();
        }
    }
}
