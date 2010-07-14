<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<CmsWeb.Areas.Main.Controllers.OrgMemberDialogController.OrgMove>>" %>
<table class="Design2">
    <tr>
        <td><strong><%=ViewData["Name"] %></strong></td>
        <td align="right"><a class="display" href='/OrgMemberDialog/Display/<%=ViewData["oid"] %>?pid=<%=ViewData["pid"] %>'>cancel</a></td>
    </tr>
    <tr>
        <td colspan="2">
    <% if (Model != null)
       {
           foreach (var o in Model)
           { %>
        <a class="move" href="/OrgMemberDialog/MoveSelect/<%=o.id %>"><%=o.OrgName%></a><br />
        <% }
       }
       else
       { %>
        Organization must have a main division
    <% } %>
        </td>
    </tr>
</table>
