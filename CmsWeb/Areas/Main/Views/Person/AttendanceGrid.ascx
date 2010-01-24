<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonAttendHistoryModel>" %>
<form action="/Person/AttendanceGrid/<%=Model.person.PeopleId %>">
<table class="grid" cellPadding="4">
<thead>
<tr>
    <th>Meeting</th>
    <th>Organization</th>
    <th>MemberType</th>
    <th>AttendType</th>
</tr>
</thead>
<tbody>
<% foreach(var a in Model.Attendances())
   { %>
    <tr>
    <td><a href="/Meeting.aspx?id=<%=a.MeetingId %>"><%=a.MeetingDate.Value.ToString("MM/dd/y h:mmtt") %></a></td>
    <td><a href="/Organization.aspx?id=<%=a.OrganizationId %>"><%=a.OrganizationName %></a></td>
    <td><%=a.MemberType %></td>
    <td><%=a.AttendType %></td>
    </tr>
<% } %>
</tbody>
</table>
<%=Html.Hidden("Pager.Page")%>
<%=Html.Hidden("Pager.PageSize")%>
<%=Html.Hidden("Pager.Sort")%>
<%=Html.Hidden("Pager.Direction")%>
<% Html.RenderPartial("Pager2", Model.Pager); %>
</form>
