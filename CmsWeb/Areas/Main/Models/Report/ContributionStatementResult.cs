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

            var document = new Document(PageSize.LETTER);
            document.SetMargins(36f, 36f, 33f, 36f);
            var w = PdfWriter.GetInstance(document, Response.OutputStream);
            document.Open();
            dc = w.DirectContent;

            //writer - have our own path!!! and see you have write permissions...
            document.Open();
            //html -text - kan be from database or editor too
            String htmlText = @"<font  
color=""#0000FF""><b><i>Title One</i></b></font><font   
color=""black""><br><br>Some text here<br><br><br><font  
color=""#0000FF""><b><i>Another title here   
</i></b></font><font   
color=""black""><br><br>Text1<br>Text2<br><OL><LI>hi</LI><LI>how are u</LI></OL>";

            //make an arraylist ....with STRINGREADER since its no IO reading file...
            var htmlarraylist = HTMLWorker.ParseToList(new StringReader(htmlText), null);
            //add the collection to the document
            for (int k = 0; k < htmlarraylist.Count; k++)
            {
                document.Add((IElement)htmlarraylist[k]);
            }

            document.Add(new Paragraph("And the same with indentation...."));

            // or add the collection to an paragraph 
            // if you add it to an existing non emtpy paragraph it will insert it from
            //the point youwrite -
            Paragraph mypara = new Paragraph();//make an emtphy paragraph as "holder"
            mypara.IndentationLeft = 36;
            mypara.InsertRange(0, htmlarraylist);
            document.Add(mypara);
            document.Close();

        }
    }
}

