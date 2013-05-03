<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (Request.IsAuthenticated) { %>
        Hi <%= Html.Encode(Page.User.Identity.Name) %><br />
        <a href="/Account/ChangePasswordx">Change Password</a> | <a href="/Account/LogOff/">Log Off</a>
<% } else { %> 
        <a href="/Logon">Login</a>
<% } %>
