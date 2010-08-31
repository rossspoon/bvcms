using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections;
using CmsData;
using UtilityExtensions;
using System.Text;
using CmsWeb.Models;

namespace CmsWeb.Models.Prayer
{
    public partial class Report : System.Web.Mvc.ViewPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            var g = Group.LoadByName("Prayer Partners");
            if (!g.IsAdmin)
            {
                Response.Write("You need admin rights.");
                return;
            }
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "filename=foo.pdf");
            var doc = new Document(PageSize.LETTER, 36, 36, 64, 64);
            var w = PdfWriter.GetInstance(doc, Response.OutputStream);

            var headtext = "Prayer Signup - {0:M/d/yy hh:mm tt}".Fmt(DateTime.Now);

            var header = new HeaderFooter(new Phrase(headtext), false);
            header.Border = Rectangle.NO_BORDER;
            doc.Header = header;

            doc.Open();

            var boldfont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD);
            var m = new CmsWeb.Models.PrayerModel();
            for (int day = 0; day < 7; day++)
            {
                var t = new PdfPTable(2);
                t.HeaderRows = 1;
                t.WidthPercentage = 100;
                t.SetWidths(new int[] { 13, 87 });
                t.Add("Time", boldfont);
                var cell = new PdfPCell(new Paragraph(((DayOfWeek)day).ToString(), boldfont));
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                t.AddCell(cell);

                foreach (var hh in m.FetchSlots(day))
                {
                    t.AddCell(hh.Time.ToString("h:mm tt"));
                    var sb = new StringBuilder();
                    var i = 0;
                    foreach (var u in hh.Owners)
                        if (u.Key > 0)
                        {
                            if (i>0)
                                sb.Append(",  ");
                            i++;
                            var n = "{0}) {1}".Fmt(i, u.Value);
                            n = n.Replace(" ", "\u00a0");
                            sb.Append(n);
                        }
                    t.AddCell(sb.ToString());
                }
                doc.Add(t);
                doc.Add(Chunk.NEXTPAGE);
            }
            doc.Close();
            Response.End();
        }
    }
}
