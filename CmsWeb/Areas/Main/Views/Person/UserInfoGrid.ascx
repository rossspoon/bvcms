<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsData.Person>" %>
<% if (Model.Users.Count > 0)
   { %>
<table class="grid">
<thead>
    <tr>
        <th>Locked</th>
        <th>Username</th>
        <th>Last Activity</th>
        <th>Roles</th>
    </tr>
</thead>
<tbody>
    <% foreach (var u in Model.Users)
       { %>
    <tr>
        <td valign="top"><%=u.IsLockedOut%></td>
        <% if (Page.User.IsInRole("Admin"))
           { %>
        <td valign="top"><a href="/Person/UserDialog/<%=u.UserId %>" class="membertype"><%=u.Username%></a></td>
        <% }
           else
           { %>
        <td valign="top"><%=u.Username%></td>
        <% } %>
        <td valign="top"><%=u.LastActivityDate.ToString2("M/d/yy h:mm tt") %></td>
        <td>
        <table>
        <% foreach (var r in u.Roles)
           { %>
           <tr><td><%=r %></td></tr>
        <% } %>
        </table>
        </td>
    </tr>
    <% } %>
</tbody>
</table>
<% } %>
