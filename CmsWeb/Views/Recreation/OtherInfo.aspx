<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecreationModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Recreation Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register for <%=Model.division.Name %></h2>
    <% using (Html.BeginForm()) 
    { %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr>
                    <td><label for="shirtsize">ShirtSize</label></td>
                    <td><%= Html.DropDownList("shirtsize", Model.ShirtSizes())%></td>
                    <td><%= Html.ValidationMessage("shirtsize")%></td>
                </tr>
                <tr>
                    <td><label for="request">Request Teammate</label></td>
                    <td><%= Html.TextBox("request") %></td>
                    <td><%= Html.ValidationMessage("request") %></td>
                </tr>
                <tr>
                    <td><label for="emcontact">Emergency Friend</label></td>
                    <td><%= Html.TextBox("emcontact") %></td>
                    <td><%= Html.ValidationMessage("emcontact")%></td>
                </tr>
                <tr>
                    <td><label for="emphone">Emergency Phone</label></td>
                    <td><%= Html.TextBox("emphone")%></td>
                    <td><%= Html.ValidationMessage("emphone")%></td>
                </tr>

                <tr>
                    <td><label for="insurance">Health Insurance Carrier</label></td>
                    <td><%= Html.TextBox("insurance")%></td>
                    <td><%= Html.ValidationMessage("insurance")%></td>
                </tr>
                <tr>
                    <td><label for="policy">Policy #</label></td>
                    <td><%= Html.TextBox("policy")%></td>
                    <td><%= Html.ValidationMessage("policy")%></td>
                </tr>
                <tr>
                    <td><label for="doctor">Family Physician Name</label></td>
                    <td><%= Html.TextBox("doctor")%></td>
                    <td><%= Html.ValidationMessage("doctor")%></td>
                </tr>
                <tr>
                    <td><label for="docphone">Family Physician Phone</label></td>
                    <td><%= Html.TextBox("docphone")%></td>
                    <td><%= Html.ValidationMessage("docphone")%></td>
                </tr>

                <tr>
                    <td><label for="medical">Allergies or<br />
                           Medical Problems</label></td>
                    <td><%= Html.TextArea("medical") %></td>
                    <td><%= Html.ValidationMessage("medical")%>Leave blank if none</td>
                </tr>
                <tr>
                    <td><label for="mname">Mother's Name</label></td>
                    <td><%= Html.TextBox("mname")%></td>
                    <td><%= Html.ValidationMessage("mname")%></td>
                </tr>
                <tr>
                    <td><label for="fname">Father's Name</label></td>
                    <td><%= Html.TextBox("fname")%></td>
                    <td><%= Html.ValidationMessage("fname")%></td>
                </tr>
                 <tr>
                    <td><label for="coaching">Interested in Coaching?</label></td>
                    <td><%= Html.RadioButton("coaching", 1) %> Yes
                    <%= Html.RadioButton("coaching", 0) %> No</td>
                    <td><%= Html.ValidationMessage("coaching2") %></td>
                </tr>
                <tr>
                    <td><label for="church">Parent's Church</label></td>
                    <td><%= Html.CheckBox("member") %> Member of Bellevue<br />
                    <%= Html.CheckBox("otherchurch") %> Active in another Local Church</td>
                    <td><%= Html.ValidationMessage("member")%></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Next Page" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>

</asp:Content>
