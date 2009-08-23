<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="rhead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Forgot Username</title>
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Forgot Username</h2>
    <p>
        Use the form below to request your username. 
    </p>
    <form action="/Account/ForgotUsername" method="post">
        <div>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="currentPassword">Email Address on record:</label>
                    <%= Html.TextBox("email") %>
                    <%= Html.ValidationMessage("email") %>
                </p>
                <p>
                    <label for="dob">Date of Birth:</label>
                    <%= Html.TextBox("dob") %>
                    <%= Html.ValidationMessage("dob") %>
                </p>
                <p>
                    <input type="submit" value="Request Username" />
                </p>
            </fieldset>
        </div>
    </form>
</asp:Content>
