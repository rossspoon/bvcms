<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.StepClassModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Step 2 Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register for Step 2 Class</h2>

    <%= Html.ValidationSummary() %>
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
                    <td><label for="lastname">Last Name</label></td>
                    <td><%= Html.TextBox("lastname") %></td>
                    <td><%= Html.ValidationMessage("lastname") %></td>
                </tr>
                <tr>
                    <td><label for="dob">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob") %></td>
                    <td><%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="homephone">Phone #</label></td>
                    <td><%= Html.TextBox("phone")%></td>
                    <td><%= Html.ValidationMessage("phone")%></td>
                </tr>
                <tr>
                    <td><label for="email">Email</label></td>
                    <td><%= Html.TextBox("email") %></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan = "2"><%= Html.CheckBox("preferredEmail") %> This is my preferred email address</td>
                </tr>
                <tr>
                    <td><label for="MeetingId">Step 2 Class Dates</label></td>
                    <td><%= Html.DropDownList("MeetingId", Model.AvailableClasses("Step 2"))%></td>
                    <td><%= Html.ValidationMessage("MeetingId")%></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                </tr>
                </table>
            </fieldset>
            If you are having difficulty registering online, please call Gail Stewart at 347-5763.
        </div>
    <% } %>
</asp:Content>
