<%@ Page Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.DiscipleLifeModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title><%=Model.division.Name %> Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>    
<% if(Model.shownew)
   { %>
    <script type="text/javascript">
        $(function() {
            $("#zip").blur(function() {
                $.post('/Register/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state').val(ret.state);
                        $('#city').val(ret.city);
                    }
                }, 'json');
            });
        });
    </script>
<% } %>
    <h2>Register for <%=Model.division.Name %></h2>
    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <% if (Model.FilledClasses().Count() > 0)
                   { %>
                <tr>
                    <td><span style="color:Red">Filled Classes</span></td>
                    <td>
                    <% foreach (var c in Model.FilledClasses())
                       { %>
                       <%= c%><br />
                    <% } %>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <% } %>
                <tr><td colspan="3">
<% if (Model.Classes().Count() > 1)
   { %>
                    <%= Html.DropDownList("OrgId", Model.Classes())%>
<% }
   else
   { %>
                    <%=Model.Classes().Single().Text%>
                    <%= Html.Hidden("OrgId", Model.Classes().Single().Value)%>
<% } %>
                    <%= Html.ValidationMessage("OrgId") %>
                    </td>
                </tr>
                <tr>
                    <td><label for="first">First Name</label></td>
                    <td><%= Html.TextBox("first", Model.first, new { maxlength = 25 })%></td>
                    <td><%= Html.ValidationMessage("first") %><%= Html.ValidationMessage("find") %></td>
                </tr>
                <tr>
                    <td><label for="last">Last Name</label></td>
                    <td><%= Html.TextBox("last", Model.last, new { maxlength = 30 })%></td>
                    <td><%= Html.ValidationMessage("last") %></td>
                </tr>
                 <tr>
                    <td><label for="dob">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob", Model.dob, new { title = "m/d/y, mmddyy, mmddyyyy" }) %> (m/d/y)</td>
                    <td><%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="phone">Phone</label></td>
                    <td><%= Html.TextBox("phone")%></td>
                    <td><%= Html.RadioButton("homecell", "h") %>&nbsp;Home<br />
                    <%= Html.RadioButton("homecell", "c") %>&nbsp;Cell
                    <%= Html.ValidationMessage("phone")%></td>
                </tr>
                <tr>
                    <td><label for="email">Contact Email</label></td>
                    <td><%= Html.TextBox("email", Model.email, new { maxlength = 50 })%></td>
                    <td><%= Html.ValidationMessage("email") %></td>
                </tr>
            <% if (Model.shownew)
               { %>
               <tr><th colspan="3"><span style="color:Red">Please provide additional information</span><%=Html.Hidden("shownew") %></th></tr>
                 <tr>
                    <td><label for="gender">Gender</label></td>
                    <td><%= Html.RadioButton("gender", 1) %> Male
                    <%= Html.RadioButton("gender", 2) %> Female</td>
                    <td><%= Html.ValidationMessage("gender2") %></td>
                </tr>
                 <tr>
                    <td><label for="married">Marital Status</label></td>
                    <td><%= Html.RadioButton("married", 1) %> Single
                    <%= Html.RadioButton("married", 2) %> Married</td>
                    <td><%= Html.ValidationMessage("married2") %></td>
                </tr>
                <tr>
                    <td><label for="addr">Address</label></td>
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
                    <td><%= Html.DropDownList("state", Model.StateList())%></td>
                    <td><%= Html.ValidationMessage("state")%></td>
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
