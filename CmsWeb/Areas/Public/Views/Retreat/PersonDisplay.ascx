<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RetreatModel>" %>
<tr><td>
<table>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><%=Model.first %>
        <input type="hidden" name="orgid" value="<%=Model.orgid %>" />
        <input type="hidden" name="Found" value="<%=Model.Found%>" />
        <input type="hidden" name="IsNew" value="<%=Model.IsNew%>" />
        <input type="hidden" name="ShowAddress" value="<%=Model.ShowAddress %>" />
        <input type="hidden" name="first" value="<%=Model.first%>" />
        <input type="hidden" name="last" value="<%=Model.last%>" />
        <input type="hidden" name="dob" value="<%=Model.birthday.ToShortDateString()%>" />
        <input type="hidden" name="phone" value="<%=Model.phone%>" />
        <input type="hidden" name="homecell" value="<%=Model.homecell%>" />
        <input type="hidden" name="address" value="<%=Model.address%>" />
        <input type="hidden" name="zip" value="<%=Model.zip%>" />
        <input type="hidden" name="city" value="<%=Model.city%>" />
        <input type="hidden" name="state" value="<%=Model.state%>" />
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
Roomate Request (optional)<br />
<input type="text" name="request" />
</td>
</tr>