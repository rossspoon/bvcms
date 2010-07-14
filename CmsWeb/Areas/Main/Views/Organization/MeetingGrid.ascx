<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrganizationPage.MeetingModel>" %>
<p><a id="NewMeeting" href="Organization/NewMeeting">Create New Meeting</a> | 
<%=Html.CheckBox("future") %> Show Future Meetings</p>
<table id="MemberGrid" class="grid" cellpadding="4">
<thead>
<tr>
    <th><a href="#" class="sortable">Date</a></th>
    <th><a href="#" class="sortable">Time</a></th>
    <th><a href="#" class="sortable">Present</a></th>
    <th><a href="#" class="sortable">Visitors</a></th>
    <th><a href="#" class="sortable">Location</a></th>
    <th align="left"><a href="#" class="sortable">Description</a></th>
    <th></th>
</tr>
</thead>
<tbody>
<% foreach(var m in Model.Meetings())
   { %>
<tr>
    <td><a href="/Meeting.aspx?id=<%=m.MeetingId %>"><%=m.MeetingDate.FormatDate() %></a></td>
    <td><%=m.MeetingDate.ToString2("t") %></td>
    <td><%=m.NumPresent %></td>
    <td><%=m.NumVisitors %></td>
    <td><%=m.Location %></td>
    <td><%=m.Description %></td>
    <td><a id="m.<%=m.MeetingId %>" href="#" class="delmeeting"><img border="0" src="/images/delete.gif" /></a></td>
</tr>
<% } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>
<%--Are you sure you want to delete this meeting (this action cannot be undone)?--%>