<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskInfo>" %>
    <td><% if(Model.IsAnOwner) { %><input type="checkbox" value= "<%=Model.Id%>" class = "actionitem" /></td><% } %>
    <td><%=Model.Priority%></td>
    <td><%="{0:d}".Fmt(Model.DueOrCompleted)%></td>
    <td><%=Html.HyperLink("javascript:ShowDetail(" + Model.Id + ")", Model.Description)%></td>
    <td><%=Model.WhoId.HasValue ? Html.HyperLink("/Person/Index/" + Model.WhoId, Model.Who) : ""%></td>
    <td><%=Model.Status%></td>
    <td><%=Model.DispOwner%></td>