<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RegisterModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Register</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function() {
            $("#zip").change(function() {
                $.post('/RecReg/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state').val(ret.state);
                        $('#city').val(ret.city);
                    }
                }, 'json');
            });
        });
    </script>
    <h2>Church Database Registration</h2>
    <p>
        Use the form below to register in our Church Database. 
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
                    <td>(m/d/yy)<%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="gender">Gender</label></td>
                    <td><%= Html.RadioButton("gender", 1) %> Male
                    <%= Html.RadioButton("gender", 2) %> Female
                    <td><%= Html.ValidationMessage("gender") %></td>
                </tr>
                <tr>
                    <td><label for="married">Marital Status</label></td>
                    <td><%= Html.DropDownList("married", Model.MaritalStatus())%></td>
                    <td><%= Html.ValidationMessage("married")%></td>
                </tr>
                <tr>
                    <td><label for="address">Address Line 1</label></td>
                    <td><%= Html.TextBox("address1") %></td>
                    <td><%= Html.ValidationMessage("address1") %></td>
                </tr>
                <tr>
                    <td><label for="address">Address Line 2</label></td>
                    <td><%= Html.TextBox("address2") %></td>
                    <td><%= Html.ValidationMessage("address2") %></td>
                </tr>
                <tr>
                    <td><label for="zip">Zip</label></td>
                    <td><%= Html.TextBox("zip") %></td>
                    <td><%= Html.ValidationMessage("zip") %></td>
                </tr>
                <tr>
                    <td><label for="city">City</label></td>
                    <td><%= Html.TextBox("city") %></td>
                    <td><%= Html.ValidationMessage("city") %></td>
                </tr>
                <tr>
                    <td><label for="state">State</label></td>
                    <td><%= Html.DropDownList("state", CMSWeb.Models.RegisterModel.StateList()) %></td>
                    <td><%= Html.ValidationMessage("state") %></td>
                </tr>
                <tr>
                    <td><label for="homephone">Home Phone</label></td>
                    <td><%= Html.TextBox("homephone")%></td>
                    <td><%= Html.ValidationMessage("homephone")%>
                    <%= Html.ValidationMessage("phone")%></td>
                </tr>
                <tr>
                    <td><label for="cellphone">Cell Phone</label></td>
                    <td><%= Html.TextBox("cellphone") %></td>
                    <td><%= Html.ValidationMessage("cellphone") %></td>
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
