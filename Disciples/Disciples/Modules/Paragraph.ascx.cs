using System;
using System.Linq;
using DiscData;
using UtilityExtensions;

namespace Disciples.Modules
{
    public partial class Paragraph : System.Web.UI.UserControl
    {
        private Content _Content;
        public Content Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        private string contentName;
        public string ContentName
        {
            get { return contentName; }
            set
            {
                contentName = value;
                Content = DbUtil.Db.Contents.SingleOrDefault(c => c.ContentName == contentName);
            }
        }
        public string HeaderText
        {
            get { return Content.Title; }
        }
        public bool CanEdit { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ContentName != string.Empty)
            {
                if (Content != null)
                    Literal1.Text = Content.Body.Replace("\"~/", "\"{0}/".Fmt(Util.AppRoot));
                if (Content != null && Literal1.Text == string.Empty)
                    Literal1.Text = "Double click to add text...";
            }
            else
                Literal1.Text = "Set both the ContentID and PageName";

            if (Content != null && Page.User.IsInRole("Administrator") || CanEdit)
            {
                Panel1.CssClass = "contentboxadmin";
                string dblclick = string.Format("showPopWin('{0}?id={1}', 800, 650, null);",
                    Page.ResolveClientUrl("~/admin/CMSParagraph.aspx"), Content.ContentID);
                Panel1.Attributes.Add("ondblclick", dblclick);
                Panel1.Attributes.Add("onmouseover", "this.className='contentboxhover'");
                Panel1.Attributes.Add("onmouseout", "this.className='contentboxadmin'");
                Panel1.ToolTip = "Double Click to Edit";
            }
            else
                Panel1.CssClass = "contentbox";
        }
    }
}