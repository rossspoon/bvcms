<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrgGroupsModel>" %>
    <tr><th colspan="2">Manage Groups</th></tr>
    <tr>
        <th align="right">Change Active Group:</th>
        <td><%=Html.DropDownList("groupid", Model.Groups())%></td>
    </tr>
    <tr>
        <th align="right">New Group Name:</th>
        <td><%=Html.TextBox("GroupName")%></td>
    </tr>
    <tr>
        <th></th>
        <td><a class="groupmanager" href="/OrgGroups/MakeNewGroup/<%=Model.orgid %>">Make New Group</a> |
            <a class="groupmanager" href="/OrgGroups/RenameGroup/<%=Model.orgid %>">Rename Group</a> |
            <a class="groupmanager" href="/OrgGroups/DeleteGroup/<%=Model.orgid %>">Delete Group</a>
        </td>
    </tr>
	<tr>
		<td colspan="2"><a id="ClearSearch" href="#">clear</a></td>
	</tr>
	<tr>
		<th>Member Type:</th>
		<td><%=Html.DropDownList("memtype", Model.MemberTypeCodesWithNotSpecified(), new { @class = "filter" })%></td>
	</tr>
