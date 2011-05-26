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

namespace CmsWeb.Models
{
    public class QueryBitsExcelResult : ActionResult
    {
        private int qid;
        public QueryBitsExcelResult(int qid)
        {
            this.qid = qid;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=CMSOrganizations.xls");
            Response.Charset = "";

            var q = BatchController.QueryBitsFlags(DbUtil.Db);
            var s = string.Join(",", q.Select(a => "{0} as {1}".Fmt(
            		a[0], a[1].Replace('.', '_').Replace('-','_').Replace(' ','_'))));

			var q2 = from p in DbUtil.Db.PeopleQuery(qid)
				select new
				{
                    p,
					F1 = p.Tags.Any(tt => tt.Tag.Name == "F1" && tt.Tag.TypeId == 100) ? "X" : "",
					F2 = p.Tags.Any(tt => tt.Tag.Name == "F2" && tt.Tag.TypeId == 100) ? "X" : "",
					F3 = p.Tags.Any(tt => tt.Tag.Name == "F3" && tt.Tag.TypeId == 100) ? "X" : "",
					F4 = p.Tags.Any(tt => tt.Tag.Name == "F4" && tt.Tag.TypeId == 100) ? "X" : "",
					F5 = p.Tags.Any(tt => tt.Tag.Name == "F5" && tt.Tag.TypeId == 100) ? "X" : "",
					F6 = p.Tags.Any(tt => tt.Tag.Name == "F6" && tt.Tag.TypeId == 100) ? "X" : "",
					F7 = p.Tags.Any(tt => tt.Tag.Name == "F7" && tt.Tag.TypeId == 100) ? "X" : "",
					F8 = p.Tags.Any(tt => tt.Tag.Name == "F8" && tt.Tag.TypeId == 100) ? "X" : "",
					F9 = p.Tags.Any(tt => tt.Tag.Name == "F9" && tt.Tag.TypeId == 100) ? "X" : "",
					F10 = p.Tags.Any(tt => tt.Tag.Name == "F10" && tt.Tag.TypeId == 100) ? "X" : "",
					F11 = p.Tags.Any(tt => tt.Tag.Name == "F11" && tt.Tag.TypeId == 100) ? "X" : "",
					F12 = p.Tags.Any(tt => tt.Tag.Name == "F12" && tt.Tag.TypeId == 100) ? "X" : "",
					F13 = p.Tags.Any(tt => tt.Tag.Name == "F13" && tt.Tag.TypeId == 100) ? "X" : "",
					F14 = p.Tags.Any(tt => tt.Tag.Name == "F14" && tt.Tag.TypeId == 100) ? "X" : "",
					F15 = p.Tags.Any(tt => tt.Tag.Name == "F15" && tt.Tag.TypeId == 100) ? "X" : "",
					F16 = p.Tags.Any(tt => tt.Tag.Name == "F16" && tt.Tag.TypeId == 100) ? "X" : "",
					F17 = p.Tags.Any(tt => tt.Tag.Name == "F17" && tt.Tag.TypeId == 100) ? "X" : "",
					F18 = p.Tags.Any(tt => tt.Tag.Name == "F18" && tt.Tag.TypeId == 100) ? "X" : "",
					F19 = p.Tags.Any(tt => tt.Tag.Name == "F19" && tt.Tag.TypeId == 100) ? "X" : "",
					F20 = p.Tags.Any(tt => tt.Tag.Name == "F20" && tt.Tag.TypeId == 100) ? "X" : "",
				};
			var q3 = q2.Select("new(p.PeopleId,p.PreferredName,p.LastName,{0})".Fmt(s));
            var dg = new DataGrid();
            dg.DataSource = q3;
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}