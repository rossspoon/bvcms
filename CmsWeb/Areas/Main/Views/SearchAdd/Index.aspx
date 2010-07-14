<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.SearchModel>" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
<%= SquishIt.Framework.Bundle.Css()
        .Add("/Content/jquery-ui-1.8.2.custom.css")
        .Add("/Content/Dialog.css")
        .Add("/Content/jquery.tooltip.css")
    .Render("/Content/SearchAdd_#.css")
%>
</head>
<body>
<%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery-1.4.2.js")
        .Add("/Content/js/jquery-ui-1.8.2.custom.js")
        .Add("/Content/js/jquery.hoverIntent.js")
        .Add("/Content/js/jquery.tooltip.js")
        .Add("/Scripts/SearchAdd.js")
    .Render("/Content/SearchAdd_#.js")
%>
<form action="/SearchAdd/Complete/" method="post" class="DisplayEdit">
<% Html.RenderPartial("SearchPerson", Model); %>
</form>
</body>
</html>