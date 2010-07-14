<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurgeProgress.aspx.cs" Inherits="CmsWeb.PurgeProgress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purge Progress</title>
    <meta http-equiv="refresh" content="10">
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Count: <%=Session["purgecount"] %>
        |
    Time: <%=Session["purgetime"] %>
        |
    Speed: <%=Session["purgespeed"] %>
        |
    <%=Session["purgefinished"] %>
    </div>
    </form>
</body>
</html>
