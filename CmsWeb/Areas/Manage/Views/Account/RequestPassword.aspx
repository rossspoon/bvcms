<%@Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordSuccessHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Password Reset</title>
</asp:Content>

<asp:Content ID="changePasswordSuccessContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Password Reset Notice Sent</h2>
    <p>
        You should receive an email shortly with a link to reset your password if you gave us your correct username and birthday.
    </p>
</asp:Content>
