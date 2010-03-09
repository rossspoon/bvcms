<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrgSearchModel>" %>
<table id="results">
<thead>
    <tr><td colspan="8">
<% Html.RenderPartial("Pager2", Model); %>
    </td></tr>
    <tr>
        <th align="left"><a href="#" class="sortable">Division</a></th>
        <th align="left"><a href="#" class="sortable">Name</a></th>
        <th align="left"><a href="#" class="sortable">Leader</a></th>
        <th align="right"><a href="#" class="sortable">Members</a></th>
        <th align="left"><a href="#" class="sortable">Schedule</a></th>
        <th align="left"><a href="#" class="sortable">Location</a></th>
        <th><a href="#" class="sortable">Self CheckIn</a></th>
        <th><a href="#" class="sortable">DivTag</a></th>
    </tr>
</thead>
<tbody>
<% foreach (var o in Model.OrganizationList())
   { %>
    <tr>
        <td><span title="<%=o.DivisionId %>"><%=o.DivisionName %></span></td>
        <td><a href="/Organization.aspx?id=<%=o.OrganizationId %>" title="<%=o.ToolTip %>"><%=o.OrganizationName %></a></td>
        <td><a href="/Person/Index/<%=o.LeaderId %>"><%=o.LeaderName %></a></td>
        <td align="right"><%=o.MemberCount %></td>
        <td><%=o.Schedule %></td>
        <td><%=o.Location %></td>
        <td><%=o.AllowSelfCheckIn ? "yes" : "" %></td>
        <td><a href="#" class="taguntag" title="Add to/Remove from Active Division"><%=o.HasTag ? "Remove" : "Add" %></a></td>
    </tr>
<% } %>
</tbody>
    <tr><td colspan="8">
<% Html.RenderPartial("Pager2", Model); %>
    </td></tr>
</table>
