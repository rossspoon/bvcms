using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;
using UtilityExtensions;
using CmsData;

namespace CmsWeb.Areas.Dialog.Controllers
{
    public class SearchUsersController : Controller
    {
        [HttpGet]
        public ActionResult Index(bool? singlemode, bool? ordered, int? topid)
        {
            Response.NoCache();
            var m = new SearchUsersModel 
            { 
                singlemode = singlemode ?? false, 
                ordered = ordered ?? false, 
                topid = topid 
            };
            return View(m);
        }
        [HttpPost]
        public ActionResult Results(SearchUsersModel m)
        {
            return View(m);
        }
        [HttpPost]
        public ActionResult MoveToTop(SearchUsersModel m)
        {
            return View("Results", m);
        }
        [HttpPost]
        public ActionResult TagUntag(int id, bool ischecked)
        {
            var t = DbUtil.Db.FetchOrCreateTag(Util.SessionId, Util.UserPeopleId, DbUtil.TagTypeId_AddSelected);
            if (ischecked)
            {
                var tp = DbUtil.Db.TagPeople.SingleOrDefault(tt => tt.PeopleId == id && tt.Id == t.Id);
                DbUtil.Db.TagPeople.DeleteOnSubmit(tp);
            }
            else
                t.PersonTags.Add(new TagPerson { PeopleId = id });
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
    }
}
