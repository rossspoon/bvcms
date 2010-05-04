<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>
<table class="modalPopup">
    <tr><th colspan="2">Manage Groups</th></tr>
    <tr>
        <th align="right">Change Active Group:</th>
        <td><%=Html.DropDownList("groupid", Model.Groups())%>
            <a id="groupMembersUpdate" href="/OrgMembersDialog/Index/<%=Model.OrganizationId %>" title="Update Group Members" class="memberdialog">Group Members</a>
        </td>
    </tr>
    <tr>
        <th align="right">New Group Name:</th>
        <td><%=Html.TextBox("GroupName")%></td>
    </tr>
    <tr>
        <th></th>
        <td><a class="groupmanager" href="/Organization/MakeNewGroup/<%=Model.OrganizationId %>">Make New Group</a> |
            <a class="groupmanager" href="/Organization/RenameGroup/<%=Model.OrganizationId %>">Rename Group</a> |
            <a class="groupmanager" href="/Organization/DeleteGroup/<%=Model.OrganizationId %>">Delete Group</a>
        </td>
    </tr>
</table>
