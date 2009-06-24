<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SoulMateModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>SML Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Childcare Registration for the <%=Model.NextEvent.ToString("MMM d, yyyy") %> Event</h2>

    <%= Html.ValidationSummary() %>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr><th colspan="2">Parent</th></tr>
                <tr>
                    <td><label for="ChildParent">Parent</label></td>
                    <td><%=Html.DropDownList("ChildParent", Model.Parents())%></td>
                    <td><%= Html.ValidationMessage("ChildParent")%></td>
                </tr>
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
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                </tr>
                </table>
            </fieldset>
            <table>
            <tr>
                <th>Name</th><th>Date of Birth</th>
                <% foreach (var c in Model.Children())
                   { %>
                <td></td><td></td>
                <% } %>
            </tr>
            </table>
            If you are having difficulty registering online, please call SOMEONE at 347-5000.
        </div>
    <% } %>
</asp:Content>
