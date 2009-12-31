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
using CMSWeb;
using CMSWeb.Models;
using CmsData;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace CMSWeb.Controllers
{
    public class VolunteersController : CMSWebCommon.Controllers.CmsController
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
        public ActionResult UpdateAll(int id, string view)
        {
            var orgkeys = Person.OrgKeys(view);
            var Qb = DbUtil.Db.LoadQueryById(id);
            var q = DbUtil.Db.People.Where(Qb.Predicate());
            q = from p in q
                where p.VolInterestInterestCodes.Count(c => orgkeys.Contains(c.VolInterestCode.Org)) > 0
                select p;
            foreach (var person in q)
            {
                var m = new Models.VolunteerModel { View = view, person = person };
                m.person.BuildVolInfoList(view); // gets existing
                m.person.BuildVolInfoList(view); // 2nd time updates existing
                m.person.RefreshCommitments(view);
            }
            return Content("done");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EmailReminders(int id)
        {
            var meeting = DbUtil.Db.Meetings.Single(m => m.MeetingId == id);

            var q = from a in meeting.Attends
                    where a.AttendanceFlag == true || a.Registered == true
                    select a.Person;
            var o = meeting.Organization;
            var sb = new StringBuilder(
@"{0} - {1:M/d/yy h:mm tt}<br/>
The following people have been sent a meeting notice:<br/>
<blockquote>
".Fmt(o.OrganizationName, meeting.MeetingDate));
            var smtp = new SmtpClient();
            var memail = DbUtil.Db.CurrentUser.Person.EmailAddress;
            foreach (var person in q)
            {
                var em = person.EmailAddress.Trim();
                if (!Util.ValidEmail(em))
                    continue;
                sb.AppendFormat("{0}</br>\n", person.Name);
                Util.Email(memail, person.Name, em,
                    "{0} Reminder".Fmt(o.OrganizationName),
@"<blockquote><table>
<tr><td>Time:</td><td>{0:dddd, M/d/yy h:mm tt}</td></tr>
<tr><td>Location:</td><td>{1}</td></tr>
</table></blockquote>".Fmt(meeting.MeetingDate, o.Location));
            }
            sb.Append("</blockquote>\n");

            Util.EmailHtml2(smtp, memail, memail,
                "Meeting Reminder emails sent", sb.ToString());

            return Content("done");
        }
    }
}
