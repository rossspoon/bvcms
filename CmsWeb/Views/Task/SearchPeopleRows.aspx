<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchPeopleModel>" %>
<% if(ViewData.Model.Count == 0)
   { %>
<tr><td colspan="5">No matching records.</td></tr>
<% } %>
<% foreach (var c in ViewData.Model.PeopleList())
   { %>
<tr title='<%=c.ToolTip%>'>
    <td><%=Html.HyperLink("javascript:SelectPerson({0})".Fmt(c.PeopleId), "select")%></td>
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

