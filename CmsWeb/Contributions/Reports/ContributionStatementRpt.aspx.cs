using System;
using System.Linq;
using Microsoft.Reporting.WebForms;
using UtilityExtensions;
using CMSPresenter;
using System.Collections.Generic;
using CmsData;

namespace CMSWeb.Contributions.Reports
{
    public partial class ContributionStatementRpt : System.Web.UI.Page
    {
        DateTime fromDate;
        DateTime toDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            RenderReport();
        }

        private void localReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "ContributionsSubReport")
            {
                var ctl = new ContributionStatementController();
                var rd1 = new ReportDataSource("ContributionInfo", ctl.contributions(e.Parameters[0].Values[0].ToInt(),
                                                                                     e.Parameters[1].Values[0].ToInt(),
                                                                                     fromDate,
                                                                                     toDate));
                e.DataSources.Add(rd1);
            }
            else if (e.ReportPath == "GiftInKindSubRpt")
            {
                var ctl = new ContributionStatementController();
                var rd1 = new ReportDataSource("ContributionInfo", ctl.gifts(e.Parameters[0].Values[0].ToInt(),
                                                                             e.Parameters[1].Values[0].ToInt(),
                                                                             fromDate,
                                                                             toDate).DefaultIfEmpty());
                e.DataSources.Add(rd1);
            }

            else if (e.ReportPath == "PledgeSummarySubRpt")
            {
                var ctl = new ContributionStatementController();
                var rd1 = new ReportDataSource("PledgeSummaryInfo", ctl.pledges(e.Parameters[0].Values[0].ToInt(),
                                                                             e.Parameters[1].Values[0].ToInt()));
                var rd2 = new ReportDataSource("ContributionInfo", ctl.quarterlySummary(e.Parameters[0].Values[0].ToInt(),
                                                                             e.Parameters[1].Values[0].ToInt(),
                                                                             fromDate,
                                                                             toDate));
                e.DataSources.Add(rd1);
                e.DataSources.Add(rd2);
            }
        }

        private void RenderReport()
        {
            var typ = this.QueryString<int?>("typ"); //indicates individual(1) or family(2) statement
            var id = this.QueryString<int?>("id");  //this will be a people id or a family id determined by type of statement, 1 or 2 respectively
            var fdate = this.QueryString<string>("fd");
            var tdate = this.QueryString<string>("td");

            if (!(typ.HasValue || id.HasValue ))
              Response.End();
            fromDate = DateTime.Parse(fdate).Date;
            toDate = DateTime.Parse(tdate).Date;

            LocalReport localReport = new LocalReport();
            ReportDataSource rd1 = null;

            localReport.ReportPath = Server.MapPath("./rdlc/ContributionStatementRpt.rdlc");
            var ctl = new ContributionStatementController();

            if (typ.Value == 1)
                rd1 = new ReportDataSource("ContributorInfo", ctl.contributor(id.Value, fromDate, toDate));
            else
            {
                var fid = DbUtil.Db.People.Single(p => p.PeopleId == id.Value).FamilyId;
                rd1 = new ReportDataSource("ContributorInfo", ctl.family(fid, fromDate, toDate));
            }

            localReport.DataSources.Add(rd1);
            localReport.SubreportProcessing += new SubreportProcessingEventHandler(localReport_SubreportProcessing);

            ReportParameter rp = new ReportParameter("StartDate", fdate, false);
            ReportParameter rp2 = new ReportParameter("EndDate", tdate, false);

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
