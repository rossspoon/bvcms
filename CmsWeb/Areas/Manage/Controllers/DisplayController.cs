/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;
using CmsWeb.Models;
using System.Data.SqlClient;
// Used for pulling image from service
using System.Net;

namespace CmsWeb.Areas.Manage.Controllers
{
	[Authorize(Roles = "Admin,Design")]
	[ValidateInput(false)]
	public class DisplayController : CmsStaffController
	{
		public const int TYPE_HTML = 0;
		public const int TYPE_TEXT = 1;
		public const int TYPE_EMAIL_TEMPLATE = 2;
		public const int TYPE_SAVED_DRAFT = 3;

		public ActionResult Index()
		{
			return View(new ContentModel());
		}

		public ActionResult ContentView(string id)
		{
			return View();
		}

		public ActionResult ContentEdit(int id)
		{
			var content = DbUtil.ContentFromID(id);
			return RedirectEdit(content);
		}

		[HttpPost]
		public ActionResult ContentCreate(int newType, string newName, int? newRole)
		{
			var content = new Content();
			content.Name = newName;
			content.TypeID = newType;
			content.RoleID = newRole ?? 0;
			content.Title = content.Body = "";

			DbUtil.Db.Contents.InsertOnSubmit(content);
			DbUtil.Db.SubmitChanges();

			return RedirectEdit(content);
		}

		[HttpPost]
		public ActionResult ContentUpdate(int id, string name, string title, string body, int? roleid)
		{
			var content = DbUtil.ContentFromID(id);
			content.Name = name;
			content.Title = title;
			content.Body = body;
			content.RoleID = roleid ?? 0;

            if (DbUtil.Db.Setting("RenderEmailTemplate", "false") == "true")
            {
                var coll = new NameValueCollection();
			    coll.Add("sHTML", body.Replace("\r", "").Replace("\n","").Replace("\t","") );

                var wc = new WebClient();
                var resp = wc.UploadValues("http://192.168.100.28:8080/Home/CreateWebsiteThumbnail", "POST", coll);

                if (content.ThumbID != 0) ImageData.Image.UpdateImageFromBits(content.ThumbID, resp);
                else content.ThumbID = ImageData.Image.NewImageFromBits(resp).Id;
                
                //if (content.ThumbID != 0) ImageData.Image.UpdateImageFromBits(content.ThumbID, CaptureWebPageBytes(body, 100, 150));
                //else content.ThumbID = ImageData.Image.NewImageFromBits(CaptureWebPageBytes(body, 100, 150)).Id;
            }

			DbUtil.Db.SubmitChanges();
			return RedirectToAction("Index");
		}

		public ActionResult ContentDelete(int id)
		{
			var content = DbUtil.ContentFromID(id);
			DbUtil.Db.Contents.DeleteOnSubmit(content);
			DbUtil.Db.SubmitChanges();
			return RedirectToAction("Index", "Display");
		}

		public ActionResult RedirectEdit(Content cContent)
		{
			switch (cContent.TypeID) // 0 = HTML, 1 = Text, 2 = eMail Template
			{
				case TYPE_HTML:
					return View("EditHTML", cContent);

				case TYPE_TEXT:
					return View("EditText", cContent);

				case TYPE_EMAIL_TEMPLATE:
				case TYPE_SAVED_DRAFT:
					return View("EditTemplate", cContent);
			}

			return View("Index");
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