<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% if ((bool)ViewData["finished"])
   { %>
<!--Completed-->
<h3>Email has completed</h3> 
<% }
   else
   { %>
<h3>Your emails have been queued and will be sent.</h3>
<% } %>
<table cellspacing="0" border="1" cellpadding="3">
<tr><td>Queued</td><td><%=ViewData["queued"] %></td></tr>
<tr><td>Started</td><td><%=ViewData["started"] %></td></tr>
<tr><td>Completed</td><td><%=ViewData["completed"] %></td></tr>
<tr><td>Total Emails</td><td><%=ViewData["total"] %></td></tr>
<tr><td>Sent</td><td><%=ViewData["sent"] %></td></tr>
<tr><td>Elapsed</td><td><%=ViewData["elapsed"] %></td></tr>
</table>
