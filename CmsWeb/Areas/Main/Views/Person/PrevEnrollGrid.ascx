<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PersonPage.PersonPrevEnrollmentsModel>" %>
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
<% if (Page.User.IsInRole("Access"))
   {
       foreach (var om in Model.PrevEnrollments())
       { %>
    <tr>
    <td><a href="/Organization/Index/<%=om.OrgId %>" title="<%=om.DivisionName %>"><%=om.Name%></a></td>
    <td><%=om.Location%></td>
    <td><a href="/Person/Index/<%=om.LeaderId %>"><%=om.LeaderName%></a></td>
    <td><%=om.Schedule%></td>
    <td><%=om.EnrollDate.FormatDate()%></td>
    <td><a target="_blank" 
        href='<%="/AttendStrDetail.aspx?id={0}&oid={1}".Fmt(om.PeopleId, om.OrgId) %>'><%=om.MemberType%></a></td>
    <td><%=om.AttendPct.HasValue ? om.AttendPct.Value.ToString("N1") : ""%></td>
    </tr>
    <% }
   }
   else
   {
       foreach (var om in Model.PrevEnrollments())
       { %>
    <tr>
    <td><span title="<%=om.DivisionName %>"><%=om.Name%></span></td>
    <td><%=om.Location%></td>
    <td><%=om.LeaderName%></td>
    <td><%=om.Schedule%></td>
    <td><%=om.EnrollDate.FormatDate()%></td>
    <td><%=om.MemberType%></td>
    <td><%=om.AttendPct.HasValue ? om.AttendPct.Value.ToString("N1") : ""%></td>
    </tr>
    <% }
   } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>

