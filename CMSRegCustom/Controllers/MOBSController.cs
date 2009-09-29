using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSRegCustom.Models;
using UtilityExtensions;
using System.Configuration;

namespace CMSRegCustom.Controllers
{
    public class MOBSController : Controller
    {
        public ActionResult Index()
        {
            var m = new MOBSModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count > 1)
                    ModelState.AddModelError("find", "More than one match, sorry");
                else if (count == 0)
                    if (!m.shownew)
                    {
                        ModelState.AddModelError("find", "Cannot find church record.");
                        m.shownew = true;
                    }
            }
            if (!ModelState.IsValid)
                return View(m);

            if (m.person == null)
                m.AddPerson();
            m.AttendEvent();

            var reg = DbUtil.Db.MOBSRegs.SingleOrDefault(r =>
                r.PeopleId == m.person.PeopleId
                && r.MeetingId == m.meeting.MeetingId);

            if (reg == null)
            {
                reg = new MOBSReg
                {
                    PeopleId = m.person.PeopleId,
                    MeetingId = m.meeting.MeetingId,
                    Created = DateTime.Now,
                    Email = m.email,
                    FeePaid = false,
                };
                DbUtil.Db.MOBSRegs.InsertOnSubmit(reg);
            }

            DbUtil.Db.SubmitChanges();

            Util.Email2(m.email,
                    DbUtil.Settings("MOBSMail"), "MOBS Registration",
@"{0}({1}) has registered for MOBS event on {2} (check cms to confirm feepaid)</p>".Fmt(
m.person.Name, m.peopleid, m.MeetingTime));

            TempData["regid"] = reg.Id;
            return RedirectToAction("Payment");

        }
        public ActionResult Payment()
        {
            if (!TempData.ContainsKey("regid"))
                return View("Unknown");
            var m = new MOBSModel { regid = (int)TempData["regid"] };
            return View(m);
        }
        public ActionResult Confirm(int? id, string TransactionID)
        {
            if (!id.HasValue)
                return View("Unknown");
            var m = new MOBSModel { regid = id };
            if (TransactionID.HasValue())
            {
                m.registration.FeePaid = true;
                m.registration.TransactionId = TransactionID;
                DbUtil.Db.SubmitChanges();
            }

            Util.Email(DbUtil.Settings("MOBSMail"),
    "", m.registration.Email, "MOBS Event Registration",
@"<p>Thank you for registering for a MOBS event on {0:d}".Fmt(m.meeting.MeetingDate));
            return View(m);
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
