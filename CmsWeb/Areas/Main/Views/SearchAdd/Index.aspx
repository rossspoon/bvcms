<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchModel>" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Content/Dialog.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
    <%= SquishIt.Framework.Bundle.JavaScript()
            .Add("/Content/js/jquery-1.4.2.js")
            .Add("/Content/js/jquery.hoverIntent.js")
            .Add("/Content/js/jquery.tooltip.js")
            .Add("/Scripts/SearchAdd.js")
        .Render("/Content/SearchAdd_#.js")
            %>
</head>
<body>
<form action="/SearchAdd/Complete/" method="post" class="DisplayEdit">
<% Html.RenderPartial("SearchPerson", Model); %>
</form>
</body>
</html>