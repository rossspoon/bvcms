<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Forms.Models.RegisterModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Register</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Church Database Registration</h2>
    <p>
        Use the form below to register another person in your family for our Church Database. 
    </p>

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
                    <td><label for="nickname">Goes By Name</label></td>
                    <td><%= Html.TextBox("nickname") %></td>
                    <td><%= Html.ValidationMessage("nickname") %></td>
                </tr>
                <tr>
                    <td><label for="lastname">Last Name</label></td>
                    <td><%= Html.TextBox("lastname") %></td>
                    <td><%= Html.ValidationMessage("lastname") %></td>
                </tr>
                 <tr>
                    <td><label for="dob">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob") %></td>
                    <td><%= Html.ValidationMessage("dob") %>(like 5/15/64)</td>
                </tr>
                <tr>
                    <td><label for="married">Married</label></td>
                    <td><%= Html.CheckBox("married") %></td>
                    <td><%= Html.ValidationMessage("married")%></td>
                </tr>
                <tr>
                    <td><label for="position">Position in Family</label></td>
                    <td><%= Html.RadioButton("position", 10) %> Primary Adult
                    <%= Html.RadioButton("position", 20) %> Secondary Adult
                    <%= Html.RadioButton("position", 30) %> Child
                    <%= Html.Hidden("position", null) %></td>
                    <td><%= Html.ValidationMessage("position") %><br />
                    (Husband and wife should both be primary)</td>
                </tr>
                 <tr>
                    <td><label for="gender">Gender</label></td>
                    <td><%= Html.RadioButton("gender", 1) %> Male
                    <%= Html.RadioButton("gender", 2) %> Female
                    <%= Html.Hidden("gender", null) %></td>
                    <td><%= Html.ValidationMessage("gender") %></td>
                </tr>
                <tr>
                    <td><label for="grade">Grade</label></td>
                    <td><%=Html.TextBox("grade")%></td>
                    <td><%= Html.ValidationMessage("grade") %></td>
                </tr>
                <tr>
                    <td><label for="cellphone">Cell Phone</label></td>
                    <td><%= Html.TextBox("cellphone") %></td>
                    <td><%= Html.ValidationMessage("cellphone") %></td>
                </tr>
                <tr>
                    <td><label for="workphone">Work Phone</label></td>
                    <td><%= Html.TextBox("workphone") %></td>
                    <td><%= Html.ValidationMessage("workphone")%></td>
                </tr>
                <tr>
                    <td><label for="email">Email</label></td>
                    <td><%= Html.TextBox("email") %></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Register" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
