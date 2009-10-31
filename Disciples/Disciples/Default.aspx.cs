using System;
using DiscData;
using System.Web;
using System.Web.Security;
using System.Linq;
using System.Web.UI.WebControls;
using UtilityExtensions;

namespace Disciples
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;
            if (!User.Identity.IsAuthenticated)
            {
                defaultTop.ContentName = "default_welcome";
                Panel1.Visible = true;
                Label1.Text = defaultTop.HeaderText;
            }
            else
            {
                var u = DbUtil.Db.CurrentUser;
                Panel1.Visible = false;
                var gc = Group.FetchUserGroups();
                if (!u.DefaultGroup.HasValue() && gc.Count() > 0)
                {
                    u.DefaultGroup = gc.First().Name;
                    DbUtil.Db.SubmitChanges();
                }

                string groupname = Request.QueryString<string>("group");
                Group g;
                switch (groupname)
                {
                    case "default":
                    case "default1":
                    case "default2":
                        defaultTop.ContentName = groupname + "_welcome";
                        break;
                    default:
                        if (groupname.HasValue())
                            g = Group.LoadById(groupname.ToInt());
                        else
                            g = Group.LoadByName(u.DefaultGroup);
                        if (g != null && !g.HasWelcomeText)
                        {
                            g.WelcomeText = Group.GetNewWelcome();
                            DbUtil.Db.SubmitChanges();
                        }
                        if (g != null && (User.IsInRole("Administrator") || g.IsMember || g.IsAdmin))
                        {
                            defaultTop.Content = g.WelcomeText;
                            if (User.IsInRole("Administrator") || g.IsAdmin || g.IsBlogger)
                                defaultTop.CanEdit = true;
                        }
                        else if (Group.FetchUserGroups().Count() > 0)
                            defaultTop.ContentName = "default2_welcome";
                        else
                            defaultTop.ContentName = "default1_welcome";
                        break;
                }
                Label1.Text = defaultTop.HeaderText;
                
            }
        }

        protected void Login1_Authenticate(object sender, System.Web.UI.WebControls.AuthenticateEventArgs e)
        {
            e.Authenticated = Membership.ValidateUser(Login1.UserName, Login1.Password);
        }
    }
}