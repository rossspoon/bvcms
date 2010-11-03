<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<table class="login box" cellspacing="6" style="width: auto;margin-top:1em" >
    <tr>
        <td><label for="username">Username</label></td>
        <td><input id="username" type="text" name="m.username" value="<%=Model.username%>" tabindex="1" /></td>
        <td valign="bottom"><%= Html.ValidationMessage("username") %></td>
    </tr>
    <tr>
        <td><label for="password">Password</label></td>
        <td><input id="password" type="password" name="m.password" value="<%=Model.password%>" tabindex="2" /></td>
        <td><%= Html.ValidationMessage("password") %></td>
    </tr>
    <tr>
        <td colspan="2" align="center"><a href="/OnlineReg/Login/" class="submitbutton" tabindex="3">Login</a>
            <div><%= Html.ValidationMessage("authentication")%></div></td>
        <td></td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            Forgot <a href="/Account/ForgotUsername">username</a> or <a href="/Account/ForgotPassword">password</a>?
        </td>
        <td></td>
    </tr>
</table>
<table class="login" cellspacing="6" style="margin-top:1em">
    <tr><td><a href="/CreateAccount">Create an account</a></td></tr>
    <tr><td><a href="/OnlineReg/NoLogin" class="submitlink">Register without an account</a></td></tr>
</table>
