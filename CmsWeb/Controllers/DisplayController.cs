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
using CMSWeb.Models;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.Controllers
{
    [HandleError]
    public class DisplayController : Controller
    {
        public ActionResult Menu()
        {
            var menu = DbUtil.Db.Contents.SingleOrDefault(m => m.Title == "menu");
            var c = new ContentResult();
            if (menu != null)
                c.Content = menu.Body;
            return c;
        }
        public ActionResult RsdLink()
        {
            var link = "<link rel=\"EditURI\" type=\"application/rsd+xml\" title=\"RSD\" href=\"http://"
                + HttpContext.Request.Url.Authority
                + "/rsdcontent.ashx\" />";
            return new ContentResult { Content = link };
        }
        public ActionResult Page(string page)
        {
            if (!page.HasValue())
                page = "Home";
            Content content = null;
            if (page == "Recent")
                content = DbUtil.Db.Contents.OrderByDescending(c => c.DateCreated).First();
            else
                content = DbUtil.Db.Contents.SingleOrDefault(c => c.Name == page);
            ViewData["title"] = page;
            if (content != null)
            {
                ViewData["html"] = content.Body;
                ViewData["title"] = content.Title;
            }
            ViewData["page"] = page;
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditPage(string page)
        {
            var content = DbUtil.Db.Contents.SingleOrDefault(c => c.Name == page);
            if (content != null)
            {
                ViewData["html"] = content.Body;
                ViewData["title"] = content.Title;
            }
            ViewData["page"] = page;
            return View();
        }
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePage(string page, string title, string html)
        {
            var content = DbUtil.Db.Contents.SingleOrDefault(c => c.Name == page);
            if (content == null)
            {
                content = new CmsData.Content { Name = page };
                DbUtil.Db.Contents.InsertOnSubmit(content);
            }
            content.Body = html;
            content.Title = title;
            DbUtil.Db.SubmitChanges();
            return RedirectToAction("Page", "Display", new { page = page });
        }
    }
}
