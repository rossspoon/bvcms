<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Prayer.Models.SearchPeopleModel>" %>
<% if(ViewData.Model.Count == 0)
   { %>
<tr><td colspan="5">No matching records.</td></tr>
<% } %>
<% foreach (var c in ViewData.Model.PeopleList())
   { %>
<tr title='<%=c.ToolTip%>'>
    <td><a href="/Account/Register/<%=c.PeopleId%>">select</a></td>
    <td><%=c.Name%></td>
    <td><%=c.Address%></td>
    <td><%=c.CityStateZip%></td>
    <td><%=c.Age%></td>
</tr>
<% } %>
<tr><td colspan="5">
<%=Html.Hidden("Count",ViewData.Model.Count)%>
<%=Html.Hidden("Sort",ViewData.Model.Sort)%>
</td></tr>

