<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>
<% Html.RenderPartial("Columns", ViewData.Model.FetchTask((int)ViewData["rowid"])); %>
<---------->
<% Html.RenderPartial("Detail", ViewData.Model.FetchTask((int)ViewData["detailid"])); %>
