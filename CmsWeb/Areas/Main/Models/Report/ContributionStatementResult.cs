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
using iTextSharp.text.html.simpleparser;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class ContributionStatementResult : ActionResult
    {
        public Person person { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        private PdfContentByte dc;
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 12);

        public ContributionStatementResult(int id, DateTime FromDate, DateTime ToDate)
        {
            this.FromDate = FromDate;
            this.ToDate = ToDate;
            person = DbUtil.Db.LoadPersonById(id);
        }
        public ContributionStatementResult(int id)
        {
            person = DbUtil.Db.LoadPersonById(id);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            var doc = new Document(PageSize.LETTER);
            doc.SetMargins(36f, 36f, 33f, 36f);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);
            doc.Open();
            dc = w.DirectContent;

            var mct = new MultiColumnText();
            mct.AddRegularColumns(doc.Left, doc.Right, 20f, 2);
            var t = new PdfPTable(new float[] { 10f, 19f, 6f });
            t.WidthPercentage = 100;
            t.SetWidths(new int[] { 30, 4, 6, 30 });
            t.DefaultCell.Border = PdfPCell.NO_BORDER;






            doc.Open();
            String html = @"<font  
color=""#0000FF""><b><i>Title One</i></b></font><font   
color=""black""><br><br>Some text here<br><br><br><font  
color=""#0000FF""><b><i>Another title here   
</i></b></font><font   
color=""black""><br><br>Text1<br>Text2<br><OL><LI>hi</LI><LI>how are u</LI></OL>";

            var list = HTMLWorker.ParseToList(new StringReader(html), null);
            for (int k = 0; k < list.Count; k++)
                doc.Add((IElement)list[k]);

            doc.Add(new Paragraph("And the same with indentation...."));

            var p = new Paragraph();
            p.IndentationLeft = 36;
            p.InsertRange(0, list);
            doc.Add(p);
            doc.Close();

        }
    }
}

