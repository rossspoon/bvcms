<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsData.OrganizationMember>" %>
<% string comboid = "-" + Model.OrganizationId + "-" + Model.PeopleId; %>
<table class="Design2">
    <tr>
        <td><strong><%=Model.Person.Name %></strong></td>
        <td align="right"><a class="display" href="/OrgMemberDialog/Edit/<%=Model.OrganizationId %>?pid=<%=Model.PeopleId %>">Edit</a></td>
    </tr>
    <tr>
        <th>Member Type:</th>
        <td><%=Model.MemberType.Description %></td>
    </tr>
    <tr>
        <th>Inactive Date:</th>
        <td><%=Model.InactiveDate.FormatDate() %></td>
    </tr>
    <tr>
        <th>Enrollment Date:</th>
        <td><%=Model.EnrollmentDate.FormatDate() %></td>
    </tr>
    <tr>
        <th>Pending:</th>
        <td><input type="checkbox" disabled="disabled" <%=Model.Pending == true ? "checked='checked'" : "" %> /></td>
    </tr>
    <tr>
        <th>Request:</th>
        <td><%=Model.Request%></td>
    </tr>
    <tr>
        <th>Amount:</th>
        <td><%=Model.Amount.HasValue ? Model.Amount.Value.ToString("C") : "" %></td>
    </tr>
    <tr>
        <th>Extra Member Info:</th>
        <td><%=Util.SafeFormat(Model.UserData) %></td>
    </tr>
    <tr>
        <th>Drop:</th>
        <td><a id="d<%=comboid %>" class="delete" href="/EditMember/Drop/"><img src="/images/delete.gif" border="0" /></a></td>
    </tr>
    <tr>
        <td></td>
        <td align="right"><a class="display" href="/OrgMemberDialog/Move/<%=Model.OrganizationId %>?pid=<%=Model.PeopleId %>">move</a></td>
    </tr>
</table>
