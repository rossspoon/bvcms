<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VBSRegModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Register</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Vacation Bible School Registration</h2>
    <p>
        Use the form below to register your child for Vacation Bible School. 
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
                    <td><%= Html.ValidationMessage("dob") %></td>
                </tr>
                 <tr>
                    <td><label for="gender">Gender</label></td>
                    <td><%= Html.RadioButton("gender", 1) %> Male
                    <%= Html.RadioButton("gender", 2) %> Female</td>
                    <td><%= Html.ValidationMessage("gender2") %></td>
                </tr>
                <tr>
                    <td><label for="grade">Grade</label></td>
                    <td><%=Html.DropDownList("grade", ViewData.Model.GradeCompleteds())%></td>
                    <td><%= Html.ValidationMessage("grade") %></td>
                </tr>
                <tr>
                    <td><label for="address">Child's Address</label></td>
                    <td><%= Html.TextBox("address") %></td>
                    <td><%= Html.ValidationMessage("address") %></td>
                </tr>
                <tr>
                    <td><label for="city">City</label></td>
                    <td><%= Html.TextBox("city") %></td>
                    <td><%= Html.ValidationMessage("city") %></td>
                </tr>
                <tr>
                    <td><label for="state">State</label></td>
                    <td><%= Html.DropDownList("state", ViewData.Model.StateList()) %></td>
                    <td><%= Html.ValidationMessage("state") %></td>
                </tr>
                <tr>
                    <td><label for="zip">Zip</label></td>
                    <td><%= Html.TextBox("zip") %></td>
                    <td><%= Html.ValidationMessage("zip") %></td>
                </tr>
                <tr>
                    <td><label for="locaddr">Local Address</label></td>
                    <td><%= Html.TextBox("locaddr") %></td>
                    <td><%= Html.ValidationMessage("locaddr") %></td>
                </tr>
                <tr>
                    <td><label for="homephone">Home Phone</label></td>
                    <td><%= Html.TextBox("homephone")%></td>
                    <td><%= Html.ValidationMessage("homephone")%></td>
                </tr>
                <tr>
                    <td><label for="parent">Parent's Name</label></td>
                    <td><%= Html.TextBox("parent")%></td>
                    <td><%= Html.ValidationMessage("parent")%></td>
                </tr>
                <tr>
                    <td><label for="cell">Work or Cell Phone</label></td>
                    <td><%= Html.TextBox("cell") %></td>
                    <td><%= Html.ValidationMessage("cell") %></td>
                </tr>
                <tr>
                    <td><label for="email">Email</label></td>
                    <td><%= Html.TextBox("email") %></td>
                    <td><%= Html.ValidationMessage("email") %></td>
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
                    <td><label for="request">Request Teacher/Mate</label></td>
                    <td><%= Html.TextBox("request") %></td>
                    <td><%= Html.ValidationMessage("request") %></td>
                </tr>
                <tr>
                    <td><label for="medical">Allergies or<br />
                           Medical Problems</label></td>
                    <td><%= Html.TextArea("medical") %></td>
                    <td><%= Html.ValidationMessage("medical")%>Leave blank if none</td>
                </tr>
                <tr>
                    <td><label for="church">Parent's Church</label></td>
                    <td><%= Html.CheckBox("bellevue") %> Member of Bellevue<br />
                    <%= Html.CheckBox("otherchurch") %> Active in another Local Church</td>
                    <td><%= Html.ValidationMessage("bellevue")%></td>
                </tr>
                <tr>
                    <td><label for="bringer">Who Brings Child to VBS?</label></td>
                    <td><%= Html.TextBox("bringer") %></td>
                    <td><%= Html.ValidationMessage("bringer") %></td>
                </tr>
                <tr>
                    <td><label for="bringerphone">Phone number for person bringing child</label></td>
                    <td><%= Html.TextBox("bringerphone") %></td>
                    <td><%= Html.ValidationMessage("bringerphone") %></td>
                </tr>
                 <tr>
                    <td><label for="parentvbs">Does parent 
                        <br />
                        work in VBS?</label></td>
                    <td><%= Html.RadioButton("parentvbs", 1)%> Yes
                    <%= Html.RadioButton("parentvbs", 0)%> No</td>
                    <td><%= Html.ValidationMessage("parentvbs")%></td>
                </tr>
                 <tr>
                    <td><label for="pubphoto">May we publish your child's Photo?</label></td>
                    <td><%= Html.RadioButton("pubphoto", 1)%> Yes
                    <%= Html.RadioButton("pubphoto", 0)%> No</td>
                    <td><%= Html.ValidationMessage("pubphoto2")%></td>
                </tr>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Register" /></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
