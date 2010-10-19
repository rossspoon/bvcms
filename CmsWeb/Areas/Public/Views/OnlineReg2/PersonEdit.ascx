<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel2>" %>
<table cellspacing="6">
    <tr>
        <td><label for="first">First Name</label></td>
        <td><input id="first" type="text" name="m.List[<%=Model.index%>].first" value="<%=Model.first%>" /></td>
        <td>middle:</td><td><input type="text" name="m.List[<%=Model.index%>].middle" class="short" value="<%=Model.middle%>" /></td>
        <td><%= Html.ValidationMessage("first") %> <%= Html.ValidationMessage("find") %></td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td nowrap="nowrap"><input id="last" type="text" name="m.List[<%=Model.index%>].last" value="<%=Model.last%>" /></td>
        <td>suffix:</td><td><input type="text" name="m.List[<%=Model.index%>].suffix" class="short" value="<%=Model.suffix%>" /></td>
        <td><%= Html.ValidationMessage("last") %></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td nowrap="nowrap"><input id="dob" type="text" name="m.List[<%=Model.index%>].dob" value="<%=Model.dob%>" class="dob" title="m/d/y, mmddyy, mmddyyyy" /></td>
        <td>age:</td><td><span id="age"><%=Model.age %></span> (m/d/y)</td>
        <td><%= Html.ValidationMessage("dob") %></td>
    </tr>
    <tr>
        <td><label for="phone">Phone</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].phone" value="<%=Model.phone%>" /></td>
        <td colspan="3"><%= Html.ValidationMessage("phone")%></td>
    </tr>
    <tr>
        <td><label for="email">Contact Email</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].email" value="<%=Model.email%>" /></td>
        <td colspan="3"><%= Html.ValidationMessage("email")%></td>
    </tr>
    <% if (!Model.ShowAddress)
       { %>
    <tr><td></td>
        <td colspan="4">
        <% if (!Model.Found.HasValue)
           { %>
            <a href="/OnlineReg2/PersonFind/<%=Model.index %>" class="submitbutton">Find Record</a>
        <% }
           else
           { %>
           <p class="blue"><%=Model.NotFoundText %></p>
           <a href="/OnlineReg2/PersonFind/<%=Model.index %>" class="submitbutton">Try Find Again</a>
            <% if (Model.IsValidForContinue && !Model.MemberOnly())
               { %>
           or <a id="regnew" href="/OnlineReg2/ShowMoreInfo/<%=Model.index %>" class="submitbutton">Register as new</a>
            <% }
           } %>
        </td>
    </tr>
    <% }
       else
       { %>
    <% Html.RenderPartial("AddressEdit", Model); %>
    <tr><td></td>
        <td colspan="4"><a href="/OnlineReg2/SubmitNew/<%=Model.index %>" class="submitbutton">Submit</a></td>
    </tr>
    <% } %>
</table>
