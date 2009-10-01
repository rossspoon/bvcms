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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using CmsData;
using UtilityExtensions;
using CMSPresenter;
using System.Text;

namespace CMSWeb.Reports
{
    public partial class ContactReport : System.Web.UI.Page
    {
        private Font boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 8, new GrayColor(50));
        private PageEvent pageEvents = new PageEvent();
        private PdfPTable t;
        private Document doc;
        private DateTime dt;
        private PdfContentByte dc;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            var qid = this.QueryString<int?>("id");
            dt = DateTime.Now;

            doc = new Document(PageSize.LETTER.Rotate(), 36, 36, 64, 64);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);
            w.PageEvent = pageEvents;
            doc.Open();

            var ctl = new RollsheetController();
            dc = w.DirectContent;

            StartPageSet();
            if (qid.HasValue) // print using a query
            {
                var qB = DbUtil.Db.LoadQueryById(qid.Value);
                var q = from p in DbUtil.Db.People.Where(qB.Predicate())
                        orderby p.Name2
                        select p;
                foreach (var p in q)
                    AddRow(p);
                if (t.Rows.Count > 1)
                    doc.Add(t);
                else
                    doc.Add(new Phrase("no data"));
            }
            else
                doc.Add(new Phrase("no data"));
            pageEvents.EndPageSet();
            doc.Close();
            Response.End();
        }

        private void StartPageSet()
        {
            var w = new float[] { 40, 70, 80, 40, 130 };

            t = new PdfPTable(w);

            t.WidthPercentage = 100;
            t.DefaultCell.Border = PdfPCell.NO_BORDER;
            t.DefaultCell.Padding = 5;
            pageEvents.StartPageSet("Contact Report: {0:d}".Fmt(dt));

            t.AddCell(new Phrase("\nPerson", boldfont));
            t.AddCell(new Phrase("\nAddress", boldfont));
            t.AddCell(new Phrase("Phone\nEmail", boldfont));
            t.AddCell(new Phrase("Birthday\nAttends", boldfont));
            t.AddCell(new Phrase("Member Status\nContacts", boldfont));

        }
        private void AddRow(Person p)
        {
            if (t.Rows.Count % 2 == 0)
                t.DefaultCell.BackgroundColor = new GrayColor(240);
            else
                t.DefaultCell.BackgroundColor = Color.WHITE;
            var ph = new Phrase(p.Name + "\n", font);
            ph.Add(new Chunk("  ({0})".Fmt(p.PeopleId), smallfont));
            t.AddCell(ph);

            var addr = new StringBuilder(p.PrimaryAddress);
            AddLine(addr, p.PrimaryAddress2);
            AddLine(addr, "{0}, {1} {2}".Fmt(p.PrimaryCity, p.PrimaryState, p.PrimaryZip.FmtZip()));
            t.AddCell(new Phrase(addr.ToString(), font));

            var phones = new StringBuilder();
            AddPhone(phones, p.HomePhone, "h ");
            AddPhone(phones, p.CellPhone, "c ");
            AddPhone(phones, p.WorkPhone, "w ");
            AddLine(phones, p.EmailAddress);
            t.AddCell(new Phrase(phones.ToString(), font));

            var q = from a in p.Attends
                    where a.AttendanceFlag == true
                    orderby a.MeetingDate descending
                    select a.MeetingDate;
            var attends = q.Take(3);
            ph = new Phrase();
            ph.Add(new Chunk(p.DOB, font));
            foreach(var dt in attends)
                ph.Add(new Chunk("\n{0:d}".Fmt(dt), smallfont));
            t.AddCell(ph);

            var ctl = new CMSPresenter.CodeValueController();
            var cts = ctl.ContactTypeCodes();
            ph = new Phrase();
            ph.Add(new Chunk(p.MemberStatus.Description, font));

            var contactcell = new PdfPCell();
            var cq = from ce in DbUtil.Db.Contactees
                    where ce.PeopleId == p.PeopleId
                    orderby ce.contact.ContactDate descending
                    select new
                    {
                        contact = ce.contact,
                        madeby = ce.contact.contactsMakers.FirstOrDefault().person,
                    };
            foreach(var c in cq)
            {
                var name = "unknown";
                if (c.madeby != null)
                    name = c.madeby.Name;
                ph.Add(new Chunk("\n-----------------\n{0:d}: {1} by {2}\n".Fmt(
                    c.contact.ContactDate,
                    cts.Single(ct => ct.Id == c.contact.ContactTypeId).Value,
                    name), smallfont));
                ph.Add(new Chunk(c.contact.Comments, font));
            }
            t.AddCell(ph);
        }
        private void AddLine(StringBuilder sb, string value)
        {
            AddLine(sb, value, String.Empty);
        }
        private void AddLine(StringBuilder sb, string value, string postfix)
        {
            if(value.HasValue())
            {
                if (sb.Length > 0)
                    sb.Append("\n");
                sb.Append(value);
                if(postfix.HasValue())
                    sb.Append(postfix);
            }
        }
        private void AddPhone(StringBuilder sb, string value, string prefix)
        {
            if (value.HasValue())
            {
                value = value.FmtFone(prefix);
                if (sb.Length > 0)
                    sb.Append("\n");
                sb.Append(value);
            }
        }

        class PageEvent : PdfPageEventHelper
        {
            private PdfTemplate npages;
            private PdfWriter writer;
            private Document document;
            private PdfContentByte dc;
            private BaseFont font;
            private string HeadText;
            private string HeadText2;

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                this.writer = writer;
                this.document = document;
                base.OnOpenDocument(writer, document);
                font = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                dc = writer.DirectContent;
            }
            public void EndPageSet()
            {
                if (npages == null)
                    return;
                npages.BeginText();
                npages.SetFontAndSize(font, 8);
                npages.ShowText((writer.PageNumber + 1).ToString());
                npages.EndText();
            }
            public void StartPageSet(string header1)
            {
                EndPageSet();
                document.NewPage();
                document.ResetPageCount();
                this.HeadText = header1;
                npages = dc.CreateTemplate(50, 50);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);

                string text;
                float len;

                //---Header left
                text = HeadText;
                const float HeadFontSize = 11f;
                len = font.GetWidthPoint(text, HeadFontSize);
                dc.BeginText();
                dc.SetFontAndSize(font, HeadFontSize);
                dc.SetTextMatrix(30, document.PageSize.Height - 30);
                dc.ShowText(text);
                dc.EndText();
                dc.BeginText();
                dc.SetFontAndSize(font, HeadFontSize);
                dc.SetTextMatrix(30, document.PageSize.Height - 30 - (HeadFontSize * 1.5f));
                dc.ShowText(HeadText2);
                dc.EndText();

                //---Column 1
                text = "Page " + (writer.PageNumber + 1) + " of ";
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(30, 30);
                dc.ShowText(text);
                dc.EndText();
                dc.AddTemplate(npages, 30 + len, 30);

                //---Column 2
                text = HeadText;
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(document.PageSize.Width / 2 - len / 2, 30);
                dc.ShowText(text);
                dc.EndText();

                //---Column 3
                text = Util.Now.ToShortDateString();
                len = font.GetWidthPoint(text, 8);
                dc.BeginText();
                dc.SetFontAndSize(font, 8);
                dc.SetTextMatrix(document.PageSize.Width - 30 - len, 30);
                dc.ShowText(text);
                dc.EndText();
            }
            private float PutText(string text, BaseFont font, float size, float x, float y)
            {
                dc.BeginText();
                dc.SetFontAndSize(font, size);
                dc.SetTextMatrix(x, y);
                dc.ShowText(text);
                dc.EndText();
                return font.GetWidthPoint(text, size);
            }
        }
    }
}
