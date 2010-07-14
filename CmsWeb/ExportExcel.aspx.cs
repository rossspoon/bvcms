using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMSPresenter;
using UtilityExtensions;
using System.Collections;
using CmsData;

namespace CmsWeb
{
    public partial class ExportExcel1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var r = Response;
            r.Clear();
            var useweb = Request.QueryString["web"];

            if (useweb != "true")
            {
                r.ContentType = "application/vnd.ms-excel";
                r.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.xls");
            }
            string header =
@"<html xmlns:x=""urn:schemas-microsoft-com:office:excel"">
<head>
    <style>
    <!--table
    br {mso-data-placement:same-cell;}
    tr {vertical-align:top;}
    -->
    </style>
</head>
<body>";
                r.Write(header);
                r.Charset = "";

            int? qid = Request.QueryString["id"].ToInt2();
            if (!qid.HasValue)
            {
                r.Write("no queryid");
                r.Flush();
                r.End();
            }
            var labelNameFormat = Request.QueryString["format"];
            var ctl = new MailingController();
            var useTitles = Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";
            var dg = new DataGrid();
            dg.EnableViewState = false;
            switch (labelNameFormat)
            {
                case "Individual":
                    dg.DataSource = PersonSearchController.FetchExcelList(qid.Value, maxExcelRows);
                    break;
                case "Family":
                    dg.DataSource = ctl.FetchExcelFamily(qid.Value, maxExcelRows);
                    break;
                case "ParentsOf":
                    dg.DataSource = ctl.FetchExcelParents(qid.Value, maxExcelRows);
                    break;
                case "CouplesEither":
                    dg.DataSource = ctl.FetchExcelCouplesEither(qid.Value, maxExcelRows);
                    break;
                case "CouplesBoth":
                    dg.DataSource = ctl.FetchExcelCouplesBoth(qid.Value, maxExcelRows);
                    break;
                case "Involvement":
                    dg.DataSource = InvolvementController.InvolvementList(qid.Value);
                    break;
                case "Children":
                    dg.DataSource = InvolvementController.ChildrenList(qid.Value, maxExcelRows);
                    break;
                case "Church":
                    dg.DataSource = InvolvementController.ChurchList(qid.Value, maxExcelRows);
                    break;
                case "Attend":
                    dg.DataSource = InvolvementController.AttendList(qid.Value, maxExcelRows);
                    break;
                case "Organization":
                    dg.DataSource = InvolvementController.OrgMemberList(qid.Value, maxExcelRows);
                    break;
                case "Promotion":
                    dg.DataSource = InvolvementController.PromoList(qid.Value, maxExcelRows);
                    break;
                case "SML":
                    dg.DataSource = InvolvementController.SoulmateList(qid.Value, maxExcelRows);
                    break;
            }
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(r.Output));
            r.Write("</body></HTML>");
        }
        private static int maxExcelRows
        {
            get { return DbUtil.Settings("MaxExcelRows", "10000").ToInt(); }
        }
    }
}
