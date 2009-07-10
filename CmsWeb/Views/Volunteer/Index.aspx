<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteerModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Volunteer</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Volunteer for <%=Model.Opportunity.Description %></h2>

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
                    <td><%= Html.ValidationMessage("phone")%>
                    <%= Html.ValidationMessage("phone")%></td>
                </tr>
                <tr>
                    <td><label for="email">Email</label></td>
                    <td><%= Html.TextBox("email") %></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
                <tr>
                    <td><label for="interests">Interests</label></td>
                    <td>
                    <% foreach (var i in Model.Opportunity.VolInterestCodes)
                       { %>
                       <input type="checkbox" name="interests" value="<%=i.Id %>" <%=Model.Checked(i.Id)%> /> <%=i.Description %><br />
                    <% } %>
                    </td>
                    <td><%= Html.ValidationMessage("interests") %></td>
                </tr>
                <% if (Model.Opportunity.ExtraQuestion.HasValue())
                   { %>
                <tr>
                    <td><%=Model.Opportunity.ExtraQuestion %></td>
                    <td><%=Html.TextBox("question") %></td>
                    <td>&nbsp;</td>
                </tr>
                <% } %>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
