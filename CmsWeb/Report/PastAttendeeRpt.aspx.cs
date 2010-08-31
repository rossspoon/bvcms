using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Reporting.WebForms;
using CmsData;
using CMSPresenter;
using UtilityExtensions;

namespace CmsWeb
{
    public partial class PastAttendeeRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RenderReport();
        }

        private void RenderReport()
        {

            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("./rdlc/PastAttendeeRpt.rdlc");
            DbUtil.LogActivity("Viewing Recent Attendance Rpt");

            //A method that returns a collection for our report
            //Note: A report can have multiple data sources

            //List<Employee> employeeCollection = GetData();

            //Give the collection a name (EmployeeCollection) so that we can reference it in our report designer
            //ReportDataSource reportDataSource = new ReportDataSource("EmployeeCollection", employeeCollection);
            var orgid = this.QueryString<int>("id");
            var ac = new AttendenceController();
            ReportDataSource reportDataSource1 = new ReportDataSource("PastAttendeeInfo", 
                ac.PastAttendees(orgid).ToList());
            ReportDataSource reportDataSource2 = new ReportDataSource("OrganizationInfo", 
                ac.GetOrganizationInfo(orgid).ToList());

            localReport.DataSources.Add(reportDataSource1);
            localReport.DataSources.Add(reportDataSource2);

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
