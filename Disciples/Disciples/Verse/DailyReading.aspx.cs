using System;
using System.Data;
using System.Web.UI;
using DiscData;
using System.Web;
using System.Linq;
using System.Collections.Generic;

public partial class Verse_DailyReading : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int? day = Request.QueryString<int?>("day");
        if (day.HasValue)
            BibleStartDate = DateTime.Today.AddDays(-day.Value);
        PlaceHolder1.Controls.Add(new Readings(BibleStartDate));
        //http://feeds.feedburner.com/~r/dailyaudiobible/~5/181911114/November08-2007.mp3
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
public class Readings : Control
{
    private DateTime Start;
    public Readings(DateTime Start)
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
        writer.WriteLine("\t<th>Date</th><th>Old Testament</th><th>New Testament</th><th>Psalms</th><th>Proverbs</th></tr>");
        writer.WriteLine("</tr>");

        var q = from rp in DbUtil.Db.ReadPlans
                orderby rp.Day, rp.Section
                select rp;
        var rpa = q.ToArray();
        int d = 0;
        var ss = new string[4];
        for (DateTime day = Start; day < End; day = day.AddDays(1))
        {
            writer.WriteLine("<tr>");
            writer.Write("\t" + "<td>");
            string doysty = "";
            if (day == DateTime.Today)
            {
                writer.Write("<a name=\"doy\"/>");
                doysty = " style='font-weight: bold'";
            }

            string Url = "http://www.biblegateway.com/cgi-bin/bible?version=NLT&passage=";
            for (int i = 0; i < 4; i++)
            {
                ss[i] = Verse.FormatRef(rpa[d+i]);
                if (i > 0)
                    Url += ";";
                Url += ss[i].Replace(" ", "+");
            }
            d += 4;
            Url += "&showfn=off&showxref=off&interface=largeprint";
            writer.WriteLine("<a target='_blank' href='{0}'{1}>{2:MM/dd/yy}</a>", Url, doysty, day);
            writer.Write("<a href='DailyReading.aspx?day={0}' title='make this the current day'"
                + " style='font-size: 7pt'>current</a>", day.Subtract(Start).Days);
            if (day <= DateTime.Today && day >= first)
                writer.Write(" <a href='http://dailyaudiobible.dreamhosters.com/mp3/{0:MMMMdd-yyyy}.mp3' "
                + "style='font-size: 7pt'>listen</a>", day);
            writer.WriteLine("</td>");
            for (int i = 0; i < 4; i++)
                writer.WriteLine("\t<td>{0}</td>", ss[i]);
            writer.WriteLine("</tr>");
        }
        writer.WriteLine("</table>");
    }
}
