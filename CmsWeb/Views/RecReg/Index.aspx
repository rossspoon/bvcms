<%@ Page Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecRegModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Recreation Registration</title>
    <script src="/Content/js/jquery-1.3.2.min.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<% if(Model.shownew)
   { %>
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
<% } %>
    <% if (Model.filled)
       { %>
    <h2><%=Model.division.Name%> is Filled, Sorry</h2>
    <% }
       else
       { %>
    <h2>Register for <%=Model.division.Name%></h2>
    <% } %>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="first">Participant First Name</label></td>
                    <td><%= Html.TextBox("first", Model.first, new { maxlength = 25 })%></td>
                    <td><%= Html.ValidationMessage("first") %><%= Html.ValidationMessage("find") %></td>
                </tr>
                <tr>
                    <td><label for="last">Participant Last Name</label></td>
                    <td><%= Html.TextBox("last", Model.last, new { maxlength = 30 })%></td>
                    <td><%= Html.ValidationMessage("last") %></td>
                </tr>
                 <tr>
                    <td><label for="dob">Participant Date of Birth</label></td>
                    <td><%= Html.TextBox("dob") %></td>
                    <td><%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="phone">Phone</label></td>
                    <td><%= Html.TextBox("phone")%></td>
                    <td><%= Html.RadioButton("homecell", "h") %> Home<br />
                    <%= Html.RadioButton("homecell", "c") %> Cell
                    <%= Html.ValidationMessage("phone")%></td>
                </tr>
                 <tr>
                    <td><label for="gender">Participant Gender</label></td>
                    <td><%= Html.RadioButton("gender", 1) %> Male
                    <%= Html.RadioButton("gender", 2) %> Female</td>
                    <td><%= Html.ValidationMessage("gender2") %></td>
                </tr>
                <tr>
                    <td><label for="email">Contact Email</label></td>
                    <td><%= Html.TextBox("email", Model.email, new { maxlength = 50 })%></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
            <% if (Model.shownew)
               { %>
               <tr><th colspan="3"><span style="color:Red">Please provide address</span></th></tr>
                <tr>
                    <td><%=Html.Hidden("shownew") %>
                    <label for="addr">Address</label></td>
                    <td><%= Html.TextBox("addr", Model.addr, new { maxlength = 40 })%></td>
                    <td><%= Html.ValidationMessage("addr")%></td>
                </tr>
                <tr>
                    <td><label for="zip">Zip</label></td>
                    <td><%= Html.TextBox("zip")%></td>
                    <td><%= Html.ValidationMessage("zip")%></td>
                </tr>
                <tr>
                    <td><label for="city">City</label></td>
                    <td><%= Html.TextBox("city", Model.city, new { maxlength = 20 })%></td>
                    <td><%= Html.ValidationMessage("city")%></td>
                </tr>
                <tr>
                    <td><label for="state">State</label></td>
                    <td><%= Html.DropDownList("state", CMSWeb.Models.RegisterModel.StateList()) %></td>
                    <td></td>
                </tr>
                <% } %>
                <tr>
                    <td>&nbsp;</td><td><input <%=Model.disabled %> type="submit" value="Next Page" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
