<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="rhead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Forgot Password</title>
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Forgot Password</h2>
    <p>
        Use the form below to request a new password.
        Note: you will need to know your username for this to work. 
        If you do not know your username, then please use the forgot username link first.
    </p>
    <form action="/Account/ForgotPassword" method="post">
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="currentPassword">User Name:</label>
                    <%= Html.TextBox("username") %>
                    <%= Html.ValidationMessage("username") %>
                </p>
                <p>
                    <input type="submit" value="Request New Password" />
                </p>
            </fieldset>
        </div>
    </form>
</asp:Content>
