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
using CmsWeb.Models;
using System.Data.SqlClient;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles = "Admin")]
    [ValidateInput(false)]
    public class DisplayController : CmsStaffController
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
        public ActionResult File(string id)
        {
            var content = DbUtil.Content(id);
            if (content != null)
                ViewData["text"] = content.Body;
            ViewData["file"] = id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateFile(string file, string text)
        {
            var content = DbUtil.Content(file);
            if (content == null)
            {
                content = new CmsData.Content { Name = file, TextOnly = true };
                DbUtil.Db.Contents.InsertOnSubmit(content);
            }
            content.Body = text;
            content.Title = file;
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditPage(string id)
        {
            var content = DbUtil.Content(id);
            if (content != null)
            {
                ViewData["html"] = content.Body;
                ViewData["title"] = content.Title;
            }
            ViewData["page"] = id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePage(string page, string title, string html)
        {
            var content = DbUtil.Content(page);
            if (content == null)
            {
                content = new CmsData.Content { Name = page };
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
        public ActionResult OrgContent(int id, string what, bool? div)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);
            if (div == true && org.Division == null)
                return Content("no main division");

            switch (what)
            {
                case "message":
                    if (div == true)
                    {
                        ViewData["html"] = org.Division.EmailMessage;
                        ViewData["title"] = org.Division.EmailSubject;
                    }
                    break;
                case "instructions":
                    if (div == true)
                        ViewData["html"] = org.Division.Instructions;
                    ViewData["title"] = "Instructions";
                    break;
                case "terms":
                    if (div == true)
                        ViewData["html"] = org.Division.Terms;
                    ViewData["title"] = "Terms";
                    break;
            }
            ViewData["id"] = id;
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateOrgContent(int id, bool? div, string what, string title, string html)
        {
            var org = DbUtil.Db.LoadOrganizationById(id);

            switch (what)
            {
                case "message":
                    if (div == true)
                    {
                        org.Division.EmailMessage = html;
                        org.Division.EmailSubject = title;
                    }
                    break;
                case "instructions":
                    if (div == true)
                        org.Division.Instructions = html;
                    break;
                case "terms":
                    if (div == true)
                        org.Division.Terms = html;
                    break;
            }
            DbUtil.Db.SubmitChanges();
            return Redirect("/Organization/Index/" + id);
        }
    }
}
