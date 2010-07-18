<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UrgentMessage.aspx.cs" Inherits="CmsWeb.Admin.UrgentMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <%=CmsWeb.ViewExtensions2.StandardCss() %>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="Message" runat="server" Width="644px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="SetMessage" runat="server" Text="Submit" 
            onclick="SetMessage_Click" />
        <asp:Button ID="Cancel" runat="server" Text="Cancel" onclick="Cancel_Click" />
    </div>
    </form>
</body>
</html>
