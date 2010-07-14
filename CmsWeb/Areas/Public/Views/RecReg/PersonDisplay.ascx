<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.RecRegModel>" %>
<tr>
<td>
<table>
    <tr>
        <td><label for="first">First Name</label></td>
        <td><%=Model.first %>
        <%=Html.Hidden("PeopleId") %>
        <%=Html.Hidden("divid") %>
        <%=Html.Hidden("Found") %>
        <%=Html.Hidden("IsNew") %>
        <%=Html.Hidden("ShowAddress") %>
        <%=Html.Hidden("first") %>
        <%=Html.Hidden("last") %>
        <%=Html.Hidden("dob") %>
        <%=Html.Hidden("phone") %>
        <%=Html.Hidden("homecell") %>
        <%=Html.Hidden("address") %>
        <%=Html.Hidden("email") %>
        <%=Html.Hidden("zip") %>
        <%=Html.Hidden("city") %>
        <%=Html.Hidden("state") %>
        <%=Html.Hidden("gender") %>
        <%=Html.Hidden("married") %>
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
    <% }
       if (Model.ShowAddress)
       {
           Html.RenderPartial("AddressDisplay", Model);
       } %>
</table>
</td>
</tr>    
