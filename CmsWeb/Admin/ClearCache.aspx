<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClearCache.aspx.cs" Inherits="CmsWeb.Admin.ClearCache" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <%=CmsWeb.ViewExtensions2.StandardCss()%>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2>Cache Cleared</h2>
        <cc1:HyperLinkDialog ID="HyperLinkDialog1" runat="server" NavigateUrl="~/">Go Home</cc1:HyperLinkDialog>    
     </div>
    </form>
</body>
</html>
