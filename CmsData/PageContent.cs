using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;

namespace CmsData
{
    public partial class PageContent
    {
        public int Level = 0;

        public static PageContent LoadByUrl(string url)
        {
            return DbUtil.Db.PageContents.SingleOrDefault(p => p.PageUrl == url);
        }
        public static PageContent LoadById(int id)
        {
            return DbUtil.Db.PageContents.SingleOrDefault(p => p.PageID == id);
        }
    }
}