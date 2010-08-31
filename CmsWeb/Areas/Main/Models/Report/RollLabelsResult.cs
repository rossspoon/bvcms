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
    public class RollLabelsResult : ActionResult
    {
        public int? qid { get; set; }
        public string format { get; set; }
        public bool titles { get; set; }

        protected float H = .925f;
        protected float W = 3f;

        protected PdfContentByte dc;
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 10);

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            var document = new Document();
            document.SetPageSize(new Rectangle(72 * W, 72 * H));
            document.SetMargins(18f, 0f, 3.6f, 0f);
            var w = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            dc = w.DirectContent;

            var ctl = new MailingController();
            ctl.UseTitles = titles;

            const string STR_Name = "Name";
            IEnumerable<MailingInfo> q = null;
            switch (format)
            {
                case "Individual":
                    q = ctl.FetchIndividualList(STR_Name, qid.Value);
                    break;
                case "Family":
                    q = ctl.FetchFamilyList(STR_Name, qid.Value);
                    break;
                case "ParentsOf":
                    q = ctl.FetchParentsOfList(STR_Name, qid.Value);
                    break;
                case "CouplesEither":
                    q = ctl.FetchCouplesEitherList(STR_Name, qid.Value);
                    break;
                case "CouplesBoth":
                    q = ctl.FetchCouplesBothList(STR_Name, qid.Value);
                    break;
            }
            AddLabel(document, "=====================", Util.UserName, "{0} labels printed".Fmt(q.Count()), "{0:M/d/yy h:mm tt}".Fmt(DateTime.Now));
            foreach (var m in q)
                AddLabel(document, m.LabelName, m.Address, m.Address2, m.CityStateZip);

            document.Close();
            Response.End();
        }
        public void AddLabel(Document d, string name, string addr, string addr2, string csz)
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
                cc = new PdfPCell(new Phrase(addr2, font));
                cc.Border = PdfPCell.NO_BORDER;
                t2.AddCell(cc);
            }

            cc = new PdfPCell(new Phrase(csz, font));
            cc.Border = PdfPCell.NO_BORDER;
            t2.AddCell(cc);

            d.Add(t2);
            d.NewPage();
        }

    }
}

