using System;
using System.Linq;
using Microsoft.Reporting.WebForms;
using UtilityExtensions;
using CMSPresenter;
using System.Collections.Generic;
using CmsData;

namespace CMSWeb.Reports
{
    public partial class ProspectCardsRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Viewing Prospect Rpt");
            RenderReport();
            //var id = this.QueryString<int?>("id");
            //var orgid = this.QueryString<int?>("orgid");
            //ReportDataSource rd = null;
            //var ctl = new LabelController();
            //if (id.HasValue)
            //    rd = new ReportDataSource("LabelInfo", ctl.QueryLabelInfo(id.Value));
            //else if (orgid.HasValue)
            //    rd = new ReportDataSource("LabelInfo", ctl.OrgLabelInfo(orgid.Value));
            //ReportViewer1.LocalReport.DataSources.Add(rd);
        }

        private void localReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "AttendanceSummarySubRpt")
            {
                var ctl = new ProspectController();
                e.DataSources.Add(new ReportDataSource("ProspectAttendInfo", 
                    ctl.AttendHistory(e.Parameters[0].Values[0].ToInt()).ToList()));
            }
            else if (e.ReportPath == "PastContactsSummarySubRpt")
            {
                var ctl = new ContactController();
                e.DataSources.Add(new ReportDataSource("ContactInfo",
                    ctl.ContactList(e.Parameters[0].Values[0].ToInt()).ToList()));
            }
            else if (e.ReportPath == "CurrentEnrollmentSubRpt")
            {
                var ctl = new ProspectController();
                e.DataSources.Add(new ReportDataSource("OrganizationView",
                    ctl.EnrollData(e.Parameters[0].Values[0].ToInt()).ToList()));
            }
            else if (e.ReportPath == "FamilySummarySubRpt")
            {
                var ctl = new ProspectController();
                e.DataSources.Add(new ReportDataSource("FamilyMember",
                    ctl.FamilyMembers(e.Parameters[0].Values[0].ToInt()).ToList()));
            }
        }

        private void RenderReport()
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("./rdlc/ProspectCardsRpt.rdlc");
            //localReport.ReportEmbeddedResource = "CMSWeb.Reports.rdlc.ProspectCardsRpt.rdlc";

            var id = this.QueryString<int?>("id");

            ReportDataSource rd = null;
            var ctl = new ProspectController();
            rd = new ReportDataSource("ProspectInfo",ctl.ListByQuery(id.Value).ToList());
            localReport.DataSources.Add(rd);
            localReport.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);


            //localReport.DataSources.Add(reportDataSource2);
            //ReportParameter rp = new ReportParameter("Requestor", requestor, false);
            //ReportParameter[] rpc = {rp};
            //localReport.SetParameters(rpc);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo = " <DeviceInfo> " +
                                " <OutputFormat>PDF</OutputFormat>" +
                                " <StartPage>0</StartPage>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.50in</MarginTop>" +
            "  <MarginLeft>0.50in</MarginLeft>" +
            "  <MarginRight>0.50in</MarginRight>" +
            "  <MarginBottom>0.50in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            //Clear the response stream and write the bytes to the outputstream
            //Set content-disposition to "attachment" so that user is prompted to take an action
            //on the file (open or save)
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "filename=foo." + fileNameExtension);
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }


    }
}