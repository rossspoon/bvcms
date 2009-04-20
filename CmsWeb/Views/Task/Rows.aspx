<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>
<% if(ViewData.Model.Count == 0)
   { %>
<tr id="nomatch"><td colspan="7">No matching records.</td></tr>
<% } %>
<% foreach (var t in ViewData.Model.FetchTasks())
    Html.RenderPartial("Row", t); %>
<tr><td colspan="7">
<input type="hidden" id="Count" value='<%=ViewData.Model.Count%>' />
<%=Html.Hidden("Sort",ViewData.Model.Sort)%>
</td></tr>

