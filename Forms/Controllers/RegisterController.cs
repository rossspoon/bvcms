using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using System.Configuration;
using Forms.Models;

namespace Forms.Controllers
{
    [HandleError]
    public class RegisterController : Controller
    {
        public RegisterController()
        {
            ViewData["header"] = ConfigurationManager.AppSettings["regheader"];
        }
        public ActionResult Form()
        {
            if (Session["auth"] == null || (string)Session["auth"] != "true")
                return RedirectToAction("Login");
            var m = new Models.RegisterModel();
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel1(ModelState);
            if (ModelState.IsValid)
            {
                var p = m.SaveFirstPerson() as Person;
                Session["familyid"] = p.FamilyId;
                Session["lastname"] = p.LastName;
                EmailUser(m);
                return RedirectToAction("Confirm");
            }
            return View(m);
        }
        public ActionResult Form2()
        {
            if (Session["auth"] == null || (string)Session["auth"] != "true")
                return RedirectToAction("Login");
            if (Session["familyid"] == null)
                return RedirectToAction("Form");
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
                m.SavePerson((int)Session["familyid"]);
                EmailUser(m);
                return RedirectToAction("Confirm");
            }
            return View(m);
        }
        public ActionResult Confirm()
        {
            return View();
        }
        public ActionResult Login(string name, string password, string email)
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
                if (password == ConfigurationManager.AppSettings["regpassword"])
                {
                    Session["name"] = name;
                    Session["email"] = email;
                    Session["auth"] = "true";
                    return RedirectToAction("Form");
                }
            System.Threading.Thread.Sleep(20000);
            ModelState.AddModelError("auth", "incorrect password");
            return View();
        }
        private void EmailUser(RegisterModel m)
        {
            HomeController.Email(ConfigurationManager.AppSettings["regmail"],
                                (string)Session["name"], (string)Session["email"], "Church Registration",
@"<p>Thank you for helping us build our Church Database.</p>
<p>We have the following information:
<pre>
{0}
</pre>".Fmt(m.PrepareSummaryText()));
        }
    }
}
