<%@ Page Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.DiscipleLifeModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title><%=Model.division.Name %> Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery-1.3.2.min.js" type="text/javascript"></script>
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
                <tr>
                    <td><label for="OrgId">Class</label></td>
<% if (Model.Classes().Count() > 1)
   { %>
                    <td colspan="2"><%= Html.DropDownList("OrgId", Model.Classes())%></td>
<% }
   else
   { %>
                    <td colspan="2"><%=Model.Classes().Single().Text%>
                    <%= Html.Hidden("OrgId", Model.Classes().Single().Value)%></td>
<% } %>
                    <td><%= Html.ValidationMessage("OrgId") %></td>
                </tr>
                <tr>
                    <td><label for="first">First Name</label></td>
                    <td><%= Html.TextBox("first") %></td>
                    <td><%= Html.ValidationMessage("first") %><%= Html.ValidationMessage("find") %></td>
                </tr>
                <tr>
                    <td><label for="last">Last Name</label></td>
                    <td><%= Html.TextBox("last") %></td>
                    <td><%= Html.ValidationMessage("last") %></td>
                </tr>
                 <tr>
                    <td><label for="dob">Date of Birth</label></td>
                    <td><%= Html.TextBox("dob") %></td>
                    <td><%= Html.ValidationMessage("dob") %></td>
                </tr>
                <tr>
                    <td><label for="phone">Phone</label></td>
                    <td><%= Html.TextBox("phone")%></td>
                    <td align="left"><%= Html.RadioButton("homecell", "h") %> Home<br />
                    <%= Html.RadioButton("homecell", "c") %> Cell</td>
                    <td><%= Html.ValidationMessage("phone")%></td>
                </tr>
                <tr>
                    <td><label for="email">Contact Email</label></td>
                    <td><%= Html.TextBox("email") %></td>
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
                    <td><%= Html.RadioButton("single", 1) %> Single
                    <%= Html.RadioButton("married", 2) %> Married</td>
                    <td><%= Html.ValidationMessage("married2") %></td>
                </tr>
                <tr>
                    <td><label for="addr">Address</label></td>
                    <td><%= Html.TextBox("addr")%></td>
                    <td><%= Html.ValidationMessage("addr")%></td>
                </tr>
                <tr>
                    <td><label for="zip">Zip</label></td>
                    <td><%= Html.TextBox("zip")%></td>
                    <td><%= Html.ValidationMessage("zip")%></td>
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
                <% } %>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
