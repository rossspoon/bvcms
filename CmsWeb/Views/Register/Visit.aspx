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
            $('input:text:first').focus();
            $('#dob').change(function() {
                var bd = $(this).val();
                if (bd.length == 6)
                    bd = bd.substr(0, 1) + '/' + bd.substr(2, 3) + '/' + bd.substr(4, 5);
                else
                    bd = bd.replace("-", "/");
                var d = bd.split("/");
                var y = parseInt(d[2]);
                if (y < 30)
                    y += 2000;
                if (y < 100)
                    y += 1900;
                var bday = new Date(y, d[0] - 1, d[1]);
                var by = bday.getFullYear();
                var bm = bday.getMonth();
                var bd = bday.getDate();
                var age = 0;
                var today = new Date();
                while (bday <= today) {
                    bday = new Date(by + age, bm, bd);
                    age++;
                }
                age -= 2;
                $('#age').text(age);
            });
        });
    </script>
    <h2>Church Visit Registration</h2>
    <p>
        Use the form below to register a visit in our Church Database.<br />
        <a href='/Register/Add/<%=Model.campusid %>'>Add to an existing family</a>
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
                    <td><label for="lastname">Last Name</label></td>
                    <td><%= Html.TextBox("lastname") %></td>
                    <td><%= Html.ValidationMessage("lastname") %></td>
                </tr>
                <tr>
                    <td><label for="dob">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob") %> <span id="age"></span></td>
                    <td>(m/d/yy or mmddyy)<%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="gender">Gender</label></td>
                    <td>
                    <%= Html.RadioButton("gender", 1, new { accesskey="m" })%> <u>M</u>ale 
                    <%= Html.RadioButton("gender", 2, new { accesskey = "f" })%> <u>F</u>emale </td>
                    <td><%= Html.ValidationMessage("gender") %></td>
                </tr>
                <tr>
                    <td><label for="married">Married</label></td>
                    <td>
                    <%= Html.RadioButton("married", 10, new { accesskey = "s" })%> <u>S</u>ingle
                    <%= Html.RadioButton("married", 20, new { accesskey = "a" })%> M<u>a</u>rried</td>
                    <td><%= Html.ValidationMessage("married") %></td>
                </tr>
                <tr>
                    <td><label for="address">Address Line 1</label></td>
                    <td><%= Html.TextBox("address1") %></td>
                    <td><%= Html.ValidationMessage("address1") %></td>
                </tr>
                <tr>
                    <td><label for="address2">Address Line 2</label></td>
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
                    <td><%= Html.DropDownList("state", Model.StateList()) %></td>
                    <td><%= Html.ValidationMessage("state") %></td>
                </tr>
                <tr>
                    <td><label for="homephone">Home Phone</label></td>
                    <td><%= Html.TextBox("homephone")%></td>
                    <td><%= Html.ValidationMessage("homephone")%>
                    <%= Html.ValidationMessage("homephone")%></td>
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
                    <td><label for="org">Visiting Where</label></td>
                    <td><%= Html.DropDownList("org", Model.OrgList())%></td>
                    <td><%= Html.ValidationMessage("org") %></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td>
                    <input type="submit" name="submit" value="Register" />
            <% if (Model.existingfamily ?? false)
               { %>
                    <%=Html.Hidden("existingfamily", true) %>
                    <input type="submit" name="submit" value="Add to Existing Family" />
            <% } %>
                    </td>
                    <td><%= Html.ValidationMessage("existing") %></td>
                </tr>
                </table>
            </fieldset>
    <p><a href="/Register/Visit/<%=Session["campus"] %>">New Registration</a></p>
        </div>
    <% } %>
</asp:Content>
