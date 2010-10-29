<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel2>" %>
<% if ((Model.classid ?? 0) > 0)
   { %>
    <tr>
        <td>Chosen Class:</td>
        <td colspan="4"><%=Html.CodeDesc("classid", CmsWeb.Models.OnlineRegModel2.Classes(Model.divid))%></td>
    </tr>
<% } %>
    <tr>
        <td><label for="first">First Name</label></td>
        <td colspan="4"><%=Model.first %></td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td colspan="4"><%=Model.last %> <%= Html.ValidationMessage("find") %></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td colspan="4"><%=Model.birthday.FormatDate("not given") %> <span><%=Model.age %></span>
        <%= Html.ValidationMessage("dob") %></td>
    </tr>
    <tr>
        <td><label for="phone">Phone</label></td>
        <td colspan="4"><%=Model.phone.FmtFone() %></td>
    </tr>
    <% if (Model.email.HasValue())
       { %>
    <tr>
        <td><label for="email">Contact Email</label></td>
        <td colspan="4"><%=Model.email %></td>
    </tr>
    <% }
       if (Model.ShowAddress)
       {
           Html.RenderPartial("AddressDisplay", Model);
       } %>
