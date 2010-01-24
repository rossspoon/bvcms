<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PagerModel2>" %>
<table width="100%">
    <tr>
    <td class="pager">
        <% if (Model.Page > 1)
           { %>
        <%=Html.HyperLink("#", "prev", "GotoPage(this,{0})".Fmt(Model.Page - 1), new { title = "goto page " + (Model.Page - 1), @class = "page-numbers" })%>
        <% }
           foreach (var p in Model.PageList())
           {
           if (p == 0)
           { %>
        <%=Html.Span("&hellip;", new { @class = "page-numbers dots" })%>
        <% }
           if (p > 0 && Model.Page != p)
           { %>
        <%=Html.HyperLink("#", p.ToString(), "GotoPage(this,{0})".Fmt(p), new { title = "goto page " + p, @class = "page-numbers" })%>
        <% }
           if (Model.Page == p)
           { %>
        <%=Html.Span(p.ToString(), new { title = "goto page " + p, @class = "current page-numbers" })%>
        <% }
           }
           if (Model.Page < Model.LastPage)
           { %>
        <%=Html.HyperLink("#", "next", "GotoPage(this,{0})".Fmt(Model.Page + 1), new { title = "goto page " + (Model.Page + 1), @class = "page-numbers" })%>
        <% } %>
    </td>
    <td class="page-sizer">
        <% foreach (var sz in Model.PageSizeList())
           {
           if (sz == Model.PageSize)
           { %>
        <%=Html.Span(sz.ToString(), new { @class = "current page-numbers" })%>
        <% }
           else
           { %>
        <%=Html.HyperLink("#", sz.ToString(), "SetPageSize(this,{0})".Fmt(sz), new { title = "show " + sz + " items per page", @class = "page-numbers" })%>
        <% }
           } %><span class="page-numbers next">per page</span>
    </td>
    </tr>
</table>
<%=Html.Hidden("Page")%>
<%=Html.Hidden("PageSize")%>
<%=Html.Hidden("Sort")%>
<%=Html.Hidden("Direction")%>

