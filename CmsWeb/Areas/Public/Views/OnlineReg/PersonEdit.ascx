<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<table cellspacing="6">
<% if (Model.UserSelectsOrganization())
   { %>
    <tr>
        <td colspan="3"><label for="classid">Choose Class</label>
        <%=Html.DropDownList3(null, "m.List[" + Model.index + "].classid", Model.Classes(), Model.classid.ToString())%>
        <%=Html.ValidationMessage("classid") %></td>
    </tr>
<% } %>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><input id="first" type="text" name="m.List[<%=Model.index%>].first" value="<%=Model.first%>" />
        </td>
        <td><%= Html.ValidationMessage("first") %> <%= Html.ValidationMessage("find") %></td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td><input id="last" type="text" name="m.List[<%=Model.index%>].last" value="<%=Model.last%>" /></td>
        <td>suffix:<input type="text" name="m.List[<%=Model.index%>].suffix" class="short" value="<%=Model.suffix%>" />
        <%= Html.ValidationMessage("last") %></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td><input id="dob" type="text" name="m.List[<%=Model.index%>].dob" value="<%=Model.dob%>" class="dob" title="m/d/y, mmddyy, mmddyyyy" /></td>
        <td><span id="age"><%=Model.age %></span> (m/d/y) <%= Html.ValidationMessage("dob") %></td>
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
