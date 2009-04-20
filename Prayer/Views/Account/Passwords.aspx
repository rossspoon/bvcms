<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<string>>" %>

<asp:Content ID="loginHead" ContentPlaceHolderID="head" runat="server">
    <title>Passwords</title>
</asp:Content>
<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
<% foreach (var p in ViewData.Model)
   { %>
   <%=p%><br />
<% } %>
</asp:Content>
