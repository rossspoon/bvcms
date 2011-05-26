using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UtilityExtensions;
using System.Collections;
using CmsData;
using System.IO;
using CmsWeb.Models;

namespace CmsWeb
{
    public partial class ExportExcel1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var labelNameFormat = Request.QueryString["format"];
            int? qid = Request.QueryString["id"].ToInt2();
            var r = Response;
            r.Clear();
            var useweb = Request.QueryString["web"];

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
            r.Charset = "";

            if (!qid.HasValue && labelNameFormat != "Groups")
            {
                r.Write("no queryid");
                r.Flush();
                r.End();
            }
            if (useweb != "true")
            {
                r.ContentType = "application/vnd.ms-excel";
                r.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.xls");
            }
            r.Write(header);
            var ctl = new CMSPresenter.MailingController();
            var useTitles = Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";
            var dg = new DataGrid();
            dg.EnableViewState = false;
            switch (labelNameFormat)
            {
                case "Individual":
                    dg.DataSource = ExportPeople.FetchExcelList(qid.Value, maxExcelRows);
                    break;
                case "IndividualPicture":
                    GridView1.EnableViewState = false;
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = ExportPeople.FetchExcelListPics(qid.Value, maxExcelRows);
                    break;
                case "Library":
                    dg.DataSource = ExportPeople.FetchExcelLibraryList(qid.Value);
                    break;
                case "Family":
                    dg.DataSource = ctl.FetchExcelFamily(qid.Value, maxExcelRows);
                    break;
                case "FamilyMembers":
                    dg.DataSource = ExportPeople.FetchExcelListFamilyMembers(qid.Value);
                    break;
                case "AllFamily":
                    dg.DataSource = ExportPeople.FetchExcelListFamily(qid.Value);
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
                    dg.DataSource = ExportInvolvements.InvolvementList(qid.Value);
                    break;
                case "Children":
                    dg.DataSource = ExportInvolvements.ChildrenList(qid.Value, maxExcelRows);
                    break;
                case "Church":
                    dg.DataSource = ExportInvolvements.ChurchList(qid.Value, maxExcelRows);
                    break;
                case "Attend":
                    dg.DataSource = ExportInvolvements.AttendList(qid.Value, maxExcelRows);
                    break;
                case "Organization":
                    dg.DataSource = ExportInvolvements.OrgMemberList(qid.Value, maxExcelRows);
                    break;
                case "Groups":
                    dg.DataSource = ExportInvolvements.OrgMemberListGroups();
                    break;
                case "Promotion":
                    dg.DataSource = ExportInvolvements.PromoList(qid.Value, maxExcelRows);
                    break;
            }
            if (labelNameFormat == "IndividualPicture")
            {
                GridView1.DataBind();
                GridView1.RenderControl(new HtmlTextWriter(r.Output));
            }
            else
            {
                dg.DataBind();
                dg.RenderControl(new HtmlTextWriter(r.Output));
            }
            r.Write("</body></HTML>");
            r.Flush();
            r.End();
        }
        private static int maxExcelRows
        {
            get { return DbUtil.Db.Setting("MaxExcelRows", "10000").ToInt(); }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}
