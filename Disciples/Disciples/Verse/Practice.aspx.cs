using System;
using CmsData;
using UtilityExtensions;

public partial class Verse_Practice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Util.IncludeCss(head, "~/App_Themes/VerseMemory/StyleSheet.css");
        Util.IncludeCss(head, "~/App_Themes/VerseMemory/colorbox.css");
        Util.IncludeScript(head, "~/js/jquery-1.3.2.min.js");
        Util.IncludeScript(head, "~/js/jquery.HighlightFade.js");
        Util.IncludeScript(head, "~/js/jquery.colorbox-min.js");
        Util.IncludeScript(head, "Practice-1.2.js");
        var v = Verse.LoadById(Request.QueryString<int>("id"));
        HiddenField1.Value = v.VerseText;
        Ref.Text = v.VerseRef;
    }
}
