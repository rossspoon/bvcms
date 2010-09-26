<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsData.Person>" %>
<% if (Model.PeopleExtras.Count > 0)
   { %>
<table class="grid">
<thead>
    <tr>
        <th>Field</th>
        <th>Time</th>
        <th>Value</th>
    </tr>
</thead>
<tbody>
    <% foreach (var c in Model.PeopleExtras)
       { %>
    <tr>
        <td valign="top"><%=c.Field%></td>
        <td valign="top"><%=c.TransactionTime.ToString("M/d/yy h:mm tt")%></td>
        <% if (c.StrValue.HasValue())
           { %>        
        <td><%=c.StrValue%></td>
        <% }
           else if (c.Data.HasValue())
           { %>
        <td><%=Util.SafeFormat(c.Data)%></td>
        <% }
           else if (c.DateValue.HasValue)
           { %>
        <td><%=c.DateValue.FormatDate()%></td>
        <% }
           else
           { %>   
        <td><%=c.IntValue%> <%=c.IntValue2%></td>
        <% } %>
    </tr>
    <% } %>
</tbody>
</table>
<% } %>
<table class="grid">
<thead>
    <tr>
        <th>User</th>
        <th>Field</th>
        <th>Time</th>
        <th>Value</th>
    </tr>
</thead>
<tbody>
    <% var q = from c in DbUtil.Db.ChangeLogs
               let userp = DbUtil.Db.People.Single(u => u.PeopleId == c.UserPeopleId)
               where c.PeopleId == Model.PeopleId || c.FamilyId == Model.FamilyId
               orderby c.Created descending
               select new { User = userp.Name, Field = c.Field, Data = c.Data, Time = c.Created };
       foreach (var c in q)
       { %>
    <tr>
        <td valign="top"><%=c.User%></td>
        <td valign="top"><%=c.Field%></td>
        <td valign="top"><%=c.Time.ToString("M/d/yy h:mm tt")%></td>
        <td><%=c.Data %></td>
    </tr>
    <% } %>
</tbody>
</table>
