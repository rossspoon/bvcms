<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.SearchModel>" %>
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<input type="hidden" name="m.name" value="<%=Model.name%>" />
<input type="hidden" name="m.dob" value="<%=Model.dob%>" />
<input type="hidden" name="m.address" value="<%=Model.address%>" />
<input type="hidden" name="m.phone" value="<%=Model.phone%>" />
<% 
    %>
<table id="people" width="100%" style="font-size:12px">
<thead>
<tr><th align="left" colspan="5">Selection List</th></tr>
<tr class="headrow">
    <th>Name</th>
    <th>Address</th>
    <th>CityStateZip</th>
    <th>Age</th>
    <th></th>
</tr>
</thead>
<tbody>
<% 
   int n = 0;
   CmsWeb.Models.SearchPersonModel p = null;
   for (; n < Model.List.Count; n++ )
   {
       p = Model.List[n];
       p.index = n;
    Html.RenderPartial("PersonDisplay", p);
    Html.RenderPartial("HiddenPerson", p);
   } 
%>
</tbody>
</table>
<p> 
<a href="/SearchAdd/Complete/<%=Model.typeid %>" class="bt formlink default">Commit and Add</a>
or <a href="/SearchAdd/SearchPerson/" class="bt formlink">Another Search</a>
<% if (Model.CanAdd)
   { %>
or <a href="/SearchAdd/FormAbbreviated/<%=p.FamilyId %>" class="bt formlink">Add to Last Family</a>
<% } %>
</p> 