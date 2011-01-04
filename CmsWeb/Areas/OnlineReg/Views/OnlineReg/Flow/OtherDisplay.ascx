<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<% if (Model.org.AskShirtSize == true)
   { %>
    <tr>
        <td><label for="shirtsize">ShirtSize</label></td>
        <td colspan="4"><%=Model.shirtsize %> </td>
    </tr>
<% }
   if (Model.org.AskRequest == true)
   { %>
    <tr>
        <td><label for="request"><%=Util.PickFirst(Model.org.RequestLabel, "Request") %></label></td>
        <td colspan="4"><%=Model.request %> </td>
    </tr>
<% }
   if (Model.org.AskGrade == true)
   { %>
    <tr>
        <td><label for="request">Grade</label></td>
        <td colspan="4"><%=Model.grade %> </td>
    </tr>
<% }
   if (Model.org.AskEmContact == true)
   { %>
    <tr>
        <td><label for="emcontact">Emergency Friend</label></td>
        <td colspan="4"><%=Model.emcontact %> </td>
    </tr>
    <tr>
        <td><label for="emphone">Emergency Phone</label></td>
        <td colspan="4"><%=Model.emphone %> </td>
    </tr>
<% }
   if (Model.org.AskInsurance == true)
   { %>
    <tr>
        <td><label for="insurance">Health Insurance Carrier</label></td>
        <td colspan="4"><%=Model.insurance %> </td>
    </tr>
    <tr>
        <td><label for="policy">Policy #</label></td>
        <td colspan="4"><%=Model.policy %> </td>
    </tr>
<% }
   if (Model.org.AskDoctor == true)
   { %>
    <tr>
        <td><label for="doctor">Family Physician Name</label></td>
        <td colspan="4"><%=Model.doctor %> </td>
    </tr>
    <tr>
        <td><label for="docphone">Family Physician Phone</label></td>
        <td colspan="4"><%=Model.docphone %> </td>
    </tr>
<% }
   if (Model.org.AskAllergies == true)
   { %>
    <tr>
        <td><label for="medical">Allergies or<br />
               Medical Problems</label></td>
        <td colspan="4"><%=Model.medical %> </td>
    </tr>
<% }
   if (Model.org.AskTylenolEtc == true)
   { %>
    <tr>
        <td><label for="medical">May we give your child</label></td>
        <td colspan="4">
        Tylenol?: <%=Model.tylenol == true ? "Yes" : Model.tylenol == false ? "No" : "" %>,
        Advil?: <%=Model.advil == true ? "Yes" : Model.advil == true ? "No" : "" %>,
        Robitussin?: <%=Model.robitussin == true ? "Yes" : Model.robitussin == false ? "No" : "" %>,
        Maalox?: <%=Model.maalox == true ? "Yes" : Model.maalox == false ? "No" : "" %>
        </td>
    </tr>
<% }
   if (Model.org.AskParents == true)
   { %>
    <tr>
        <td><label for="mname">Mother's Name (first last)</label></td>
        <td colspan="4"><%=Model.mname %> </td>
    </tr>
    <tr>
        <td><label for="fname">Father's Name (first last)</label></td>
        <td colspan="4"><%=Model.fname%> </td>
    </tr>
<% }
   if (Model.org.AskCoaching == true)
   { %>
     <tr>
        <td><label for="coaching">Interested in Coaching?</label></td>
        <td colspan="4"><%=Model.coaching == true ? "Yes" : "No" %> </td>
    </tr>
<% }
   if (Model.org.AskChurch == true)
   { %>
    <tr>
        <td><label for="church"><%= Model.org.AskParents == true ? "Parent's Church" : "Church" %></label></td>
        <td colspan="4"><%=Model.memberus? "Member of our church" : "not member of our church" %> <br />
        <%=Model.otherchurch? "Active in another Local Church" : "not active in another church" %> </td>
    </tr>
<% }
   if (Model.org.AskTickets == true)
   { %>
    <tr>
        <td><label for="ntickets">No. of Items</label></td>
        <td><%=Model.ntickets %> </td>
    </tr>
<% }
   if(Model.org.AskOptions.HasValue())
   { %>
    <tr>
        <td><div class="wraparound"><%=Util.PickFirst(Model.org.OptionsLabel, "Options")%></div></td>
        <td colspan="4"><%=Model.option %> </td>
    </tr>
<% }
   if(Model.org.ExtraOptions.HasValue())
   { %>
    <tr>
        <td><div class="wraparound"><%=Util.PickFirst(Model.org.ExtraOptionsLabel, "Extra Option")%></div></td>
        <td colspan="4"><%=Model.option2 %> </td>
    </tr>
<% }
   if(Model.org.GradeOptions.HasValue())
   { %>
    <tr>
        <td>Grade Option</td>
        <td colspan="4"><%=Model.GradeOptions().SingleOrDefault(s => s.Value == (Model.gradeoption ?? "00")).Text %> </td>
    </tr>
<% }
   foreach (var a in Model.ExtraQuestions())
   { %>
    <tr>
        <td><div class="wraparound"><%=a.question%></div></td>
        <td colspan="4"> <%=Model.ExtraQuestion[a.question] %> </td>
    </tr>
<% }
   foreach (var a in Model.YesNoQuestions())
   { %>
    <tr>
        <td><div class="wraparound"><%=a.desc%></div></td>
        <td colspan="4"> <%=Model.YesNoQuestion[a.name] == true ? "Yes" : "No" %> </td>
    </tr>
<% }
   foreach (var i in Model.MenuItemsChosen())
   { %>
    <tr>
        <td></td>
        <td colspan="4"> <%=i.number%> <%=i.desc%> </td>
    </tr>
<% }
   if (Model.org.Deposit > 0)
   { %>
    <tr>
        <td><label for="paydeposit">Payment</label></td>
        <td colspan="4"><%=Model.paydeposit == true ? "Pay Deposit Only" : "Pay Full Amount" %></td>
    </tr>
<% } %>
