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
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DisplayController : CmsController
    {
        public ActionResult Index()
        {
            var q = from c in DbUtil.Db.Contents
                    orderby c.Name
                    select c;
            return View(q);
        }
        public ActionResult Page(string id)
        {
            if (!id.HasValue())
                id = "Home";
            Content content = null;
            if (id == "Recent")
                content = DbUtil.Db.Contents.OrderByDescending(c => c.DateCreated).First();
            else
                content = DbUtil.Db.Contents.SingleOrDefault(c => c.Name == id);
            ViewData["title"] = id;
            if (content != null)
            {
                ViewData["html"] = content.Body;
                ViewData["title"] = content.Title;
            }
            ViewData["page"] = id;
            return View();
        }
        public ActionResult EditPage(string id, bool? ishtml)
        {
            var content = DbUtil.Content(id);
            if (content != null)
            {
                ViewData["html"] = content.Body;
                ViewData["title"] = content.Title;
            }
            ViewData["id"] = id;
            ViewData["ishtml"] = ishtml ?? true;
            return View();
        }
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePage(string id, string title, string html)
        {
            var content = DbUtil.Content(id);
            if (content == null)
            {
                content = new CmsData.Content { Name = id };
                DbUtil.Db.Contents.InsertOnSubmit(content);
            }
            content.Body = html;
            content.Title = title;
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeletePage(string id)
        {
            var content = DbUtil.Content(id);
            DbUtil.Db.Contents.DeleteOnSubmit(content);
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index", "Display");
        }
        public ActionResult OrgContent(int id, string what)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            switch (what)
            {
                case "message":
                    ViewData["html"] = org.EmailMessage;
                    ViewData["title"] = org.EmailSubject;
                    break;
                case "instructions":
                    ViewData["html"] = org.Instructions;
                    ViewData["title"] = "Instructions";
                    break;
            }
            ViewData["id"] = id;
            return View();
        }
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateOrgContent(int id, string what, string title, string html)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            switch (what)
            {
                case "message":
                    org.EmailMessage = html;
                    org.EmailSubject = title;
                    break;
                case "instructions":
                    org.Instructions = html;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return Redirect("/Organization.aspx?id=" + id);
        }
        public ActionResult LeagueContent(int id)
        {
            var league = DbUtil.Db.RecLeagues.Single(rl => rl.DivId == id);
            ViewData["html"] = league.EmailMessage;
            ViewData["title"] = league.EmailSubject;
            ViewData["id"] = id;
            return View();
        }
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateLeagueContent(int id, string title, string html)
        {
            var league = DbUtil.Db.RecLeagues.Single(rl => rl.DivId == id);
            league.EmailMessage = html;
            league.EmailSubject = title;
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setup/Recreation/");
        }
    }
}
