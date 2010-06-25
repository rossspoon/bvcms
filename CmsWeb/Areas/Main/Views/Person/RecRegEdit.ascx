<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.RecRegInfo>" %>
<a class="displayedit" href="/Person/RecRegDisplay/<%=Model.PeopleId %>">cancel</a>
<table class="Design2">
    <tr>
        <td><label for="shirtsize">ShirtSize</label></td>
        <td><%= Html.DropDownList("shirtsize", CMSWeb.Models.RecRegModel.ShirtSizes(null))%></td>
        <td><%= Html.ValidationMessage("shirtsize")%></td>
    </tr>
    <tr>
        <td><label for="emcontact">Emergency Friend</label></td>
        <td><%= Html.TextBox("emcontact", Model.emcontact, new { maxlength = 100 }) %></td>
        <td><%= Html.ValidationMessage("emcontact")%></td>
    </tr>
    <tr>
        <td><label for="emphone">Emergency Phone</label></td>
        <td><%= Html.TextBox("emphone", Model.emphone, new { maxlength = 15 })%></td>
        <td><%= Html.ValidationMessage("emphone")%></td>
    </tr>
    <tr>
        <td><label for="insurance">Health Insurance Carrier</label></td>
        <td><%= Html.TextBox("insurance", Model.insurance, new { maxlength = 100 })%></td>
        <td><%= Html.ValidationMessage("insurance")%></td>
    </tr>
    <tr>
        <td><label for="policy">Policy #</label></td>
        <td><%= Html.TextBox("policy", Model.policy, new { maxlength = 100 })%></td>
        <td><%= Html.ValidationMessage("policy")%></td>
    </tr>
    <tr>
        <td><label for="doctor">Family Physician Name</label></td>
        <td><%= Html.TextBox("doctor", Model.doctor, new { maxlength = 100 })%></td>
        <td><%= Html.ValidationMessage("doctor")%></td>
    </tr>
    <tr>
        <td><label for="docphone">Family Physician Phone</label></td>
        <td><%= Html.TextBox("docphone", Model.docphone, new { maxlength = 15 })%></td>
        <td><%= Html.ValidationMessage("docphone")%></td>
    </tr>
    <tr>
        <td><label for="medical">Allergies or<br />
               Medical Problems</label></td>
        <td><%= Html.TextArea("medical", Model.medical, new { maxlength = 30 })%></td>
        <td><%= Html.ValidationMessage("medical")%>Leave blank if none</td>
    </tr>
    <tr>
        <td colspan="2">
        <table>
        <tr>
        <td>Tylenol: <%=Html.CheckBox("tylenol") %></td>
        <td>Advil: <%=Html.CheckBox("advil") %></td>
        <td>Robitussin: <%=Html.CheckBox("robitussin") %></td>
        <td>Maalox: <%=Html.CheckBox("maalox") %></td>
        </tr>
        </table>
        </td>
    </tr>
    <tr>
        <td><label for="mname">Mother's Name (first last)</label></td>
        <td><%= Html.TextBox("mname", Model.mname, new { maxlength = 80 })%></td>
        <td><%= Html.ValidationMessage("mname")%></td>
    </tr>
    <tr>
        <td><label for="fname">Father's Name (first last)</label></td>
        <td><%= Html.TextBox("fname", Model.fname, new { maxlength = 80 })%></td>
        <td><%= Html.ValidationMessage("fname")%></td>
    </tr>
     <tr>
        <td><label for="coaching">Interested in Coaching?</label></td>
        <td><%= Html.RadioButton("coaching", "true") %> Yes
        <%= Html.RadioButton("coaching", "false") %> No</td>
        <td><%= Html.ValidationMessage("coaching2") %></td>
    </tr>
    <tr>
        <td><label for="church">Church</label></td>
        <td><%= Html.CheckBox("member") %> Member of our church<br />
        <%= Html.CheckBox("otherchurch") %> Active in another Local Church</td>
        <td><%= Html.ValidationMessage("member")%></td>
    </tr>
<% if(Page.User.IsInRole("Admin"))
   { %>
    <tr>
        <td><label for="comments">History</label></td>
        <td><%= Html.TextArea("comments", Model.Comments)%></td>
        <td></td>
    </tr>
<% } %>
<tr><td></td></tr>
     <tr><td></td><td><a href="/Person/RecRegUpdate/<%=Model.PeopleId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
