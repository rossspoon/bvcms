<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.RecreationModel>" %>
<% var participants = Model.FetchParticipants();
   foreach (var r in participants)
   { %>
<tr>
    <td><input name="selected" type="checkbox" <%=r.Checked %> value="<%=r.PeopleId%>" class="check" /></td>
    <td><a href="/Person/Index/<%=r.PeopleId %>"><%=r.Name%></a></td>
    <td><%=r.MemberType %></td>
    <td><%=r.MemberStatus %></td>
    <td><%=r.TeamName%></td>
    <td><%=r.Birthday%></td>
    <td><%=r.ShirtSize%></td>
    <td><%=r.FeePaid%></td>
    <td><%=r.Request%></td>
</tr>
<% } %>
