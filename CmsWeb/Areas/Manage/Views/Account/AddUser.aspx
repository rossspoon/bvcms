<%@Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsData.User>" %>

<asp:Content ID="changePasswordSuccessHead" ContentPlaceHolderID="head" runat="server">
    <title>User Added</title>
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>New User Added</h2>
<table>
<tr><td>Userid:</td><td><%=Model.UserId %></td></tr>
<tr><td>Name:</td><td><%=Model.Name %></td></tr>
<tr><td>Username:</td><td><%=Model.Username %></td></tr>
<tr><td>Password:</td><td><%=ViewData["newpassword"] %></td></tr>
</table>
    <% using (Html.BeginForm("SendNewUserEmail", "Account"))
       { %>
       <%=Html.Hidden("userid", Model.UserId) %>
       <%=Html.Hidden("newpassword", ViewData["newpassword"]) %>
       <input type="submit" name="SendNewUserWelcome" value="Send Email" />
    <% } %>
    <% using (Html.BeginForm("UsersPage", "Account"))
       { %>
       <%=Html.Hidden("newpassword", ViewData["newpassword"]) %>
       <input type="submit" name="Submit" value="Go directly to Users" /> 
    <% } %>
</asp:Content>
