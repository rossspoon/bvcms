<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrganizationPage.VisitorModel>" %>
<% Html.RenderPartial("ExportToolBar"); %>
&nbsp;<div style="clear: both"></div>
<p style="bottom-margin:5px">
Count: <strong><%=Model.Count() %></strong></p>
<a class="filtergroupslink" href="#">Filter by name/group<%= Model.isFiltered ? "(filtered)" : "" %></a>
<table id="MemberGrid" class="grid" cellpadding="4">
<thead>
<tr>
    <th><br /><a href="#" class="sortable">Name</a></th>
    <th></th>
    <th align="left">
        <a title="Sort by Church Member Status" href="#" class="sortable">Church</a><br />
        <a href="#" class="sortable">Age</a> - <a title="Sort by Birthday" href="#" class="sortable">Bday</a>
    </th>
    <th><br /><a href="#" class="sortable">Primary Address</a></th>
    <th><br />Communication</th>
    <th><br /><a href="#" class="sortable">Last Attended</a></th>
    <th><br />Tag</th>
</tr>
</thead>
<tbody>
<% foreach(var om in Model.Visitors())
   { %>
<tr>
    <td><img src="/images/individual.gif" border="0" /> <a href="/Person/Index/<%=om.PeopleId %>"><%=om.Name %></a></td>
    <td><a id="j.<%=om.PeopleId %>.<%=Model.OrganizationId %>" href="#" class="joinlink">Join</a></td>
    <td><%=om.MemberStatus %><br />
        <%=om.Age %> - <%=om.BirthDate %>
    </td>
    <td><a href="http://www.google.com/maps?q=<%=om.Address %>,<%=om.CityStateZip %>" target="_blank">
            <%=om.Address %></a><br />
        <%=om.CityStateZip %>
    </td>
    <td>
<% foreach (var ph in om.Phones)
   { %>    
        <%=ph%><br />
<% } %>        
        <a href="mailto:<%=om.Name %> <<%=om.EmailAddress %>>"><%=om.EmailAddress %></a>
    </td>
    <td><%=om.LastAttended.FormatDate2() %></td>
    <td>
        <a pid='<%=om.PeopleId %>' title="Add to/Remove from Active Tag" class="taguntag" href="#"><%=om.HasTag? "Remove" : "Add" %></a>
    </td>
</tr>
<% } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>