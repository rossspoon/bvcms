<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RecRegModel>" %>
<% var IsAdult = Model.IsAdult(); %>
<tr>
<td>
<table>
    <tr>
        <td><label for="shirtsize">ShirtSize</label></td>
        <td><%=Model.shirtsize %>
        <%=Html.Hidden("shirtsize") %>
        <%=Html.Hidden("request") %>
        <%=Html.Hidden("emcontact") %>
        <%=Html.Hidden("emphone") %>
        <%=Html.Hidden("insurance") %>
        <%=Html.Hidden("policy") %>
        <%=Html.Hidden("doctor") %>
        <%=Html.Hidden("phone") %>
        <%=Html.Hidden("docphone") %>
        <%=Html.Hidden("coaching") %>
        <%=Html.Hidden("member") %>
        <%=Html.Hidden("otherchurch") %>
        </td>
    </tr>
    <tr>
        <td><label for="request">Request Teammate</label></td>
        <td><%=Model.request %></td>
    </tr>
    <tr>
        <td><label for="emcontact">Emergency Friend</label></td>
        <td><%=Model.emcontact %></td>
    </tr>
    <tr>
        <td><label for="emphone">Emergency Phone</label></td>
        <td><%=Model.emphone %></td>
    </tr>

    <tr>
        <td><label for="insurance">Health Insurance Carrier</label></td>
        <td><%=Model.insurance %></td>
    </tr>
    <tr>
        <td><label for="policy">Policy #</label></td>
        <td><%=Model.policy %></td>
    </tr>
    <tr>
        <td><label for="doctor">Family Physician Name</label></td>
        <td><%=Model.doctor %></td>
    </tr>
    <tr>
        <td><label for="docphone">Family Physician Phone</label></td>
        <td><%=Model.docphone %></td>
    </tr>

<% if (!IsAdult)
{ %>
    <tr>
        <td><label for="medical">Allergies or<br />
               Medical Problems</label></td>
        <td><%=Model.medical %>
        <%=Html.Hidden("medical") %>
        <%=Html.Hidden("mname") %>
        <%=Html.Hidden("fname") %>
        </td>
    </tr>
    <tr>
        <td><label for="mname">Mother's Name (first last)</label></td>
        <td><%=Model.mname %></td>
    </tr>
    <tr>
        <td><label for="fname">Father's Name (first last)</label></td>
        <td><%=Model.fname%></td>
    </tr>
<% } %>
     <tr>
        <td><label for="coaching">Interested in Coaching?</label></td>
        <td><%=Model.coaching == 1 ? "Yes" : "No" %></td>
    </tr>
    <tr>
        <td><label for="church"><%= !IsAdult ? "Parent's Church" : "Church" %></label></td>
        <td><%=Model.member? "Member of our church" : "not member of our church" %> <br />
        <%=Model.otherchurch? "Active in another Local Church" : "not active in another church" %>
        </td>
    </tr>
    <tr><td></td>
        <td><input id="submitit" type="submit" class="submitbutton" 
        value="Complete Registration and Pay" /></td>
    </tr>
</table>
</td>
</tr>