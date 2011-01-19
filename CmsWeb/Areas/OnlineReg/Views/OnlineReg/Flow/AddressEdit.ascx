<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
    <tr><td colspan="3">Please provide additional<br />information for a new profile</td></tr>
<% if(Model.whatfamily == 3)
   { %>
    <tr>
        <td><%=Html.IsRequired(Model.RequiredAddr()) %>
            <label for="addr">Address</label> </td>
        <td><%=Html.TextBox(Model.inputname("address"), Model.address) %>
            <div><%= Html.ValidationMessage(Model.inputname("address"))%></div></td>
        <td colspan="3"></td>
    </tr>
    <tr>
        <td><%=Html.IsRequired(Model.RequiredZip()) %>
            <label for="zip">Zip</label></td>
        <td><%=Html.TextBox3("zip", Model.inputname("zip"), Model.zip) %>
            <div><%= Html.ValidationMessage(Model.inputname("zip"))%></div></td>
        <td colspan="3"></td>
    </tr>
    <tr>
        <td><%=Html.NotRequired() %>
            <label for="city">City</label></td>
        <td><%=Html.TextBox3("city", Model.inputname("city"), Model.city) %>
            <div><%= Html.ValidationMessage(Model.inputname("city"))%></div></td>
        <td colspan="3"></td>
    </tr>
    <tr>
        <td><%=Html.NotRequired() %>
            <label for="state">State</label></td>
        <td><%=Html.DropDownList3("state", Model.inputname("state"), Model.StateCodes(), Model.state) %></td>
        <td colspan="3"></td>
    </tr>
<% } 
   else
   { %>
   <%=Html.Hidden3(Model.inputname("address"), Model.address)%>
   <%=Html.Hidden3(Model.inputname("city"), Model.city)%>
   <%=Html.Hidden3(Model.inputname("state"), Model.state)%>
   <%=Html.Hidden3(Model.inputname("zip"), Model.zip)%>
<% } %>
   <%=Html.Hidden3(Model.inputname("whatfamily"), Model.whatfamily)%>
    <tr>
        <td><%=Html.IsRequired(Model.RequiredGender()) %>
            <label for="gender">Gender</label></td>
        <td><%=Html.RadioButton(Model.inputname("gender"), "1", Model.gender == 1) %> Male
            <%=Html.RadioButton(Model.inputname("gender"), "2", Model.gender == 2) %> Female
            <div><%= Html.ValidationMessage(Model.inputname("gender")) %></div></td>
        <td colspan="3"></td>
    </tr>
    <tr>
        <td><%=Html.IsRequired(Model.RequiredMarital()) %>
            <label for="married">Marital Status</label></td>
        <td><%=Html.RadioButton(Model.inputname("married"), "10", Model.married == 10) %> Single
            <%=Html.RadioButton(Model.inputname("married"), "20", Model.married == 20) %> Married
            <div><%= Html.ValidationMessage(Model.inputname("married")) %></div></td>
        <td colspan="3"></td>
    </tr>
