<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchModel>" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="/Content/Dialog.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery.cluetip.css" rel="stylesheet" type="text/css" />
    <script src="/Content/js/jquery-1.4.1.min.js" type="text/javascript"></script>    
    <script src="/Content/js/jquery.cluetip.min.js" type="text/javascript"></script>    
    <script src="/Scripts/SearchAdd.js" type="text/javascript"></script>
</head>
<body>
    <% 
        var d = new Dictionary<string, object>();
        d.Add("class", "DisplayEdit");
        var rv = ViewData["rv"] as RouteValueDictionary;
        using (Html.BeginForm("CompleteRegistration", "SearchAdd", rv, FormMethod.Post, d))
        {
            Html.RenderPartial("SearchPerson", Model);
        }
    %>
</body>
</html>