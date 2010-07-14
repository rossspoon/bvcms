/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using CmsData;
using UtilityExtensions;
using CMSPresenter;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Diagnostics;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class AveryResult : ActionResult
    {
        public int? id;
        protected float H = 1.0f;
        protected float W = 2.625f;
        protected float GAP = .125f;

        protected PdfContentByte dc;
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 20);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 8);

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            var document = new Document(PageSize.LETTER);
            document.SetMargins(36f, 36f, 33f, 36f);
            var w = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            dc = w.DirectContent;

            var ctl = new RollsheetController();

            var cols = new float[3 * 2 - 1];
            var twid = 0f;
            var t = new PdfPTable(cols.Length);
            for (var i = 0; i < cols.Length; i++)
                if (i % 2 == 1)
                    cols[i] = GAP * 72f;
                else
                    cols[i] = W * 72f;
            foreach (var wid in cols)
                twid += wid;

            t.TotalWidth = twid;
            t.SetWidths(cols);
            t.HorizontalAlignment = Element.ALIGN_CENTER;
            t.LockedWidth = true;
            t.DefaultCell.Border = PdfPCell.NO_BORDER;

            var qB = DbUtil.Db.LoadQueryById(id.Value);
            var q = from p in DbUtil.Db.People.Where(qB.Predicate())
                    orderby p.Name2
                    select new
                    {
                        First = p.PreferredName,
                        Last = p.LastName,
                        PeopleId = p.PeopleId,
                        dob = p.DOB,
                        Phone = p.CellPhone ?? p.HomePhone
                    };
            foreach (var m in q)
                AddCell(t, m.First, m.Last, m.Phone, m.PeopleId);
            while (t.Rows.Count == 0)
                t.AddCell("");
            document.Add(t);

            document.Close();
            Response.End();
        }
        int n;
        public void AddCell(PdfPTable t, string fname, string lname, string phone, int pid)
        {
            var t2 = new PdfPTable(2);
            t2.WidthPercentage = 100f;
            t2.DefaultCell.Border = PdfPCell.NO_BORDER;

            var cc = new PdfPCell(new Phrase(fname, font));
            cc.Border = PdfPCell.NO_BORDER;
            cc.Colspan = 2;
            t2.AddCell(cc);

            cc = new PdfPCell(new Phrase(lname, font));
            cc.Border = PdfPCell.NO_BORDER;
            cc.Colspan = 2;
            t2.AddCell(cc);

            var pcell = new PdfPCell(new Phrase(pid.ToString(), smallfont));
            pcell.Border = PdfPCell.NO_BORDER;
            pcell.HorizontalAlignment = Element.ALIGN_LEFT;
            t2.AddCell(pcell);

            pcell = new PdfPCell(new Phrase(phone.FmtFone(), smallfont));
            pcell.Border = PdfPCell.NO_BORDER;
            pcell.HorizontalAlignment = Element.ALIGN_RIGHT;
            t2.AddCell(pcell);

            var cell = new PdfPCell(t2);
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.PaddingLeft = 8f;
            cell.PaddingRight = 8f;
            cell.Border = PdfPCell.NO_BORDER;
            cell.FixedHeight = H * 72f;

            t.AddCell(cell);
            n++;
            if (n % 3 > 0)
                t.AddCell("");
        }
    }
}

