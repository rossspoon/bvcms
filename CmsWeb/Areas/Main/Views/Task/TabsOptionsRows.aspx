<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.TaskModel>" %>
<% Html.RenderPartial("TabsOptions", Model); %>
<---------->
<% Html.RenderPartial("Rows", Model); %>

