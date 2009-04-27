<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PagerModel>" %>
<table width="100%">
    <tr>
        <td class="pager">
        <%=Html.HyperlinkIf(Model.Page > 1, "#", "prev", "GotoPage({0})".Fmt(Model.Page - 1),
                    new { title = "goto page " + (Model.Page - 1), @class = "page-numbers" }) %>
        <% foreach (var p in Model.PageList())
           { %>
        <%=Html.SpanIf(p == 0, "&hellip;", new { @class = "page-numbers dots"}) %>
        <%=Html.HyperlinkIf(p > 0 && Model.Page != p, "#", p.ToString(), "GotoPage({0})".Fmt(p),
                    new { title = "goto page " + p, @class = "page-numbers" })%>
        <%=Html.SpanIf(Model.Page == p, p.ToString(), new { title = "goto page " + p, @class = "current page-numbers" })%>
        <% } %>
        <%=Html.HyperlinkIf(Model.Page < Model.LastPage, "#", "next", "GotoPage({0})".Fmt(Model.Page + 1),
                    new { title = "goto page " + (Model.Page + 1), @class = "page-numbers" })%>
        </td>
        <td class="page-sizer">
        <% foreach (var sz in Model.PageSizeList())
           { %>
        <%=Html.SpanIf(sz == Model.PageSize, sz.ToString(), new { @class = "current page-numbers" })%>
        <%=Html.HyperlinkIf(sz != Model.PageSize, "#", sz.ToString(), "SetPageSize({0})".Fmt(sz),
                    new { title = "show " + sz + " items per page", @class = "page-numbers" })%>

        <% } %> 
        <span class="page-numbers next">per page</span>
        </td>
    </tr>
</table>
