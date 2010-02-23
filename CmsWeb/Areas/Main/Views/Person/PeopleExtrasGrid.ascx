<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsData.Person>" %>
<% if (Model.PeopleExtras.Count > 0)
   { %>
<table class="grid">
<thead>
    <tr>
        <th>Field</th>
        <th>Value</th>
    </tr>
</thead>
<tbody>
<% foreach (var c in Model.PeopleExtras)
   { %>
    <tr>
        <td><%=c.Field%></td>
        <td><%=c.StrValue.HasValue() ? c.StrValue : c.DateValue.FormatDate()%></td>
    </tr>
<% } %>
</tbody>
</table>
<% } %>
