<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<CMSWeb.Models.TaskModel.TasksAbout>>" %>
<table class="grid">
    <thead>
    <tr>
        <th>Task</th>
        <th>Date</th>
        <th>Assigned to</th>
    </tr>
    </thead>
    <tbody>
    <% foreach (var t in Model)
       { %>
        <tr>
            <td><a href="<%=t.link %>"><%=t.Desc %></a></td>
            <td><%=t.AssignedDt %></td>
            <td><%=t.AssignedTo %></td>
        </tr>
    <% } %>
    </tbody>
</table>

