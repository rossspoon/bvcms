<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrgGroupsModel>" %>
    <tr><th colspan="2">Manage Groups</th></tr>
    <tr>
        <th align="right">Change Target Group:</th>
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
    <tr><td colspan="2"><hr /></td></tr>
    <tr>
        <th align="right">Is in group starting with:</th>
        <td><%=Html.TextBox("ingroup", "", new { @class = "filter" })%></td>
    </tr>
    <tr>
        <th align="right">Not in group starting with:</th>
        <td><%=Html.TextBox("notgroup", "", new { @class = "filter" })%>
            Active: <%=Html.CheckBox("notgroupactive", new { @class = "filter" }) %>
        </td>
    </tr>
	<tr>
		<th>Member Type:</th>
		<td><%=Html.DropDownList("memtype", Model.MemberTypeCodesWithNotSpecified(), new { @class = "filter" })%>
            <input type="button" id="Filter" value="Filter" />
		</td>
	</tr>
