using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using CmsData;
using CmsWeb.Models;
using UtilityExtensions;
using System.Configuration;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CmsWeb.Areas.Manage.Controllers;
using CmsWeb.Areas.OnlineReg.Models.Payments;

namespace CmsWeb.Areas.OnlineReg.Controllers
{
    public partial class OnlineRegController : CmsController
    {
        public class ConfirmTestInfo
        {
            public ExtraDatum ed;
            public OnlineRegModel m;
        }
        public class TransactionTestInfo
        {
            public ExtraDatum ed;
            public TransactionInfo ti;
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmTest(int? id, int? count)
        {
            IEnumerable<ExtraDatum> q;
            if (id.HasValue)
                q = DbUtil.Db.ExtraDatas.Where(e => e.Id == id);
            else
                q = from ed in DbUtil.Db.ExtraDatas
                    where ed.Data.StartsWith("<OnlineRegModel ")
                    orderby ed.Stamp descending
                    select ed;
            var list = q.Take(count ?? 20).ToList();
            var q2 = from ed in list
                     let s = ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models")
                     select new ConfirmTestInfo
                     {
                         ed = ed,
                         m = Util.DeSerialize<OnlineRegModel>(s) as OnlineRegModel
                     };
            return View(q2);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult TransactionTest(int? id, int? count)
        {
            var q = from ed in DbUtil.Db.ExtraDatas
                    where ed.Data.StartsWith("<TransactionInfo ")
                    where ed.Id >= id
                    orderby ed.Stamp descending
                    select ed;
            var list = q.Take(count ?? 1000).ToList();
            var q2 = from ed in list
                     let s = ed.Data.Replace("CMSWeb.Models", "CmsWeb.Models")
                     select new TransactionTestInfo
                     {
                         ed = ed,
                         ti = Util.DeSerialize<TransactionInfo>(s) as TransactionInfo
                     };
            return View(q2);
        }

    }
}
