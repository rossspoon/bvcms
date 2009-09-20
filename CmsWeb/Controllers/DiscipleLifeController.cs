using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CMSWeb.Models;
using UtilityExtensions;
using System.Configuration;

namespace CMSWeb.Controllers
{
    public class DiscipleLifeController : Controller
    {
        public ActionResult Index(int? id)
        {
            var m = new DiscipleLifeModel { divid = id };
            if (m.division == null)
                return Content("No division Found");
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
                        ModelState.AddModelError("find", "Cannot find a record.");
                        m.shownew = true;
                    }
            }
            if (!ModelState.IsValid)
                return View(m);
            if (m.person == null)
                m.AddPerson();
            m.EnrollInOrg(m.person);

            var c = DbUtil.Db.Contents.SingleOrDefault(ms => ms.Name == "DiscLifeMessage-" + id);
            if (c == null)
            {
                c = new Content();
                c.Body = "<p>Hi {first}</p>\n<p>Thank you for registering for {orgname}.</p>";
                c.Title = "DiscipleLife Registration";
            }
            c.Body = c.Body.Replace("{first}", m.person.NickName.HasValue() ? m.person.NickName : m.person.FirstName);
            c.Body = c.Body.Replace("{orgname}", m.organization.OrganizationName);

            string email = DbUtil.Settings("DiscLifeMail-" + id);
            if (!email.HasValue())
                email = DbUtil.Settings("DiscLifeMail");
            HomeController.Email(email, m.person.Name, m.email, c.Title, c.Body);
            HomeController.Email(m.email, "", email, "{0} Registration".Fmt(m.division.Name),            
                "{0}({1}) has registered for {2}: {3}</p>".Fmt(
                    m.person.Name, m.person.PeopleId, m.division.Name, m.organization.OrganizationName));

            return View("Confirm", m);
        }
    }
}
