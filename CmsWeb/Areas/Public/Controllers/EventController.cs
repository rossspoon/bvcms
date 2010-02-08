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
        public ActionResult Childcare(int id, bool? testing)
        {
            return RedirectToAction("Index", new { id = id, testing = testing });
        }
        private decimal ComputeFee(IList<EventModel> list)
        {
            decimal fee = 0;
            switch (list[0].evtype)
            {
                case "childcare":
                    fee = 6M;
                    break;
                case "5kfunrun":
                    foreach (var m in list)
                        fee += m.age >= 10 ? 25 : 15;
                    break;
            }
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

            var ser = new DataContractSerializer(typeof(IList<EventModel>));
            var ms = new MemoryStream();
            ser.WriteObject(ms, list);
            var s = Encoding.Default.GetString(ms.ToArray());
            var d = new ExtraDatum { Data = s, Stamp = Util.Now };
            DbUtil.Db.ExtraDatas.InsertOnSubmit(d);
            DbUtil.Db.SubmitChanges();

            var p = list[0];
            var m = new PaymentModel
            {
                address = p.address,
                amount = ComputeFee(list),
                city = p.city,
                email = p.email,
                name = p.first + " " + p.last,
                phone = p.phone.FmtFone(),
                state = p.state,
                zip = p.zip,
                testing = testing ?? false,
                description = org.OrganizationName,
                postbackurl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Event/Confirm/" + d.Id,
                oid = id,
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
            var ser = new DataContractSerializer(typeof(IList<EventModel>));
            var ms2 = new MemoryStream(Encoding.Default.GetBytes(s));
            var list = ser.ReadObject(ms2) as IList<EventModel>;

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
            Util.EmailHtml2(smtp, list[0].email, org.EmailAddresses,
                org.EmailSubject,
                "<p>{0}({1}) has registered for {2}</p>\n<p>Participants<br />\n{3}".Fmt(
                p.Name, p.PeopleId, org.OrganizationName, sb.ToString()));

            Util.Email(smtp, org.EmailAddresses, p.Name, list[0].email, org.EmailSubject, msg);
            if (list[0].email != p.EmailAddress)
            {
                Util.Email(smtp, org.EmailAddresses, p.Name, p.EmailAddress, org.EmailSubject, msg);
                Util.EmailHtml2(smtp, org.EmailAddresses, org.EmailAddresses,
                    "different email address than one on record",
                    "<p>{0}({1}) registered  with {2} but has {3} in record.</p>".Fmt(
                    p.Name, p.PeopleId, list[0].email, p.EmailAddress));
            }

            ViewData["orgname"] = org.OrganizationName;
            ViewData["email"] = list[0].email;
            return View();
        }
    }
}
