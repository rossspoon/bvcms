<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<% if (Model.org.AskShirtSize == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].shirtsize", Model.shirtsize)%>
<% }
   if (Model.org.AskRequest == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].request", Model.request)%>
<% }
   if (Model.org.AskGrade == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].grade", Model.grade)%>
<% }
   if (Model.org.AskEmContact == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].emcontact", Model.emcontact)%>
<%=Html.Hidden3("m.list[" + Model.index + "].emphone", Model.emphone)%>
<% }
   if (Model.org.AskInsurance == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].insurance", Model.insurance)%>
<%=Html.Hidden3("m.list[" + Model.index + "].policy", Model.policy)%>
<% }
   if (Model.org.AskDoctor == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].doctor", Model.doctor)%>
<%=Html.Hidden3("m.list[" + Model.index + "].docphone", Model.docphone)%>
<% }
   if (Model.org.AskAllergies == true)
   { %>
        <%=Html.Hidden3("m.list[" + Model.index + "].medical", Model.medical)%>
<% }
   if (Model.org.AskTylenolEtc == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].tylenol", Model.tylenol) %>
<%=Html.Hidden3("m.list[" + Model.index + "].advil", Model.advil)%>
<%=Html.Hidden3("m.list[" + Model.index + "].maalox", Model.maalox)%>
<%=Html.Hidden3("m.list[" + Model.index + "].robitussin", Model.robitussin)%>
<% }
   if (Model.org.AskParents == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].mname", Model.mname)%>
<%=Html.Hidden3("m.list[" + Model.index + "].fname", Model.fname)%>
<% }
   if (Model.org.AskCoaching == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].coaching", Model.coaching)%>
<% }
   if (Model.org.AskChurch == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].memberus", Model.memberus)%>
<%=Html.Hidden3("m.list[" + Model.index + "].otherchurch", Model.otherchurch)%>
<% }
   if (Model.org.AskTickets == true)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].ntickets", Model.ntickets)%>
<% }
   if(Model.org.AskOptions.HasValue())
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].option", Model.option)%>
<% }
   if(Model.org.ExtraOptions.HasValue())
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].option2", Model.option2)%>
<% }
   if(Model.org.GradeOptions.HasValue())
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].gradeoption", Model.gradeoption)%>
<% }
   foreach (var a in Model.ExtraQuestions())
   { %>
<input type="hidden" name="m.List[<%=Model.index%>].ExtraQuestion[<%=a.n %>].Key" value="<%=a.question %>" />
<input type="hidden" name="m.List[<%=Model.index%>].ExtraQuestion[<%=a.n %>].Value" value="<%=Model.ExtraQuestion[a.question] %>" />
<% }
   foreach (var a in Model.YesNoQuestions())
   { %>
<input type="hidden" name="m.List[<%=Model.index%>].YesNoQuestion[<%=a.n %>].Key" value="<%=a.name %>" />
<input type="hidden" name="m.List[<%=Model.index%>].YesNoQuestion[<%=a.n %>].Value" value="<%=Model.YesNoQuestion[a.name] %>" />
<% }
   if (Model.Checkbox != null)
   {
        foreach (var a in Model.Checkbox)
        { %>
        <input type="hidden" name='<%=Model.inputname("Checkbox")%>' value="<%=a%>" />
<%      }
   }
   foreach (var i in Model.MenuItemsChosen())
   { %>
<input type="hidden" name="m.List[<%=Model.index%>].MenuItem[<%=i.n %>].Key" value="<%=i.sg %>" />
<input type="hidden" name="m.List[<%=Model.index%>].MenuItem[<%=i.n %>].Value" value="<%=Model.MenuItemValue(i.sg) %>" />
<% }
   if (Model.org.Deposit > 0)
   { %>
<%=Html.Hidden3("m.list[" + Model.index + "].paydeposit", Model.paydeposit)%>
<% } %>
