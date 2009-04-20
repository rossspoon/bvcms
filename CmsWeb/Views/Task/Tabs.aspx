<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>
<% foreach (var list in ViewData.Model.FetchTaskLists())
   { %>
<li id="<%=list.Id %>" onclick="return ClickTab('<%=list.Id%>')"><a href="#"><span><%=list.Name%></span></a></li>
<% } %>

