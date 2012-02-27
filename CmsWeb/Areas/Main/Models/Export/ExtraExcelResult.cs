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
using System.Linq.Dynamic;
using CmsWeb.Areas.Main.Controllers;
using CmsWeb.Areas.Manage.Controllers;
using System.Text;
using System.Data.SqlClient;

namespace CmsWeb.Models
{
    public class ExtraValueExcelResult : ActionResult
    {
        private int qid;
        public ExtraValueExcelResult(int qid)
        {
            this.qid = qid;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;

            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.xls");
            Response.Charset = "";

            var name = "ExtraExcelResult " + DateTime.Now;
            var tag = DbUtil.Db.PopulateSpecialTag(qid, DbUtil.TagTypeId_ExtraValues);

            var cmd = new SqlCommand("dbo.ExtraValues {0}, ''".Fmt(tag.Id));
            cmd.Connection = new SqlConnection(Util.ConnectionString);
            cmd.Connection.Open();

            var dg = new DataGrid();
            dg.DataSource = cmd.ExecuteReader();
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}