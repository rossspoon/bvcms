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

namespace CMSWeb.Areas.Public.Controllers
{
    public class EventController : Controller
    {
        public ActionResult Index(int id, bool? testing)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return Content("invalid organization");
            if (!org.LeaderId.HasValue)
                return Content("no leader in org");
            if (testing == true)
                ViewData["testing"] = "?testing=true";
            ViewData["OrgId"] = id;
            ViewData["EventName"] = org.OrganizationName + " Registration";
            var list = new List<PersonEventModel>();
#if DEBUG
            list.Add(new PersonEventModel
            {
                first = "David",
                last = "Carroll",
                dob = "5/30/52",
                email = "david@davidcarroll.name",
                phone = "9017581862",
                homecell = "h"
            });
#else
            list.Add(new PersonEventModel());
#endif
            return View(list);
        }
        private decimal ComputeFee(IList<PersonEventModel> list)
        {
            decimal fee = 0;
            foreach (var m in list)
                fee += m.age >= 10 ? 25 : 15;
            ViewData["fee"] = fee.ToString("c");
            return fee;
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonFind(int id, IList<PersonEventModel> list)
        {
            list[id].ValidateModelForFind(ModelState);
            ComputeFee(list);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(int id, IList<PersonEventModel> list)
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
        public ActionResult SubmitNew(int id, IList<PersonEventModel> list)
        {
            list[id].ValidateModelForNew(ModelState);
            if (ModelState.IsValid)
                list[id].IsNew = true;
            ComputeFee(list);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cancel(int id, IList<PersonEventModel> list)
        {
            list.RemoveAt(id);
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddAnotherPerson(IList<PersonEventModel> list)
        {
#if DEBUG
            list.Add(new PersonEventModel
            {
                first = "Delaine",
                last = "Carroll",
                dob = "9/29/46",
                email = "davcar@pobox.com",
                phone = "9017581862",
                homecell = "h"
            });
#else
            list.Add(new PersonEventModel());
#endif
            return View("list", list);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(int id, bool? testing, IList<PersonEventModel> list)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);

            var ser = new DataContractSerializer(typeof(IList<PersonEventModel>));
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
            var leader = DbUtil.Db.LoadPersonById(org.LeaderId.Value);

            var s = DbUtil.Db.ExtraDatas.Single(e => e.Id == id).Data;
            var ser = new DataContractSerializer(typeof(IList<PersonEventModel>));
            var ms2 = new MemoryStream(Encoding.Default.GetBytes(s));
            var list = ser.ReadObject(ms2) as IList<PersonEventModel>;
            for (var i = 0; i < list.Count; i++ )
            {
                var m = list[i];
                if (m.IsNew)
                    m.AddPerson(i == 0 ? null : list[0].person);
                var om = OrganizationMember.InsertOrgMembers(orgid,
                    m.person.PeopleId, 220, DateTime.Now, null, false);
                if (om.UserData.HasValue())
                    om.UserData += "<br />\n";
                om.UserData += "TransactionId: " + TransactionID;
                om.AddToGroup(m.option == 1 ? "EV: 5K" : "EV: FunRun");
            }

            var c = DbUtil.Content("EventMessage-" + orgid);
            if (c == null)
            {
                c = new Content();
                c.Body =
@"<p>Hi {first},</p><p>Thank you for registering for {description} event.</p>
<p>You purchased {tickets} entry fees for a total cost of {amount}.</p>
<p>Particpants:</p>
<p>
{particpants}
</p>";
                c.Title = "Event Registration";
            }
            var sb = new StringBuilder();
            foreach (var m in list)
                sb.AppendLine(m.ToString());

            var p = list[0].person;
            c.Body = c.Body.Replace("{first}", p.PreferredName);
            c.Body = c.Body.Replace("{tickets}", list.Count.ToString());
            c.Body = c.Body.Replace("{amount}", ComputeFee(list).ToString("C"));
            c.Body = c.Body.Replace("{description}", org.OrganizationName);
            c.Body = c.Body.Replace("{particpants}", sb.ToString());
            c.Body = c.Body.Replace("{participants}", sb.ToString());

            Util.EmailHtml2(new SmtpClient(), list[0].email, DbUtil.Settings("EventMail-" + orgid, DbUtil.SystemEmailAddress),
                c.Title,
                "<p>{0}({1}) has registered for {2}</p>\n<p>Participants<br />\n{3}".Fmt(
                p.Name, p.PeopleId, org.OrganizationName, sb.ToString()));

            Util.Email(DbUtil.Settings("EventMail-" + orgid, DbUtil.SystemEmailAddress),
                 p.Name, list[0].email, c.Title, c.Body);

            ViewData["orgname"] = org.OrganizationName;
            ViewData["email"] = list[0].email;
            return View();
        }
        public JsonResult CityState(string id)
        {
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == id);
            if (z == null)
                return Json(null);
            return Json(new { city = z.City.Trim(), state = z.State });
        }

    }
}
