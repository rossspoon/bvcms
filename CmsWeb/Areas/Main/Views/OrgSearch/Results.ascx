<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrgSearchModel>" %>
<table id="results" <%=Page.User.IsInRole("Edit") ? "class='edit'" : "" %>>
<thead>
    <tr><td colspan="9">
<% Html.RenderPartial("Pager2", Model); %>
    </td></tr>
    <tr>
        <th><a href="#" class="sortable">ID</a></th>
        <th align="left"><a href="#" class="sortable">Name</a></th>
        <th align="left"><a href="#" class="sortable">Leader</a></th>
        <th align="right"><a href="#" class="sortable">Members</a></th>
        <th align="left"><a href="#" class="sortable">Schedule</a></th>
        <th><a href="#" class="sortable">Self CheckIn</a></th>
        <th><a href="#" class="sortable">BDay Start</a></th>
        <th><a href="#" class="sortable">BDay End</a></th>
        <th><a href="#" class="sortable">Tag</a></th>
    </tr>
</thead>
<tbody>
<% foreach (var o in Model.OrganizationList())
   { %>
    <tr>
        <td class="tip" title="<%=o.ToolTip %>"><img alt="group" src="/content/images/group.png" /></td>
        <td><a href="/Organization/Index/<%=o.Id %>"><%=o.OrganizationName%></a></td>
        <td><a href="/Person/Index/<%=o.LeaderId %>"><%=o.LeaderName %></a></td>
        <td align="right"><%=o.MemberCount %></td>
        <td><%=o.Schedule %></td>
        <td><span id='ck-<%=o.Id %>' class='yesno'><%=o.AllowSelfCheckIn ? "yes" : "no" %></span></td>
        <td><span id='bs-<%=o.Id %>' class='bday'><%=o.BDayStart%></span></td>
        <td><span id='be-<%=o.Id %>' class='bday'><%=o.BDayEnd%></span></td>
        <td><a id='tt-<%=o.Id %>' href="/OrgSearch/ToggleTag/<%=o.Id %>" class="taguntag" title="Add to/Remove from Tag Division"><%=o.Tag %></a></td>
    </tr>
<% } %>
</tbody>
    <tr><td colspan="9">
<% Html.RenderPartial("Pager2", Model); %>
    </td></tr>
</table>
