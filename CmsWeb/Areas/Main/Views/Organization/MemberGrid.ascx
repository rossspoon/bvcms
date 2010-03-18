<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.OrganizationMemberModel>" %>
<table class="grid" cellpadding="4">
<thead>
<tr>
    <th></th>
    <th></th>
    <th align="left"><a title="Sort by Church Member Status" href="#" class="sortable">Church</a></th>
    <th></th>
    <th></th>
    <th colspan="2">Attendance</th>
    <th></th>
</tr>
<tr>
    <th><a href="#" class="sortable">Name</a></th>
    <th><a title="Sort by Organization Member Type" href="#" class="sortable">Member</a></th>
    <th align="left"><a href="#">Age</a> - <a title="Sort by Birthday" href="#" class="sortable">Bday</a></th>
    <th><a href="#" class="sortable">Primary Address</a></th>
    <th>Communication</th>
    <th><a href="#" class="sortable">Pct</a></th>
    <th><a href="#" class="sortable">Last</a></th>
    <th>Tag</th>
</tr>
</thead>
<tbody>
<% foreach(var om in Model.Members())
   { %>
<tr>
    <td><img src="/images/individual.gif" border="0" /> <a href="/Person/Index/<%=om.PeopleId %>"><%=om.Name %></a></td>
    <td><a href="/OrgMemberDialog/Index/<%=Model.OrganizationId %>?pid=<%=om.PeopleId %>&from=MemberPanel"><%=om.MemberType %></a></td>
    <td><%=om.MemberStatus %><br />
        <%=om.Age %> - <%=om.BirthDate %>
    </td>
    <td><a href="http://bing.com/maps/default.aspx?q=<%=om.Address %>,<%=om.CityStateZip %>" target="_blank">
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
    <td>
        <span><%=om.AttendPct.ToString2("N1") %>%</span>
    </td>
    <td>
        <span><%=om.LastAttended.FormatDate2() %></span>
    </td>
    <td>
        <a title="Add to/Remove from Active Tag" href="#">Add</a>
    </td>
</tr>
<% } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>