<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Confirm</title>
</head>
<body>
    <div>
    <h2>Opt Out Confirmation</h2>
        <div>You have been removed from any more emails from address: <strong><%=ViewData["fromemail"] %></strong></div>
    </div>
</body>
</html>
