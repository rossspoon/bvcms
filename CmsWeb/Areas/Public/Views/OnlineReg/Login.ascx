<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<table class="login box" cellspacing="6" style="width: auto" >
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
<table class="login" cellspacing="6">
    <tr><td>You can create an <a href="/CreateAccount">account</a> to login with. 
            See <a target="_blank" href="http://www.bvcms.com/Page/WhyMemberAccount">benefits</a></td></tr>
    <tr><td>Or you can register <a href="/OnlineReg/NoLogin" class="submitlink">without an account</a> too.</td></tr>
</table>
