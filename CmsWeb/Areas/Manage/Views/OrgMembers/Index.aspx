<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.OrgMembersModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Org Members Management</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Scripts/Pager.js")
        .Add("/Scripts/OrgMembers.js")
        .Render("/Content/OrgMembers_#.js")
            %>
    <form id="form" method="post" action="/OrgMembers/Move">
    <% Html.RenderPartial("List", Model); %>
    </form>
</asp:Content>
