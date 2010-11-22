<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Progress</title>
    <% if ((string)ViewData["completed"] == "still running")
       { %>
    <script type="text/javascript">
        window.setTimeout('document.location.replace(document.location.href);', 3000);
    </script>
    <% } %>
</head>
<body style="font-family: Arial, Helvetica">
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
</body>
</html>
