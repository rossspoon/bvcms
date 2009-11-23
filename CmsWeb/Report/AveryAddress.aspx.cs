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

namespace CMSWeb.Reports
{
    public partial class AveryAddress : System.Web.UI.Page
    {
        const float Pts = 72f;
        const float H = 1.0f * Pts;
        const float W = 2.625f * Pts;
        const float GAP = .125f * Pts;

        protected PdfContentByte dc;
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 8);

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            var id = this.QueryString<int?>("id");

            var labelNameFormat = this.QueryString<string>("format");

            var ctl = new MailingController();
            var useTitles = Request.QueryString["titles"];
            ctl.UseTitles = useTitles == "true";

            const string STR_Name = "Name";
            IEnumerable<MailingInfo> q = null;
            switch (labelNameFormat)
            {
                case "Individual":
                    q = ctl.FetchIndividualList(STR_Name, id.Value);
                    break;
                case "Family":
                    q = ctl.FetchFamilyList(STR_Name, id.Value);
                    break;
                case "ParentsOf":
                    q = ctl.FetchParentsOfList(STR_Name, id.Value);
                    break;
                case "CouplesEither":
                    q = ctl.FetchCouplesEitherList(STR_Name, id.Value);
                    break;
                case "CouplesBoth":
                    q = ctl.FetchCouplesBothList(STR_Name, id.Value);
                    break;
            }

            var document = new Document(PageSize.LETTER);
            document.SetMargins(36f, 36f, 33f, 36f);
            var w = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            dc = w.DirectContent;

            var cols = new float[] { W, GAP, W, GAP, W };
            var twid = 0f;
            for (var i = 0; i < cols.Length; i++)
                twid += cols[i];

            var t = new PdfPTable(cols.Length);
            t.TotalWidth = twid;
            t.SetWidths(cols);
            t.HorizontalAlignment = Element.ALIGN_CENTER;
            t.LockedWidth = true;
            t.DefaultCell.Border = PdfPCell.NO_BORDER;

            bool rowcomplete = false;
            foreach (var m in q)
                rowcomplete = AddCell(t, m.LabelName, m.Address, m.Address2, m.CityStateZip);
            while (!rowcomplete)
                rowcomplete = AddCell(t, "", "", "", "");
            document.Add(t);

            document.Close();
            Response.End();
        }
        int n;
        public bool AddCell(PdfPTable t, string name, string addr, string addr2, string csz)
        {
            var t2 = new PdfPTable(1);
            t2.WidthPercentage = 100f;
            t2.DefaultCell.Border = PdfPCell.NO_BORDER;

            var cc = new PdfPCell(new Phrase(name, font));
            cc.Border = PdfPCell.NO_BORDER;
            t2.AddCell(cc);

            cc = new PdfPCell(new Phrase(addr, font));
            cc.Border = PdfPCell.NO_BORDER;
            t2.AddCell(cc);

            if (addr2.HasValue())
            {
                cc = new PdfPCell(new Phrase(addr, font));
                cc.Border = PdfPCell.NO_BORDER;
                t2.AddCell(cc);
            }

            cc = new PdfPCell(new Phrase(csz, font));
            cc.Border = PdfPCell.NO_BORDER;
            t2.AddCell(cc);

            var cell = new PdfPCell(t2);
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.PaddingLeft = 8f;
            cell.PaddingRight = 8f;
            cell.Border = PdfPCell.NO_BORDER;
            cell.FixedHeight = H;

            t.AddCell(cell);
            n++;
            if (n % 3 > 0)
                t.AddCell("");
            return n % 3 == 0;
        }
    }
}
