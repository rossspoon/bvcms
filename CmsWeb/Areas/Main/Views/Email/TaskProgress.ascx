<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if ((string)ViewData["completed"] == "still running")
    { %>
<h3>Your emails are being sent, You can watch on this page,<br />
or you can <a href="/">go back to the home page at any time.</a></h3>
<% }
    else
    { %>
<h3>Email has completed. <a href="/">Go back to the home page</a></h3>
<% } %>

<table cellspacing="0" border="1" cellpadding="3">
<tr><td>Queued</td><td><%=ViewData["queued"] %></td></tr>
<tr><td>Started</td><td><%=ViewData["started"] %></td></tr>
<tr><td>Completed</td><td><%=ViewData["completed"] %></td></tr>
<tr><td>Total Emails</td><td><%=ViewData["total"] %></td></tr>
<tr><td>Sent</td><td><%=ViewData["sent"] %></td></tr>
<tr><td>Elapsed</td><td><%=ViewData["elapsed"] %></td></tr>
</table>
