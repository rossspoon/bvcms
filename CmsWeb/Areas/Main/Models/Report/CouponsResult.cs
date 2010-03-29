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

namespace CMSWeb.Areas.Main.Models.Report
{
    public class CouponsResult : ActionResult
    {
        int? divid, orgid;
        public CouponsResult(int? divid, int? orgid)
        {
            this.divid = divid;
            this.orgid = orgid;
        }
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

            var rnd = new Random();
            var n = 0;
            while (n < 30)
            {
                int rndNext = rnd.Next(1000001, 9999999);
                var c = DbUtil.Db.Coupons.SingleOrDefault(cp => cp.Id == rndNext);
                if (c == null)
                {
                    c = new Coupon
                    {
                        Id = rndNext,
                        Created = DateTime.Now,
                        DivId = divid,
                        OrgId = orgid,
                    };
                    if (divid.HasValue)
                    {
                        DbUtil.Db.Coupons.InsertOnSubmit(c);
                        AddCell(t, c.Division.Name, rndNext);
                    }
                    else if (orgid.HasValue)
                    {
                        DbUtil.Db.Coupons.InsertOnSubmit(c);
                        AddCell(t, c.Organization.OrganizationName, rndNext);
                    }
                    else
                        AddCell(t, "A Test Name", rndNext);
                    n++;
                }
            }
            DbUtil.Db.SubmitChanges();
            document.Add(t);

            document.Close();
            Response.End();
        }

        protected float H = 1.0f;
        protected float W = 2.625f;
        protected float GAP = .125f;

        protected PdfContentByte dc;
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 16);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

        int n;
        public bool AddCell(PdfPTable t, string desc, int code)
        {
            var bc = new Barcode39();
            bc.X = 1.2f;
            bc.Font = null;
            bc.Code = code.ToString();
            var img = bc.CreateImageWithBarcode(dc, null, null);
            var p = new Phrase();
            p.Add(new Chunk(img, 0, 0));
            p.Add(new Phrase("\n" + code.ToString().Insert(3," "), font));
            p.Add(new Phrase("\n" + desc, smallfont));
            var c = new PdfPCell(p);
            c.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            c.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            c.Border = PdfPCell.NO_BORDER;
            c.FixedHeight = H * 72f;
           
            t.AddCell(c);
            n++;
            if (n % 3 > 0)
                t.AddCell("");
            return n % 3 == 0;
        }
    }
}

