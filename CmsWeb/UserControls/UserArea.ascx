<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserArea.ascx.cs" Inherits="CmsWeb.UserArea" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:LoginView ID="LoginView1" EnableViewState="false" runat="server">
    <AnonymousTemplate>
        <a href='<%=Page.ResolveUrl("~/login.aspx")%>'>Login</a>
    </AnonymousTemplate>
    <LoggedInTemplate>
        Hi
        <asp:LoginName ID="LoginName1" runat="server" EnableViewState="False" />
        <br />
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ChangePassword.aspx"
            EnableViewState="False">Change Password</asp:HyperLink>
        |
        <asp:LoginStatus ID="LoginStatus1" runat="server" EnableViewState="False" 
            onloggingout="LoginStatus1_LoggingOut" />
    </LoggedInTemplate>
</asp:LoginView>
<div>
    Active Tag:
    <asp:HyperLink ID="TagLink" NavigateUrl="~/MyTags.aspx" runat="server"></asp:HyperLink>
</div>
<asp:HiddenField ID="HiddenField1" runat="server" />
