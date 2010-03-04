<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<% 
    int n = 0;
    foreach(var p in Model.List)
    {
        p.index = n++;
        Html.RenderPartial("Hidden", p);
    } 
%>
<table width="95%">
<tr>
<td rowspan="2">
    <table id="people" class="grid" style="font-size:12px;" cellpadding="3">
    <thead>
        <tr class="headrow">
            <th>Name</th>
            <th>Address</th>
            <th>CityStateZip</th>
            <th>Age</th>
        </tr>
    </thead>
    <tbody>
    <% if(ViewData.Model.Count == 0)
       { %>
    <tr><td colspan="5">No matching records.</td></tr>
    <% } %>
    <% foreach (var c in ViewData.Model.PeopleList())
       { %>
    <tr>
        <td><a href="/SearchAdd/Select/<%=c.PeopleId %>" class="namecol"><%=c.Name%></a></td>
        <td class="addrcol" title="<%=c.ToolTip %>"><%=c.Address%></td>
        <td><%=c.CityStateZip%></td>
        <td><%=c.Age%></td>
    </tr>
    <% } %>
    <tr><td colspan="4" align="right">
        <a href="/SearchAdd/SearchFamily/" class="submitbutton formlink">Search for Family</a>
    </td></tr>
    </tbody>
    </table>
</td>
<td valign="top" align="right">
    <a class="formlink" href="/SearchAdd/SearchPerson/">cancel</a>
</td>
</tr>
</table>

