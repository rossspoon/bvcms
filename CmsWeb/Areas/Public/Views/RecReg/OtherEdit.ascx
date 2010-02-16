<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RecRegModel>" %>
<% var IsAdult = Model.IsAdult(); %>
<tr>
<td>
<table>
    <tr>
        <td><label for="shirtsize">ShirtSize</label></td>
        <td><%= Html.DropDownList("shirtsize", CMSWeb.Models.RecRegModel.ShirtSizes(Model.RecAgeDiv))%></td>
        <td><%= Html.ValidationMessage("shirtsize")%></td>
    </tr>
    <tr>
        <td><label for="request">Request Teammate</label></td>
        <td><%= Html.TextBox("request", Model.request, new { maxlength = 140 })%></td>
        <td><%= Html.ValidationMessage("request") %></td>
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

<% if (!IsAdult)
{ %>
    <tr>
        <td><label for="medical">Allergies or<br />
               Medical Problems</label></td>
        <td><%= Html.TextArea("medical", Model.medical, new { maxlength = 30 })%></td>
        <td><%= Html.ValidationMessage("medical")%>Leave blank if none</td>
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
<% } %>
     <tr>
        <td><label for="coaching">Interested in Coaching?</label></td>
        <td><%= Html.RadioButton("coaching", 1) %> Yes
        <%= Html.RadioButton("coaching", 0) %> No</td>
        <td><%= Html.ValidationMessage("coaching2") %></td>
    </tr>
    <tr>
        <td><label for="church"><%= !IsAdult ? "Parent's Church" : "Church" %></label></td>
        <td><%= Html.CheckBox("member") %> Member of Bellevue<br />
        <%= Html.CheckBox("otherchurch") %> Active in another Local Church</td>
        <td><%= Html.ValidationMessage("member")%></td>
    </tr>
    <tr><td></td>
        <td><a href="/RecReg/SubmitOtherInfo" class="submitbutton">Submit</a></td>
    </tr>
</table>
</td>
</tr>