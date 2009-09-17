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
                var p = m.SaveFirstPerson();

                Session["familyid"] = p.FamilyId;
                Session["lastname"] = p.LastName;
                Session["name"] = p.Name;
                EmailUser(m, p);
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
                var p = m.SavePerson((int)Session["familyid"]);
                EmailUser(m, p);
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
            m.ValidateModel1(ModelState);
            if (ModelState.IsValid)
            {
                var p = m.SaveFirstPerson() as Person;

                Session["familyid"] = p.FamilyId;
                Session["lastname"] = p.LastName;
                Session["name"] = p.Name;
                Session["campus"] = id;
                EmailUser(m, p);
                return RedirectToAction("ConfirmVisit");
            }
            return View("Visit", m);
        }
        public ActionResult Visit2()
        {
            if (Session["familyid"] == null)
                return RedirectToAction("Visit");
            var m = new Models.RegisterModel { campusid = (int?)Session["campus"] };
            if (Request.HttpMethod.ToUpper() == "GET")
            {
                m.lastname = (string)Session["lastname"];
                return View(m);
            }

            UpdateModel(m);
            m.ValidateModel2(ModelState);
            if (ModelState.IsValid)
            {
                var p = m.SavePerson((int)Session["familyid"]);
                EmailUser(m, p);
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
        private void EmailUser(RegisterModel m, Person p)
        {
            HomeController.Email(DbUtil.Settings("RegMail"),
                                p.Name,p.EmailAddress, "Church Registration",
@"<p>Thank you for helping us build our Church Database.</p>
<p>We have the following information:
<pre>
{0}
</pre>".Fmt(m.PrepareSummaryText()));
        }
    }
}
