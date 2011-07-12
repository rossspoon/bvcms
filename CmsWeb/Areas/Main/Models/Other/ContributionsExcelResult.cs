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
using CmsWeb.Areas.Main.Models.Report;

namespace CmsWeb.Models
{
    public class ContributionsExcelResult : ActionResult
    {
        public DateTime startdt { get; set; }
        public DateTime enddt { get; set; }
        public int qid { get; set; }
        public bool totals { get; set; }

        public ContributionsExcelResult(int qid, string start, string end, bool totals)
        {
            startdt = start.ToDate() ?? DateTime.Now.AddYears(-1);
            enddt = end.ToDate() ?? DateTime.Now;
            this.qid = qid;
            this.totals = totals;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            var dg = new DataGrid();
            string filename = null;
            if (totals)
            {

                filename = "ContributionTotals";
                dg.DataSource =
                    ExportPeople.ExcelContributionTotals(qid, startdt, enddt);
            }
            else
            {
                filename = "ContributionDetails";
                dg.DataSource =
                    ExportPeople.ExcelContributions(qid, startdt, enddt);
            }
            dg.DataBind();
            Response.AddHeader("Content-Disposition", "attachment;filename={0}.xls".Fmt(filename));
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}