<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PromotionModel>" %>
<% foreach (var r in Model.FetchStudents())
   { %>
<tr>
    <td><input name="selected" type="checkbox" <%=r.Checked %> value="<%=r.PeopleId%>,<%=r.CurrClassId %>" class="check" gender='<%=r.Gender %>' attend='<%=r.AttendIndicator %>' />
    </td>
    <td><%=r.Gender%></td>
    <td><%=r.AttendIndicator%></td>
    <td><%="{0:N1}".Fmt(r.AttendPct)%></td>
    <td><a href="/Person/Index/<%=r.PeopleId %>"><%=r.Name%></a></td>
    <td><a href="/Organization/Index/<%=r.CurrClassId %>"><%=r.CurrClassName%></a></td>
    <td><a href="/Organization/Index/<%=r.PendingClassId %>"><%=r.PendingClassName%></a></td>
    <td><%=r.Birthday%></td>
</tr>
<% } %>
