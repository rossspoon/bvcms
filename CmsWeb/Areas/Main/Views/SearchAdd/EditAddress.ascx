<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchPersonModel>" %>
    <tr>
        <td><label for="homephone">Home Phone</label></td>
        <td><input type="text" name="m.list[<%=Model.index%>].homephone" value="<%=Model.homephone%>" /></td>
        <td><%= Html.ValidationMessage("homephone")%></td>
    </tr>
    <tr>
        <td><label for="address">Address</label></td>
        <td><input id="address" type="text" name="m.list[<%=Model.index%>].address" value="<%=Model.address%>" /></td>
        <td><a id="verifyaddress" href="/SearchAdd/VerifyAddress/" class="bt">Verify</a>
        <%= Html.ValidationMessage("address")%></td>
    </tr>
    <tr>
        <td><label for="address2">Address line 2</label></td>
        <td><input id="address2" type="text" name="m.list[<%=Model.index%>].address2" value="<%=Model.address2%>" />
        <td><%= Html.ValidationMessage("address2")%></td>
    </tr>
    <tr>
        <td><label for="zip">Zip</label></td>
        <td><input id="zip" type="text" name="m.list[<%=Model.index%>].zip" value="<%=Model.zip%>" /></td>
        <td><%= Html.ValidationMessage("zip")%></td>
    </tr>
    <tr>
        <td><label for="city">City</label></td>
        <td><input id="city" type="text" name="m.list[<%=Model.index%>].city" value="<%=Model.city%>" /></td>
        <td><%= Html.ValidationMessage("city")%></td>
    </tr>
    <tr><td><label for="state">State</label></td>
        <td><%=Html.DropDownList3("state", "m.list[" + Model.index + "].state", Model.StateCodes(), Model.state) %></td>
        <td><%= Html.ValidationMessage("state")%></td>
    </tr>

