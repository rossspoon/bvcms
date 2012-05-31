using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CmsWeb.Models;
using CmsData;
using UtilityExtensions;
using System.Linq.Dynamic;

namespace CmsWeb.Areas.Main.Controllers
{
    public class ExportController : CmsStaffController
    {
        public ActionResult UpdatePeople(int id)
        {
            return new UpdatePeopleModel(id);
        }
        public ActionResult MucketyMap(int id)
        {
            return new MucketyMapResult(id);
        }
        public ActionResult QueryBits(int id)
        {
            return new QueryBitsExcelResult(id);
        }
        public ActionResult ExtraValues(int id)
        {
            return new ExtraValueExcelResult(id);
        }
        [Authorize(Roles="Admin")]
        public ActionResult FreshBooks(int id)
        {
            return new FreshBooksResult(id);
        }
        [Authorize(Roles="Finance")]
        public ActionResult Contributions(ContributionsExcelResult m)
        {
        	return m;
        }
        public ActionResult RedeemerCampus()
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(Util.QueryBuilderScratchPadId);
            var q = from p in query
                    select new
                    {
                        PeopleId = p.PeopleId,
						Campus = p.Campu.Description,
						ContactId = p.GetExtra("ContactId"),
                    };
			return new DataGridResult(q, excel: true);
        }
    }
}
