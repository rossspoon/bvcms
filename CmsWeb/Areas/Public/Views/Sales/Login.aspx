<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Download
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <% using (Html.BeginForm("Download", "Sales")) { %>
        <div>
            <fieldset>
                <legend>Download Credentials</legend>
                <p>
                    <label for="username">Username:</label>
                    <%= Html.TextBox("username") %>
                </p>
                <p>
                    <label for="password">Password:</label>
                    <%= Html.Password("password") %>
                </p>
                <p>
                    <input type="submit" value="Log On" />
                    <%= Html.ValidationMessage("login") %>
                </p>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
