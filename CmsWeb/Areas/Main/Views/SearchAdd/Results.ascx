<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.SearchModel>" %>
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
<table id="people" width="100%" class="grid" style="font-size:12px;" cellpadding="3">
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
<p>Showing <%=Model.Showcount %> of <%=Model.Count %>
</p>
<div align="right">
Click a link above to select that person or
<% if (Model.type == "family")
   { %>
    <a href="/SearchAdd/FormAbbreviated/<%=Model.typeid %>" class="bt formlink">Add New Person</a>
<% }
   else
   { %>
    <a href="/SearchAdd/SearchFamily/" class="bt formlink">Search for Family</a>
<% } %>
<a class="bt formlink" href="/SearchAdd/SearchPerson/" title="back to person search">go back</a>
</div>
