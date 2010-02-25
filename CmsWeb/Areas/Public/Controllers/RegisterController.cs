using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using CMSWeb.Models;
using UtilityExtensions;
using System.Net.Mail;
using System.Text;

namespace CMSWeb.Areas.Public.Controllers
{
    public class RegisterController : CmsController
    {
        public RegisterController()
        {
            ViewData["head"] = HeaderHtml("RegHeader",
                DbUtil.Settings("RegHeader", "change RegHeader setting"),
                DbUtil.Settings("RegLogo", "/Content/Crosses.png"));
        }

        public ActionResult Inside(int? campus)
        {
            Session["auth"] = "true";
            return RedirectToAction("Index", new { campus = campus });
        }
        public ActionResult Index(int? campus)
        {
            if (Session["auth"] == null || (string)Session["auth"] != "true")
                return RedirectToAction("Login", new { campus = campus });
            var m = new Models.RegisterModel { campusid = campus };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel1(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count >= 1)
                {
                    ModelState.AddModelError("first", "Already Registered");
                    return View(m);
                }
            }

            if (ModelState.IsValid)
            {
                m.SaveFirstPerson();

                EmailRegister(m);
                return RedirectToAction("Confirm", new { familyid = m.person.FamilyId });
            }
            return View(m);
        }
        public ActionResult Form2(int familyid)
        {
            if (Session["auth"] == null || (string)Session["auth"] != "true")
                return RedirectToAction("Login");
            var m = new Models.RegisterModel { familyid = familyid };
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                m.last = m.HeadOfHousehold.LastName;
                return View(m);
            }

            UpdateModel(m);
            m.ValidateModel2(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count >= 1)
                {
                    ModelState.AddModelError("first", "Already Registered");
                    return View(m);
                }
            }
            if (ModelState.IsValid)
            {
                m.SavePerson(familyid);
                EmailRegister(m);
                return RedirectToAction("Confirm", new { familyid = familyid });
            }
            return View(m);
        }
        public ActionResult Visit(int? id, string submit, int? thisday)
        {
            var m = new Models.RegisterModel { campusid = id, thisday = thisday };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel1(ModelState);
            if (!ModelState.IsValid)
                return View(m);
            bool foundFamily = m.FindFamily() == 1;
            if (m.existingfamily ?? false)
            {
                if (!foundFamily)
                {
                    m.existingfamily = false;
                    ModelState.AddModelError("existing", "could not find family");
                    return View(m);
                }
                if (submit.StartsWith("Add"))
                {
                    m.SavePerson(m.HeadOfHousehold.FamilyId);
                    return RedirectToAction("ConfirmVisit",
                        new
                        {
                            familyid = m.HeadOfHousehold.FamilyId,
                            id = id,
                            thisday = thisday
                        });
                }
            }
            else if (foundFamily)
            {
                m.existingfamily = true;
                ModelState.AddModelError("submit", "existing family found");
                return View(m);
            }
            m.SaveFirstPerson();
            EmailVisit(m);
            return RedirectToAction("ConfirmVisit",
                new
                {
                    familyid = m.person.FamilyId,
                    id = id,
                    thisday = thisday
                });
        }
        public ActionResult Add(int? id, int? thisday)
        {
            var m = new Models.RegisterModel { campusid = id, thisday = thisday };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel3(ModelState);
            if (!ModelState.IsValid)
                return View(m);
            int count = m.FindFamily();
            if (count == 1)
            {
                return RedirectToAction("Visit2",
                    new
                    {
                        familyid = m.HeadOfHousehold.FamilyId,
                        id = id,
                        thisday = thisday
                    });
            }
            if (count == 0)
                ModelState.AddModelError("last", "Family not found");
            else
                ModelState.AddModelError("last", "More than one family found");
            return View(m);
        }
        public ActionResult Visit2(int familyid, int? id, int? thisday)
        {
            var m = new Models.RegisterModel { familyid = familyid, campusid = id, thisday = thisday };
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                m.last = m.HeadOfHousehold.LastName;
                m.hcellphone = m.HeadOfHousehold.CellPhone;
                m.email = m.HeadOfHousehold.EmailAddress;
                return View(m);
            }

            UpdateModel(m);
            m.ValidateModel2(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count >= 1)
                {
                    ModelState.AddModelError("first", "Already Registered");
                    return View(m);
                }
            }
            if (ModelState.IsValid)
            {
                m.SavePerson(familyid);
                EmailVisit(m);
                return RedirectToAction("ConfirmVisit",
                    new
                    {
                        id = id,
                        thisday = thisday,
                        familyid = familyid
                    });
            }
            return View(m);
        }
        public ActionResult Confirm(int familyid)
        {
            var m = new Models.RegisterModel { familyid = familyid };
            return View(m);
        }
        public ActionResult ConfirmVisit(int familyid, int? id, int? thisday)
        {
            var m = new Models.RegisterModel { campusid = id, thisday = thisday, familyid = familyid };
            return View(m);
        }
        public ActionResult Login(string password, int? campus)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!password.HasValue())
                ModelState.AddModelError("password", "required");
            if (ModelState.IsValid)
                if (password == DbUtil.Settings("RegPassword", "fgsltw"))
                {
                    Session["auth"] = "true";
                    return RedirectToAction("Index");
                }
            System.Threading.Thread.Sleep(20000);
            ModelState.AddModelError("auth", "incorrect password");
            return View();
        }

        private void EmailRegister(Models.RegisterModel m)
        {
            var c = DbUtil.Content("RegisterMessage");
            if (c == null)
            {
                c = new Content();
                c.Body = "<p>Thank you for helping us build our Church Database.</p>";
                c.Title = "Church Database Registration";
            }
            c.Body += "<p>We have the following information: <pre>\n{0}\n</pre></p>".Fmt(m.PrepareSummaryText());

            var smtp = Util.Smtp();
            Util.Email(smtp, DbUtil.Settings("RegMail", DbUtil.SystemEmailAddress), m.person.Name, m.person.EmailAddress, c.Title, c.Body);
            Util.Email2(smtp, m.person.EmailAddress, DbUtil.Settings("RegMail", DbUtil.SystemEmailAddress), 
                "new registration on {0}".Fmt(Util.Host), 
                "{0}({1}) registered in cms".Fmt(m.person.Name, m.person.PeopleId));
        }

        private void EmailVisit(Models.RegisterModel m)
        {
            string email = DbUtil.Settings("VisitMail-" + m.campusid, "");
            if (!email.HasValue())
                email = DbUtil.Settings("RegMail", DbUtil.SystemEmailAddress);

            var c = DbUtil.Content("VisitMessage-" + m.campusid);
            if (c == null)
            {
                c = new Content();
                c.Body = "<p>Hi {first},</p><p>Thank you for visiting us.</p>";
                c.Title = "Church Database Registration";
            }
            var p = m.person;
            c.Body = c.Body.Replace("{first}", p.PreferredName);
            c.Body = c.Body.Replace("{firstname}", p.PreferredName);
            c.Body += "<p>We have the following information: <pre>\n{0}\n</pre></p>".Fmt(m.PrepareSummaryText());

            var smtp = Util.Smtp();
            Util.Email(smtp, email, p.Name, p.EmailAddress, c.Title, c.Body);
            Util.Email2(smtp, m.person.EmailAddress, email, "new registration in cms", "{0}({1}) registered in cms".Fmt(m.person.Name, m.person.PeopleId));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Schools(string q, int limit)
        {
            var qu = from p in DbUtil.Db.People
                    where p.SchoolOther.Contains(q)
                    group p by p.SchoolOther into g
                    select g.Key;
            return Content(string.Join("\n", qu.Take(limit).ToArray()));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult CityState(string id)
        {
            var z = DbUtil.Db.ZipCodes.SingleOrDefault(zc => zc.Zip == id);
            if (z == null)
                return Json(null);
            return Json(new { city = z.City.Trim(), state = z.State });
        }
    }
}
