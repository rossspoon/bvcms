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
using System.Linq;

namespace CmsWeb
{
    public enum ListType
    {
        Ordered,
        Unordered,
        TableCell
    }
    public static class ViewExtensions2
    {
        public static string StandardCss()
        {
            return SquishIt.Framework.Bundle.Css()
                           .Add("/Content/jquery-ui-1.8.2.custom.css")
                           .Add("/Content/pager.css")
                           .Add("/Content/style2.css")
                           .Add("/Content/superfish.css")
                           .Add("/Content/supertabfish.css")
                           .Add("/Content/thickbox.css")
                           .Render("/Content/AllWebForm_#.css");
        }
        public static string RegisterScript(this HtmlHelper helper, string scriptFileName)
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
        public static string HyperLink(this HtmlHelper helper, string link, string text)
        {
            var tb = new TagBuilder("a");
            tb.InnerHtml = HttpUtility.HtmlEncode(text);
            var b = tb;
            b.MergeAttribute("href", link);
            return b.ToString(TagRenderMode.Normal);
        }
        public static string HyperLink(this HtmlHelper helper,
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
        public static string HyperLink(this HtmlHelper helper,
            string link,
            string text,
            string onclick)
        {
            return helper.HyperLink(link, text, onclick, null);
        }
        public static string HyperLink(this HtmlHelper helper,
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
        public static string PageSizesDropDown(this HtmlHelper helper, string id, string onchange)
        {
            var tb = new TagBuilder("select");
            tb.MergeAttribute("id", id);
            if (onchange.HasValue())
                tb.MergeAttribute("onchange", onchange);
            var sb = new StringBuilder();
            foreach (var o in PageSizes(null))
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
        public static IEnumerable<SelectListItem> PageSizes(this HtmlHelper helper)
        {
            var sizes = new int[] { 10, 25, 50, 75, 100, 200 };
            var list = new List<SelectListItem>();
            foreach (var size in sizes)
                list.Add(new SelectListItem { Text = size.ToString() });
            return list;
        }
        public static string HyperlinkIf(this HtmlHelper helper, bool condition, string link, string text, string onclick, object htmlAttributes)
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
        public static string SpanIf(this HtmlHelper helper, bool condition, string text, object htmlAttributes)
        {
            if (!condition)
                return null;
            var tb = new TagBuilder("span");
            var attr = new RouteValueDictionary(htmlAttributes);
            tb.InnerHtml = text;
            tb.MergeAttributes<string, object>(attr);
            return tb.ToString();
        }
        public static string Span(this HtmlHelper helper, string text, object htmlAttributes)
        {
            var tb = new TagBuilder("span");
            var attr = new RouteValueDictionary(htmlAttributes);
            tb.InnerHtml = text;
            tb.MergeAttributes<string, object>(attr);
            return tb.ToString();
        }
        public static bool IsDebug(this HtmlHelper helper)
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
        public static string DropDownList2(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list, bool visible)
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
        public static string DropDownList3(this HtmlHelper helper, string id, string name, IEnumerable<SelectListItem> list, string value)
        {
            var tb = new TagBuilder("select");
            if (id.HasValue())
                tb.MergeAttribute("id", id);
            tb.MergeAttribute("name", name);
            var sb = new StringBuilder();
            foreach (var o in list)
            {
                var ot = new TagBuilder("option");
                ot.MergeAttribute("value", o.Value);
                if (value == o.Value)
                    ot.MergeAttribute("selected", "selected");
                ot.SetInnerText(o.Text);
                sb.Append(ot.ToString());
            }
            tb.InnerHtml = sb.ToString();
            return tb.ToString();
        }
        public static string TextBox2(this HtmlHelper helper, string name, bool visible)
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
        public static string TextBoxClass(this HtmlHelper helper, string name, string @class)
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
        public static string DatePicker(this HtmlHelper helper, string name)
        {
            var tb = new TagBuilder("input");
            tb.MergeAttribute("type", "text");
            tb.MergeAttribute("id", name);
            tb.MergeAttribute("name", name);
            tb.MergeAttribute("class", "datepicker");
            var s = helper.TryGetModel(name);
            var viewDataValue = (DateTime?)helper.ViewData.Eval(name);
            tb.MergeAttribute("value", viewDataValue.FormatDate2());
            return tb.ToString();
        }
        public static string CheckBoxReadonly(this HtmlHelper helper, bool? ck)
        {
            var tb = new TagBuilder("input");
            tb.MergeAttribute("type", "checkbox");
            tb.MergeAttribute("disabled", "disabled");
            if (ck == true)
                tb.MergeAttribute("checked", "checked");
            return tb.ToString();
        }
        public static string CodeDesc(this HtmlHelper helper, string name, IEnumerable<SelectListItem> list)
        {
            var tb = new TagBuilder("span");
            var viewDataValue = helper.ViewData.Eval(name);
            var i = (int?)viewDataValue ?? 0;
            tb.InnerHtml = list.Single(v => v.Value == i.ToString()).Text;
            return tb.ToString();
        }
        public static string Hidden3(this HtmlHelper helper, string id, string name, object value)
        {
            var tb = new TagBuilder("input");
            if (id.HasValue())
                tb.MergeAttribute("id", id);
            tb.MergeAttribute("type", "hidden");
            tb.MergeAttribute("name", name);
            tb.MergeAttribute("value", value != null ? value.ToString() : "");
            return tb.ToString();
        }
        public static string Hidden3(this HtmlHelper helper, string name, object value)
        {
            return helper.Hidden3(null, name, value);
        }
        public static string HiddenIf(this HtmlHelper helper, string name, bool? include)
        {
            if (include == true)
            {
                var tb = new TagBuilder("input");
                tb.MergeAttribute("type", "hidden");
                tb.MergeAttribute("id", name);
                tb.MergeAttribute("name", name);
                var viewDataValue = helper.ViewData.Eval(name);
                tb.MergeAttribute("value", viewDataValue.ToString());
                return tb.ToString();
            }
            return "";
        }
        public static string HiddenIf(this HtmlHelper helper, string name, object value, bool? include)
        {
            if (include == true)
            {
                var tb = new TagBuilder("input");
                tb.MergeAttribute("type", "hidden");
                tb.MergeAttribute("id", name);
                tb.MergeAttribute("name", name);
                tb.MergeAttribute("value", value.ToString());
                return tb.ToString();
            }
            return "";
        }
    }
}