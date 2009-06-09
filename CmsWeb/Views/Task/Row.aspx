<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskInfo>" %>
<tr id="r<%=ViewData.Model.Id%>">
<% if (ViewData.Model.IsSelected)
    Html.RenderPartial("Detail", Model.GetDetail());
else
    Html.RenderPartial("Columns", Model); %>
</tr>
