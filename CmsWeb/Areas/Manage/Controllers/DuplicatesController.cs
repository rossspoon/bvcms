using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using UtilityExtensions;
using System.Text;
using CmsData;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using CmsWeb.Models;
using CMSPresenter;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using Alias = System.Threading.Tasks;

namespace CmsWeb.Areas.Manage.Controllers
{
    [Authorize(Roles="Admin")]
    public class DuplicatesController : CmsStaffController
    {
        public class DuplicateInfo
        {
            public Duplicate d {get; set;}
            public string name {get; set;}
        }
        public ActionResult Index()
        {
            var q = from d in DbUtil.Db.Duplicates
                    where DbUtil.Db.People.Any(pp => pp.PeopleId == d.Id1)
                    where DbUtil.Db.People.Any(pp => pp.PeopleId == d.Id2)
                    let name = DbUtil.Db.People.SingleOrDefault(pp => pp.PeopleId == d.Id1).Name
                    select new DuplicateInfo { d = d, name = name };
            return View(q);
        }

        class LongRunningStatus
        {
            private static object lockobject = new object();
            private static Dictionary<string, int> Status { get; set; }
            public LongRunningStatus()
            {
                if (Status == null)
                    Status = new Dictionary<string, int>();
            }
            public void SetStatus(string id, int i)
            {
                lock (lockobject)
                    Status[id] = i;
            }
            public int GetStatus(string id)
            {
                lock (lockobject)
                    if (Status.Keys.Count(i => i == id) == 1)
                        return Status[id];
                return 0;
            }
            public void RemoveStatus(string id)
            {
                lock (lockobject)
                    Status.Remove(id);
            }
        }
        [HttpGet]
        public ActionResult Find()
        {
            string host = Util.Host;
            var i = 0;
            var id = "test";
            Alias.Task.Factory.StartNew(() =>
            {
                var e = new LongRunningStatus();
                var Db = new CMSDataContext(Util.GetConnectionString(host));
                Db.ExecuteCommand("delete duplicate");
                foreach(var p in Db.People.Select(pp => pp.PeopleId))
                {
                    e.SetStatus(id, i);
                    i++;
                    var pids = Db.FindPerson4(p);
                    if (pids.Count() == 0)
                        continue;
                    foreach(var pid in pids)
                        Db.InsertDuplicate(p, pid.PeopleId.Value);
                }
                e.RemoveStatus(id);
            });
            return Redirect("/Manage/Duplicates/FindProgress");
        }
        [HttpGet]
        public ActionResult FindProgress()
        {
            var e = new LongRunningStatus();
            ViewData["count"] = e.GetStatus("test").ToString();
            return View();
        }
    }
}
