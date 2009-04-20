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
using DiscData;

namespace BellevueTeachers.Blog
{
    public partial class Archives : System.Web.UI.UserControl
    {
        public DiscData.Blog blog;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (blog == null)
                return;
            ArchiveList.DataSource = BlogPostController.FetchArchive(blog.Id);
            ArchiveList.DataBind();
        }
    }
}