<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PromoteInfo>" %>
    <td><input type="checkbox" <%=Model.IsSelected? "checked='checked'" : "" %> value= "<%=Model.PeopleId%>" class = "actionitem" /></td>
    <td><a href="/Person.aspx?id=<%=Model.PeopleId %>"><%=Model.Name%></a></td>
    <td><a href="/Organization.aspx?id=<%=Model.CurrClassId %>"><%=Model.CurrClassName %></a></td>
    <td><a href="/Organization.aspx?id=<%=Model.PendingClassId %>"><%=Model.PendingClassName %></a></td>
    <td>"{0:N1}".Fmt(<%=Model.AttendPct %>) <%=Model.AttendIndicator %></td>
    <td><%=Model.Gender %></td>
    <td><%=Model.Birthday %></td>