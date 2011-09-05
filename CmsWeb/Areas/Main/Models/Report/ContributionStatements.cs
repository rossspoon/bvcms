/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using CmsData;
using UtilityExtensions;
using iTextSharp.text.html.simpleparser;
using CmsWeb.Areas.Main.Models.Report;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class ContributionStatements
    {
        public int FamilyId { get; set; }
        public int PeopleId { get; set; }
        public int? SpouseId { get; set; }
        public int typ { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public void Run(Stream stream, CMSDataContext Db, IEnumerable<ContributorInfo> q, ref int current)
        {
            IEnumerable<ContributorInfo> contributors = q;

            PdfContentByte dc;
            var font = FontFactory.GetFont(FontFactory.HELVETICA, 11);
            var boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);

            var doc = new Document(PageSize.LETTER);
            var pageEvents = new PageEvent();
            doc.SetMargins(36f, 30f, 24f, 36f);
            var w = PdfWriter.GetInstance(doc, stream);
            w.PageEvent = pageEvents;
            doc.Open();
            dc = w.DirectContent;

            int prevfid = 0;
            foreach (var ci in contributors)
            {
                if (prevfid != ci.FamilyId)
                {
                    //Debug.WriteLine(ci.FamilyId);
                    pageEvents.StartPageSet();
                    prevfid = ci.FamilyId;
                }
                else
                    doc.NewPage();

                var st = new StyleSheet();
                st.LoadTagStyle("h1", "size", "18px");
                st.LoadTagStyle("h2", "size", "8px");
                st.LoadTagStyle("p", "size", "8px");

                //----Church Name
                var t1 = new PdfPTable(1);
                t1.TotalWidth = 72f * 5f;
                t1.DefaultCell.Border = PdfPCell.NO_BORDER;
                string html1 = @"<h1>Bellevue Baptist Church</h1>
<h2>2000 Appling Rd. | Cordova | TN 38088-1210 | (901) 347-2000</h2>";
                var content = Db.Contents.SingleOrDefault(c => c.Name == "StatementHeader");
                if (content != null)
                    html1 = content.Body;
                var list = HTMLWorker.ParseToList(new StringReader(html1), st);
                var cell = new PdfPCell(t1.DefaultCell);
                for (int k = 0; k < list.Count; k++)
                    cell.AddElement((IElement)list[k]);
                //cell.FixedHeight = 72f * 1.25f;
                t1.AddCell(cell);
                t1.AddCell("\n");

                var t1a = new PdfPTable(1);
                t1a.TotalWidth = 72f * 5f;
                t1a.DefaultCell.Border = PdfPCell.NO_BORDER;

                var ae = new PdfPTable(1);
                ae.DefaultCell.Border = PdfPCell.NO_BORDER;
                ae.WidthPercentage = 100;

                var a = new PdfPTable(1);
                a.DefaultCell.Indent = 25f;
                a.DefaultCell.Border = PdfPCell.NO_BORDER;
                a.AddCell(new Phrase(ci.Name, font));
                a.AddCell(new Phrase(ci.Address1, font));
                if (ci.Address2.HasValue())
                    a.AddCell(new Phrase(ci.Address2, font));
                a.AddCell(new Phrase(ci.CityStateZip, font));
                cell = new PdfPCell(a);
                cell.Border = PdfPCell.NO_BORDER;
                //cell.FixedHeight = 72f * 1.0625f;
                ae.AddCell(cell);

                cell = new PdfPCell(t1a.DefaultCell);
                cell.AddElement(ae);
                t1a.AddCell(ae);


                //-----Notice
                var t2 = new PdfPTable(1);
                t2.TotalWidth = 72f * 3f;
                t2.DefaultCell.Border = PdfPCell.NO_BORDER;
                t2.AddCell(new Phrase("\nPrint Date: {0:M/d/yy}   (id:{1} {2})".Fmt(DateTime.Now, ci.PeopleId, ci.CampusId), font));
                t2.AddCell("");
                string html2 = @"<p><i>
NOTE: No goods or services were provided to you by the church in connection with any contibution;
any value received consisted entirely of intangible religious benefits.
Bellevue Baptist Church, FEIN # 62-60017-10, is a 501(c)(3) organization and
qualifies as a part of the Southern Baptist Convention's group tax exemption ruling number GEN #1674.
</i></p>
<p> </p>
<p><i>
Thank you for your faithfulness in the giving of your time, talents, and resources. Together we can share the love of Jesus with our city.
</i></p>";
                content = Db.Contents.SingleOrDefault(c => c.Name == "StatementNotice");
                if (content != null)
                    html2 = content.Body;
                list = HTMLWorker.ParseToList(new StringReader(html2), st);
                cell = new PdfPCell(t1.DefaultCell);
                for (int k = 0; k < list.Count; k++)
                    cell.AddElement((IElement)list[k]);
                t2.AddCell(cell);


                // POSITIONING OF ADDRESSES
                //----Header
                var yp = doc.BottomMargin + 
                    DbUtil.Db.Setting("StatementRetAddrPos", "10.125").ToFloat() * 72f;
                t1.WriteSelectedRows(0, -1, 
                    doc.LeftMargin - 0.1875f *72f, yp, dc);

                yp = doc.BottomMargin + 
                    DbUtil.Db.Setting("StatementAddrPos", "8.3375").ToFloat() * 72f;
                t1a.WriteSelectedRows(0, -1, doc.LeftMargin, yp, dc);

                yp = doc.BottomMargin + 10.125f * 72f;
                t2.WriteSelectedRows(0, -1, doc.LeftMargin + 72f * 4.4f, yp, dc);


                //----Contributions
                doc.Add(new Paragraph(" "));
                doc.Add(new Paragraph(" ") { SpacingBefore = 72f * 2.125f });

                doc.Add(new Phrase("\n  Period: {0:M/d/yy} - {1:M/d/yy}".Fmt(FromDate, ToDate), boldfont));

                var mct = new MultiColumnText();
                mct.AddRegularColumns(doc.Left, doc.Right, 20f, 2);

                var t = new PdfPTable(new float[] { 10f, 24f, 10f });
                t.WidthPercentage = 100;
                t.DefaultCell.Border = PdfPCell.NO_BORDER;
                t.HeaderRows = 2;

                cell = new PdfPCell(t.DefaultCell);
                cell.Colspan = 3;
                cell.Phrase = new Phrase("Contributions\n", boldfont);
                t.AddCell(cell);

                t.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
                t.AddCell(new Phrase("Date", boldfont));
                t.AddCell(new Phrase("Description", boldfont));
                cell = new PdfPCell(t.DefaultCell);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Phrase = new Phrase("Amount", boldfont);
                t.AddCell(cell);

                t.DefaultCell.Border = PdfPCell.NO_BORDER;

                var total = 0m;
                foreach (var c in ContributionModel.contributions(Db, ci, FromDate, ToDate))
                {
                    t.AddCell(new Phrase(c.ContributionDate.ToString2("M/d/yy"), font));
                    t.AddCell(new Phrase(c.Fund, font));
                    cell = new PdfPCell(t.DefaultCell);
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    cell.Phrase = new Phrase(c.ContributionAmount.ToString2("N2"), font);
                    t.AddCell(cell);
                    total += (c.ContributionAmount ?? 0);
                }
                t.DefaultCell.Border = PdfPCell.TOP_BORDER;
                cell = new PdfPCell(t.DefaultCell);
                cell.Colspan = 2;
                cell.Phrase = new Phrase("Total Contributions for period", boldfont);
                t.AddCell(cell);
                cell = new PdfPCell(t.DefaultCell);
                cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                cell.Phrase = new Phrase(total.ToString("N2"), font);
                t.AddCell(cell);

                mct.AddElement(t);


                //------Pledges
                var pledges = ContributionModel.pledges(Db, ci, ToDate);
                if (pledges.Count() > 0)
                {
                    t = new PdfPTable(new float[] { 16f, 12f, 12f });
                    t.WidthPercentage = 100;
                    t.DefaultCell.Border = PdfPCell.NO_BORDER;
                    t.HeaderRows = 2;

                    cell = new PdfPCell(t.DefaultCell);
                    cell.Colspan = 3;
                    cell.Phrase = new Phrase("\n\nPledges\n", boldfont);
                    t.AddCell(cell);

                    t.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
                    t.AddCell(new Phrase("Fund", boldfont));
                    cell = new PdfPCell(t.DefaultCell);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Phrase = new Phrase("Pledge", boldfont);
                    t.AddCell(cell);
                    cell = new PdfPCell(t.DefaultCell);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Phrase = new Phrase("Given", boldfont);
                    t.AddCell(cell);

                    t.DefaultCell.Border = PdfPCell.NO_BORDER;

                    foreach (var c in pledges)
                    {
                        t.AddCell(new Phrase(c.Fund, font));
                        cell = new PdfPCell(t.DefaultCell);
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        cell.Phrase = new Phrase(c.PledgeAmount.ToString2("N2"), font);
                        t.AddCell(cell);
                        cell = new PdfPCell(t.DefaultCell);
                        cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        cell.Phrase = new Phrase(c.ContributionAmount.ToString2("N2"), font);
                        t.AddCell(cell);
                    }
                    mct.AddElement(t);
                }

                //-----Summary
                t = new PdfPTable(new float[] { 29f, 9f });
                t.WidthPercentage = 100;
                t.DefaultCell.Border = PdfPCell.NO_BORDER;
                t.HeaderRows = 2;

                cell = new PdfPCell(t.DefaultCell);
                cell.Colspan = 2;
                cell.Phrase = new Phrase("\n\nPeriod Summary\n", boldfont);
                t.AddCell(cell);

                t.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
                t.AddCell(new Phrase("Fund", boldfont));
                cell = new PdfPCell(t.DefaultCell);
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Phrase = new Phrase("Amount", boldfont);
                t.AddCell(cell);

                t.DefaultCell.Border = PdfPCell.NO_BORDER;
                foreach (var c in ContributionModel.quarterlySummary(Db, ci, FromDate, ToDate))
                {
                    t.AddCell(new Phrase(c.Fund, font));
                    cell = new PdfPCell(t.DefaultCell);
                    cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    cell.Phrase = new Phrase(c.ContributionAmount.ToString2("N2"), font);
                    t.AddCell(cell);
                }
                t.DefaultCell.Border = PdfPCell.TOP_BORDER;
                t.AddCell(new Phrase("Total contributions for period", boldfont));
                cell = new PdfPCell(t.DefaultCell);
                cell.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                cell.Phrase = new Phrase(total.ToString("N2"), font);
                t.AddCell(cell);
                mct.AddElement(t);

                doc.Add(mct);
                current++;
            }
            if (!pageEvents.EndPageSet())
            {
                pageEvents.StartPageSet();
                doc.Add(new Phrase("no data"));
                pageEvents.EndPageSet();
            }
            doc.Close();
        }
        class PageEvent : PdfPageEventHelper
        {
            private PdfTemplate npages;
            private PdfWriter writer;
            private Document document;
            private PdfContentByte dc;
            private BaseFont font;

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                this.writer = writer;
                this.document = document;
                base.OnOpenDocument(writer, document);
                font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                dc = writer.DirectContent;
            }
            public bool EndPageSet()
            {
                if (npages == null)
                    return false;
                npages.BeginText();
                npages.SetFontAndSize(font, 8);
                npages.ShowText((writer.PageNumber + 1).ToString());
                npages.EndText();
                return true;
            }
            public void StartPageSet()
            {
                EndPageSet();
                document.NewPage();
                document.ResetPageCount();
                npages = dc.CreateTemplate(50, 50);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                string text;
                float len;

                text = "Page " + (writer.PageNumber + 1) + " of ";
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(document.PageSize.Width - 30 - len, 30);
                dc.ShowText(text);
                dc.EndText();
                dc.AddTemplate(npages, document.PageSize.Width - 30, 30);
            }
        }
    }
}

