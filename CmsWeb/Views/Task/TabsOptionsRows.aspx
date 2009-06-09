<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>
<% Html.RenderPartial("TabsOptions", Model); %>
<---------->
<% Html.RenderPartial("Rows", Model); %>

