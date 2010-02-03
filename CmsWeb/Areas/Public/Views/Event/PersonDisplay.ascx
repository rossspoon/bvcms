<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonEventModel>" %>
<tr <%=Model.index % 2 == 1 ? "class='alt'" : "" %>><td>
<table>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><%=Model.first %>
        <input type="hidden" name="list[<%=Model.index%>].index" value="<%=Model.index%>" />
        <input type="hidden" name="list[<%=Model.index%>].Found" value="<%=Model.Found%>" />
        <input type="hidden" name="list[<%=Model.index%>].IsNew" value="<%=Model.IsNew%>" />
        <input type="hidden" name="list[<%=Model.index%>].ShowAddress" value="<%=Model.ShowAddress %>" />
        <input type="hidden" name="list[<%=Model.index%>].first" value="<%=Model.first%>" />
        <input type="hidden" name="list[<%=Model.index%>].last" value="<%=Model.last%>" />
        <input type="hidden" name="list[<%=Model.index%>].dob" value="<%=Model.birthday.ToShortDateString()%>" />
        <input type="hidden" name="list[<%=Model.index%>].phone" value="<%=Model.phone%>" />
        <input type="hidden" name="list[<%=Model.index%>].homecell" value="<%=Model.homecell%>" />
        <input type="hidden" name="list[<%=Model.index%>].address" value="<%=Model.address%>" />
        <input type="hidden" name="list[<%=Model.index%>].zip" value="<%=Model.zip%>" />
        <input type="hidden" name="list[<%=Model.index%>].city" value="<%=Model.city%>" />
        <input type="hidden" name="list[<%=Model.index%>].state" value="<%=Model.state%>" />
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
<input type="hidden" name="list[<%=Model.index%>].email" value="<%=Model.email%>" />
<input type="radio" name="list[<%=Model.index%>].option" value="1" 
<%=Model.option == 1 ? "checked='checked'" : "" %> class="option" /> 5K Run<br />
<input type="radio" name="list[<%=Model.index%>].option" value="2" 
<%=Model.option == 2 ? "checked='checked'" : "" %> class="option" /> 1 mile Fun Run
<input type="hidden" name="list[<%=Model.index%>].age" value="<%=Model.age%>" class="age" />
</td>
</tr>