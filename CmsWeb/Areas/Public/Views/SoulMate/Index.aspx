<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3b.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SoulMateModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>SML Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>    

<% if(Model.shownew1.ToInt() >= 2)
   { %>
    <script type="text/javascript">
        $(function() {
            $("#zip1").blur(function() {
                $.post('/Register/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state1').val(ret.state);
                        $('#city1').val(ret.city);
                    }
                }, 'json');
            });
        });
    </script>
<% }
   if(Model.shownew2.ToInt() >= 2)
   { %>
    <script type="text/javascript">
        $(function() {
            $("#zip2").blur(function() {
                $.post('/Register/CityState/' + $(this).val(), null, function(ret) {
                    if (ret) {
                        $('#state2').val(ret.state);
                        $('#city2').val(ret.city);
                    }
                }, 'json');
            });
            $('#same').click(function(ev) {
                ev.preventDefault();
                $('#addr2').val($('#addr1').val());
                $('#state2').val($('#state1').val());
                $('#city2').val($('#city1').val());
                $('#zip2').val($('#zip1').val());
                return false;
            });
        });
    </script>    
<% } %>
    <div class="smlheader">Register for the <%=Model.NextEvent.ToString("MMM d, yyyy") %> Event</div>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <input type="hidden" name="shownew1" value='<%=Model.shownew1 %>' />
                <input type="hidden" name="shownew2" value='<%=Model.shownew2 %>' />
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
                    <td><label for="phone1">Phone #</label></td>
                    <td colspan="2"><%= Html.TextBox("phone1")%>
                    <%= Html.RadioButton("homecell1", "h") %> Home
                    <%= Html.RadioButton("homecell1", "c") %> Cell
                    <%= Html.ValidationMessage("phone1")%></td>
                </tr>
                <tr>
                    <td><label for="email1">Email</label></td>
                    <td><%= Html.TextBox("email1") %></td>
                    <td><%= Html.ValidationMessage("email1") %></td>
                </tr>
            <% if (Model.shownew1.ToInt() >= 2)
               { %>
               <tr><th colspan="3"><span style="color:Red">Please provide address</span></th></tr>
                <tr>
                    <td><label for="addr1">Address</label></td>
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
                    <td><label for="phone2">Phone #</label></td>
                    <td colspan="2"><%= Html.TextBox("phone2")%>
                    <%= Html.RadioButton("homecell2", "h") %> Home
                    <%= Html.RadioButton("homecell2", "c") %> Cell
                    <%= Html.ValidationMessage("phone2")%></td>
                </tr>
                <tr>
                    <td><label for="email2">Email</label></td>
                    <td><%= Html.TextBox("email2") %></td>
                    <td><%= Html.ValidationMessage("email2") %></td>
                </tr>
            <% if (Model.shownew2.ToInt() >= 2)
               { %>
               <tr><th colspan="3"><span style="color:Red">Please provide address</span>
               <a id="same" href="#">same</a></th></tr>
                <tr>
                    <td><label for="addr2">Address</label></td>
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
        </div>
    <% } %>
    <p style="color:Black;">If you are having difficulty registering online, <%=DbUtil.Settings("SmlHelp", "[create SmlHelp setting]") %></p>
</asp:Content>
