<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PersonPage.PersonPendingEnrollmentsModel>" %>
<table class="grid" cellPadding="4">
<thead>
<tr>
    <th>Organization</th>
    <th>Location</th>
    <th>Leader</th>
    <th>Schedule</th>
    <th>Enroll Date</th>
    <th>MemberType</th>
</tr>
</thead>
<tbody>
<% if (Page.User.IsInRole("Access"))
   {
       foreach (var om in Model.PendingEnrollments())
       { %>
    <tr>
    <td><a href="/Organization/Index/<%=om.OrgId %>" title="<%=om.DivisionName %>"><%=om.Name%></a></td>
    <td><%=om.Location%></td>
    <td><a href="/Person/Index/<%=om.LeaderId %>"><%=om.LeaderName%></a></td>
    <td><%=om.Schedule%></td>
    <td><%=om.EnrollDate.FormatDate()%></td>
    <td><a class="membertype" pid="<%=om.PeopleId %>" oid="<%=om.OrgId %>" 
        href="<%="/OrgMemberDialog/Index/{0}?pid={1}&from={2}".Fmt(om.OrgId, om.PeopleId, "na") %>"><%=om.MemberType%></a></td>
    </tr>
    <% }
   }
   else
   {
       foreach (var om in Model.PendingEnrollments())
       { %>
    <tr>
    <td><span title="<%=om.DivisionName %>"><%=om.Name%></span></td>
    <td><%=om.Location%></td>
    <td><%=om.LeaderId %>"><%=om.LeaderName%></td>
    <td><%=om.Schedule%></td>
    <td><%=om.EnrollDate.FormatDate()%></td>
    <td><%=om.MemberType%></td>
    </tr>
    <% }
   } %>
</tbody>
</table>

