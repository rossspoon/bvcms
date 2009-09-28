<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3b.Master" Inherits="System.Web.Mvc.ViewPage<CMSRegCustom.Models.SoulMateModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>SML Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Childcare Registration for the <%=Model.NextEvent.ToString("MMM d, yyyy") %> Event</h2>
    <p>Childcare is available for birth through 5th grade only.</p>

        <div>
            <% if (Model.Children().Count() > 0)
               { %>
            <h2>Children Registered</h2>
            <table>
            <tr><th>Name</th><th>Date of Birth</th><th>Age</th><th>Gender</th></tr>
                <% foreach (var c in Model.Children())
                   { %>
            <tr><td><%=c.Name%></td><td><%=c.Birthday%></td><td><%=c.Age%></td><td><%=c.Gender%></td></tr>
                <% } %>
            </table>
            <h3>Register another child below or Complete your registration if finished</h3>
            <% }
               else
               { %>
            <h3>Register your child below or Complete your registration if finished</h3>
            <% } %>
            <form action="/SoulMate/Confirm/<%=Model.SoulMateId %>" method="post">
            <%=Html.SubmitButton("confirm", "Complete Registration") %>
            </form>
        </div>
        <div>
     <form action="/SoulMate/AddChild/<%=Model.SoulMateId %>" method="post">
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
                    <td><%= Html.TextBox("first1", Model.first1) %></td>
                    <td><%= Html.ValidationMessage("first1") %><%= Html.ValidationMessage("findkid") %></td>
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
                    <td><%= Html.TextBox("dob1", Model.dob1) %></td>
                    <td><%= Html.ValidationMessage("dob1") %></td>
                </tr>

                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Register Child" /></td>
                </tr>
                </table>
            </fieldset>
            
            <p>If you are having difficulty registering online, <%=DbUtil.Settings("SmlHelp") %></p>
        </div>
    </form>
</asp:Content>
