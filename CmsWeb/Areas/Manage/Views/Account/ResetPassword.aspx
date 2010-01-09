<%@Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordSuccessHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Change Password</title>
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Change Password</h2>
    <p>
        Your password has been changed successfully.<br />
        You should receive an email shortly with your new password.
    </p>
</asp:Content>
