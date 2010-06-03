<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OnlineRegPersonModel>" %>
<table>
<% if (Model.org.AskShirtSize == true)
   { %>
    <tr>
        <td><label for="shirtsize">ShirtSize</label></td>
        <td><%=Model.shirtsize %>
        <%=Html.Hidden3("m.list[" + Model.index + "].shirtsize", Model.shirtsize)%>
        </td>
    </tr>
<% }
   if (Model.org.AskRequest == true)
   { %>
    <tr>
        <td><label for="request"><%=Util.PickFirst(Model.org.RequestLabel, "Request") %></label></td>
        <td><%=Model.request %>
        <%=Html.Hidden3("m.list[" + Model.index + "].request", Model.request)%>
        </td>
    </tr>
<% }
   if (Model.org.AskGrade == true)
   { %>
    <tr>
        <td><label for="request">Grade</label></td>
        <td><%=Model.grade %>
        <%=Html.Hidden3("m.list[" + Model.index + "].grade", Model.grade)%>
        </td>
    </tr>
<% }
   if (Model.org.AskEmContact == true)
   { %>
    <tr>
        <td><label for="emcontact">Emergency Friend</label></td>
        <td><%=Model.emcontact %>
        <%=Html.Hidden3("m.list[" + Model.index + "].emcontact", Model.emcontact)%>
        </td>
    </tr>
    <tr>
        <td><label for="emphone">Emergency Phone</label></td>
        <td><%=Model.emphone %>
        <%=Html.Hidden3("m.list[" + Model.index + "].emphone", Model.emphone)%>
        </td>
    </tr>
<% }
   if (Model.org.AskInsurance == true)
   { %>
    <tr>
        <td><label for="insurance">Health Insurance Carrier</label></td>
        <td><%=Model.insurance %>
        <%=Html.Hidden3("m.list[" + Model.index + "].insurance", Model.insurance)%>
        </td>
    </tr>
    <tr>
        <td><label for="policy">Policy #</label></td>
        <td><%=Model.policy %>
        <%=Html.Hidden3("m.list[" + Model.index + "].policy", Model.policy)%>
        </td>
    </tr>
<% }
   if (Model.org.AskDoctor == true)
   { %>
    <tr>
        <td><label for="doctor">Family Physician Name</label></td>
        <td><%=Model.doctor %>
        <%=Html.Hidden3("m.list[" + Model.index + "].doctor", Model.doctor)%>
        </td>
    </tr>
    <tr>
        <td><label for="docphone">Family Physician Phone</label></td>
        <td><%=Model.docphone %>
        <%=Html.Hidden3("m.list[" + Model.index + "].docphone", Model.docphone)%>
        </td>
    </tr>
<% }
   if (Model.org.AskAllergies == true)
   { %>
    <tr>
        <td><label for="medical">Allergies or<br />
               Medical Problems</label></td>
        <td><%=Model.medical %>
        <%=Html.Hidden3("m.list[" + Model.index + "].medical", Model.medical)%>
        </td>
    </tr>
<% }
   if (Model.org.AskTylenolEtc == true)
   { %>
    <tr>
        <td><label for="medical">May we give your child</label></td>
        <td>
        Tylenol?: <%=Model.tylenol == true ? "Yes" : Model.tylenol == false ? "No" : "" %>,
        Advil?: <%=Model.advil == true ? "Yes" : Model.advil == true ? "No" : "" %>,
        Robitussin?: <%=Model.robitussin == true ? "Yes" : Model.robitussin == false ? "No" : "" %>,
        Maalox?: <%=Model.maalox == true ? "Yes" : Model.maalox == false ? "No" : "" %>
        <%=Html.Hidden3("m.list[" + Model.index + "].tylenol", Model.tylenol) %>
        <%=Html.Hidden3("m.list[" + Model.index + "].advil", Model.advil)%>
        <%=Html.Hidden3("m.list[" + Model.index + "].maalox", Model.maalox)%>
        <%=Html.Hidden3("m.list[" + Model.index + "].robitussin", Model.robitussin)%>
        </td>
    </tr>
<% }
   if (Model.org.AskParents == true)
   { %>
    <tr>
        <td><label for="mname">Mother's Name (first last)</label></td>
        <td><%=Model.mname %>
        <%=Html.Hidden3("m.list[" + Model.index + "].mname", Model.mname)%>
        </td>
    </tr>
    <tr>
        <td><label for="fname">Father's Name (first last)</label></td>
        <td><%=Model.fname%>
        <%=Html.Hidden3("m.list[" + Model.index + "].fname", Model.fname)%>
        </td>
    </tr>
<% }
   if (Model.org.AskCoaching == true)
   { %>
     <tr>
        <td><label for="coaching">Interested in Coaching?</label></td>
        <td><%=Model.coaching == true ? "Yes" : "No" %>
        <%=Html.Hidden3("m.list[" + Model.index + "].coaching", Model.coaching)%>
        </td>
    </tr>
<% }
   if (Model.org.AskChurch == true)
   { %>
    <tr>
        <td><label for="church"><%= Model.org.AskParents == true ? "Parent's Church" : "Church" %></label></td>
        <td><%=Model.memberus? "Member of Bellevue" : "not member of bellevue" %> <br />
        <%=Model.otherchurch? "Active in another Local Church" : "not active in another church" %>
        <%=Html.Hidden3("m.list[" + Model.index + "].memberus", Model.memberus)%>
        <%=Html.Hidden3("m.list[" + Model.index + "].otherchurch", Model.otherchurch)%>
        </td>
    </tr>
<% }
   if (Model.org.AskTickets == true)
   { %>
    <tr>
        <td><label for="ntickets">No. of Items</label></td>
        <td><%=Model.ntickets %>
        <%=Html.Hidden3("m.list[" + Model.index + "].ntickets", Model.ntickets)%>
        </td>
    </tr>
<% }
   if(Model.org.AskOptions.HasValue())
   { %>
    <tr>
        <td>Option</td>
        <td><%=Model.option %>
        <%=Html.Hidden3("m.list[" + Model.index + "].option", Model.option)%>
        </td>
    </tr>
<% }
   if(Model.org.GradeOptions.HasValue())
   { %>
    <tr>
        <td>Grade Option</td>
        <td><%=Model.GradeOptions().SingleOrDefault(s => s.Value == (Model.gradeoption ?? "00")).Text %>
        <%=Html.Hidden3("m.list[" + Model.index + "].gradeoption", Model.gradeoption)%>
        </td>
    </tr>
<% }
   foreach (var a in Model.ExtraQuestions())
   { %>
    <tr>
        <td><%=a.question%></td>
        <td>
            <input type="hidden" name="m.List[<%=Model.index%>].ExtraQuestion[<%=a.n %>].Key" value="<%=a.question %>" />
            <input type="hidden" name="m.List[<%=Model.index%>].ExtraQuestion[<%=a.n %>].Value" value="<%=Model.ExtraQuestion[a.question] %>" />
            <%=Model.ExtraQuestion[a.question] %>
        </td>
    </tr>
<% }
   foreach (var a in Model.YesNoQuestions())
   { %>
    <tr>
        <td><%=a.desc%></td>
        <td>
            <input type="hidden" name="m.List[<%=Model.index%>].YesNoQuestion[<%=a.n %>].Key" value="<%=a.name %>" />
            <input type="hidden" name="m.List[<%=Model.index%>].YesNoQuestion[<%=a.n %>].Value" value="<%=Model.YesNoQuestion[a.name] %>" />
            <%=Model.YesNoQuestion[a.name] == true ? "Yes" : "No" %>
        </td>
    </tr>
<% }
   if (Model.org.Deposit > 0)
   { %>
    <tr>
        <td><label for="paydeposit">Payment</label></td>
        <td><%=Model.paydeposit == true ? "Pay Deposit Only" : "Pay Full Amount" %>
            <%=Html.Hidden3("m.list[" + Model.index + "].paydeposit", Model.paydeposit)%>
        </td>
    </tr>
<% } %>
</table>
