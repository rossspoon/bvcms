<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<input type="hidden" name="m.name" value="<%=Model.name%>" />
<input type="hidden" name="m.dob" value="<%=Model.dob%>" />
<input type="hidden" name="m.address" value="<%=Model.address%>" />
<input type="hidden" name="m.phone" value="<%=Model.phone%>" />
<% 
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
        <td><a href="/SearchAdd/Select/<%=c.PeopleId %>" class="formlink"><%=c.Name%></a></td>
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
<% if (Model.type == "family")
   { %>
    <a href="/SearchAdd/FormAbbreviated/<%=Model.typeid %>" class="bt formlink">Add<br />Add New<br />Person</a>
<% }
   else
   { %>
    <a href="/SearchAdd/SearchFamily/" class="bt formlink">Search for<br />Family</a>
<% } %>
</td></tr>
</table>

