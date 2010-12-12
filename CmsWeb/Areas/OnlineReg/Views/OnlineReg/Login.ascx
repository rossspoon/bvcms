<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<% Model.ShowLoginInstructions = true; %>
<table class="login box" cellspacing="6" style="margin-top:1em" >
    <tr>
        <td><label for="username">Email or Username</label></td>
        <td><input id="username" type="text" name="m.username" value="<%=Model.username%>" tabindex="1" /></td>
        <td valign="bottom"><%= Html.ValidationMessage("username") %></td>
    </tr>
    <tr>
        <td><label for="password">Password</label></td>
        <td><input id="password" type="password" name="m.password" value="<%=Model.password%>" tabindex="2" /></td>
        <td><%= Html.ValidationMessage("password") %></td>
    </tr>
    <tr>
        <td colspan="3" align="center"><a href="/OnlineReg/Login/" class="submitbutton" tabindex="3">Login</a>
            <div style="width:300px"><%= Html.ValidationMessage("authentication")%></div></td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            Forgot <a href="/Account/ForgotUsername">username</a> or <a href="/Account/ForgotPassword">password</a>?
        </td>
        <td></td>
    </tr>
    <% if (Model.IsCreateAccount() || Model.ManagingSubscriptions())
       { %>
    <tr><td colspan="2" align="center"><a href="/OnlineReg/NoLogin" class="submitlink">Create an Account<br />or Send me a One-Time link</a><br /></td></tr>
    <% }
       else
       { %>
    <tr><td colspan="2" align="center"><a href="/OnlineReg/NoLogin" class="submitlink">Register without an account</a><br />
    <div style="width:200px">You will have the option to create an account later in the process.</div>
    </td></tr>
    <% } %>
</table>
