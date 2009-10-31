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
    <h1>Page Not Found</h1>
    <p>URL: <%=Request.QueryString["aspxerrorpath"] %></p>
    <p>click <a href="/">here</a> to go the home page or try again with a different url.</p>
</body>
</html>
