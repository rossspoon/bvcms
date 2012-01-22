<%@ Page Language="C#" AutoEventWireup="True" %>
<html>
<head>
    <title>Page Not Found</title>
    <style type="text/css">
        body
        {
            font-family: Arial;
        }
        H1
        {
            color: Red;
        }
    </style>
</head>
<body>
    <p><a href="http://www.bvcms.com">BVCMS homepage</a></p>
    <h1>Database Not Found</h1>
    <p>error: <%=HttpContext.Current.Items["message"] %></p>
</body>
</html>
