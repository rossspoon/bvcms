﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.PersonContactsReceivedModel>" %>
<table class="grid">
<thead>
    <tr>
        <th>Contact Date</th>
        <th>Type</th>
        <th>Reason</th>
    </tr>
</thead>
<tbody>
<% foreach (var c in Model.Contacts())
   { %>
    <tr>
        <td><a href="/Contact.aspx?id=<%=c.ContactId %>"><%=c.ContactDate.ToShortDateString() %></a></td>
        <td><%=c.TypeOfContact %></td>
        <td><%=c.ContactReason %></td>
    </tr>
<% } %>
</tbody>
</table>
<% Html.RenderPartial("Pager2", Model.Pager); %>
