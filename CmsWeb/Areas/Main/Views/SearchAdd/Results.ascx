<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<% 
    Html.RenderPartial("HiddenModel", Model);
    int n = 0;
    foreach(var p in Model.List)
    {
        p.index = n++;
        Html.RenderPartial("HiddenPerson", p);
    }
%>
<table width="100%">
<tr><th colspan="4" align="left">Showing <%=Model.Showcount %> of <%=Model.Count %></th></tr>
<tr>
<td rowspan="2">
    <table id="people" class="grid" style="font-size:12px;" cellpadding="3">
    <tr class="headrow">
        <th>Name</th>
        <th>Address</th>
        <th>CityStateZip</th>
        <th>Age</th>
    </tr>
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
    </table>
</td>
<td valign="top" align="right">
    <a class="formlink" href="/SearchAdd/SearchPerson/" title="back to person search">go back</a>
</td>
</tr>
<tr><td valign="bottom" align="right">
    <a href="/SearchAdd/SearchFamily/" class="bt formlink">Search for<br />Family</a>
</td></tr>
</table>

