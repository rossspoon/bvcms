using System;
using DiscData;
using UtilityExtensions;

public partial class Verse_Practice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Util.IncludeScript(header, "Practice.js");
        var v = Verse.LoadById(Request.QueryString<int>("id"));
        HiddenField1.Value = v.VerseText;
        Ref.Text = v.VerseRef;
    }
}
