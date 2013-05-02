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
	<meta http-equiv=Content-Type content=""text/html; charset=utf-8""> 
    <style>
    <!--table
    br {mso-data-placement:same-cell;}
    tr {vertical-align:top;}
    td.Text {mso-number-format:\@}
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
            var ctl = new MailingController();
            var useTitles = Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";
            var dg = new DataGrid();
            dg.EnableViewState = false;
            switch (labelNameFormat)
            {
                case "Individual":
                    dg.DataSource = ExportPeople.FetchExcelList(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "IndividualPicture":
                    GridView1.EnableViewState = false;
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = ExportPeople.FetchExcelListPics(qid.Value, maxExcelRows);
                    GridView1.DataBind();
                    GridView1.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Library":
                    dg.DataSource = ExportPeople.FetchExcelLibraryList(qid.Value);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Family":
                    dg.DataSource = ctl.FetchExcelFamily(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "FamilyMembers":
                    FamilyMembers.EnableViewState = false;
                    FamilyMembers.AllowPaging = false;
                    FamilyMembers.DataSource = ExportPeople.FetchExcelListFamilyMembers(qid.Value);
                    FamilyMembers.DataBind();
                    FamilyMembers.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "AllFamily":
                    dg.DataSource = ExportPeople.FetchExcelListFamily(qid.Value);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "ParentsOf":
                    dg.DataSource = ctl.FetchExcelParents(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "CouplesEither":
                    dg.DataSource = ctl.FetchExcelCouplesEither(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "CouplesBoth":
                    dg.DataSource = ctl.FetchExcelCouplesBoth(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Involvement":
                    dg.DataSource = ExportInvolvements.InvolvementList(qid.Value);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Children":
                    dg.DataSource = ExportInvolvements.ChildrenList(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Church":
                    dg.DataSource = ExportInvolvements.ChurchList(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Attend":
                    dg.DataSource = ExportInvolvements.AttendList(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Organization":
                    dg.DataSource = ExportInvolvements.OrgMemberList(qid.Value);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Groups":
                    dg.DataSource = ExportInvolvements.OrgMemberListGroups();
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
                case "Promotion":
                    dg.DataSource = ExportInvolvements.PromoList(qid.Value, maxExcelRows);
                    dg.DataBind();
                    dg.RenderControl(new HtmlTextWriter(r.Output));
                    break;
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
