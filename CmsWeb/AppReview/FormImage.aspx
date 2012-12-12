<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormImage.aspx.cs" Inherits="CmsWeb.FormImage" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Volunteer Application Form</title>
    <link href="/Content/styles/Common2.css" rel="stylesheet" type="text/css" />    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink ID="HyperLink2" runat="server" ForeColor="Red"></asp:HyperLink>
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <asp:ImageButton ID="ImageButton1" runat="server" OnClick="ImageButton1_Click" ToolTip="Click to toggle image size" /><br />
    <cc1:LinkButtonConfirm ID="Delete" runat="server" Text="Delete" Confirm="Are you sure you want to delete this?"
        OnClick="Delete_Click"></cc1:LinkButtonConfirm>
    </form>
</body>
</html>
