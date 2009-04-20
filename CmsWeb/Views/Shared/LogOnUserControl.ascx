<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (Request.IsAuthenticated) { %>
        Hi <%= Html.Encode(Page.User.Identity.Name) %><br />
        <a href="/ChangePassword.aspx">Change Password</a> | <%= Html.ActionLink("Log Off", "LogOff", "Account") %>
<% } else { %> 
        <a href="/Login.aspx">Login</a>
<% } %>
