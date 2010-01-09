<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchPeopleModel>" %>
<% if(ViewData.Model.Count == 0)
   { %>
<tr><td colspan="5">No matching records.</td></tr>
<% } %>
<% foreach (var c in ViewData.Model.PeopleList())
   { %>
<tr>
    <td><a id='s<%=c.PeopleId%>' title="Select this person" href="#" class='select'>select</a>
    <a id='a<%=c.PeopleId%>' title="Add new person
    to this person's family" href="#" class='add' title="">add</a>
    </td>
    <td><span class="namecol" title="<%=c.ToolTip %>"><%=c.Name%></span></td>
        <td><%=c.Address%></td>
    <td><%=c.CityStateZip%></td>
    <td><%=c.Age%></td>
</tr>
<% } %>
<tr><td colspan="5">
<%=Html.Hidden("Count",ViewData.Model.Count)%>
<%=Html.Hidden("Sort",ViewData.Model.Sort)%>
</td></tr>

