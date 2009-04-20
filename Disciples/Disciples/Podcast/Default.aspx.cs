using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Podcast_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public string ITunesLink(object userid)
    {
        string r = "itpc://" + HttpContext.Current.Request.Url.Authority + Util.ResolveUrl(string.Format("~/podcast/feed/{0}.aspx", userid));
        return r;
    }
    public string RssLink(object userid)
    {
        string r = "http://" + HttpContext.Current.Request.Url.Authority + Util.ResolveUrl(string.Format("~/podcast/feed/{0}.aspx", userid));
        return r;
    }
}
