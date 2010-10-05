<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PersonPage.DuplicatesModel>" %>
<table class="grid">
<thead>
    <tr>
        <th>Name</th>
        <th>Age</th>
        <th>Birthday</th>
        <th>Address</th>
        <th>Cell</th>
        <th>Home</th>
        <th>Email</th>
    </tr>
</thead>
<tbody>
    <% foreach (var d in Model.FetchDuplicates())
       { %>
    <tr>
        <td><a href="/Person/Index/<%=d.PeopleId%>"><%=d.Name%></a></td>
        <td><%=d.Age %></td>
        <td><%=d.DOB.FormatDate() %></td>
        <td><%=d.Address %></td>
        <td><%=d.Cell.FmtFone("C ") %></td>
        <td><%=d.Home.FmtFone("H ") %></td>
        <td><%=d.Email %></td>
    </tr>
    <% } %>
</tbody>
</table>
