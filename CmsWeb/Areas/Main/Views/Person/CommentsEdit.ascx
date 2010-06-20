<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<a class="displayedit" href="/Person/CommentsDisplay/<%=ViewData["PeopleId"] %>">cancel</a>
Comments:<br />
<%=Html.TextArea("Comments", new { rows = "16", style = "width:90%" })%><br />
<a href="/Person/CommentsUpdate/<%=ViewData["PeopleId"] %>" class="submitbutton">Save Changes</a>
