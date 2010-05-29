<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OrgMembersModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Org Members Management</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Scripts/Pager.js?v=1" type="text/javascript"></script>
    <script src="/Scripts/OrgMembers.js?v=2" type="text/javascript"></script>
    
    <form id="form" method="post" action="/OrgMembers/Move">
    <% Html.RenderPartial("List", Model); %>
    </form>
</asp:Content>
