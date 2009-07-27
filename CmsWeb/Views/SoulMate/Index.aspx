<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3b.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SoulMateModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>SML Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery-1.3.2.min.js" type="text/javascript"></script>

<% if(Model.shownew1)
   { %>
    <script type="text/javascript">
        $(function() {
            $("#zip1").change(function() {
                $.post('/SoulMate/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state1').val(ret.state);
                        $('#city1').val(ret.city);
                    }
                }, 'json');
            });
        });
    </script>
<% }
   if(Model.shownew2)
   { %>
    <script type="text/javascript">
        $(function() {
            $("#zip2").change(function() {
                $.post('/SoulMate/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state2').text(ret.state);
                        $('#city2').text(ret.city);
                    }
                }, 'json');
            });
        });
    </script>    
<% } %>
    <h2>Register for the <%=Model.NextEvent.ToString("MMM d, yyyy") %> Event</h2>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr><th colspan="2">His Info</th><td><%= Html.ValidationMessage("findhim") %></td></tr>
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
            <% if (Model.shownew1)
               { %>
                <tr>
                    <td><%=Html.Hidden("shownew1") %>
                    <label for="addr1">Address</label></td>
                    <td><%=Html.TextBox("addr1") %></td>
                    <td><%= Html.ValidationMessage("addr1") %></td>
                </tr>
                <tr>
                    <td><label for="zip1">Zip</label></td>
                    <td><%=Html.TextBox("zip1")%></td>
                    <td><%= Html.ValidationMessage("zip1")%></td>
                </tr>
                <tr>
                    <td><label for="city1">City</label></td>
                    <td><%=Html.TextBox("city1")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="state1">State</label></td>
                    <td><%=Html.TextBox("state1")%></td>
                    <td></td>
                </tr>
            <% } %>
                <tr><th colspan="2">Her Info</th><td><%= Html.ValidationMessage("findher") %></td></tr>
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
            <% if (Model.shownew2)
               { %>
               <tr><th colspan="3">Please provide address</th></tr>
                <tr>
                    <td><%=Html.Hidden("shownew2") %>
                    <label for="addr2">Address</label></td>
                    <td><%=Html.TextBox("addr2") %></td>
                    <td><%= Html.ValidationMessage("addr2") %></td>
                </tr>
                <tr>
                    <td><label for="zip2">Zip</label></td>
                    <td><%=Html.TextBox("zip2")%></td>
                    <td><%= Html.ValidationMessage("zip2")%></td>
                </tr>
                <tr>
                    <td><label for="city2">City</label></td>
                    <td><%=Html.TextBox("city2")%></td>
                    <td></td>
                </tr>
                <tr>
                    <td><label for="state2">State</label></td>
                    <td><%=Html.TextBox("state2")%></td>
                    <td></td>
                </tr>
            <% } %>
                
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
            <p>If you are having difficulty registering online, <%=DbUtil.Settings("SmlHelp") %></p>
        </div>
    <% } %>
</asp:Content>
