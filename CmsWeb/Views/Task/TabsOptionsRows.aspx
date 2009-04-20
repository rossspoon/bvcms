<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>
<% Html.RenderPartial("TabsOptions", ViewData.Model); %>
<---------->
<% Html.RenderPartial("Rows", ViewData.Model); %>

