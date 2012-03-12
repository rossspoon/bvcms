using System;
using System.Web.Mvc;
using UtilityExtensions;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CmsWeb.Models
{
    public class ContributionsExcelResult : ActionResult
    {
        public DateTime Dt1 { get; set; }
        public DateTime Dt2 { get; set; }
        public int fundid { get; set; }
        public bool totals { get; set; }
    	public int campusid { get; set; }
    	public bool pledges { get; set; }
    	public bool nontaxdeductible { get; set; }

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
                dg.DataSource = ExportPeople.ExcelContributionTotals(Dt1, Dt2);
            }
            else
            {
                filename = "ContributionDetails";
                dg.DataSource = ExportPeople.ExcelContributions(Dt1, Dt2, fundid, campusid, pledges, nontaxdeductible);
            }
            dg.DataBind();
            Response.AddHeader("Content-Disposition", "attachment;filename={0}.xls".Fmt(filename));
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}