using System;
using System.Linq;
using Microsoft.Reporting.WebForms;
using UtilityExtensions;
using CMSPresenter;
using System.Collections.Generic;
using CmsData;

namespace CmsWeb.Reports
{
    public partial class MeetingAttendanceRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RenderReport();
        }

        private void RenderReport()
        {
            var mtgid = this.QueryString<int?>("mtgid");
            if (!mtgid.HasValue)
                Response.End();

            DbUtil.LogActivity("Viewing Meeting Attendance Rpt");

            LocalReport localReport = new LocalReport();
            ReportDataSource rd1 = null;
            ReportDataSource rd2 = null;

            localReport.ReportPath = Server.MapPath("./rdlc/MeetingAttendanceRpt.rdlc");
            var ctl = new MeetingController();

            //A method that returns a collection for our report
            //Note: A report can have multiple data sources

            //List<Employee> employeeCollection = GetData();

            //Give the collection a name (EmployeeCollection) so that we can reference it in our report designer
            //ReportDataSource reportDataSource = new ReportDataSource("EmployeeCollection", employeeCollection);
            rd1 = new ReportDataSource("PastAttendeeInfo", 
                ctl.Attendees(mtgid.Value, "VisitorsFirst").ToList());
            rd2 = new ReportDataSource("MeetingInfo", ctl.Meeting(mtgid.Value).ToList());
            localReport.DataSources.Add(rd1);
            localReport.DataSources.Add(rd2);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>0.2in</MarginLeft>" +
            "  <MarginRight>0.2in</MarginRight>" +
            "  <MarginBottom>0.7in</MarginBottom>" +
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
