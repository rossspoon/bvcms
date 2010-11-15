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
using CmsData;
using UtilityExtensions;
using System.Text;
using System.Web.Mvc;
using CmsWeb.Models;

namespace CmsWeb.Areas.Main.Models.Report
{
    public class TestResult : ActionResult
    {
        private const float cm2pts = 28.34f;
        private Document doc;
        private PdfContentByte dc;
        private Font bfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
        private Font h1font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
        private Font h2font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
        private Font font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        private Font smallfont = FontFactory.GetFont(FontFactory.HELVETICA, 6);

        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");

            doc = new Document(PageSize.LETTER, 36, 36, 36, 36);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);
            doc.Open();
            dc = w.DirectContent;

            doc.NewPage();

            var t = new PdfPTable(1);
            t.SetNoBorder();
            t.AddCentered("In-Reach/Outreach Card", 1, h1font);
            t.AddCentered("Contact Summary", 1, h2font);
            t.AddRight("Contact Date: _______________", bfont);

            doc.Add(t);

            DisplayTable("Contact Reason", 5.7f, 1.2f, 24f, new List<string> 
            { 
                "Out-Reach (not a church member)",
                "In-Reach (church/group member)",
                "Information",
                "Bereavement",
                "Health",
                "Personal",
                "Other"
            });

            DisplayTable("Type of Contact", 5.7f, 8f, 24f, new List<string> 
            { 
                "Personal Visit",
                "Phone Call",
                "Card Sent",
                "Email Sent",
            });
            DisplayTable("Results", 5.7f, 14.5f, 24f, new List<string> 
            { 
                "Not at Home",
                "Left Door Hanger",
                "Left Message",
                "Contact Made",
                "Gospel Shared",
                "Profession of Faith",
                "Prayer Request Rec'd",
                "Prayed for Person",
                "Already Saved",
            });
            DisplayNotes("Team Members", 4, 7.5f, 6f, 18.5f);
            DisplayNotes("Specific Comments on Contact", 5, 18.5f, 1.2f, 13f);

            DisplayTable("Actions to be taken", 12f, 1f, 6.5f, new List<string> 
            { 
                "Recycle to me on ____/____/____",
                "Random Recycle",
                "Follow-up Completed",
            });

            var t2 = new PdfPTable(1);
            t2.TotalWidth = 7.5f * 72f;
            t2.SetNoBorder();
            t2.LockedWidth = true;
            t2.AddCentered("Internal Use Only", 1, smallfont);
            t2.WriteSelectedRows(0, -1, 36f, 46f, dc);

            doc.Close();
            Response.End();
        }
        private void DisplayTable(string title, float width, float x, float y, List<string> reasons)
        {
            var t = new PdfPTable(new float[] { 1.3f, width-1.3f });
            t.TotalWidth = width * cm2pts;
            t.SetNoBorder();
            t.LockedWidth = true;
            t.DefaultCell.MinimumHeight = 1f * cm2pts;
            t.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

            t.AddPlainRow(title, bfont);
            foreach (var r in reasons)
            {
                t.AddRight("_____", font);
                t.Add(r, font);
            }
            t.WriteSelectedRows(0, -1, x * cm2pts, y * cm2pts, dc);
        }
        private void DisplayNotes(string title, int nrows, float width, float x, float y)
        {
            var t = new PdfPTable(1);
            t.TotalWidth = width * cm2pts;
            t.LockedWidth = true;
            t.DefaultCell.MinimumHeight = 1f * cm2pts;
            t.DefaultCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;

            t.AddPlainRow(title, bfont);
            for (int r = 0; r < nrows; r++ )
                t.AddCell("");
            t.WriteSelectedRows(0, -1, x * cm2pts, y * cm2pts, dc);
        }
    }
}