/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using UtilityExtensions;
using System.Web.Routing;
using CmsWeb;
using CmsWeb.Models;
using CmsData;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class VolunteersController : CmsStaffController
    {
        public VolunteersController()
        {
            ViewData["Title"] = "Volunteers";
        }
        public ActionResult Index(int? id)
        {
            var vols = new VolunteersModel { QueryId = id };
            Session["continuelink"] = Request.Url;
            UpdateModel(vols);
            if (!vols.View.HasValue())
                vols.View = "ns";
            DbUtil.LogActivity("Volunteers");
            return View(vols);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Codes(string id)
        {
            var q = from p in DbUtil.Db.VolInterestInterestCodes
                    where p.VolInterestCode.Org == id
                    select new
                    {
                        Key = p.VolInterestCode.Org + p.VolInterestCode.Code,
                        PeopleId = "p" + p.PeopleId,
                        Name = p.Person.Name
                    };
            return Json(q);
        }
        public ActionResult Calendar(int id)
        {
            var m = new VolunteerCommitmentsModel(id);
            if (m.times == null)
                return Content("no future meetings available");
            return View(m);
        }

        public ActionResult CustomReport(string id)
        {
            ViewData["content"] = DbUtil.Content("Volunteer-{0}.report".Fmt(id)).Body;
            return View();
        }
        public ActionResult Query(int id)
        {
            var vols = new VolunteersModel { QueryId = id };
            UpdateModel(vols);
            var qb = DbUtil.Db.QueryBuilderClauses.FirstOrDefault(c => c.QueryId == id).Clone();
            var comp = CompareType.Equal;
            if (vols.Org == "na")
                comp = CompareType.NotEqual;
            var clause = qb.AddNewClause(QueryType.HasVolunteered, comp, "1,T");
            clause.Quarters = vols.View;

            DbUtil.Db.QueryBuilderClauses.InsertOnSubmit(qb);
            DbUtil.Db.SubmitChanges();
            return Redirect("/QueryBuilder/Main/{0}".Fmt(qb.QueryId));
        }
        public ActionResult EmailReminders(int id)
        {
            var qb = DbUtil.Db.QueryBuilderScratchPad();
            qb.CleanSlate();
            var clause = qb.AddNewClause(QueryType.MeetingId, CompareType.Equal, id.ToString());
            DbUtil.Db.SubmitChanges();

            var meeting = DbUtil.Db.Meetings.Single(m => m.MeetingId == id);
            var subject = "{0} Reminder".Fmt(meeting.Organization.OrganizationName);
            var body = 
@"<blockquote><table>
<tr><td>Time:</td><td>{0:dddd, M/d/yy h:mm tt}</td></tr>
<tr><td>Location:</td><td>{1}</td></tr>
</table></blockquote><p>{2}</p>".Fmt(
                                meeting.MeetingDate,
                                meeting.Organization.Location,
                                meeting.Organization.LeaderName);

            return Redirect("/EmailPeople.aspx?id={0}&subj={1}&body={2}&ishtml=true"
                .Fmt(qb.QueryId, Server.UrlEncode(subject), Server.UrlEncode(body)));
        }
        public ActionResult UpdateAll(string id, int? qid)
        {
            var orgkeys = Person.OrgKeys(id);
            var q = DbUtil.Db.People.AsQueryable();
            if (qid.HasValue)
                q = DbUtil.Db.PeopleQuery(qid.Value);
            q = from p in q
                where p.VolInterestInterestCodes.Count(c => orgkeys.Contains(c.VolInterestCode.Org)) > 0
                select p;
            foreach (var person in q)
            {
                var m = new CmsWeb.Models
                    .VolunteerModel { View = id, person = person };
                m.person.BuildVolInfoList(id); // gets existing
                m.person.BuildVolInfoList(id); // 2nd time updates existing
                m.person.RefreshCommitments(id);
            }
            return Content("done");
        }
    }
}
