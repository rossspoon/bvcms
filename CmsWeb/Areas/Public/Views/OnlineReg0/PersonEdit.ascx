<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel0>" %>
<table cellspacing="6">
<% if (Model.UserSelectsOrganization())
   { %>
    <% if (Model.FilledClasses().Count() > 0)
       { %>
    <tr>
        <td colspan="3">
        <span style="color:Red">Filled Classes:</span><br />
        <% foreach (var c in Model.FilledClasses())
           { %>
           <%=c %><br />
        <% } %>
        </td>
    </tr>
    <% } %>
    <tr>
        <td colspan="3">
   <% if (Model.Classes().Count() > 1)
      { %>
        <span class="blue">Select a class:</span><br />
        <%=Html.DropDownList3(null, "m.List[" + Model.index + "].classid", Model.Classes(), Model.classid.ToString())%>
        <%=Html.ValidationMessage("classid") %>
   <% }
      else
      { %>
        <span class="blue">Here is the only available class:</span><br />
        <%=Model.Classes().Single().Text%>
        <%=Html.Hidden("m.List[" + Model.index + "].classid", Model.Classes().Single().Value)%>
   <% } %>
        </td>
    </tr>
<% } %>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><input id="first" type="text" name="m.List[<%=Model.index%>].first" value="<%=Model.first%>" />
        </td>
        <td><%= Html.ValidationMessage("first") %> <%= Html.ValidationMessage("find") %>
        </td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td nowrap="nowrap"><input id="last" type="text" name="m.List[<%=Model.index%>].last" value="<%=Model.last%>" />
        suffix:<input type="text" name="m.List[<%=Model.index%>].suffix" class="short" value="<%=Model.suffix%>" /></td>
        <td><%= Html.ValidationMessage("last") %></td>
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
