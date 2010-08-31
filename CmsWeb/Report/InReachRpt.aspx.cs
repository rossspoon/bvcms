using System;
using System.Linq;
using Microsoft.Reporting.WebForms;
using UtilityExtensions;
using CMSPresenter;
using System.Collections.Generic;
using CmsData;

namespace CmsWeb
{
    public partial class InReachRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RenderReport();
        }
        private void localReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            var ctl = new ContactController();
            e.DataSources.Add(new ReportDataSource("ContactInfo", 
                ctl.ContactList(e.Parameters[0].Values[0].ToInt()).ToList())); 
        }
        private void RenderReport()
        {
            var localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("./rdlc/InReachRpt.rdlc");
            DbUtil.LogActivity("Viewing InReach Rpt");

            var id = this.QueryString<int?>("id");
            var orgid = this.QueryString<int?>("orgid");
            var divid = this.QueryString<int?>("divid");
            ReportDataSource rd = null;
            var ctl = new InReachController();
            //var test = ctl.InReachList();


            if (id.HasValue)
                rd = new ReportDataSource("InReachInfo", 
                    ctl.ListByQuery(id.Value).ToList()); //rd = new ReportDataSource();
            //rd = new ReportDataSource("PastAttendeeInfo", ctl.QueryLabelInfo(id.Value));
            else if (orgid.HasValue)
                rd = new ReportDataSource("InReachInfo", 
                    ctl.InReachOrgList(orgid.Value).ToList());
            //rd = new ReportDataSource("PasAttendeeInfo", ctl.OrgLabelInfo(orgid.Value));
            else if (divid.HasValue)
                rd = new ReportDataSource("InReachInfo", 
                    ctl.InReachDivisionList(divid.Value).ToList());
            else
                rd = null;

            localReport.DataSources.Add(rd);
            localReport.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);
            //localReport.DataSources.Add(reportDataSource2);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11.0in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.5in</MarginLeft>" +
            "  <MarginRight>0.5in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
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
