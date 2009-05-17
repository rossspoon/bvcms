<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Forms.Models.RegisterModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Register</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Church Database Registration Sign-in</h2>
    <p>
        Please Sign-in with the credentials you were given: 
    </p>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="first">First and Last name</label></td>
                    <td><%= Html.TextBox("name") %></td>
                    <td><%= Html.ValidationMessage("name") %></td>
                </tr>
                <tr>
                    <td><label for="email">Email</label></td>
                    <td><%= Html.TextBox("email") %></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
                <tr>
                    <td><label for="password">Password</label></td>
                    <td><%= Html.TextBox("password") %></td>
                    <td><%= Html.ValidationMessage("password") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Sign-in" /></td>
                    <td><%= Html.ValidationMessage("auth") %></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
