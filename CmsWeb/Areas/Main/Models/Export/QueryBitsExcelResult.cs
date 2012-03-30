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

            var q = DbUtil.Db.QueryBitsFlags();
            var s = string.Join(",", q.Select(a => "{0} as {1}".Fmt(
            		a[0], a[1].Replace('.', '_').Replace('-','_').Replace(' ','_'))));
            if (!s.HasValue())
            {
                Response.Write("no querybit queries defined for this database");
                return;
            }

            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=CMSOrganizations.xls");
            Response.Charset = "";

			var q2 = from p in DbUtil.Db.PeopleQuery(qid)
                where p.Attends.Count(aa => aa.AttendanceFlag == true) > 0
				select new
				{
                    p,
                    Age = p.Age.ToString(),
                    MaritalStatus = p.MaritalStatus.Description,
                    FirstAttend = DbUtil.Db.FirstMeetingDateLastLear(p.PeopleId),
					F01 = p.Tags.Any(tt => tt.Tag.Name == "F01" && tt.Tag.TypeId == 100) ? "X" : "",
					F02 = p.Tags.Any(tt => tt.Tag.Name == "F02" && tt.Tag.TypeId == 100) ? "X" : "",
					F03 = p.Tags.Any(tt => tt.Tag.Name == "F03" && tt.Tag.TypeId == 100) ? "X" : "",
					F04 = p.Tags.Any(tt => tt.Tag.Name == "F04" && tt.Tag.TypeId == 100) ? "X" : "",
					F05 = p.Tags.Any(tt => tt.Tag.Name == "F05" && tt.Tag.TypeId == 100) ? "X" : "",
					F06 = p.Tags.Any(tt => tt.Tag.Name == "F06" && tt.Tag.TypeId == 100) ? "X" : "",
					F07 = p.Tags.Any(tt => tt.Tag.Name == "F07" && tt.Tag.TypeId == 100) ? "X" : "",
					F08 = p.Tags.Any(tt => tt.Tag.Name == "F08" && tt.Tag.TypeId == 100) ? "X" : "",
					F09 = p.Tags.Any(tt => tt.Tag.Name == "F09" && tt.Tag.TypeId == 100) ? "X" : "",
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
					F21 = p.Tags.Any(tt => tt.Tag.Name == "F21" && tt.Tag.TypeId == 100) ? "X" : "",
					F22 = p.Tags.Any(tt => tt.Tag.Name == "F22" && tt.Tag.TypeId == 100) ? "X" : "",
					F23 = p.Tags.Any(tt => tt.Tag.Name == "F23" && tt.Tag.TypeId == 100) ? "X" : "",
					F24 = p.Tags.Any(tt => tt.Tag.Name == "F24" && tt.Tag.TypeId == 100) ? "X" : "",
					F25 = p.Tags.Any(tt => tt.Tag.Name == "F25" && tt.Tag.TypeId == 100) ? "X" : "",
					F26 = p.Tags.Any(tt => tt.Tag.Name == "F26" && tt.Tag.TypeId == 100) ? "X" : "",
					F27 = p.Tags.Any(tt => tt.Tag.Name == "F27" && tt.Tag.TypeId == 100) ? "X" : "",
					F28 = p.Tags.Any(tt => tt.Tag.Name == "F28" && tt.Tag.TypeId == 100) ? "X" : "",
					F29 = p.Tags.Any(tt => tt.Tag.Name == "F29" && tt.Tag.TypeId == 100) ? "X" : "",
				};
            var q3 = q2.Select("new(p.PeopleId,p.PreferredName,p.LastName,Age,MaritalStatus,FirstAttend,{0})".Fmt(s));
            var dg = new DataGrid();
            dg.DataSource = q3;
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}