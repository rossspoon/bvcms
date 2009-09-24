using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace BellevueTeachers.Verse
{
    public partial class Chron : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dt = new DateTime(2008, 12, 31);
            var xdoc = XDocument.Parse(Resources.Resource1.Chron);
            var q = from p in xdoc.Descendants("read")
                    select new { Date = dt.AddDays(p.Attribute("day").ToInt()), Verse = p.Attribute("verses")};
            GridView1.DataSource = q;
            GridView1.DataBind();
        }
    }
}
