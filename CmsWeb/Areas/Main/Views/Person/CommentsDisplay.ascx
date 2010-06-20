<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (Page.User.IsInRole("Edit"))
   { %>
<a class="displayedit" href="/Person/CommentsEdit/<%=ViewData["PeopleId"] %>">Edit</a>
<% } %>
<strong>Comments:</strong><br />
<%=ViewData["Comments"] %>

<% 
    CmsData.Person p = DbUtil.Db.LoadPersonById((int)ViewData["PeopleId"]);
    Html.RenderPartial("PeopleExtrasGrid", p); 
%>
