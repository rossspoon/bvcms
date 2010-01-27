<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPrevEnrollmentsModel>" %>
<table class="grid" cellPadding="4">
<thead>
<tr>
    <th><a href="#" class="sortable">Organization</a></th>
    <th>Location</th>
    <th>Leader</th>
    <th>Schedule</th>
    <th><a href="#" class="sortable">Enroll Date</a></th>
    <th>MemberType</th>
    <th>AttendPct</th>
</tr>
</thead>
<tbody>
<% foreach(var om in Model.PrevEnrollments())
   { %>
    <tr>
    <td><a href="/Organization.aspx?id=<%=om.OrgId %>" title="<%=om.DivisionName %>"><%=om.Name %></a></td>
    <td><%=om.Location %></td>
    <td><a href="/Person/Index/<%=om.LeaderId %>"><%=om.LeaderName %></a></td>
    <td><%=om.MeetingTime %></td>
    <td><%=om.EnrollDate.FormatDate() %></td>
    <td><a class="membertype" pid="<%=om.PeopleId %>" oid="<%=om.OrgId %>" href="#"><%=om.MemberType %></a></td>
    <td><%=om.AttendPct.HasValue ? om.AttendPct.Value.ToString("N1") : "" %></td>
    </tr>
<% } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>

