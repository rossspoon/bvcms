<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Batch</h2>
    
    <%using(Html.BeginForm())
    { %>
    <%=Html.TextArea("text", ViewData["text"].ToString(), new { rows = "20", cols = "100", wrap = "off", style = "width:90%" }) %><br />
    <input type="submit" value="Submit" />
 <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
