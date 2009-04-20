<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Admin_Default" Title="Administrator Pages" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Admin/Scaffold.aspx">Scaffold</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Admin/Users.aspx">Users</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Admin/Roles.aspx">Groups</asp:HyperLink><br />
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Admin/CMSPageList.aspx">Pages</asp:HyperLink>
</asp:Content>
