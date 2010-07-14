<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.TaskModel>" %>
<% Html.RenderPartial("Columns", Model.FetchTask((int)ViewData["rowid"])); %>
<---------->
<% Html.RenderPartial("Detail", Model.FetchTask((int)ViewData["detailid"])); %>
