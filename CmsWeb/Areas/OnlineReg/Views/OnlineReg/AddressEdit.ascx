<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
    <tr><th colspan="3"><span class="blue">Please provide additional information for a new record</span></th></tr>
<% if(Model.whatfamily == 3)
   { %>
    <tr>
        <td><label for="addr">Address</label> <%=Html.NotRequired(!Model.RequiredAddr()) %></td>
        <td><input type="text" name="m.List[<%=Model.index%>].address" value="<%=Model.address%>" /></td>
        <td colspan="3"><%= Html.ValidationMessage("address")%></td>
    </tr>
    <tr>
        <td><label for="zip">Zip</label> <%=Html.NotRequired(!Model.RequiredZip()) %></td>
        <td><input id="zip" type="text" name="m.List[<%=Model.index%>].zip" value="<%=Model.zip%>" /></td>
        <td colspan="3"><%= Html.ValidationMessage("zip")%></td>
    </tr>
    <tr>
        <td><label for="city">City</label></td>
        <td><input id="city" type="text" name="m.List[<%=Model.index%>].city" value="<%=Model.city%>" /></td>
        <td colspan="3"><%= Html.ValidationMessage("city")%></td>
    </tr>
    <tr>
        <td><label for="state">State</label></td>
        <td><%=Html.DropDownList3("state", "m.List[" + Model.index + "].state", Model.StateCodes(), Model.state) %></td>
        <td colspan="3"></td>
    </tr>
<% } 
   else
   { %>
   <%=Html.Hidden3("m.List[" + Model.index + "].address", Model.address)%>
   <%=Html.Hidden3("m.List[" + Model.index + "].city", Model.city)%>
   <%=Html.Hidden3("m.List[" + Model.index + "].state", Model.state)%>
   <%=Html.Hidden3("m.List[" + Model.index + "].zip", Model.zip)%>
<% } %>
   <%=Html.Hidden3("m.List[" + Model.index + "].whatfamily", Model.whatfamily)%>
    <tr>
        <td><label for="gender">Gender</label> <%=Html.NotRequired(!Model.RequiredGender()) %></td>
        <td><input type="radio" name="m.List[<%=Model.index%>].gender" value="1" <%=Model.gender == 1 ? "checked='checked'" : "" %> /> Male<br />
        <input type="radio" name="m.List[<%=Model.index%>].gender" value="2" <%=Model.gender == 2 ? "checked='checked'" : "" %> /> Female</td>
        <td colspan="3"><%= Html.ValidationMessage("gender") %></td>
    </tr>
    <tr>
        <td><label for="gender">Marital Status</label> <%=Html.NotRequired(!Model.RequiredMarital()) %></td>
        <td><input type="radio" name="m.List[<%=Model.index%>].married" value="10" <%=Model.married == 10 ? "checked='checked'" : "" %> /> Single<br />
        <input type="radio" name="m.List[<%=Model.index%>].married" value="20" <%=Model.married == 20 ? "checked='checked'" : "" %> /> Married</td>
        <td colspan="3"><%= Html.ValidationMessage("married") %></td>
    </tr>
