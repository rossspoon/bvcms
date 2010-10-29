using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using CmsData;
using CMSPresenter;
using UtilityExtensions;
using System.IO;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class ContributionStatementsExtract
    {
        public bool FirstRunComplete { get; set; }
        public bool Running { get; set; }
        private int _Count;
        public int Count
        {
            get { return _Count; }
            set { _Count = value; }
        }
        private int _Current;
        public int Current
        {
            get { return _Current; }
            set { _Current = value; }
        }
        public string CurrentTask { get; set; }
        public DateTime fd { get; set; }
        public DateTime td { get; set; }
        public bool PDF { get; set; }
        public string OutputFile { get; set; }
        public string Host { get; set; }

        public ContributionStatementsExtract(DateTime fd, DateTime td, bool PDF)
        {
            this.fd = fd;
            this.td = td;
            this.PDF = PDF;
            CurrentTask = "Starting Up...";
            if (PDF)
                OutputFile = HttpContext.Current.Server.MapPath("/contributions.pdf");
            else
                OutputFile = HttpContext.Current.Server.MapPath("/contributions.txt");
            Host = Util.Host;
        }

        private bool _LastSuccess;
        public bool LastSuccess
        {
            get
            {
                if (_LastFinishTime == DateTime.MinValue)
                    throw new InvalidOperationException("The task has never completed.");
                return _LastSuccess;
            }
            set { _LastSuccess = value; }
        }
        public Exception ExceptionOccurred { get; set; }

        private DateTime _lastStartTime = DateTime.MinValue;
        public DateTime LastStartTime
        {
            get
            {
                if (_lastStartTime == DateTime.MinValue)
                    throw new InvalidOperationException("The task has never started.");
                return _lastStartTime;
            }
            set { _lastStartTime = value; }
        }
        private DateTime _LastFinishTime;
        public DateTime LastFinishTime
        {
            get
            {
                if (_LastFinishTime == DateTime.MinValue)
                    throw new InvalidOperationException("The task has never completed.");
                return _LastFinishTime;
            }
            set { _LastFinishTime = value; }
        }
        public void Run()
        {
            lock (this)
            {
                if (!Running)
                {
                    Running = true;
                    LastStartTime = DateTime.Now;
                    var t = new Thread(new ThreadStart(DoWork));
                    t.Start();
                }
                else
                {
                    throw new InvalidOperationException("The task is already running!");
                }
            }
        }

        public CMSDataContext Db { get; set; }
        public void DoWork()
        {
            try
            {
                Db = new CMSDataContext(Util.GetConnectionString(Host));
                Db.CommandTimeout = 1200;

                Current = 0;
                Count = 0;
                CurrentTask = "Fetching Contributors...";

                var qc = ContributionModel.contributors(Db, fd, td, 0, 0, 0);
                Count = qc.Count();
                if (PDF)
                {
                    var stream = new FileStream(OutputFile, FileMode.Create);
                    var c = new ContributionStatements
                    {
                        FromDate = fd,
                        ToDate = td,
                        typ = 3
                    };
                    CurrentTask = "Printing Statements...";
                    c.Run(stream, Db, qc, ref _Current);
                }
                else
                {
                    stream = new StreamWriter(OutputFile);
                    foreach (var c in qc)
                    {
                        CurrentTask = "Printing Statements...";
                        Current++;
                        pageStatement = 1;
                        writeHeader(c);
                        writeContributions(c);
            string hdrGift = "   Date        Fund Name          Description of Gift-in-Kind Given as of {0:MM/dd/yyyy}\n\n".Fmt(td);
            rWrite(hdrGift);
                        writeSummary(c);
                    }
                    stream.Close();
                }
                LastSuccess = true;
            }
            catch (Exception e)
            {
                LastSuccess = false;
                ExceptionOccurred = e;
            }
            finally
            {
                Running = false;
                LastFinishTime = DateTime.Now;
                if (!FirstRunComplete)
                    FirstRunComplete = true;
            }
        }

        private StreamWriter stream;
        private void rWrite(string s)
        {
            s = s.Replace("\n", "\r\n");
            stream.Write(s);
        }
        private void rWrite(char c)
        {
            if (c == '\n')
                stream.Write("\r");
            stream.Write(c);
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
            var q2 = ContributionModel.contributions(Db, c, fd, td);
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

        private void writeSummary(ContributorInfo c)
        {
            string hdr1Pledge = "            Pledge History                Current Year Summary ({0:MM/dd/yyyy})\n".Fmt(td);
            string hdr2Pledge = " Fund Name         Pledge        Given      Fund Name           Amount This Year\n\n";
            var recPledges = new List<string>();
            decimal Total = 0;
            rWrite(hdr1Pledge + hdr2Pledge);
            foreach (var p in ContributionModel.pledges(Db, c, td))
                if (p.Fund != null)
                    recPledges.Add("{0,-16}{1,12:N2}  {2,12:N2} ".Fmt(p.Fund, p.PledgeAmount, p.ContributionAmount));

            foreach (var ci in ContributionModel.quarterlySummary(Db, c, fd, td))
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