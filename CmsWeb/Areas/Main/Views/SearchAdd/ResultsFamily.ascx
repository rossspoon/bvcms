<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.SearchModel>" %>
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<input type="hidden" name="m.name" value="<%=Model.name%>" />
<input type="hidden" name="m.dob" value="<%=Model.dob%>" />
<input type="hidden" name="m.address" value="<%=Model.address%>" />
<input type="hidden" name="m.phone" value="<%=Model.phone%>" />
<% 
    int n=0;
    foreach(var p in Model.List)
    {
        p.index = n++;
        Html.RenderPartial("HiddenPerson", p);
    } 
%>
<h4>Family Search Results (top 20)</h4>
<a class="helplink" target="_blank" href='<%=Model.HelpLink("FamilyResults") %>'>help</a>
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
    <td><a href="/SearchAdd/FormAbbreviated/<%=c.FamilyId %>" class="formlink"><%=c.Name%></a></td>
    <td class="addrcol" title="<%=c.ToolTip %>"><%=c.Address%></td>
    <td><%=c.CityStateZip%></td>
    <td><%=c.Age%></td>
</tr>
<% } %>
</table>
<p>Showing <%=Model.Showcount %> of <%=Model.Count %></p>
<div align="right">
Click a link above to add to that person's family or
<% if (Model.CanAdd)
   { %>
    <a href="/SearchAdd/FormFull/" class="bt formlink">Add New Family</a>
<% } %>
    <a class="bt formlink" href="/SearchAdd/SearchFamily/" title="back to family search">Go Back</a>
</div>
