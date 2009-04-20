using System;
using System.Web;
using System.Text;
using System.Collections;
using System.Web.Mvc.Html;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

public enum ListType
{
    Ordered,
    Unordered,
    TableCell
}
public static class ViewExtensions
{
    public static string AppSetting(this System.Web.Mvc.HtmlHelper helper, string setting)
    {
        return ConfigurationManager.AppSettings[setting];
    }
    public static string RegisterScript(this System.Web.Mvc.HtmlHelper helper, string scriptFileName)
    {
        string scriptRoot = VirtualPathUtility.ToAbsolute("~/Scripts");
        string scriptFormat = "<script src=\"{0}/{1}\" type=\"text/javascript\"></script>\r\n";
        return string.Format(scriptFormat, scriptRoot, scriptFileName);

    }
    public static string ToFormattedList(this IEnumerable list, ListType listType)
    {
        StringBuilder sb = new StringBuilder();
        IEnumerator en = list.GetEnumerator();

        string outerListFormat = "";
        string listFormat = "";

        switch (listType)
        {
            case ListType.Ordered:
                outerListFormat = "<ol>{0}</ol>";
                listFormat = "<li>{0}</li>";
                break;
            case ListType.Unordered:
                outerListFormat = "<ul>{0}</ul>";
                listFormat = "<li>{0}</li>";
                break;
            case ListType.TableCell:
                outerListFormat = "{0}";
                listFormat = "<td>{0}</td>";
                break;
            default:
                break;
        }
        return string.Format(outerListFormat, ToFormattedList(list, listFormat));
    }
    public static string ToFormattedList(IEnumerable list, string format)
    {
        var sb = new StringBuilder();
        foreach (object item in list)
            sb.AppendFormat(format, item.ToString());
        return sb.ToString();
    }

    public static string GetSiteUrl(this ViewPage pg)
    {
        string Port = pg.ViewContext.HttpContext.Request.ServerVariables["SERVER_PORT"];
        if (Port == null || Port == "80" || Port == "443")
            Port = "";
        else
            Port = ":" + Port;

        string Protocol = pg.ViewContext.HttpContext.Request.ServerVariables["SERVER_PORT_SECURE"];
        if (Protocol == null || Protocol == "0")
            Protocol = "http://";
        else
            Protocol = "https://";

        string appPath = pg.ViewContext.HttpContext.Request.ApplicationPath;
        if (appPath == "/")
            appPath = "";

        string sOut = Protocol + pg.ViewContext.HttpContext.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
        return sOut;
    }
    public static string HyperLink(this System.Web.Mvc.HtmlHelper helper, string link, string text)
    {
        var tb = new TagBuilder("a");
        tb.InnerHtml = HttpUtility.HtmlEncode(text);
        var b = tb;
        b.MergeAttribute("href", link);
        return b.ToString(TagRenderMode.Normal);
    }
    public static string HyperLink(this System.Web.Mvc.HtmlHelper helper, 
        string link, 
        string text, 
        object htmlAttributes)
    {
        var attr = new RouteValueDictionary(htmlAttributes);
        var tb = new TagBuilder("a");
        tb.InnerHtml = HttpUtility.HtmlEncode(text);
        var b = tb;
        b.MergeAttribute("href", link);
        b.MergeAttributes<string, object>(attr);
        return b.ToString(TagRenderMode.Normal);
    }
    public static string HyperLink(this System.Web.Mvc.HtmlHelper helper,
        string link,
        string text,
        string onclick)
    {
        return helper.HyperLink(link, text, onclick, null);
    }
    public static string HyperLink(this System.Web.Mvc.HtmlHelper helper,
        string link,
        string text,
        string onclick,
        object htmlAttributes)
    {
        var tb = new TagBuilder("a");
        tb.InnerHtml = HttpUtility.HtmlEncode(text);
        var b = tb;
        b.MergeAttribute("href", link);
        if (onclick.HasValue())
            b.MergeAttribute("onclick", onclick);
        var attr = new RouteValueDictionary(htmlAttributes);
        b.MergeAttributes<string, object>(attr);
        return b.ToString(TagRenderMode.Normal);
    }
    public static string PageSizesDropDown(this System.Web.Mvc.HtmlHelper helper, string id, string onchange)
    {
        var tb = new TagBuilder("select");
        tb.MergeAttribute("id", id);
        if (onchange.HasValue())
            tb.MergeAttribute("onchange", onchange);
        var sb = new StringBuilder();
        foreach (var o in Util.PageSizes())
        {
            var ot = new TagBuilder("option");
            ot.MergeAttribute("value", o.Value);
            if (o.Selected)
                ot.MergeAttribute("selected", "selected");
            ot.SetInnerText(o.Text);
            sb.Append(ot.ToString());
        }
        tb.InnerHtml = sb.ToString();
        return tb.ToString();
    }
    public static string HyperlinkIf(this System.Web.Mvc.HtmlHelper helper, bool condition, string link, string text, string onclick, object htmlAttributes)
    {
        if (!condition)
            return null;
        return helper.HyperLink(link, text, onclick, htmlAttributes);
    }
    public static string ActionLinkIf(this System.Web.Mvc.HtmlHelper helper, bool condition, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
    {
        if (!condition)
            return null;
        return helper.ActionLink(linkText, controllerName, routeValues, htmlAttributes);
    }
    public static string SpanIf(this System.Web.Mvc.HtmlHelper helper, bool condition, string text, object htmlAttributes)
    {
        if (!condition)
            return null;
        var tb = new TagBuilder("span");
        var attr = new RouteValueDictionary(htmlAttributes);
        tb.InnerHtml = text;
        tb.MergeAttributes<string, object>(attr);
        return tb.ToString();
    }
}
