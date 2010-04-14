<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.PersonEnrollmentsModel>" %>
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
<% foreach(var om in Model.Enrollments())
   { %>
    <tr>
    <% if (!DbUtil.Db.UserPreference("neworgpage").ToBool())
       { %>        
    <td><a href="/Organization.aspx?id=<%=om.OrgId %>" title="<%=om.DivisionName %>"><%=om.Name %></a></td>
    <% } 
       else
       { %>
    <td><a href="/Organization/Index/<%=om.OrgId %>" title="<%=om.DivisionName %>"><%=om.Name %></a></td>
    <% } %>
    <td><%=om.Location %></td>
    <td><a href="/Person/Index/<%=om.LeaderId %>"><%=om.LeaderName %></a></td>
    <td><%=om.Schedule %></td>
    <td><%=om.EnrollDate.FormatDate() %></td>
    <td><a class="membertype" pid="<%=om.PeopleId %>" oid="<%=om.OrgId %>" 
        href="<%="/OrgMemberDialog/Index/{0}?pid={1}&from={2}".Fmt(om.OrgId, om.PeopleId, "na") %>">
        <%=om.MemberType %></a></td>
    <td><%=om.AttendPct > 0 ? om.AttendPct.Value.ToString("N1") : "" %></td>
    </tr>
<% } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>

