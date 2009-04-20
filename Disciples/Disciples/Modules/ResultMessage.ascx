<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultMessage.ascx.cs" Inherits="BellevueTeachers.Modules.ResultMessage" %>
<asp:Panel ID="Result" runat="server">
    <asp:Image ID="Success" ImageUrl="~/images/icons/icon_check.gif" runat="server" ImageAlign="AbsMiddle" />
    <asp:Image ID="Fail" ImageUrl="~/images/icons/icon_error.gif" runat="server" ImageAlign="AbsMiddle" />
    <b></b><asp:Label ID="Message" runat="server"></asp:Label></b>
</asp:Panel>
