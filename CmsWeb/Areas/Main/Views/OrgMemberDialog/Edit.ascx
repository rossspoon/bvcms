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
        <td><%=Html.TextBox("InactiveDate", Model.InactiveDate.FormatDate(), new { @class = "datepicker" })%>
        <br /><span style="color:Red"><%=Html.ValidationMessage("InactiveDate") %></span></td>
    </tr>
    <tr>
        <th>Enrollment Date:</th>
        <td><%=Html.TextBox("EnrollmentDate", Model.EnrollmentDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
    <tr>
        <th>Pending:</th>
        <td><%=Html.CheckBox("Pending") %></td>
    </tr>
<% if (Model.Organization.AskRequest == true)
   { %>
    <tr>
        <th>Request:</th>
        <td><%=Html.TextBox("Request") %></td>
    </tr>
<% } %>
    <tr>
        <th>Grade:</th>
        <td><%=Html.TextBox("Grade", Model.Grade) %></td>
    </tr>
<% if (Model.Organization.AskTickets == true)
   { %>
    <tr>
        <th>No. Items:</th>
        <td><%=Html.TextBox("Tickets", Model.Tickets) %></td>
    </tr>
<% }
   if (Model.Organization.RegistrationTypeId > 0)
   { %>
    <tr>
        <th>Total Amount:</th>
        <td><%=Html.TextBox("Amount", Model.Amount.ToString2("f2")) %>
        <br /><span style="color:Red"><%=Html.ValidationMessage("Amount") %></span></td>
    </tr>
    <tr>
        <th>Amount Paid:</th>
        <td><%=Html.TextBox("AmountPaid", Model.AmountPaid.ToString2("f2")) %>
        <br /><span style="color:Red"><%=Html.ValidationMessage("AmountPaid") %></span></td>
    </tr>
<% }
   if(Model.Organization.AskShirtSize == true) 
   { %>
    <tr>
        <th>ShirtSize:</th>
        <td><%=Html.DropDownList("ShirtSize", 
                CmsWeb.Models.OnlineRegPersonModel.ShirtSizes(Model.Organization)) %></td>
    </tr>
<% } %>
    <tr>
        <th valign="top">Extra Member Info:</th>
        <td><%=Html.TextArea("UserData", new { style = "height:100px;width:100%" })%></td>
    </tr>
<tr><td></td></tr>
     <tr><td></td><td><a href="/OrgMemberDialog/Update/<%=Model.OrganizationId %>?pid=<%=Model.PeopleId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
