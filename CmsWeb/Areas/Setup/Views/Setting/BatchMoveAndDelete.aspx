<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Batch Move and Delete People</h2>
    <p>please paste data, tab delimited (from Excel), no headers, fromid then toid</p>
    
    <%using(Html.BeginForm())
    { %>
    <%=Html.TextArea("text", new { rows = "30", cols = "1000", wrap = "off", style = "width:90%" }) %><br />
    <input type="submit" value="Submit" />
 <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
