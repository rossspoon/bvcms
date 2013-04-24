using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace CmsWeb.Models
{
    public class StatusFlagsExcelResult : ActionResult
    {
        private int qid;
        public StatusFlagsExcelResult(int qid)
        {
            this.qid = qid;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;

            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=CMSStatusFlags.xls");
            Response.Charset = "";

            var tag = DbUtil.Db.PopulateSpecialTag(qid, DbUtil.TagTypeId_StatusFlags);

            var roles = CMSRoleProvider.provider.GetRolesForUser(Util.UserName);

            var cmd = new SqlCommand("dbo.StatusGrid @p1");
			cmd.Parameters.AddWithValue("@p1", tag.Id);
            cmd.Connection = new SqlConnection(Util.ConnectionString);
            cmd.Connection.Open();

            var dg = new DataGrid();
            dg.DataSource = cmd.ExecuteReader();
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}