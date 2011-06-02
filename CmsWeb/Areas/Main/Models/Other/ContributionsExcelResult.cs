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

namespace CmsWeb.Models
{
    public class ContributionsExcelResult : ActionResult
    {
        public DateTime? startdt { get; set; }
        public DateTime? enddt { get; set; }
        public Tag tag { get; set; }

        public ContributionsExcelResult(string tagname, string start, string end)
        {
            tag = DbUtil.Db.FetchOrCreateTag(tagname, Util.UserPeopleId, DbUtil.TagTypeId_Personal);
            startdt = start.ToDate();
            enddt = end.ToDate();
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=CMSContributions.xls");
            Response.Charset = "";

            var people = tag.People(DbUtil.Db);

            var q = from c in DbUtil.Db.Contributions
                    where people.Any(pp => pp.PeopleId == c.PeopleId)
                    where c.ContributionDate >= startdt || startdt == null
                    select c;

            var edt = enddt;
            if (!edt.HasValue && startdt.HasValue)
                edt = startdt.Value.AddHours(24);
            if (edt.HasValue)
                q = q.Where(c => c.ContributionDate < edt);

            var q2 = from c in q
                     select new
                     {
                         PeopleId = c.PeopleId.Value,
                         Age = c.Person.Age ?? 0,
                         Position = c.Person.FamilyPosition.Description,
                         Date = c.ContributionDate.Value,
                         Amount = c.ContributionAmount.Value,
                         c.ContributionFund.FundId
                     };
            var dg = new DataGrid();
            dg.DataSource = q2;
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}