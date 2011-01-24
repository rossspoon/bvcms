<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<% if ((Model.classid ?? 0) > 0)
   { %>
    <tr>
        <td>Chosen Class:</td>
        <td><%=Html.CodeDesc("classid", CmsWeb.Models.OnlineRegModel.Classes(Model.divid, Model.classid ?? 0))%></td>
    </tr>
<% }
   if (!Model.Finished()) // name gets displayed elsewhere when finished
   { %>
    <tr>
        <td width="30%"><label for="first">First Name</label></td>
        <td><%=Model.first %></td>
    </tr>
    <tr>
        <td><label for="last">Last Name</label></td>
        <td><%=Model.last %> <%= Html.ValidationMessage("find") %></td>
    </tr>
<%  } %>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td><%=Model.birthday.FormatDate("not given") %> <span><%=Model.age %></span>
            <div><%= Html.ValidationMessage(Model.inputname("dob")) %></div></td>
    </tr>
    <tr>
        <td><label for="phone">Phone</label></td>
        <td><%=Model.phone.FmtFone() %></td>
    </tr>
<%  if (Model.email.HasValue())
    { %>
    <tr>
        <td><label for="email">Contact Email</label></td>
        <td><%=Model.email %></td>
    </tr>
<%  }
    if (Model.ShowAddress)
    {
       Html.RenderPartial("Flow/AddressDisplay", Model);
    } %>
