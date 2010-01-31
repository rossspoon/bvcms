<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.PersonAttendHistoryModel>" %>
<table class="grid" cellPadding="4">
<thead>
<tr>
    <th><a href="#" class="sortable">Meeting</a></th>
    <th><a href="#" class="sortable">Organization</a></th>
    <th><a href="#" class="sortable">MemberType</a></th>
    <th><a href="#" class="sortable">AttendType</a></th>
</tr>
</thead>
<tbody>
<% foreach(var a in Model.Attendances())
   { %>
    <tr>
    <td><a href="/Meeting.aspx?id=<%=a.MeetingId %>"><%=a.MeetingDate.Value.ToString("MM/dd/yy h:mmtt") %></a></td>
    <td><a href="/Organization.aspx?id=<%=a.OrganizationId %>"><%=a.OrganizationName %></a></td>
    <td><%=a.MemberType %></td>
    <td><%=a.AttendType %></td>
    </tr>
<% } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>
