using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

[DefaultProperty("ThemeVariableName")]
[ToolboxData("<{0}:Styles runat=\"server\"></{0}:Styles>")]
[Themeable(true)]
public class Styles : PlaceHolder
{

    [Bindable(true)]
    [Category("Appearance")]
    [DefaultValue("%Theme")]
    [Localizable(false)]
    [Description("Name of the variable that should be replaced by the actual theme path")]
    public string ThemeVariableName
    {
        get
        {
            String s = (String)ViewState["ThemeVariableName"];
            return ((s == null) ? "%Theme" : s);
        }

        set
        {
            ViewState["ThemeVariableName"] = value;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (this.Visible)
        {
            // Hide any server side css 
            foreach (Control c in this.Page.Header.Controls)
                if (c is HtmlControl && ((HtmlControl)c).TagName.Equals("link",
                                StringComparison.OrdinalIgnoreCase))
                    c.Visible = false;

            // Replace ThemeVariableName with actual theme path
            Regex reg = new Regex(ThemeVariableName,
                                  System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            foreach (Control c in this.Controls)
                if (c is LiteralControl)
                {
                    LiteralControl l = (LiteralControl)c;
                    l.Text = reg.Replace(l.Text, this.ThemePath);
                }
        }
    }

    public string ThemePath
    {
        get
        {
            return String.Format("{0}/App_Themes/{1}",
                                 this.Page.Request.ApplicationPath,
                                 this.Page.Theme);
        }
    }
}
