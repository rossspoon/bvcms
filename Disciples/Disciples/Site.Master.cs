using System;
using CustomControls;
using DiscData;
using System.Web;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace BellevueTeachers
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public BreadCrumb AddCrumb(string Text, string urlformat, params object[] args)
        {
            return BreadCrumb1.Add(Text, urlformat, args);
        }
        public string HeadTitleText
        {
            set { HeadTitle.Text = value; }
        }
        public string HeadTitleLink
        {
            set { HeadTitle.NavigateUrl = value; }
        }
        public string HeadBylineText
        {
            set { HeadByline.Text = value; }
        }
        private string _HomeMenuText = "Home";
        public string HomeMenuText
        {
            get { return _HomeMenuText; }
            set { _HomeMenuText = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            NewMenu(MainMenu, HomeMenuText, "/");

            HtmlGenericControl m;
            m = NewTopMenu(MainMenu, "Blogs", "/Blog/");
            foreach (DiscData.Blog b in new BlogController().FetchAllForUser())
                NewMenu(m, b.Title, "/Blog/" + b.Name + ".aspx");

            //var fc = new ForumController();
            //if (fc.FetchAllForUser().Count() > 0)
            //{
            //    m = NewTopMenu(MainMenu, "Forums", "/Forum/");
            //    foreach (Forum f in fc.FetchAllForUser())
            //        NewMenu(m, f.Description, "/Forum/" + f.Id.ToString() + ".aspx");
            //}

            NewMenu(MainMenu, "Podcasting", "/Podcast/");

            m = NewTopMenu(MainMenu, "Bible Tools", "#");
            NewMenu(m, "One Year Bible Plan (mix)", "/Verse/DailyReading.aspx");
            NewMenu(m, "Chronological Reading Plan", "/Verse/DailyReadingChron.aspx");
            if (Page.User.Identity.IsAuthenticated)
                NewMenu(m, "Verse Memory", "/Verse/");
            else
                NewMenu(m, "Verse Memory (login or join to see this)", "/Verse/");

            NewMenu(MainMenu, "Resources", "/view/resources.aspx");

            if (Group.FetchAdminGroups().Count() > 0 || Page.User.IsInRole("Administrator"))
            {
                m = NewTopMenu(MainMenu, "Admin", "/AdminGroups.aspx");
                if (Page.User.IsInRole("Administrator"))
                {
                    NewMenu(m, "History", "/Admin/History.aspx");
                    NewMenu(m, "Users", "/Admin/Users.aspx");
                    NewMenu(m, "Scaffold", "/Admin/Scaffold.aspx");
                    NewMenu(m, "Groups", "/Admin/Roles.aspx");
                    NewMenu(m, "Pages", "/Admin/CMSPageList.aspx");
                    NewMenu(m, "Registration", "/Register.aspx");
                    NewMenu(m, "BatchJob", "/Admin/BatchJob.aspx");
                }
            }
            if (!Page.IsPostBack)
            {
                var pv = new PageVisit();
                if (Util.CurrentUser.UserId>0)
                    pv.UserId = Util.CurrentUser.UserId;
                var dtnow = DateTime.Now;
                pv.VisitTime = dtnow;
                pv.PageUrl = Request.Url.PathAndQuery;
                pv.PageTitle = Page.Title;
                pv.CreatedOn = dtnow;
                DbUtil.Db.PageVisits.InsertOnSubmit(pv);
                DbUtil.Db.SubmitChanges();
            }
        }
        private HtmlGenericControl NewMenu(HtmlGenericControl menu, string Text, string NavigateUrl)
        {
            var li = new HtmlGenericControl("li");
            var a = new HtmlAnchor();
            a.HRef = NavigateUrl;
            a.InnerText = Text;
            li.Controls.Add(a);
            menu.Controls.Add(li);
            return li;
        }
        private HtmlGenericControl NewTopMenu(HtmlGenericControl menu, string Text, string NavigateUrl)
        {
            var li = NewMenu(menu, Text, NavigateUrl);
            var ul = new HtmlGenericControl("ul");
            li.Controls.Add(ul);
            return ul;
        }
    }
}