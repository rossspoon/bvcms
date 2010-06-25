<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OrgGroupsModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery.dimensions.js")
        .Add("/Content/js/jquery.tooltip.js")
        .Add("/Scripts/OrgGroups.js")
        .Render("/Content/OrgGroups_#.js")
            %>        
<form id="groupsform" action="/OrgGroups/Update" method="post">
<% Html.RenderPartial("Form", Model); %>
</form>
</asp:Content>

