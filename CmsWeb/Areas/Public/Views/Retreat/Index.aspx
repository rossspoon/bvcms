<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RetreatModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
tr.alt
{
    background-color: #ddd;
    width: 100%;
}
</style>
    <script src="/Content/js/jquery-1.4.1.min.js" type="text/javascript"></script>    
    <script src="/Scripts/Event.js?v=5" type="text/javascript"></script>
    <h2><%=ViewData["EventName"] %></h2>
    <form class="DisplayEdit" action="/Retreat/CompleteRegistration/<%=ViewData["oid"] %><%=ViewData["testing"] %>" method="post">
    <% Html.RenderPartial("List", Model); %>
    </form>
</asp:Content>
