<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Progress</title>
    <% if ((string)ViewData["completed"] == "still running")
       { %>
    <script type="text/javascript">
        window.setTimeout('document.location.replace("/Home");', 2000);
    </script>
    <% } %>
</head>
<body>
<h1>Email Successfully Queued for <%=ViewData["queued"] %></h1>
    <a href="/Home">home</a>
    <a href="/Email/SendNow/<%=ViewData["queueid"] %>">send now</a>
</body>
</html>
