using System;
using System.Collections.Generic;
using System.Xml;
using System.Web.Mvc;
using System.Xml.Linq;
using UtilityExtensions;
using System.Linq;
using CmsData;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Data.Common;
using CMSPresenter;
using System.IO;
using NPOI.HSSF.UserModel;
using System.Text;

namespace CmsWeb.Models
{
    public class MucketyMapResult : ActionResult
    {
        int queryid;
        public MucketyMapResult(int QueryId)
        {
            this.queryid = QueryId;
        }
        private static void ExecuteResultExtracted(NPOI.SS.UserModel.Sheet sheet, int r, MucketyItem p)
        {
            var row = sheet.CreateRow(r);
            row.CreateCell(0).SetCellValue(p.actor1);
            row.CreateCell(1).SetCellValue(p.actor1type);
            row.CreateCell(2).SetCellValue(p.actor1deceased);
            row.CreateCell(3).SetCellValue(p.actor2);
            row.CreateCell(4).SetCellValue(p.actor2type);
            row.CreateCell(5).SetCellValue(p.actor2deceased);
            row.CreateCell(6).SetCellValue(p.relation);
            row.CreateCell(7).SetCellValue(p.former);
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var fs = new FileStream(context.HttpContext.Server.MapPath(
                "/Content/MucketyTemplate.xls"), FileMode.Open, FileAccess.Read);
            var wb = new HSSFWorkbook(fs, true);
            var sheet = wb.GetSheet("Sheet1");
            var r = 1;
            foreach (var p in FetchMucketyMapOrgs())
            {
                ExecuteResultExtracted(sheet, r, p);
                r++;
            }
            foreach (var p in FetchMucketyMapFamily())
            {
                ExecuteResultExtracted(sheet, r, p);
                r++;
            }
            var Response = context.HttpContext.Response;
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=muckety.xls");
            Response.Charset = "";
            wb.Write(Response.OutputStream);
        }
        public IEnumerable<MucketyItem> FetchMucketyMapOrgs()
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(queryid);
            var q = from p in query
                    from om in p.OrganizationMembers
                    select new MucketyItem
                    {
                        actor1 = p.Name,
                        actor1type = 1,
                        actor1deceased = p.DeceasedDate == null ? 0 : 1,
                        actor2 = om.Organization.OrganizationName,
                        actor2type = 0,
                        actor2deceased = 0,
                        relation = "member",
                        former = 0
                    };
            return q;
        }
        public IEnumerable<MucketyItem> FetchMucketyMapFamily()
        {
            var Db = DbUtil.Db;
            var query = Db.PeopleQuery(queryid);
            var q = from p in query
                     from fm in Db.People.Where(pp => pp.FamilyId == p.FamilyId && p.PeopleId != pp.PeopleId)
                     select new MucketyItem
                     {
                         actor1 = p.Name,
                         actor1type = 1,
                         actor1deceased = p.DeceasedDate == null ? 0 : 1,
                         actor2 = fm.Name,
                         actor2type = 1,
                         actor2deceased = p.DeceasedDate == null ? 0 : 1,
                         relation = "family",
                         former = 0
                     };
            return q;
        }
        public class MucketyItem
        {
            public string actor1 { get; set; }
            public int actor1type { get; set; } // 1=person, 0=non-person
            public int actor1deceased { get; set; } // 1=deceased, 0=living
            public string actor2 { get; set; } // 
            public int actor2type { get; set; } // 1=person, 0=non-person
            public int actor2deceased { get; set; } // 1=deceased, 0=living
            public string relation { get; set; }
            public int former { get; set; } // 1=former, 0=current
        };
    }
}