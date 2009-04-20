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

namespace DiscData
{
    public partial class Page
    {
        public int Level = 0;

        public static Page LoadByUrl(string url)
        {
            return DbUtil.Db.Pages.SingleOrDefault(p => p.PageUrl == url);
        }
        public static Page LoadById(int id)
        {
            return DbUtil.Db.Pages.SingleOrDefault(p => p.PageID == id);
        }
    }
}