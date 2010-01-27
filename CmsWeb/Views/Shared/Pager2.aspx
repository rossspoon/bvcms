<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PagerModel2>" %>
<table class="pager">
    <tr>
    <td>
        <span class="page-numbers desc">Set # rows:</span>
        <%=Html.DropDownList("PageSize", Model.PageSizeList(), new { onchange = "$.setPageSize(this)" }) %>
    </td>
    <td><span class="page-numbers desc">Goto page:</span>
        <% if (Model.Page > 1)
           { %>
        <%=Html.HyperLink("#", "prev", "$.gotoPage(this,{0})".Fmt(Model.Page - 1), new { title = "goto page " + (Model.Page - 1), @class = "page-numbers" })%>
        <% }
           foreach (var p in Model.PageList())
           {
           if (p == 0)
           { %>
        <%=Html.Span("&hellip;", new { @class = "page-numbers dots" })%>
        <% }
           if (p > 0 && Model.Page != p)
           { %>
        <%=Html.HyperLink("#", p.ToString(), "$.gotoPage(this,{0})".Fmt(p), new { title = "goto page " + p, @class = "page-numbers" })%>
        <% }
           if (Model.Page == p)
           { %>
        <%=Html.Span(p.ToString(), new { title = "goto page " + p, @class = "current page-numbers" })%>
        <% }
           }
           if (Model.Page < Model.LastPage)
           { %>
        <%=Html.HyperLink("#", "next", "$.gotoPage(this,{0})".Fmt(Model.Page + 1), new { title = "goto page " + (Model.Page + 1), @class = "page-numbers" })%>
        <% } %>
    </td>
    </tr>
</table>
<%=Html.Hidden("Page")%>
<%=Html.Hidden("Sort")%>
<%=Html.Hidden("Direction")%>

