<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RegisterModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Register</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add to existing family Registration</h2>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="last">Last Name</label></td>
                    <td><%= Html.TextBox("last") %></td>
                    <td><%= Html.ValidationMessage("last") %></td>
                </tr>
                <tr>
                    <td><label for="phone">Phone</label></td>
                    <td><%= Html.TextBox("phone")%></td>
                    <td><%= Html.ValidationMessage("phone")%></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Find Family" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
        <p><a href="/Register/Visit/<%=Session["campus"] %>">New Registration</a></p>
        <%=Html.Hidden("campusid", Model.campusid) %>
    <% } %>
</asp:Content>
