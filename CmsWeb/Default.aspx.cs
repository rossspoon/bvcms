using System;
using System.Linq;
using System.Xml.Linq;
using System.Data.Linq.SqlClient;
using CmsData;
using UtilityExtensions;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;

namespace CMSWeb
{
    public partial class Default : System.Web.UI.Page
    {
        private const string STR_UseOldDialog = "Use <span style='color:DarkGray'>OLD</span> SearchAdd Dialog";
        private const string STR_UseNewDialog = "Use <span style='color:Red'>NEW</span> SearchAdd Dialog";
        private const string STR_UseOldOrgPage = "Use <span style='color:DarkGray'>OLD</span> Organization Page";
        private const string STR_UseNewOrgPage = "Use <span style='color:Red'>NEW</span> Organization Page";
        protected void Page_Load(object sender, EventArgs e)
        {
            BindBirthdays();
            BindMyInvolvements();
            BindNews();
            UseOldNewDialog.Text = DbUtil.Db.UserPreference("olddialog").ToBool() ? STR_UseNewDialog : STR_UseOldDialog;
            UseOldNewOrgPage.Text = DbUtil.Db.UserPreference("neworgpage").ToBool() ? STR_UseOldOrgPage : STR_UseNewOrgPage;
        }
        private void BindBirthdays()
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
            var link = grdMyInvolvement.Columns[0] as HyperLinkField;
            if (!DbUtil.Db.UserPreference("neworgpage").ToBool())
                link.DataNavigateUrlFormatString = "~/Organization.aspx?id={0}";
            else
                link.DataNavigateUrlFormatString = "/Organization/Index/{0}";
            BFClass.Visible = org != null;
            if (BFClass.Visible)
            {
                BFClass.Text = org.FullName;
                if (!DbUtil.Db.UserPreference("neworgpage").ToBool())
                    BFClass.NavigateUrl = "~/Organization.aspx?id=" + org.OrganizationId;
                else
                    BFClass.NavigateUrl = "/Organization/Index/" + org.OrganizationId;
            }

            var q2 = from p in q
                     let nextbd = DbUtil.Db.NextBirthday(p.PeopleId)
                     where SqlMethods.DateDiffDay(UtilityExtensions.Util.Now, nextbd) <= 15
                     orderby nextbd
                     select new { Birthday = nextbd, Name = p.Name, Id = p.PeopleId };
            Birthdays.DataSource = q2;
            Birthdays.DataBind();
        }
        private void BindMyInvolvements()
        {
            var u = DbUtil.Db.Users.SingleOrDefault(us => us.UserId == Util.UserId);
            if (u == null)
                return;

            if (u.PeopleId.HasValue)
            {
                var q = from om in DbUtil.Db.OrganizationMembers
                        where om.PeopleId == u.PeopleId
                        where (om.Pending ?? false) == false
                        where !(om.Organization.SecurityTypeId == 3 && Util.OrgMembersOnly)
                        select new 
                        { 
                            Name = om.Organization.OrganizationName, 
                            MemberType = om.MemberType.Description,
                            Id = om.OrganizationId
                        };
                grdMyInvolvement.DataSource = q;
                grdMyInvolvement.DataBind();
                grdMyInvolvement.Visible = true;
            }
            else
                grdMyInvolvement.Visible = false;
        }
        protected void UseDialog_Click(object sender, EventArgs e)
        {
            if (DbUtil.Db.UserPreference("olddialog").ToBool())
            {
                DbUtil.Db.SetUserPreference("olddialog", "false");
                UseOldNewDialog.Text = STR_UseOldDialog;
            }
            else
            {
                DbUtil.Db.SetUserPreference("olddialog", "true");
                UseOldNewDialog.Text = STR_UseNewDialog;
            }
            Response.Redirect("/");
        }
        protected void UseOrgPage_Click(object sender, EventArgs e)
        {
            if (DbUtil.Db.UserPreference("neworgpage").ToBool())
            {
                DbUtil.Db.SetUserPreference("neworgpage", "false");
                UseOldNewOrgPage.Text = STR_UseOldOrgPage;
            }
            else
            {
                DbUtil.Db.SetUserPreference("neworgpage", "true");
                UseOldNewOrgPage.Text = STR_UseNewOrgPage;
            }
            Response.Redirect("/");
        }
        private void BindNews()
        {
            var feedurl = DbUtil.Settings("BlogFeedUrl", "http://feeds2.feedburner.com/Bvcms-Blog");

            BlogLink.NavigateUrl = DbUtil.Settings("BlogAppUrl", "http://www.bvcms.com/blog/");

            var wr = new WebClient();
            var feed = DbUtil.Db.RssFeeds.FirstOrDefault(r => r.Url == feedurl);

            HttpWebRequest req = null;

            try
            {
                req = WebRequest.Create(feedurl) as HttpWebRequest;
            }
            catch
            {
            }

            if (feed != null)
            {
                req.IfModifiedSince = feed.LastModified.Value;
                req.Headers.Add("If-None-Match", feed.ETag);
            }
            else
            {
                feed = new RssFeed();
                DbUtil.Db.RssFeeds.InsertOnSubmit(feed);
                feed.Url = feedurl;
            }

            if (req != null)
            {
                try
                {
                    var resp = req.GetResponse() as HttpWebResponse;
                    feed.LastModified = resp.LastModified;
                    feed.ETag = resp.Headers["ETag"];
                    var sr = new StreamReader(resp.GetResponseStream());
                    feed.Data = sr.ReadToEnd();
                    sr.Close();
                    DbUtil.Db.SubmitChanges();
                }
                catch (WebException)
                {
                }
                XDocument rssFeed = XDocument.Parse(feed.Data);

                var posts = from item in rssFeed.Descendants("item")
                            let au = item.Element("author")
                            select new
                            {
                                Title = item.Element("title").Value,
                                Published = DateTime.Parse(item.Element("pubDate").Value),
                                Url = item.Element("link").Value,
                                Author = au != null ? au.Value : "David Carroll",
                            };

                NewsGrid.DataSource = posts;
                NewsGrid.DataBind();
           }
        }
    }
}
