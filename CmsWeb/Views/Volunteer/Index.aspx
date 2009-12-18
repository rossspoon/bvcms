<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteerModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Volunteer</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%=Model.Opportunity.Description %></h2>

    <%= Html.ValidationMessage("find") %>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="first">First Name</label></td>
                    <td><%= Html.TextBox("first") %></td>
                    <td><%= Html.ValidationMessage("first") %></td>
                </tr>
                <tr>
                    <td><label for="last">Last Name</label></td>
                    <td><%= Html.TextBox("last") %></td>
                    <td><%= Html.ValidationMessage("last") %></td>
                </tr>
                <tr>
                    <td><label for="dob">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob") %> mm/dd/yyyy</td>
                    <td><%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="phone">Phone #</label></td>
                    <td><%= Html.TextBox("phone")%></td>
                    <td><%= Html.ValidationMessage("phone")%></td>
                </tr>
                <tr>
                    <td><label for="email">Email</label></td>
                    <td><%= Html.TextBox("email") %></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
