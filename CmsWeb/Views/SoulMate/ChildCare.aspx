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
                    <td><label for="gender">Gender</label></td>
                    <td><%= Html.RadioButton("gender", 1) %> Male
                    <%= Html.RadioButton("gender", 2) %> Female</td>
                    <td><%= Html.ValidationMessage("gender2") %></td>
                </tr>
                <tr>
                    <td><label for="dob1">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob1") %></td>
                    <td><%= Html.ValidationMessage("dob1") %></td>
                </tr>

                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Register Child" /></td>
                </tr>
                </table>
            </fieldset>
            
            <h2>Children Registered</h2>
            <table>
            <tr><th>Name</th><th>Date of Birth</th><th>Age</th><th>Gender</th></tr>
                <% foreach (var c in Model.Children())
                   { %>
            <tr><td><%=c.Name %></td><td><%=c.Birthday %> ()</td><td><%=c.Age %></td><td><%=c.Gender %></td></tr>
                <% } %>
            </table>
            <h3><%=Html.ActionLink("Complete Registration", "Confirm", new { id = Model.SoulMateId })%></h3>
            <p>If you are having difficulty registering online, please call the Young Adult office at 347-5000.</p>
        </div>
    <% } %>
</asp:Content>
