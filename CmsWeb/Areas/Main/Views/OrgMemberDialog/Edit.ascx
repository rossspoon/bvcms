<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsData.OrganizationMember>" %>

<table class="Design2">
    <tr>
        <td><b><%=Model.Person.Name %></b></td>
        <td align="right"><a class="display" href="/OrgMemberDialog/Display/<%=Model.OrganizationId %>?pid=<%=Model.PeopleId %>">cancel</a></td>
    </tr>
    <tr>
        <th>Member Type:</th>
        <td><%=Html.DropDownList("MemberTypeId", (IEnumerable<SelectListItem>)ViewData["MemberTypes"]) %></td>
    </tr>
    <tr>
        <th>Inactive Date:</th>
        <td><%=Html.TextBox("InactiveDate", Model.InactiveDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
    <tr>
        <th>Enrollment Date:</th>
        <td><%=Html.TextBox("EnrollmentDate", Model.EnrollmentDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
    <tr>
        <th>Pending:</th>
        <td><%=Html.CheckBox("Pending") %></td>
    </tr>
    <tr>
        <th>Request:</th>
        <td><%=Html.TextBox("Request") %></td>
    </tr>
    <tr>
        <th>Amount:</th>
        <td><%=Html.TextBox("Amount", Model.Amount.HasValue ? Model.Amount.Value.ToString("f2") : "") %></td>
    </tr>
    <tr>
        <th>Extra Member Info:</th>
        <td><%=Html.TextArea("UserData") %></td>
    </tr>
<tr><td></td></tr>
     <tr><td></td><td><a href="/OrgMemberDialog/Update/<%=Model.OrganizationId %>?pid=<%=Model.PeopleId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
