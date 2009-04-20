using System;
using System.Linq;
using Microsoft.Reporting.WebForms;
using UtilityExtensions;
using CMSPresenter;
using System.Collections.Generic;
using CmsData;

namespace CMSWeb.Reports
{
    public partial class RollsheetRpt : System.Web.UI.Page
    {
        DateTime meetingdate;

        protected void Page_Load(object sender, EventArgs e)
        {
            DbUtil.LogActivity("Viewing Rollsheets");
            RenderReport();
        }

        private void localReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "RollsheetStandardDetailSubRpt")
            {
                var ctl = new RollsheetController();
                var rd1 = new ReportDataSource("PersonMemberInfo", ctl.FetchOrgMembers(e.Parameters[0].Values[0].ToInt()));
                e.DataSources.Add(rd1);
            }
            else if (e.ReportPath == "RollsheetStandardDetailVisitorSubRpt")
            {
                var ctl = new RollsheetController();
                var rd1 = new ReportDataSource("PersonVisitorInfo", ctl.FetchVisitors(e.Parameters[0].Values[0].ToInt(),meetingdate));
                e.DataSources.Add(rd1);
            }

        }

        private void RenderReport()
        {
            var oid = this.QueryString<int?>("oid");
            var did = this.QueryString<int?>("did");
            var sid = this.QueryString<int?>("sid");
            var sts = this.QueryString<int?>("sts");
            var nam = this.QueryString<string>("nam");
            var date = this.QueryString<string>("date");
            var time = this.QueryString<string>("time");

            if (!(oid.HasValue || did.HasValue || sid.HasValue || sts.HasValue || nam.HasValue() || date.HasValue() || time.HasValue()))
              Response.End();
            meetingdate = DateTime.Parse(date).Date;

            LocalReport localReport = new LocalReport();
            ReportDataSource rd1 = null;

            localReport.ReportPath = Server.MapPath("./rdlc/RollsheetRpt.rdlc");
            var ctl = new RollsheetController();

            //A method that returns a collection for our report
            //Note: A report can have multiple data sources
            //List<Employee> employeeCollection = GetData();
            //Give the collection a name (EmployeeCollection) so that we can reference it in our report designer
            //ReportDataSource reportDataSource = new ReportDataSource("EmployeeCollection", employeeCollection);

            if (oid.HasValue)
                rd1 = new ReportDataSource("OrganizationInfo", ctl.FetchOrgsList(oid.Value,meetingdate));
            else
                rd1 = new ReportDataSource("OrganizationInfo", ctl.FetchOrgsList(nam, did.Value, sid.Value, sts.Value));

            localReport.DataSources.Add(rd1);
            localReport.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);

            ReportParameter rp = new ReportParameter("MeetingDate", date, false);
            ReportParameter rp2 = new ReportParameter("MeetingTime", time, false);

            ReportParameter[] rpc = { rp, rp2 };
            localReport.SetParameters(rpc);
            
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
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.166in</MarginTop>" +
            "  <MarginLeft>0.25in</MarginLeft>" +
            "  <MarginRight>0.25in</MarginRight>" +
            "  <MarginBottom>0.166in</MarginBottom>" +
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
