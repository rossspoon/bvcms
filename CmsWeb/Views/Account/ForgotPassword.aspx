<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="rhead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Forgot Password</title>
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Forgot Password</h2>
    <p>
        Use the form below to request a new password. 
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
                    <label for="dob">Date of Birth:</label>
                    <%= Html.TextBox("dob") %>
                    <%= Html.ValidationMessage("dob") %>
                </p>
                <p>
                    <input type="submit" value="Request New Password" />
                </p>
            </fieldset>
        </div>
    </form>
</asp:Content>
