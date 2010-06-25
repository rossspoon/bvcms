<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrgGroupsModel>" %>
<%=Html.Hidden("orgid") %>
<%=Html.Hidden("newgid") %>
<h2><a href="/Organization/Index/<%=Model.orgid %>"><%=Model.OrgName %></a></h2>
<table id="ManageGroups" class="modalPopup">
<% Html.RenderPartial("ManageGroups", Model); %>
</table>
<input id="SelectAll" type="checkbox" /> Select All
<%=Html.SubmitButton("AssignSelectedToTargetGroup", "Assign Selected To Target Group")%>
<%=Html.SubmitButton("RemoveSelectedFromTargetGroup", "Remove Selected From Target Group")%>
<div>
<table class="grid">
    <thead>
	<tr>
		<th>Select </th>
		<th align="left">Name </th>
		<th align="left">Address </th>
		<th>Gender </th>
		<th>Age </th>
		<th align="left">Request</th>
		<th align="left" width="25%">Groups </th>
	</tr>
	</thead>
	<tbody>
<% Html.RenderPartial("Rows", Model); %>
    </tbody>
</table>
</div>
