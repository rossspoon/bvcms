<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<table>
<% var needother = Model.AnyOtherInfo();
   if (needother && Model.LastItem)
   {
       if (Model.IsNew)
       { %>
    <tr>
        <td></td>
        <td colspan="2"><p class="blue">OK, we have your new record, please continue below.</p></td>
    </tr>
    <% }
       else
       { %>
    <tr>
        <td></td>
        <td colspan="2"><p class="blue">OK, we found your record, please continue below.</p></td>
    </tr>
    <% }
       if (Model.index > 0 && needother)
       { %>
        <tr><th>Other Information</th><td colspan="2" align="right"><a href="#" id="copy">copy from previous</a></td></tr>
    <% }
   }
   if (Model.org.AskShirtSize == true)
   { %>
    <tr>
        <td><label for="shirtsize">ShirtSize</label></td>
        <td><%= Html.DropDownList3("", "m.List[" + Model.index + "].shirtsize", Model.ShirtSizes(), Model.shirtsize)%></td>
        <td><%= Html.ValidationMessage("shirtsize")%></td>
    </tr>
<% } 
   if (Model.org.AskRequest == true)
   { %>
    <tr>
        <td><label for="request"><%=Util.PickFirst(Model.org.RequestLabel, "Request") %></label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].request" value="<%=Model.request%>" maxlength="100" /></td>
        <td><%= Html.ValidationMessage("request") %></td>
    </tr>
<% } 
   if (Model.org.AskGrade == true)
   { %>
    <tr>
        <td><label for="grade">Grade completed<br />(during event)</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].grade" value="<%=Model.grade%>" /></td>
        <td><%= Html.ValidationMessage("grade") %></td>
    </tr>
<% } 
   if (Model.org.AskEmContact == true)
   { %>
    <tr>
        <td><label for="emcontact">Emergency Friend</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].emcontact" value="<%=Model.emcontact%>" maxlength="100" /></td>
        <td><%= Html.ValidationMessage("emcontact")%></td>
    </tr>
    <tr>
        <td><label for="emphone">Emergency Phone</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].emphone" value="<%=Model.emphone%>" maxlength="15" /></td>
        <td><%= Html.ValidationMessage("emphone")%></td>
    </tr>
<% } 
   if (Model.org.AskInsurance == true)
   { %>
    <tr>
        <td><label for="insurance">Health Insurance Carrier</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].insurance" value="<%=Model.insurance%>" maxlength="100" /></td>
        <td><%= Html.ValidationMessage("insurance")%></td>
    </tr>
    <tr>
        <td><label for="policy">Policy #</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].policy" value="<%=Model.policy%>" maxlength="100" /></td>
        <td><%= Html.ValidationMessage("policy")%></td>
    </tr>
<% } 
   if (Model.org.AskDoctor == true)
   { %>
    <tr>
        <td><label for="doctor">Family Physician Name</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].doctor" value="<%=Model.doctor%>" maxlength="100" /></td>
        <td><%= Html.ValidationMessage("doctor")%></td>
    </tr>
    <tr>
        <td><label for="docphone">Family Physician Phone</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].docphone" value="<%=Model.docphone%>" maxlength="15" /></td>
        <td><%= Html.ValidationMessage("docphone")%></td>
    </tr>
<% } 
   if (Model.org.AskAllergies == true)
   { %>
    <tr>
        <td><label for="medical">Allergies or<br />
               Medical Problems</label></td>
        <td colspan="2"><textarea name="m.List[<%=Model.index%>].medical"><%=Model.medical %></textarea>
        <span class="blue"> Leave blank if none</span></td>
    </tr>
<% }
   if (Model.org.AskTylenolEtc == true)
   { %>
    <tr>
        <td><label for="medical">May we give your child</label></td>
        <td>
            <table>
            <tr>
                <td>Tylenol?:</td>
                <td>
                    <input type="radio" name="m.List[<%=Model.index%>].tylenol" value="true" <%=Model.tylenol == true ? "checked='checked'" : "" %> />Yes
                    <input type="radio" name="m.List[<%=Model.index%>].tylenol" value="false" <%=Model.tylenol == false ? "checked='checked'" : "" %> />No
                    <%=Html.ValidationMessage("tylenol") %>
                </td>
            </tr>
            <tr>
                <td>Advil?:</td>
                <td>
                    <input type="radio" name="m.List[<%=Model.index%>].advil" value="true" <%=Model.advil == true ? "checked='checked'" : "" %> />Yes
                    <input type="radio" name="m.List[<%=Model.index%>].advil" value="false" <%=Model.advil == false ? "checked='checked'" : "" %> />No
                    <%=Html.ValidationMessage("advil")%>
                </td>
            </tr>
            <tr>
                <td>Maalox?:</td>
                <td>
                    <input type="radio" name="m.List[<%=Model.index%>].maalox" value="true" <%=Model.maalox == true ? "checked='checked'" : "" %> />Yes
                    <input type="radio" name="m.List[<%=Model.index%>].maalox" value="false" <%=Model.maalox == false ? "checked='checked'" : "" %> />No
                    <%=Html.ValidationMessage("maalox")%>
                </td>
            </tr>
            <tr>
                <td>Robitussin?:</td>
                <td>
                    <input type="radio" name="m.List[<%=Model.index%>].robitussin" value="true" <%=Model.robitussin == true ? "checked='checked'" : "" %> />Yes
                    <input type="radio" name="m.List[<%=Model.index%>].robitussin" value="false" <%=Model.robitussin == false ? "checked='checked'" : "" %> />No
                    <%=Html.ValidationMessage("robitussin")%>
                </td>
            </tr>
            </table>
        </td>
        <td></td>
    </tr>
<% }
   if (Model.org.AskParents == true)
   { %>
    <tr>
        <td><label for="mname">Mother's Name (first last)</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].mname" value="<%=Model.mname%>" maxlength="80" /></td>
        <td><%= Html.ValidationMessage("mname")%></td>
    </tr>
    <tr>
        <td><label for="fname">Father's Name (first last)</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].fname" value="<%=Model.fname%>" maxlength="80" /></td>
        <td><%= Html.ValidationMessage("fname")%></td>
    </tr>
<% }
   if (Model.org.AskCoaching == true)
   { %>
     <tr>
        <td><label for="coaching">Interested in Coaching?</label></td>
        <td><input type="radio" name="m.List[<%=Model.index%>].coaching" value = "true"  <%=Model.coaching == true ? "checked='checked'" : "" %> /> Yes
            <input type="radio" name="m.List[<%=Model.index%>].coaching" value = "false" <%=Model.coaching == false ? "checked='checked'" : "" %> /> No</td>
        <td><%= Html.ValidationMessage("coaching") %></td>
    </tr>
<% }
   if (Model.org.AskChurch == true)
   { %>
    <tr>
        <td><label for="church"><%= Model.org.AskParents == true ? "Parent's Church" : "Church" %></label></td>
        <td><input type="checkbox" name="m.List[<%=Model.index %>].memberus" value = "true" 
            <%=Model.memberus == true ? "checked='checked'" : "" %> /> Member of this Church<br />
            <input type="checkbox" name="m.List[<%=Model.index %>].otherchurch" value="true"
            <%=Model.otherchurch == true ? "checked='checked'" : "" %> /> Active in another Local Church</td>
        <td><%=Html.ValidationMessage("member")%></td>
    </tr>
<% }
   if (Model.org.AskTickets == true)
   { %>
    <tr>
        <td><label for="ntickets">No. of Items</label></td>
        <td><input type="text" name="m.List[<%=Model.index%>].ntickets" value="<%=Model.ntickets%>" /></td>
        <td><%= Html.ValidationMessage("ntickets") %></td>
    </tr>
<% }
   if(Model.org.AskOptions.HasValue())
   { %>
    <tr>
        <td><div class="wraparound"><%=Util.PickFirst(Model.org.OptionsLabel, "Options")%></div></td>
        <td><%=Html.DropDownList3("", "m.List[" + Model.index + "].option", Model.Options(), "0")%></td>
        <td><%=Html.ValidationMessage("option")%></td>
    </tr>
<% }
   if(Model.org.ExtraOptions.HasValue())
   { %>
    <tr>
        <td><div class="wraparound"><%=Util.PickFirst(Model.org.ExtraOptionsLabel, "Extra Options")%></div></td>
        <td><%=Html.DropDownList3("", "m.List[" + Model.index + "].option2", Model.ExtraOptions(), "0")%></td>
        <td><%=Html.ValidationMessage("option2")%></td>
    </tr>
<% }
   if(Model.org.GradeOptions.HasValue())
   { %>
    <tr>
        <td>Grade</td>
        <td><%=Html.DropDownList3("", "m.List[" + Model.index + "].gradeoption", Model.GradeOptions(), Model.gradeoption)%></td>
        <td><%=Html.ValidationMessage("gradeoption")%></td>
    </tr>
<% }
   foreach (var a in Model.ExtraQuestions())
   { %>
    <tr>
        <td><div class="wraparound"><%=a.question%></div></td>
        <td>
            <input type="hidden" name="m.List[<%=Model.index%>].ExtraQuestion[<%=a.n %>].Key" value="<%=a.question %>" />
            <input type="text" name="m.List[<%=Model.index%>].ExtraQuestion[<%=a.n %>].Value" value="<%=Model.ExtraQuestionValue(a.question) %>" />
        </td>
        <td><%=Html.ValidationMessage(a.question + "-QError")%></td>
    </tr>
<% }
    foreach (var a in Model.YesNoQuestions())
    { %>
    <tr>
        <td><div class="wraparound"><%=a.desc%></div></td>
        <td>
            <input type="hidden" name="m.List[<%=Model.index%>].YesNoQuestion[<%=a.n %>].Key" value="<%=a.name %>" />
            <input type="radio" name="m.List[<%=Model.index%>].YesNoQuestion[<%=a.n %>].Value" value="true" <%=Model.YesNoChecked(a.name, true) %> />Yes
            <input type="radio" name="m.List[<%=Model.index%>].YesNoQuestion[<%=a.n %>].Value" value="false" <%=Model.YesNoChecked(a.name, false) %> />No
        </td>
        <td><%=Html.ValidationMessage(a.name + "-YNError")%></td>
    </tr>
<% }
   foreach(var i in Model.MenuItems())
   { %>
    <tr>
        <td></td>
        <td>
            <input type="hidden" name="m.List[<%=Model.index%>].MenuItem[<%=i.n %>].Key" value="<%=i.sg %>" />
            <input type="text" name="m.List[<%=Model.index%>].MenuItem[<%=i.n %>].Value" value="<%=Model.MenuItemValue(i.sg) %>" class="short" />
            <%=i.desc %> ($<%=i.amt.ToString("N2") %>)
        </td>
        <td><%=Html.ValidationMessage(i.sg + "-MIError")%></td>
    </tr>
<% }
   if (Model.org.Deposit > 0)
   { %>
    <tr>
        <td><label for="deposit">Payment</label></td>
        <td><input type="radio" name="m.List[<%=Model.index%>].paydeposit" value="true" <%=Model.paydeposit == true ? "checked='checked'" : "" %> />Pay Deposit Only
            <input type="radio" name="m.List[<%=Model.index%>].paydeposit" value="false" <%=Model.paydeposit == false ? "checked='checked'" : "" %> />Pay Full Amount
        </td>
        <td><%=Html.ValidationMessage("paydeposit")%></td>
    </tr>
<% } %>
    <tr><td></td>
        <td colspan="2"><a href="/OnlineReg/SubmitOtherInfo/<%=Model.index %>" class="submitbutton">Submit</a></td>
    </tr>
</table>
