<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RegisterModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Register</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Church Database Registration Sign-in</h2>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="password">Password</label></td>
                    <td><%= Html.TextBox("password") %></td>
                    <td><%= Html.ValidationMessage("password") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                    <td><%= Html.ValidationMessage("auth") %></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
