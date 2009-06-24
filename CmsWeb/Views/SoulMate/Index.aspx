<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SoulMateModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>SML Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register for the <%=Model.NextEvent.ToString("MMM d, yyyy") %> Event</h2>

    <%= Html.ValidationSummary() %>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr><th colspan="2">His Info</th></tr>
                <tr>
                    <td><label for="first1">First Name</label></td>
                    <td><%= Html.TextBox("first1") %></td>
                    <td><%= Html.ValidationMessage("first1") %></td>
                </tr>
                <tr>
                    <td><label for="lastname1">Last Name</label></td>
                    <td><%= Html.TextBox("lastname1") %></td>
                    <td><%= Html.ValidationMessage("lastname1") %></td>
                </tr>
                <tr>
                    <td><label for="dob1">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob1") %></td>
                    <td><%= Html.ValidationMessage("dob1") %></td>
                </tr>
                <tr>
                    <td><label for="homephone1">Phone #</label></td>
                    <td><%= Html.TextBox("phone1")%></td>
                    <td><%= Html.ValidationMessage("phone1")%></td>
                </tr>
                <tr>
                    <td><label for="email1">Email</label></td>
                    <td><%= Html.TextBox("email1") %></td>
                    <td><%= Html.ValidationMessage("email1") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan = "2"><%= Html.CheckBox("preferredEmail1") %> This is his preferred email address</td>
                </tr>
            <% if (Model.person1 == null)
               { %>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            <% } %>
                <tr><th colspan="2">Her Info</th></tr>
                <tr>
                    <td><label for="first2">First Name</label></td>
                    <td><%= Html.TextBox("first2") %></td>
                    <td><%= Html.ValidationMessage("first2") %></td>
                </tr>
                <tr>
                    <td><label for="lastname2">Last Name</label></td>
                    <td><%= Html.TextBox("lastname2") %></td>
                    <td><%= Html.ValidationMessage("lastname2") %></td>
                </tr>
                <tr>
                    <td><label for="dob2">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob2") %></td>
                    <td><%= Html.ValidationMessage("dob2") %></td>
                </tr>
                <tr>
                    <td><label for="homephone2">Phone #</label></td>
                    <td><%= Html.TextBox("phone2")%></td>
                    <td><%= Html.ValidationMessage("phone2")%></td>
                </tr>
                <tr>
                    <td><label for="email2">Email</label></td>
                    <td><%= Html.TextBox("email2") %></td>
                    <td><%= Html.ValidationMessage("email2") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan = "2"><%= Html.CheckBox("preferredEmail2") %> This is her preferred email address</td>
                </tr>
                <tr><th colspan="2">Relationship</th></tr>
                <tr>
                    <td><label for="Relation">Relationship</label></td>
                    <td><%=Html.DropDownList("Relation", Model.Relations()) %></td>
                    <td><%= Html.ValidationMessage("Relation")%></td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                </tr>
                </table>
            </fieldset>
            If you are having difficulty registering online, please call SOMEONE at 347-5000.
        </div>
    <% } %>
</asp:Content>
