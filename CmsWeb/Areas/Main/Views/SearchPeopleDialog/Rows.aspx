<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchPeopleDialogModel>" %>
<% Html.RenderPartial("Pager2", Model); %>
<table id="people" class="grid" style="font-size:12px;" cellpadding="3">
<thead>
    <tr class="headrow">
        <th><a href="#" class="sortable">Id</a></th>
        <th><a href="#" class="sortable">Name</a></th>
        <th>Address</th>
        <th><a href="#" class="sortable">CityStateZip</a></th>
        <th><a href="#" class="sortable">Age</a></th>
    </tr>
</thead>
<tbody>
<% if(ViewData.Model.Count() == 0)
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
    <td class="namecol" title="<%=c.ToolTip %>"><%=c.Name%></td>
        <td><%=c.Address%></td>
    <td><%=c.CityStateZip%></td>
    <td><%=c.Age%></td>
</tr>
<% } %>
</tbody>
</table>

