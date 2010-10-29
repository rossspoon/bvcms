<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<table cellspacing="6">
    <tr>
        <td><label for="username">Username</label></td>
        <td><input id="username" type="text" name="username" value="<%=Model.username %>" /></td>
        <td><%= Html.ValidationMessage("username") %></td>
    </tr>
    <tr>
        <td><label for="password">Password</label></td>
        <td><input id="password" type="password" name="password" value="<%=Model.password%>" /></td>
        <td><%= Html.ValidationMessage("password") %></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td nowrap="nowrap"><input id="dob" type="text" name="m.List[<%=Model.index%>].dob" value="<%=Model.dob%>" class="dob" title="m/d/y, mmddyy, mmddyyyy" />
        <span id="age"><%=Model.age %></span> (m/d/y)</td>
        <td><%= Html.ValidationMessage("dob") %></td>
    </tr>
    <tr>
        <td><label for="phone">Phone</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].phone" value="<%=Model.phone%>" /></td>
        <td><%= Html.ValidationMessage("phone")%></td>
    </tr>
    <tr>
        <td><label for="email">Contact Email</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].email" value="<%=Model.email%>" /></td>
        <td><%= Html.ValidationMessage("email")%></td>
    </tr>
    <% if (!Model.ShowAddress)
       { %>
    <tr><td></td>
        <td>
        <% if (!Model.Found.HasValue)
           { %>
            <a href="/OnlineReg/PersonFind/<%=Model.index %>" class="submitbutton">Find Record</a>
        <% }
           else
           { %>
           <p class="blue"><%=Model.NotFoundText %></p>
            <a href="/OnlineReg/PersonFind/<%=Model.index %>" class="submitbutton">Try Find Again</a>
            <% if (Model.IsValidForContinue && !Model.MemberOnly() && Model.TryCount >= 3)
               { %>
            or <a id="regnew" href="/OnlineReg/ShowMoreInfo/<%=Model.index %>" class="submitbutton">Register as new</a>
            <% } %>
            Number of Tries: <%=Model.TryCount %>
        <% } %>
        </td>
    </tr>
    <% }
       else
       { %>
    <% Html.RenderPartial("AddressEdit", Model); %>
    <tr><td></td>
        <td><a href="/OnlineReg/SubmitNew/<%=Model.index %>" class="submitbutton">Submit</a></td>
    </tr>
    <% } %>
</table>
