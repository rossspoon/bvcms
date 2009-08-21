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
        
        public ActionResult Inside()
        {
            Session["auth"] = "true";
            return RedirectToAction("Index");
        }
        public ActionResult Index()
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
                if (!p.EmailAddress.HasValue())
                    p.EmailAddress = (string)Session["email"];

                Session["familyid"] = p.FamilyId;
                Session["lastname"] = p.LastName;
                Session["name"] = p.Name;
                if (string.IsNullOrEmpty((string)Session["email"])) 
                    Session["email"] = p.EmailAddress;
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
        private void EmailUser(RegisterModel m)
        {
            HomeController.Email(DbUtil.Settings("RegMail"),
                                (string)Session["name"], (string)Session["email"], "Church Registration",
@"<p>Thank you for helping us build our Church Database.</p>
<p>We have the following information:
<pre>
{0}
</pre>".Fmt(m.PrepareSummaryText()));
        }
    }
}
