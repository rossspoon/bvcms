using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsData;
using UtilityExtensions;

namespace CmsWeb.Areas.Manage.Controllers
{
    public class ExtraValuesController : CmsStaffController
    {
        public ActionResult Add(int id, string field, string value)
        {
            var list = DbUtil.Db.PeopleQuery(id).Select(pp => pp.PeopleId).ToList();
            foreach (var pid in list)
            {
                AddEditExtraValue(pid, field, value);
                DbUtil.Db.SubmitChanges();
                DbUtil.DbDispose();
            }
            return Content("done");
        }
        public ActionResult Delete(int id, string field, string value)
        {
            var list = DbUtil.Db.PeopleQuery(id).Select(pp => pp.PeopleId).ToList();
            foreach (var pid in list)
            {
                var ev = GetExtraValue(pid, field, value);
                if (ev == null)
                    continue;
                DbUtil.Db.PeopleExtras.DeleteOnSubmit(ev);
                DbUtil.Db.SubmitChanges();
                DbUtil.DbDispose();
            }
            return Content("done");
        }
		[HttpPost]
		[Authorize(Roles="Admin")]
        public ActionResult DeleteAll(string field, string val)
        {
        	DbUtil.Db.ExecuteCommand("delete PeopleExtra where field = {0} and StrValue = {1}", field, val);
            return Content("done");
        }
        public PeopleExtra GetExtraValue(int id, string field)
        {
            var q = from v in DbUtil.Db.PeopleExtras
                    where v.Field == field
                    where v.PeopleId == id
                    select v;
            var ev = q.SingleOrDefault();
            if (ev == null)
            {
                ev = new PeopleExtra
                {
                    PeopleId = id,
                    Field = field,
                    TransactionTime = DateTime.Now
                };
                DbUtil.Db.PeopleExtras.InsertOnSubmit(ev);
            }
            return ev;
        }
        public PeopleExtra GetExtraValue(int id, string field, string value)
        {
            var novalue = !value.HasValue();
            var q = from v in DbUtil.Db.PeopleExtras
                    where v.PeopleId == id
                    where v.Field == field
                    where novalue || v.StrValue == value 
                    select v;
            var ev = q.SingleOrDefault();
            return ev;
        }
        public void AddEditExtraValue(int id, string field, string value)
        {
            if (!value.HasValue())
                return;
            var ev = GetExtraValue(id, field);
            ev.StrValue = value;
        }
    }
}
