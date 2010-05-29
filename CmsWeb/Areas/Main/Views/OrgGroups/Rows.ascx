<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrgMembersDialogModel>" %>
<% foreach(var om in Model.FetchOrgMemberList())
   { %>
	<tr>
		<td><input name="list" type="checkbox" value="<%=om.PeopleId %>" <%=om.Checked() %> /></td>
		<td class="tip" title="<%=om.ToolTip %>"><%=om.Name %></td>
		<td><%=om.Address %></td>
		<td><%=om.CityStateZip %> </td>
		<td><%=om.Age %> </td>
	</tr>
<% } %>

