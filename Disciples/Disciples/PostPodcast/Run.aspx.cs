using System;
using System.Web.UI;
using DiscData;

public partial class PostPodcast_Run : System.Web.UI.Page
{
    public string directLink;
    protected void Page_Load(object sender, EventArgs e)
    {
        var tok = new TemporaryToken();
        tok.Id = Guid.NewGuid();
        tok.CreatedBy = DbUtil.Db.CurrentUser.UserId;
        tok.CreatedOn = DateTime.Now;
        DbUtil.Db.TemporaryTokens.InsertOnSubmit(tok);
        DbUtil.Db.SubmitChanges();
        directLink = "PostPodcast.application?id=" + tok.Id.ToString();
        string s = @"
runtimeVersion = ""2.0.0"";
directLink = ""{0}"";
function Initialize()
{{
  if (HasRuntimeVersion(runtimeVersion))
    BootstrapperSection.style.display = ""none"";
}}
function HasRuntimeVersion(v)
{{
  var va = GetVersion(v);
  var i;
  var a = navigator.userAgent.match(/\.NET CLR [0-9.]+/g);
  if (a != null)
    for (i = 0; i < a.length; ++i)
      if (CompareVersions(va, GetVersion(a[i])) <= 0)
		return true;
  return false;
}}
function GetVersion(v)
{{
  var a = v.match(/([0-9]+)\.([0-9]+)\.([0-9]+)/i);
    return a.slice(1);
}}
function CompareVersions(v1, v2)
{{
  for (i = 0; i < v1.length; ++i)
  {{
    var n1 = new Number(v1[i]);
    var n2 = new Number(v2[i]);
    if (n1 < n2)
      return -1;
    if (n1 > n2)
      return 1;
  }}
  return 0;
}}

function Button1_onclick() {{
    window.location.href = directLink;
}}
";
        Page.ClientScript.RegisterClientScriptBlock(typeof(PostPodcast_Run), "podcast",
            string.Format(s, directLink), true);
    }

}
