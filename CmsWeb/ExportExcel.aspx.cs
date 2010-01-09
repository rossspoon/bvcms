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

namespace CMSWeb
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
            IEnumerable d = null;
            switch (labelNameFormat)
            {
                case "Individual":
                    d = PersonSearchController.FetchExcelList(qid.Value, maxExcelRows);
                    break;
                case "Family":
                    d = ctl.FetchExcelFamily(qid.Value, maxExcelRows);
                    break;
                case "ParentsOf":
                    d = ctl.FetchExcelParents(qid.Value, maxExcelRows);
                    break;
                case "CouplesEither":
                    d = ctl.FetchExcelCouplesEither(qid.Value, maxExcelRows);
                    break;
                case "CouplesBoth":
                    d = ctl.FetchExcelCouplesBoth(qid.Value, maxExcelRows);
                    break;
                case "Involvement":
                    d = InvolvementController.InvolvementList(qid.Value);
                    break;
                case "Children":
                    d = InvolvementController.ChildrenList(qid.Value, maxExcelRows);
                    break;
                case "Church":
                    d = InvolvementController.ChurchList(qid.Value, maxExcelRows);
                    break;
                case "Attend":
                    d = InvolvementController.AttendList(qid.Value, maxExcelRows);
                    break;
                case "Organization":
                    d = InvolvementController.OrgMemberList(qid.Value, maxExcelRows);
                    break;
                case "Promotion":
                    d = InvolvementController.PromoList(qid.Value, maxExcelRows);
                    break;
                case "SML":
                    d = InvolvementController.SoulmateList(qid.Value, maxExcelRows);
                    break;
                case "LR":
                    d = CMSWeb.Models.LoveRespectModel
                        .LoveRespectList(qid.Value, maxExcelRows);
                    break;
            }
            var dg = new DataGrid();
            dg.EnableViewState = false;
            dg.DataSource = d;
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
