<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
.red
{
    color: blue;
    font-weight:  bold;
    font-size: large;
}
</style>
    <h2><%=ViewData["EventName"] %></h2>
    <p class="red">
    Sorry, this event has filled. Check back later to see if any openings become available.
    </p>
</asp:Content>
