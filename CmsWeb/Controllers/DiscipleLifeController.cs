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
        public ActionResult Index(int id)
        {
            var m = new DiscipleLifeModel { divid = id };
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
            m.EnrollInOrg(m.person);

            HomeController.Email(DbUtil.Settings("DiscLifeMail"),
    "", m.email, "DiscipleLife Registration",
@"<p>Thank you for registering for {0}.</p>
".Fmt(m.organization.OrganizationName));

            HomeController.Email(m.email,
                    "", DbUtil.Settings("DiscLifeMail"), "{0} Registration".Fmt(m.division.Name),
@"{0}({1}) has registered for {2}: {3}</p>".Fmt(
m.person.Name, m.person.PeopleId, m.division.Name, m.organization.OrganizationName));

            return View("Confirm", m);
        }
    }
}
