using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;
using System.Configuration;
using System.Net.Mail;
using CMSWeb.Models;
using System.Runtime.Serialization;
using System.IO;
using System.Text;

namespace CMSWeb.Areas.Public.Controllers
{
    public class RetreatController : CmsController
    {
        public ActionResult Index(int id, bool? testing)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (org == null)
                return Content("invalid organization");
            if (testing == true)
                ViewData["testing"] = "?testing=true";
            ViewData["oid"] = id;
            ViewData["EventName"] = org.OrganizationName + " Registration";
#if DEBUG
            var m = new RetreatModel
            {
                first = "David",
                last = "Carroll",
                dob = "5/30/52",
                email = "david@davidcarroll.name",
                phone = "9017581862",
                homecell = "h",
                orgid = id
            };
#else
            var m = new RetreatModel();
            m.orgid = id;
#endif
            if (org.OrganizationMembers.Count() >= org.Limit)
                return View("Filled");
            return View(m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PersonFind(RetreatModel m)
        {
            m.ValidateModelForFind(ModelState);
            ViewData["fee"] = m.ComputeFee().ToString("C");
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ShowMoreInfo(RetreatModel m)
        {
#if DEBUG
            m.address = "235 Riveredge Cv.";
            m.city = "Cordova";
            m.state = "TN";
            m.zip = "38018";
            m.gender = 1;
            m.married = 20;
#endif
            m.ShowAddress = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SubmitNew(RetreatModel m)
        {
            m.ValidateModelForNew(ModelState);
            if (ModelState.IsValid)
                m.IsNew = true;
            return View("list", m);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CompleteRegistration(int id, bool? testing, RetreatModel mm)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);

            var ser = new DataContractSerializer(typeof(RetreatModel));
            var ms = new MemoryStream();
            ser.WriteObject(ms, mm);
            var s = Encoding.Default.GetString(ms.ToArray());
            var d = new ExtraDatum { Data = s, Stamp = Util.Now };
            DbUtil.Db.ExtraDatas.InsertOnSubmit(d);
            DbUtil.Db.SubmitChanges();

            var p = mm;
            var m = new PaymentModel
            {
                address = p.address,
                amount = mm.ComputeFee(),
                city = p.city,
                email = p.email,
                name = p.first + " " + p.last,
                phone = p.phone.FmtFone(),
                state = p.state,
                zip = p.zip,
                testing = testing ?? false,
                description = org.OrganizationName,
                postbackurl = Request.Url.Scheme + "://" + Request.Url.Authority + "/Retreat/Confirm/" + d.Id,
                oid = id,
            };
            return View("Payment", m);
        }
        private static void AddToUserData(string s, OrganizationMember om)
        {
            if (om.UserData.HasValue())
                om.UserData += "\n";
            om.UserData += s;
        }
        public ActionResult Confirm(int? id, string TransactionID, string Misc3, string Misc4)
        {
            if (!id.HasValue)
                return View("Unknown");
            if (!TransactionID.HasValue())
                return Content("error no transaction");
            var orgid = Misc3.ToInt();
            var fee = decimal.Parse(Misc4);
            var org = DbUtil.Db.LoadOrganizationById(orgid);

            var s = DbUtil.Db.ExtraDatas.Single(e => e.Id == id).Data;
            var ser = new DataContractSerializer(typeof(RetreatModel));
            var ms2 = new MemoryStream(Encoding.Default.GetBytes(s));
            var m = ser.ReadObject(ms2) as RetreatModel;
            if (m.IsNew)
                m.AddPerson(org.EntryPointId ?? 0);
            var om = OrganizationMember.InsertOrgMembers(orgid,
                m.person.PeopleId, 220, DateTime.Now, null, false);
            om.Amount = (om.Amount ?? 0) + fee;
            AddToUserData("TransactionId: " + TransactionID, om);
            AddToUserData("AmtPaid: {0:C}".Fmt(fee), om);

            if (m.request.HasValue())
                AddToUserData("Request: " + m.request, om);

            if (m.option == 1)
                om.AddToGroup("PD: Deposit");
            else if (m.option == 2)
            {
                om.RemoveFromGroup("PD: Deposit");
                om.AddToGroup("PD: Full");
            }

            var c = DbUtil.Content("RetreatMessage-" + orgid);
            if (c == null)
            {
                c = new Content();
                c.Body =
@"<p>Hi {first},</p><p>Thank you for registering for {description}.</p>
<p>You paid a fee of {amount}.</p>
<p>
{particpants}<br/>
{request}
</p>";
                c.Title = "Event Registration";
            }

            var req = m.request;
            if (req.HasValue())
                req = "Request: " + req;

            var p = m.person;
            c.Body = c.Body.Replace("{first}", p.PreferredName);
            c.Body = c.Body.Replace("{amount}", fee.ToString("C"));
            c.Body = c.Body.Replace("{description}", org.OrganizationName);
            c.Body = c.Body.Replace("{request}", req);
            c.Body = c.Body.Replace("{particpants}", m.ToString());
            c.Body = c.Body.Replace("{participants}", m.ToString());

            var smtp = new SmtpClient();
            Util.EmailHtml2(smtp, m.email, DbUtil.Settings("RetreatMail-" + orgid, DbUtil.SystemEmailAddress),
                c.Title,
                "<p>{0}({1}) has registered for {2}</p>\n{3}".Fmt(
                p.Name, p.PeopleId, org.OrganizationName, m.ToString()));

            Util.Email(smtp, DbUtil.Settings("RetreatMail-" + orgid, DbUtil.SystemEmailAddress),
                 p.Name, m.email, c.Title, c.Body);

            ViewData["orgname"] = org.OrganizationName;
            ViewData["email"] = m.email;
            return View();
        }
    }
}
