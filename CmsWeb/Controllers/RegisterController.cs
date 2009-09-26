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

namespace CMSWeb.Controllers
{
    [HandleError]
    public class RegisterController : Controller
    {
        public RegisterController()
        {
            ViewData["header"] = DbUtil.Settings("RegHeader");
            ViewData["logoimg"] = DbUtil.Settings("RegLogo");
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

                Session["familyid"] = m.person.FamilyId;
                Session["lastname"] = m.person.LastName;
                Session["name"] = m.person.Name;
                EmailRegister(m);
                return RedirectToAction("Confirm");
            }
            return View(m);
        }
        public ActionResult Form2()
        {
            if (Session["auth"] == null || (string)Session["auth"] != "true")
                return RedirectToAction("Login");
            if (Session["familyid"] == null)
                return RedirectToAction("Index");
            var m = new Models.RegisterModel();
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                m.lastname = (string)Session["lastname"];
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
                m.SavePerson(Session["familyid"].ToInt());
                EmailRegister(m);
                return RedirectToAction("Confirm");
            }
            return View(m);
        }
        public ActionResult Visit(int? id)
        {
            var m = new Models.RegisterModel { campusid = id };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            if (m.FindFamily() == 1)
            {
                Session["familyid"] = m.HeadOfHousehold.FamilyId;
                Session["lastname"] = m.HeadOfHousehold.LastName;
                Session["name"] = m.HeadOfHousehold.Name;
                Session["campus"] = id;
                Session["email"] = m.HeadOfHousehold.EmailAddress;
                return RedirectToAction("Visit2");
            }
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

                Session["familyid"] = m.person.FamilyId;
                Session["lastname"] = m.person.LastName;
                Session["name"] = m.person.Name;
                Session["campus"] = id;
                Session["email"] = m.person.EmailAddress;
                EmailVisit(m);
                return RedirectToAction("ConfirmVisit");
            }
            return View("Visit", m);
        }
        public ActionResult Add(int? id)
        {
            var m = new Models.RegisterModel { campusid = id };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel3(ModelState);
            if (!ModelState.IsValid)
                return View(m);
            if (m.FindFamily() == 1)
            {
                Session["familyid"] = m.HeadOfHousehold.FamilyId;
                Session["lastname"] = m.HeadOfHousehold.LastName;
                Session["name"] = m.HeadOfHousehold.Name;
                Session["campus"] = m.campusid;
                Session["email"] = m.HeadOfHousehold.EmailAddress;
                return RedirectToAction("Visit2");
            }
            ModelState.AddModelError("lastname", "Family not found");
            return View(m);
        }
        public ActionResult Visit2()
        {
            if (Session["familyid"] == null)
                return RedirectToAction("Visit");
            var m = new Models.RegisterModel { campusid = (int?)Session["campus"], email = Session["email"].ToString() };
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                m.lastname = (string)Session["lastname"];
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
                m.SavePerson(Session["familyid"].ToInt());
                EmailVisit(m);
                return RedirectToAction("ConfirmVisit");
            }
            return View(m);
        }
        public ActionResult Confirm()
        {
            return View();
        }
        public ActionResult ConfirmVisit()
        {
            return View();
        }
        public ActionResult Login(string name, string password, string email, int? campus)
        {
            if (Request.HttpMethod.ToUpper() == "GET")
                return View();

            if (!name.HasValue())
                ModelState.AddModelError("name", "required");
            if (!password.HasValue())
                ModelState.AddModelError("password", "required");
            if (!Util.ValidEmail(email))
                ModelState.AddModelError("email", "valid registration email required");
            if (ModelState.IsValid)
                if (password == DbUtil.Settings("RegPassword"))
                {
                    Session["name"] = name;
                    Session["email"] = email;
                    Session["auth"] = "true";
                    return RedirectToAction("Index");
                }
            System.Threading.Thread.Sleep(20000);
            ModelState.AddModelError("auth", "incorrect password");
            return View();
        }

        private void EmailRegister(RegisterModel m)
        {
            var c = DbUtil.Db.Contents.SingleOrDefault(ms => ms.Name == "RegisterMessage");
            if (c == null)
            {
                c = new Content();
                c.Body = "<p>Thank you for helping us build our Church Database.</p>";
                c.Title = "Church Database Registration";
            }
            c.Body += "<p>We have the following information: <pre>\n{0}\n</pre></p>".Fmt(m.PrepareSummaryText());

            var smtp = new SmtpClient();
            Util.Email(smtp, DbUtil.Settings("RegMail"), m.person.Name, m.person.EmailAddress, c.Title, c.Body);
            Util.Email2(smtp, m.person.EmailAddress, DbUtil.Settings("RegMail"), "new registration in cms", 
                "{0}({1}) registered in cms".Fmt(m.person.Name, m.person.PeopleId));
        }

        private void EmailVisit(RegisterModel m)
        {
            string email = DbUtil.Settings("VisitMail-" + Session["campus"]);
            if (!email.HasValue())
                email = DbUtil.Settings("RegMail");

            var c = DbUtil.Db.Contents.SingleOrDefault(ms => ms.Name == "VisitMessage-" + Session["campus"]);
            if (c == null)
            {
                c = new Content();
                c.Body = "<p>Hi {first},</p><p>Thank you for visiting us.</p>";
                c.Title = "Church Database Registration";
            }
            var p = m.person;
            c.Body = c.Body.Replace("{first}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            c.Body = c.Body.Replace("{firstname}", p.NickName.HasValue() ? p.NickName : p.FirstName);
            c.Body += "<p>We have the following information: <pre>\n{0}\n</pre></p>".Fmt(m.PrepareSummaryText());

            var smtp = new SmtpClient();
            Util.Email(smtp, email, p.Name, p.EmailAddress, c.Title, c.Body);
            Util.Email2(smtp, m.person.EmailAddress, email, "new registration in cms",
                "{0}({1}) registered in cms".Fmt(m.person.Name, m.person.PeopleId));
        }
    }
}
