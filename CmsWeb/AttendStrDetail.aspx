<%@ Page Language="C#" StylesheetTheme="Standard" AutoEventWireup="true" CodeBehind="AttendStrDetail.aspx.cs" Inherits="CMSWeb.AttendStrDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HyperLink ID="Name" runat="server"></asp:HyperLink><br />
    <asp:HyperLink ID="Org" runat="server"></asp:HyperLink>
    <div style="text-align:center">
        <asp:GridView ID="GridView1" runat="server" style="text-align:left" 
            SkinID="GridViewSkin">
        </asp:GridView>
        <div></div>
        <asp:GridView ID="GridView2" runat="server" style="text-align:left" 
            SkinID="GridViewSkin">
        </asp:GridView>
    </div>
    </form>
</body>
</html>
