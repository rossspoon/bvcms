<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PagerModel>" %>
<% CMSWeb.Models.PagerModel pm = ViewData.Model; %>
<table width="100%">
    <tr>
        <td class="pager">
        <%=Html.HyperlinkIf(pm.Page > 1, "#", "prev", "GotoPage({0})".Fmt(pm.Page - 1),
                    new { title = "goto page " + (pm.Page - 1), @class = "page-numbers" }) %>
        <% foreach (var p in pm.PageList())
           { %>
        <%=Html.SpanIf(p == 0, "&hellip;", new { @class = "page-numbers dots"}) %>
        <%=Html.HyperlinkIf(p > 0 && pm.Page != p, "#", p.ToString(), "GotoPage({0})".Fmt(p),
                    new { title = "goto page " + p, @class = "page-numbers" })%>
        <%=Html.SpanIf(pm.Page == p, p.ToString(), new { title = "goto page " + p, @class = "current page-numbers" })%>
        <% } %>
        <%=Html.HyperlinkIf(pm.Page < pm.LastPage, "#", "next", "GotoPage({0})".Fmt(pm.Page + 1),
                    new { title = "goto page " + (pm.Page + 1), @class = "page-numbers" })%>
        </td>
        <td class="page-sizer">
        <% foreach (var sz in pm.PageSizeList())
           { %>
        <%=Html.SpanIf(sz == pm.PageSize, sz.ToString(), new { @class = "current page-numbers" })%>
        <%=Html.HyperlinkIf(sz != pm.PageSize, "#", sz.ToString(), "SetPageSize({0})".Fmt(sz),
                    new { title = "show " + sz + " items per page", @class = "page-numbers" })%>

        <% } %> 
        <span class="page-numbers next">per page</span>
        </td>
    </tr>
</table>
