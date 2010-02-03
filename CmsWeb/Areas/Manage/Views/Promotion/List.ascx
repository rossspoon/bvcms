<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PromotionModel>" %>
<% foreach (var r in Model.FetchStudents())
   { %>
<tr>
    <td><input name="selected" type="checkbox" <%=r.Checked %> value="<%=r.PeopleId%>" class="check" gender='<%=r.Gender %>' attend='<%=r.AttendIndicator %>' /></td>
    <td><%=r.Gender%></td>
    <td><%=r.AttendIndicator%></td>
    <td><%="{0:N1}".Fmt(r.AttendPct)%></td>
    <td><a href="/Person/Index/<%=r.PeopleId %>"><%=r.Name%></a></td>
    <td><a href="/Organization.aspx?id=<%=r.CurrClassId %>"><%=r.CurrClassName%></a></td>
    <td><a href="/Organization.aspx?id=<%=r.PendingClassId %>"><%=r.PendingClassName%></a></td>
    <td><%=r.Birthday%></td>
</tr>
<% } %>
