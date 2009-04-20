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
using DiscData;

namespace Prayer.Views.Signup
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
            var m = new Prayer.Models.SignupModel(Util.CurrentUser);
            for (int day = 0; day < 7; day++)
            {
                var t = new PdfPTable(3 + 1);
                t.HeaderRows = 2;
                t.WidthPercentage = 100;
                t.SetWidths(new int[] { 13, 29, 29, 29 });
                t.AddCell("");
                var cell = new PdfPCell(new Paragraph(((DayOfWeek)day).ToString(), boldfont));
                cell.Colspan = 3;
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                t.AddCell(cell);
                t.AddCell(new Phrase("Time", boldfont));
                t.AddCell(new Phrase("Cube A", boldfont));
                t.AddCell(new Phrase("Cube B", boldfont));
                t.AddCell(new Phrase("Cube C", boldfont));

                foreach (var hh in m.FetchSlots(day))
                {
                    t.AddCell(hh.Time.ToString("h:mm tt"));
                    foreach (var u in Owners(hh.Owners))
                        if (u.Key > 0)
                            t.AddCell(u.Value);
                        else
                            t.AddCell("");
                }
                doc.Add(t);
                doc.Add(Chunk.NEXTPAGE);
            }
            doc.Close();
            Response.End();
        }

        private IEnumerable<KeyValuePair<int, string>> Owners(Dictionary<int, string> o)
        {
            int i = 0;
            foreach (var owner in o)
            {
                if (i == 3)
                    break;
                i++;
                yield return owner;
            }
            for (; i < 3; i++)
                yield return new KeyValuePair<int, string>(0, "");
        }
    }
}
