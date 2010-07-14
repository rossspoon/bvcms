using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Setup.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MemberTypeController : CmsStaffController
    {
        public MemberTypeController()
        {

        }
        public ActionResult Index()
        {
            var m = DbUtil.Db.MemberTypes.AsEnumerable();
            return View(m);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int id)
        {
            var m = new MemberType { Id = id };
            DbUtil.Db.MemberTypes.InsertOnSubmit(m);
            DbUtil.Db.SubmitChanges();
            return Redirect("/Setup/MemberType/");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult Edit(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var mt = DbUtil.Db.MemberTypes.SingleOrDefault(m => m.Id == iid);
            if (id.StartsWith("v"))
                mt.Description = value;
            else if (id.StartsWith("c"))
                mt.Code = value;
            DbUtil.Db.SubmitChanges();
            var c = new ContentResult();
            c.Content = value;
            return c;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ContentResult EditAttendType(string id, string value)
        {
            var iid = id.Substring(1).ToInt();
            var mt = DbUtil.Db.MemberTypes.SingleOrDefault(m => m.Id == iid);
            mt.AttendanceTypeId = value.ToInt();
            DbUtil.Db.SubmitChanges();
            return Content(mt.AttendType.Description);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Delete(string id)
        {
            var iid = id.Substring(1).ToInt();
            var mt = DbUtil.Db.MemberTypes.SingleOrDefault(m => m.Id == iid);
            if (mt == null)
                return new EmptyResult();
            DbUtil.Db.MemberTypes.DeleteOnSubmit(mt);
            DbUtil.Db.SubmitChanges();
            return new EmptyResult();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AttendTypeCodes()
        {
            var q = from c in DbUtil.Db.AttendTypes
                    select new
                    {
                        Code = c.Id.ToString(),
                        Value = c.Description,
                    };
            return Json(q.ToDictionary(k => k.Code, v => v.Value));
        }
    }
}
