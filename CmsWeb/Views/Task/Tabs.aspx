<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>
<% foreach (var list in ViewData.Model.FetchTaskLists())
   { %>
   <li><a href='#<%=list.Id %>'><span><%=list.Name%></span></a></li>
<% } %>
