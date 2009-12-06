using System;
using System.Data;
using System.Web.UI;
using CmsData;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using UtilityExtensions;

public partial class Verse_DailyReadingChron : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int? day = Request.QueryString<int?>("day");
        if (day.HasValue)
            BibleStartDate = DateTime.Today.AddDays(-day.Value);
        PlaceHolder1.Controls.Add(new Readings2(BibleStartDate));
    }

    private System.DateTime _BibleStartDate = System.DateTime.MinValue;
    public System.DateTime BibleStartDate
    {
        set
        {
            HttpCookie cookie = Response.Cookies.Get("BibleReading");
            if (cookie == null)
            {
                cookie = new HttpCookie("BibleReading");
                Response.Cookies.Add(cookie);
            }
            cookie["StartDate"] = value.ToString();
            cookie.Expires = System.DateTime.Now.AddMonths(12);
            _BibleStartDate = value;
        }
        get
        {
            if (_BibleStartDate != System.DateTime.MinValue)
                return _BibleStartDate;
            if (Request.Cookies["BibleReading"] != null)
            {
                HttpCookie ck = Request.Cookies["BibleReading"];
                _BibleStartDate = DateTime.Parse(ck.Values["StartDate"]);
                while (DateTime.Today.Subtract(_BibleStartDate).Days >= 365)
                    BibleStartDate = _BibleStartDate.AddYears(1);
            }
            else
                BibleStartDate = new System.DateTime(System.DateTime.Now.Year, 1, 1);
            return _BibleStartDate;
        }
    }
}
public class Readings2 : Control
{
    private DateTime Start;
    public Readings2(DateTime Start)
    {
        this.Start = Start;
    }
    protected override void Render(HtmlTextWriter writer)
    {
        int doy = DateTime.Today.Subtract(Start).Days;
        DateTime End = Start.AddDays(365);
        DateTime first = DateTime.Parse("1/1/" + DateTime.Today.Year);

        base.Render(writer);
        writer.WriteLine("<table border=1>");
        writer.WriteLine("<tr>");
        writer.WriteLine("\t<th>Date</th><th>Verses</th></tr>");
        writer.WriteLine("</tr>");

        var xdoc = XDocument.Parse(Resources.Resource1.Chron);
        var q = from p in xdoc.Descendants("read")
                orderby p.Attribute("day").Value.ToInt()
                select new
                {
                    Date = Start.AddDays(p.Attribute("day").Value.ToInt()-1),
                    Verse = p.Attribute("verses").Value
                };
        var rpa = q.ToArray();
        var ss = new string[4];
        foreach(var p in q)
        {
            writer.WriteLine("<tr>");
            writer.Write("\t" + "<td>");
            string doysty = "";
            if (p.Date == DateTime.Today)
            {
                writer.Write("<a name=\"doy\"/>");
                doysty = " style='font-weight: bold'";
            }

            string Url = string.Format("http://www.biblegateway.com/cgi-bin/bible?version=NLT&passage={0}&showfn=off&showxref=off&interface=largeprint",
                p.Verse.Replace(" ", "+"));
            writer.WriteLine("<a target='_blank' href='{0}'{1}>{2:MM/dd/yy}</a>", Url, doysty, p.Date);
            writer.Write("<a href='DailyReadingChron.aspx?day={0}' title='make this the current day'"
                + " style='font-size: 7pt'>current</a>", p.Date.Subtract(Start).Days);
            writer.Write("</td>");
            writer.Write("<td>{0}</td>", p.Verse);
            writer.WriteLine("</tr>");
        }
        writer.WriteLine("</table>");
    }
}
