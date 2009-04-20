<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskInfo>" %>
    <td><% if(ViewData.Model.IsAnOwner) { %><input type="checkbox" value= "<%=ViewData.Model.Id%>" class = "actionitem" /></td><% } %>
    <td><%=ViewData.Model.Priority%></td>
    <td><%="{0:d}".Fmt(ViewData.Model.DueOrCompleted)%></td>
    <td><%=Html.HyperLink("javascript:ShowDetail(" + ViewData.Model.Id + ")", ViewData.Model.Description)%></td>
    <td><%=ViewData.Model.WhoId.HasValue ? Html.HyperLink("/Person.aspx?id=" + ViewData.Model.WhoId, ViewData.Model.Who) : ""%></td>
    <td><%=ViewData.Model.Status%></td>
    <td><%=ViewData.Model.DispOwner%></td>