<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.RecRegInfo>" %>
<% if (Page.User.IsInRole("Edit"))
   { %>
<a class="displayedit" href="/Person/RecRegEdit/<%=Model.PeopleId %>">Edit</a>
<% } %>
<table>
    <tr>
        <td><label for="shirtsize">ShirtSize</label></td>
        <td><%=Model.shirtsize %></td>
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
    <tr>
        <td><label for="medical">Allergies or<br />Medical Problems</label></td>
        <td><%=Model.medical %></td>
    </tr>
    <tr>
        <td colspan="2">
        <table>
        <tr>
        <% if (Model.tylenol.HasValue)
           { %>
        <td>Tylenol: <%=Model.tylenol == true ? "Yes" : "No"%></td>
        <% } 
           if (Model.advil.HasValue)
           { %>
        <td>Advil: <%=Model.advil == true ? "Yes" : "No" %></td>
        <% } 
           if (Model.robitussin.HasValue)
           { %>
        <td>Robitussin: <%=Model.robitussin == true ? "Yes" : "No" %></td>
        <% } 
           if (Model.maalox.HasValue)
           { %>
        <td>Maalox: <%=Model.maalox == true ? "Yes" : "No" %></td>
        <% } %>
        </tr>
        </table>
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
     <tr>
        <td><label for="coaching">Interested in Coaching?</label></td>
        <td><%=Model.coaching == true ? "Yes" : "No" %></td>
    </tr>
    <tr>
        <td><label for="church"><%="Church" %></label></td>
        <td><%=Model.member? "Member of Bellevue" : "not member of bellevue" %> <br />
        <%=Model.otherchurch? "Active in another Local Church" : "not active in another church" %>
        </td>
    </tr>
    <tr>
        <td valign="top">Previous Registrations</td>
        <td><%=Util.SafeFormat(Model.Comments) %></td>
    </tr>
</table>
