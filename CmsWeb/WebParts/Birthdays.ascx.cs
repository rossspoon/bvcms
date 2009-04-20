using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;
using CmsData;
using UtilityExtensions;

namespace CMSWeb.WebParts
{
    public partial class Birthdays : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = DbUtil.Db.CurrentUser;
            if (user == null || user.Person == null)
                return;
            var n = UtilityExtensions.Util.Now;
            var tag = DbUtil.Db.FetchOrCreateTag("TrackBirthdays", user.PeopleId, DbUtil.TagTypeId_Personal);
            var q = tag.People();
            if (q.Count() == 0)
                q = from p in DbUtil.Db.People
                    where p.OrganizationMembers.Any(om => om.OrganizationId == user.Person.BibleFellowshipClassId)
                    select p;
            var org = DbUtil.Db.Organizations.SingleOrDefault(o => o.OrganizationId == user.Person.BibleFellowshipClassId);
            BFClass.Visible = org != null;
            if (BFClass.Visible)
            {
                BFClass.Text = org.FullName;
                BFClass.NavigateUrl = "~/Organization.aspx?id=" + org.OrganizationId;
            }

            var q2 = from p in q
                     let nextbd = DbUtil.Db.NextBirthday(p.PeopleId)
                     where SqlMethods.DateDiffDay(UtilityExtensions.Util.Now, nextbd) <= 15
                     orderby nextbd
                     select new { Birthday = nextbd, Name = p.Name, Id = p.PeopleId };
            GridView1.DataSource = q2;
            GridView1.DataBind();
        }
    }
}