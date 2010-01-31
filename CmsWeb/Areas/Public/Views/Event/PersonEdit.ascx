﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonEventModel>" %>
<tr <%=Model.index % 2 == 1 ? "class='alt'" : "" %>><td>
<table>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><input type="text" name="list[<%=Model.index%>].first" value="<%=Model.first%>" /></td>
        <td><%= Html.ValidationMessage("first") %> <%= Html.ValidationMessage("find") %>
        <input type="hidden" name="list[<%=Model.index%>].ShowAddress" value="<%=Model.ShowAddress %>" />
        <input type="hidden" name="list[<%=Model.index%>].index" value="<%=Model.index%>" /></td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td><input type="text" name="list[<%=Model.index%>].last" value="<%=Model.last%>" /></td>
        <td><%= Html.ValidationMessage("last") %></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td><input type="text" name="list[<%=Model.index%>].dob" value="<%=Model.dob%>" class="dob" /></td>
        <td><span id="age"></span> <%= Html.ValidationMessage("dob") %></td>
    </tr>
    <tr>
        <td><label for="phone">Phone</label></td>
        <td><input type="text" name="list[<%=Model.index%>].phone" value="<%=Model.phone%>" /></td>
        <td><input type="radio" name="list[<%=Model.index%>].homecell" value="h" <%=Model.homecell == "h" ? "checked='checked'" : "" %> /> Home<br />
        <input type="radio" name="list[<%=Model.index%>].homecell" value="c" <%=Model.homecell == "c" ? "checked='checked'" : "" %> /> Cell
        <%= Html.ValidationMessage("phone")%></td>
    </tr>
    <% if (Model.index == 0)
       { %>
    <tr>
        <td><label for="email">Contact Email</label></td>
        <td><input type="text" name="list[<%=Model.index%>].email" value="<%=Model.email%>" /></td>
        <td><%= Html.ValidationMessage("email")%></td>
    </tr>
    <% } %>
    <% if (!Model.ShowAddress)
       { %>
    <tr><td></td>
        <td>
        <% if (!Model.Found.HasValue)
           { %>
            <a href="/Event/PersonFind/<%=Model.index %>" class="submitbutton">Find Record</a>
        <% }
           else
           { %>
            <a href="/Event/PersonFind/<%=Model.index %>" class="submitbutton">Try Find Again</a>
            or <a href="/Event/ShowMoreInfo/<%=Model.index %>" class="submitbutton">Register as new</a>
        <% } %>
        </td>
    </tr>
    <% }
       else
       { %>
    <% Html.RenderPartial("AddressEdit", Model); %>
    <tr><td></td>
        <td><a href="/Event/SubmitNew/<%=Model.index %>" class="submitbutton">Submit</a></td>
    </tr>
    <% } %>
</table>
</td>
<td align="right" valign="top">
<% if (Model.index > 0)
   { %>
    <a class="cancel" href="/Event/Cancel/<%=Model.index %>">Cancel this participant</a>
<% } %>
</td>
</tr>