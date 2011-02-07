﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<% Model.ShowLoginInstructions = true; %>
<div class="login box" style="margin-top:1em;width=350px" >
    <div style="margin: 10px">
        <div><label for="username">Username or Email</label></div>
        <div><input id="username" type="text" name="m.username" value="<%=Model.username%>" tabindex="1" /></div>
        <div><%=Html.ValidationMessage("username") %></div>
    </div>
    <div style="margin: 10px">
        <div><label for="password">Password</label></div>
        <div><input id="password" type="password" name="m.password" value="<%=Model.password%>" tabindex="2" /></div>
        <div><%= Html.ValidationMessage("password") %></div>
    </div>
    <div style="margin: 20px 10px">
        <div style="text-align:right"><a href="/OnlineReg/Login/" class="submitbutton" tabindex="3">Login</a></div>
        <div><%= Html.ValidationMessage("authentication")%></div>
    </div>
    <div style="margin: 5px; text-align:center">
        Forgot <a href="/Account/ForgotUsername">Username</a> or <a href="/Account/ForgotPassword">Password</a>?
    </div>
</div>
<div style="margin: 10px; text-align:center; width:350px">
    <a href="/OnlineReg/NoLogin" class="submitlink">Login without an account</a>
</div>