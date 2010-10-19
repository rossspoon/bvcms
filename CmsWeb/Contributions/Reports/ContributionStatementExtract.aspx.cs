/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CmsData;
using UtilityExtensions;
using CMSPresenter;
using CMSPresenter.InfoClasses;

namespace CmsWeb.Contributions.Reports
{
    public partial class ContributionStatementExtract : System.Web.UI.Page
    {
        public delegate void AsyncTaskDelegate(object data);
        AsyncTaskDelegate runner = null;


        protected void ProcessRequest(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            var a = new ContributionExtract
            {
                fd = DateTime.Parse(FromDate.Text),
                td = DateTime.Parse(ToDate.Text),
                current = HttpContext.Current,
            };
            if (PDF.Checked)
            {
                Response.Redirect("/Reports/ContributionStatement/0?FromDate={0:d}&ToDate={1:d}&typ=3".Fmt(a.fd, a.td));
                return;
            }
            var task = new PageAsyncTask(OnBegin, OnEnd, null, a);
            RegisterAsyncTask(task);
            btnCSExtract.Enabled = false;
        }
        IAsyncResult OnBegin(object sender, EventArgs e, AsyncCallback cb, object state)
        {
            runner = new AsyncTaskDelegate(this.DoJob);
            IAsyncResult result = runner.BeginInvoke(state, cb, state);
            return result;
        }
        void OnEnd(IAsyncResult ar)
        {
            runner.EndInvoke(ar);
        }
        public void DoJob(object data2)
        {
            var extract = data2 as ContributionExtract;
            HttpContext.Current = extract.current;
            extract.Run();
        }
        private class ContributionExtract
        {
            public DateTime fd { get; set; }
            public DateTime td { get; set; }
            public HttpContext current { get; set; }
            public void Run()
            {
                var Db = DbUtil.Db;
                Db.CommandTimeout = 1200;

                current.Response.Clear();
                current.Response.Buffer = true;
                current.Response.ContentType = "text/plain";
                current.Response.AddHeader("Content-Disposition", "attachment;filename=cn_stmt.txt");
                current.Response.Charset = "";

                var ctl = new ContributionStatementController();
                var qc = ctl.contributors(fd, td);
                foreach (var c in qc)
                {
                    pageStatement = 1;
                    writeHeader(c);
                    writeContributions(c);
                    writeGifts(c);
                    writeSummary(c);
                }
                current.Response.End();
            }
            private void rWrite(string s)
            {
                s = s.Replace("\n", "\r\n");
                current.Response.Write(s);
            }
            private void rWrite(char c)
            {
                if (c == '\n')
                    current.Response.Write('\r');
                current.Response.Write(c);
            }
            private int pageStatement = 0;
            private string dateLine = "Date: " + Util.Now.Date.ToString("MMM dd yyyy") + "\n";
            private string totalLine = "                                               TOTAL";
            private string churchAddr = "\nBellevue Baptist Church\n" +
                                        "   2000 Appling Road\n" +
                                        "   P.O. Box 1210\n" +
                                        "   Cordova, TN  38016\n" +
                                        "(901) 347-2000\n\n";

            private string disclaimer = "\n\n" +
                                        "    NOTE: No goods or services were provided to you by the church in\n" +
                                        "    connection with any contribution; any value received consisted\n" +
                                        "    entirely of intangible religious benefits.\n";
            private string nineSpaces = "         ";
            private char newPage = (char)0; // start out as zero, then changes to 12

            private int countRec = 0;


            private void writeHeader(ContributorInfo c)
            {
                var isAddress2 = c.Address2 != null;
                if (newPage > 0)
                    rWrite(newPage);
                newPage = (char)12;
                rWrite(dateLine);
                rWrite(churchAddr);
                rWrite(nineSpaces);
                rWrite(c.Name.PadRight(42) + "Envelope: # " + c.PeopleId.ToString() + "\n");
                rWrite(nineSpaces);
                if (pageStatement == 1)
                {
                    rWrite(c.Address1 + "\n");
                    if (isAddress2)
                    {
                        rWrite(nineSpaces);
                        rWrite(c.Address2);
                        rWrite("\n");
                    }
                    rWrite(nineSpaces);
                    rWrite(c.City + ", " + c.State + " " + Util.FmtZip(c.Zip) + "\n");
                    if (!isAddress2) rWrite("\n");
                    rWrite(disclaimer);
                }
                else
                    rWrite("\n" + nineSpaces + "Cont.....Page " + pageStatement.ToString() + "\n".PadRight(6, '\n'));
                rWrite("-------------------------------------------------------------------------------\n\n");
            }

            private void writeContributions(ContributorInfo c)
            {
                string hdrContributions = "   Date        Fund Name          Amount       Date        Fund Name          Amount\n\n";

                rWrite(hdrContributions);
                var ctl = new ContributionStatementController();
                var q2 = ctl.contributions(c.PeopleId, c.SpouseID, fd, td);
                var contrib = new List<string>();
                foreach (var ci in q2)
                    contrib.Add("{0:MM/dd/yyyy}     {1,-16}{2,11:N2}"
                            .Fmt(ci.ContributionDate, ci.Fund, ci.ContributionAmount));

                var n = contrib.Count;
                var nrows = n / 32 * 16;
                var remainder = n % 32;
                if (remainder < 16)
                    nrows += remainder;
                else
                    nrows += 16;

                int row, c1;
                for (row = 0, c1 = 0; row < nrows; row++, c1++)
                {
                    if (row % 16 == 0)
                        c1 = row * 2;
                    var line = contrib[c1];
                    var c2 = c1 + 16;
                    if (c2 < n)
                        line += "  " + contrib[c2];

                    if (row > 0 && row % 16 == 0)
                    {
                        pageStatement += 1;
                        writeHeader(c);
                        rWrite(hdrContributions);
                        rWrite("\n");
                    }
                    rWrite(line + "\n");
                }
                if (nrows > 0)
                    rWrite("\n");
            }

            private void writeGifts(ContributorInfo c)
            {
                string hdrGift = "   Date        Fund Name          Description of Gift-in-Kind Given as of {0:MM/dd/yyyy}\n\n".Fmt(td);
                rWrite(hdrGift);
                foreach (var gift in new ContributionStatementController().gifts(c.PeopleId, c.SpouseID, fd, td))
                {
                    countRec += 1;
                    if (countRec > 6 && (countRec % 6) == 1)
                    {
                        pageStatement += 1;
                        writeHeader(c);
                        rWrite(hdrGift);
                    }
                    rWrite(gift.ContributionDate.Value.ToString("MM/dd/yyyy") +
                            "     " +
                            gift.Fund.PadRight(19) +
                            gift.Description +
                            "\n");
                }
                if (countRec != 0) rWrite("\n");
                countRec = 0;
            }

            private void writeSummary(ContributorInfo c)
            {
                string hdr1Pledge = "            Pledge History                Current Year Summary ({0:MM/dd/yyyy})\n".Fmt(td);
                string hdr2Pledge = " Fund Name         Pledge        Given      Fund Name           Amount This Year\n\n";
                var recPledges = new List<string>();
                decimal Total = 0;
                var ctl = new ContributionStatementController();
                rWrite(hdr1Pledge + hdr2Pledge);
                foreach (var p in ctl.pledges(c.PeopleId, c.SpouseID, td))
                    if (p.Fund != null)
                        recPledges.Add("{0,-16}{1,12:N2}  {2,12:N2} ".Fmt(p.Fund, p.PledgeAmount, p.ContributionAmount));

                foreach (var ci in ctl.quarterlySummary(c.PeopleId, c.SpouseID, fd, td))
                {
                    Total += ci.ContributionAmount.Value;
                    if (recPledges != null && recPledges.Count > 0 && countRec < recPledges.Count)
                        recPledges[countRec] += "{0,-20}{1,14:N2} ".Fmt(ci.Fund, ci.ContributionAmount);
                    else
                        recPledges.Add("{0,43}{1,-20}{2,14:N2} ".Fmt("", ci.Fund, ci.ContributionAmount));
                    countRec++;
                }

                countRec = 0;
                foreach (var s in recPledges)
                {
                    countRec++;
                    if (countRec > 6 && (countRec % 6) == 1)
                    {
                        pageStatement++;
                        writeHeader(c);
                        rWrite(hdr1Pledge + hdr2Pledge);
                    }
                    rWrite(s + "\n");
                }
                if (countRec != 0)
                    rWrite("\n");
                rWrite(totalLine + Total.ToString("C").PadLeft(25) + "\n");
                countRec = 0;
            }
        }
    }
}
