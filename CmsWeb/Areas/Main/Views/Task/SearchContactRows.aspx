<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.SearchContactModel>" %>
<% if(ViewData.Model.Count == 0)
   { %>
<tr><td colspan="5">No matching records.</td></tr>
<% } %>
<% foreach (var c in ViewData.Model.ContactList())
   { %>
<tr>
    <td><%=Html.HyperLink("javascript:AddSourceContact({0})".Fmt(c.ContactId), "select")%></td>
    <td><%=c.ContactDate.ToShortDateString()%></td>
    <td><%=c.ContactReason%></td>
    <td><%=c.TypeOfContact%></td>
    <td><%=ViewData.Model.GetContacteeList(c.ContactId)%></td>
</tr>
<% } %>
<tr><td colspan="5">
<%=Html.Hidden("Count",ViewData.Model.Count)%>
<%=Html.Hidden("Sort",ViewData.Model.Sort)%>
</td></tr>
