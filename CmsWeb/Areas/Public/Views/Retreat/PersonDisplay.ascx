<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RetreatModel>" %>
<tr><td>
<table>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><%=Model.first %>
        <%=Html.Hidden("Found") %>
        <%=Html.Hidden("IsNew") %>
        <%=Html.Hidden("ShowAddress") %>
        <%=Html.Hidden("first") %>
        <%=Html.Hidden("last") %>
        <%=Html.Hidden("dob", Model.birthday.ToShortDateString())%>
        <%=Html.Hidden("phone") %>
        <%=Html.Hidden("homecell") %>
        <%=Html.Hidden("address") %>
        <%=Html.Hidden("zip") %>
        <%=Html.Hidden("city") %>
        <%=Html.Hidden("state") %>
        </td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td><%=Model.last %>
        </td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td><%=Model.birthday.ToShortDateString() %> <span><%=Model.age %></span>
        </td>
    </tr>
    <tr>
        <td><label for="phone">Phone</label></td>
        <td><%=Model.phone.FmtFone(Model.homecell) %>
        </td>
    </tr>
    <% if (Model.email.HasValue())
       { %>
    <tr>
        <td><label for="email">Contact Email</label></td>
        <td><%=Model.email %>
        </td>
    </tr>
    <% } %>
    <% if (Model.ShowAddress)
       { %>
    <% Html.RenderPartial("AddressDisplay", Model); %>
    <% } %>
</table>
</td>
<td>
<input type="hidden" name="email" value="<%=Model.email%>" />
<% Model.option = Model.amountpaid == 0 ? 1 : 2;
   if (Model.amountpaid == 0)
   { %>
<input type="radio" name="option" value="1" 
<%=Model.option == 1 ? "checked='checked'" : "" %> class="option" /> Deposit ($50.00)<br />
<% } %>
<input type="radio" name="option" value="2" 
<%=Model.option == 2 ? "checked='checked'" : "" %> class="option" /> Balance Due (<%=Model.BalanceDue().ToString("C") %>)<br />
<p><strong>Roomate Request</strong> (optional)<br />
<input type="text" name="request" /></p>
</td>
</tr>