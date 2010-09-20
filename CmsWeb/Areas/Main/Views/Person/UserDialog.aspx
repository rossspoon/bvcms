<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsData.User>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
<%= SquishIt.Framework.Bundle.Css()
        .Add("/Content/jquery-ui-1.8.2.custom.css")
        .Add("/Content/Dialog.css")
        .Add("/Content/jquery.tooltip.css")
    .Render("/Content/UserDialog_#.css")
%>
</head>
<body>
    <%= SquishIt.Framework.Bundle.JavaScript()
                .Add("/Content/js/jquery-1.4.2.js")
                .Add("/Content/js/jquery-ui-1.8.2.custom.js")
                .Add("/Content/js/jquery.tooltip.js")
                .Add("/Scripts/UserDialog.js")
        .Render("/Content/UserDialog_#.js")
            %>        

<a class="helplink" target="_blank" href='<%=Util.HelpLink("UserUpdate")%>'>help</a>
<form class="DisplayEdit" action="">
<table class="Design2">
    <tr><td>Username:</td><td><%=Html.TextBox("Username") %></td></tr>
    <tr><td>Password:</td><td><%=Html.TextBox("Password2") %></td></tr>
    <tr><td>LockedOut:</td><td><%=Html.CheckBox("IsLockedOut") %></td></tr>
<tr><td></td></tr>
    <tr>
    <td>
    <% foreach (var r in DbUtil.Db.Roles.Select(ro => ro.RoleName).OrderBy(ro=> ro))
       { %>
       <input name="role" value="<%=r %>" class="ckbox" type="checkbox" <%=Model.Roles.Any(ro => ro == r) ? "checked='checked'" : "" %> />
       <%=r %><br />
    <% } %>
    </td>
    </tr>
    <tr>
        <td></td>
        <td><a href="/Person/UserUpdate/<%=Model.UserId %>" class="submitbutton">Save Changes</a></td>
    </tr>
</table>
</form>
</body>
</html>
