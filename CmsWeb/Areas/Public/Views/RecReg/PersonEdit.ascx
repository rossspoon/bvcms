<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RecRegModel>" %>
<tr>
<td>
<table>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><input type="text" name="first" value="<%=Model.first%>" />
        <input type="hidden" name="ShowAddress" value="<%=Model.ShowAddress %>" />
        <input type="hidden" name="divid" value="<%=Model.divid %>" />
        </td>
        <td><%= Html.ValidationMessage("first") %> <%= Html.ValidationMessage("find") %></td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td><input type="text" name="last" value="<%=Model.last%>" /></td>
        <td>suffix:<input type="text" name="suffix" class="short" value="<%=Model.suffix%>" />
        <%= Html.ValidationMessage("last") %></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td><input type="text" name="dob" value="<%=Model.dob%>" class="dob" title="m/d/y, mmddyy, mmddyyyy" /></td>
        <td><span id="age"><%=Model.age %></span> (m/d/y) <%= Html.ValidationMessage("dob") %></td>
    </tr>
    <tr>
        <td><label for="phone">Phone</label></td>
        <td><input type="text" name="phone" value="<%=Model.phone%>" /></td>
        <td><input type="radio" name="homecell" value="h" <%=Model.homecell == "h" ? "checked='checked'" : "" %> /> Home<br />
        <input type="radio" name="homecell" value="c" <%=Model.homecell == "c" ? "checked='checked'" : "" %> /> Cell
        <%= Html.ValidationMessage("phone")%></td>
    </tr>
    <tr>
        <td><label for="email">Contact Email</label></td>
        <td><input type="text" name="email" value="<%=Model.email%>" /></td>
        <td><%= Html.ValidationMessage("email")%></td>
    </tr>
    <% if (!Model.ShowAddress)
       { %>
    <tr><td></td>
        <td>
        <% if (!Model.Found.HasValue || Model.RecAgeDiv == null)
           { %>
            <a href="/RecReg/PersonFind" class="submitbutton">Find Record</a>
        <% }
           else
           { %>
           <p><%=CMSWeb.Models.SearchPeopleModel.NotFoundText %></p>
            <a href="/RecReg/PersonFind" class="submitbutton">Try Find Again</a>
            or <a href="/RecReg/ShowMoreInfo" class="submitbutton">Register as new</a>
        <% } %>
        </td>
    </tr>
    <% }
       else
       { %>
    <% Html.RenderPartial("AddressEdit", Model); %>
    <tr><td></td>
        <td><a href="/RecReg/SubmitNew" class="submitbutton">Submit</a></td>
    </tr>
    <% } %>
</table>
</td>
</tr>