/* Author: David Carroll
 * Copyright (c) 2008, 2009 Bellevue Baptist Church 
 * Licensed under the GNU General Public License (GPL v2)
 * you may not use this code except in compliance with the License.
 * You may obtain a copy of the License at http://bvcms.codeplex.com/license 
 */
using System;
using System.Web;
using System.Text;
using System.Collections;
using System.Web.Mvc.Html;
using UtilityExtensions;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

public enum ListType
{
    Ordered,
    Unordered,
    TableCell
}
public static class ViewExtensions
{
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
            Protocol = "http://";

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
            b.MergeAttribute("onclick", "return " + onclick);
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
    //public static string ActionLinkIf(this System.Web.Mvc.HtmlHelper helper, bool condition, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
    //{
    //    if (!condition)
    //        return null;
    //    return helper.ActionLink(linkText, controllerName, routeValues, htmlAttributes);
    //}
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
    public static bool IsDebug(this System.Web.Mvc.HtmlHelper helper)
    {
        var d = false;
#if DEBUG
        d = true;
#endif
        return d;
    }
    private static string TryGetModel(this HtmlHelper helper, string name)
    {
        ModelState val;
        helper.ViewData.ModelState.TryGetValue(name, out val);
        string s = null;
        if (val != null)
            s = val.Value.AttemptedValue;
        return s;
    }
    public static string DropDownList2(this System.Web.Mvc.HtmlHelper helper, string name, IEnumerable<SelectListItem> list, bool visible)
    {
        var tb = new TagBuilder("select");
        tb.MergeAttribute("id", name);
        tb.MergeAttribute("name", name);
        if (!visible)
            tb.MergeAttribute("style", "display: none");
        var s = helper.TryGetModel(name);
        var sb = new StringBuilder();
        foreach (var o in list)
        {
            var ot = new TagBuilder("option");
            ot.MergeAttribute("value", o.Value);
            bool selected = false;
            if (s.HasValue())
                selected = s == o.Value;
            else if (o.Selected)
                selected = true;
            if (selected)
               ot.MergeAttribute("selected", "selected");
            ot.SetInnerText(o.Text);
            sb.Append(ot.ToString());
        }
        tb.InnerHtml = sb.ToString();
        return tb.ToString();
    }
    public static string TextBox2(this System.Web.Mvc.HtmlHelper helper, string name, bool visible)
    {
        var tb = new TagBuilder("input");
        tb.MergeAttribute("type", "text");
        tb.MergeAttribute("id", name);
        tb.MergeAttribute("name", name);
        if (!visible)
            tb.MergeAttribute("style", "display: none");
        var s = helper.TryGetModel(name);
        var viewDataValue = Convert.ToString(helper.ViewData.Eval(name));
        tb.MergeAttribute("value", s ?? viewDataValue);
        return tb.ToString();
    }
    public static string TextBoxClass(this System.Web.Mvc.HtmlHelper helper, string name, string @class)
    {
        var tb = new TagBuilder("input");
        tb.MergeAttribute("type", "text");
        tb.MergeAttribute("id", name);
        tb.MergeAttribute("name", name);
        tb.MergeAttribute("class", @class);
        var s = helper.TryGetModel(name);
        var viewDataValue = Convert.ToString(helper.ViewData.Eval(name));
        tb.MergeAttribute("value", s ?? viewDataValue);
        return tb.ToString();
    }
}
