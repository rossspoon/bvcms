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
    public class VolunteerController : Controller
    {
        public VolunteerController()
        {
            ViewData["header"] = DbUtil.Settings("VolHeader");
            ViewData["logoimg"] = DbUtil.Settings("VolLogo");
        }
        public ActionResult Index(int id)
        {
            var m = new Models.VolunteerModel { OpportunityId = id };
            if (Request.HttpMethod.ToUpper() == "GET")
                return View(m);

            UpdateModel(m);
            m.ValidateModel(ModelState);
            if (ModelState.IsValid)
            {
                var count = m.FindMember();
                if (count > 1)
                    ModelState.AddModelError("_FORM", "More than one match, sorry");
                else if (count == 0)
                    ModelState.AddModelError("_FORM", "Cannot find your church record");
                else
                {
                    if (m.person.MemberStatusId != 10)
                        ModelState.AddModelError("_FORM", "You must be a member of the church");
                    if (m.person.Age < 16)
                        ModelState.AddModelError("_FORM", "You must be a at least 16");
                }
            }
            if (!ModelState.IsValid)
                return View(m);
            var v = new VolInterest 
            { 
                Created = DateTime.Now, 
                PeopleId = m.person.PeopleId
            };
            m.person.EmailAddress = m.email;
            m.Opportunity.VolInterests.Add(v);
            foreach(var i in m.interests)
                v.VolInterestInterestCodes.Add(new VolInterestInterestCode { InterestCodeId = i.ToInt() });
            var cva = m.person.Volunteers.OrderByDescending(vo => vo.ProcessedDate).FirstOrDefault();
            DbUtil.Db.SubmitChanges();
            var em = new Emailer(m.Opportunity.Email);
            if (cva != null && cva.StatusId == 10)
                em.SendPersonEmail(m.person, m.Opportunity.Description, Util.SafeFormat(m.Opportunity.EmailYesCva));
            else
                em.SendPersonEmail(m.person, m.Opportunity.Description, Util.SafeFormat(m.Opportunity.EmailYesCva));

            return RedirectToAction("Confirm");
        }
        public ActionResult Confirm()
        {
            return View();
        }
    }
}
