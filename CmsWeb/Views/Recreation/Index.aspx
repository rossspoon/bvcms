<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecreationModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Recreation Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register for <%=Model.division.Name %></h2>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="first">Participant First Name</label></td>
                    <td><%= Html.TextBox("first") %></td>
                    <td><%= Html.ValidationMessage("first") %><%= Html.ValidationMessage("find") %></td>
                </tr>
                <tr>
                    <td><label for="last">Participant Last Name</label></td>
                    <td><%= Html.TextBox("last") %></td>
                    <td><%= Html.ValidationMessage("last") %></td>
                </tr>
                 <tr>
                    <td><label for="dob">Participant Date of Birth</label></td>
                    <td><%= Html.TextBox("dob") %></td>
                    <td><%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="phone">Phone (adult)</label></td>
                    <td><%= Html.TextBox("phone")%></td>
                    <td><%= Html.ValidationMessage("phone")%></td>
                </tr>
                 <tr>
                    <td><label for="gender">Participant Gender</label></td>
                    <td><%= Html.RadioButton("gender", 1) %> Male
                    <%= Html.RadioButton("gender", 2) %> Female</td>
                    <td><%= Html.ValidationMessage("gender2") %></td>
                </tr>
                <tr>
                    <td><label for="email1">Contact Email (adult)</label></td>
                    <td><%= Html.TextBox("email") %></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
            <% if (Model.shownew)
               { %>
                <tr>
                    <td><label for="address">Address</label></td>
                    <td><%= Html.TextBox("address")%></td>
                    <td><%= Html.ValidationMessage("address")%></td>
                </tr>
                <tr>
                    <td><label for="city">City</label></td>
                    <td><%= Html.TextBox("city")%></td>
                    <td><%= Html.ValidationMessage("city")%></td>
                </tr>
                <tr>
                    <td><label for="state">State</label></td>
                    <td><%= Html.DropDownList("state", Model.StateList())%></td>
                    <td><%= Html.ValidationMessage("state")%></td>
                </tr>
                <tr>
                    <td><label for="zip">Zip</label></td>
                    <td><%= Html.TextBox("zip")%></td>
                    <td><%= Html.ValidationMessage("zip")%></td>
                </tr>
                <% } %>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Next Page" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
