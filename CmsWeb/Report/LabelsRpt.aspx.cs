using System;
using System.Linq;
using Microsoft.Reporting.WebForms;
using UtilityExtensions;
using CMSPresenter;
using System.Collections.Generic;
using CmsData;

namespace CmsWeb
{
    public partial class LabelsRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("./rdlc/LabelsRpt.rdlc");

            var id = this.QueryString<int?>("id");
            if (!id.HasValue) // error
                Response.End();

            DbUtil.LogActivity("Viewing Labels Rpt");
            var labelNameFormat = this.QueryString<string>("format");

            ReportDataSource rd = null;
            var ctl = new MailingController();
            var useTitles = Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";

            const string STR_MailingInfo = "MailingInfo";
            const string STR_Name = "Name";
            switch (labelNameFormat)
            {
                case "Individual":
                    rd = new ReportDataSource(STR_MailingInfo, 
                        ctl.FetchIndividualList(STR_Name, id.Value).ToList());
                    break;
                case "Family":
                    rd = new ReportDataSource(STR_MailingInfo,
                        ctl.FetchFamilyList(STR_Name, id.Value).ToList());
                    break;
                case "ParentsOf":
                    rd = new ReportDataSource(STR_MailingInfo,
                        ctl.FetchParentsOfList(STR_Name, id.Value).ToList());
                    break;
                case "CouplesEither":
                    rd = new ReportDataSource(STR_MailingInfo,
                        ctl.FetchCouplesEitherList(STR_Name, id.Value).ToList());
                    break;
                case "CouplesBoth":
                    rd = new ReportDataSource(STR_MailingInfo,
                        ctl.FetchCouplesBothList(STR_Name, id.Value).ToList());
                    break;
            }

            localReport.DataSources.Add(rd);
            //localReport.DataSources.Add(reportDataSource2);
            ReportParameter rp = new ReportParameter("Requestor", DbUtil.Db.CurrentUser.BestName, false);
            ReportParameter[] rpc = { rp };
            localReport.SetParameters(rpc);

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo = " <DeviceInfo> " +
                                " <OutputFormat>PDF</OutputFormat>" +
                                " <StartPage>0</StartPage>" +
            "  <PageWidth>3.0in</PageWidth>" +
            "  <PageHeight>0.925in</PageHeight>" +
            "  <MarginTop>0.05in</MarginTop>" +
            "  <MarginLeft>0.0in</MarginLeft>" +
            "  <MarginRight>0.0in</MarginRight>" +
            "  <MarginBottom>0.0in</MarginBottom>" +
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
