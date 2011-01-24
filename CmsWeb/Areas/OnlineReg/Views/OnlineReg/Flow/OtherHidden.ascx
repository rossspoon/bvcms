<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegPersonModel>" %>
<% if (Model.org.AskShirtSize == true)
   { %>
<%=Html.Hidden3(Model.inputname("shirtsize"), Model.shirtsize) %>
<% }
   if (Model.org.AskRequest == true)
   { %>
<%=Html.Hidden3(Model.inputname("request"), Model.request)%>
<% }
   if (Model.org.AskGrade == true)
   { %>
<%=Html.Hidden3(Model.inputname("grade"), Model.grade)%>
<% }
   if (Model.org.AskEmContact == true)
   { %>
<%=Html.Hidden3(Model.inputname("emcontact"), Model.emcontact)%>
<%=Html.Hidden3(Model.inputname("emphone"), Model.emphone)%>
<% }
   if (Model.org.AskInsurance == true)
   { %>
<%=Html.Hidden3(Model.inputname("insurance"), Model.insurance)%>
<%=Html.Hidden3(Model.inputname("policy"), Model.policy)%>
<% }
   if (Model.org.AskDoctor == true)
   { %>
<%=Html.Hidden3(Model.inputname("doctor"), Model.doctor)%>
<%=Html.Hidden3(Model.inputname("docphone"), Model.docphone)%>
<% }
   if (Model.org.AskAllergies == true)
   { %>
        <%=Html.Hidden3(Model.inputname("medical"), Model.medical)%>
<% }
   if (Model.org.AskTylenolEtc == true)
   { %>
<%=Html.Hidden3(Model.inputname("tylenol"), Model.tylenol) %>
<%=Html.Hidden3(Model.inputname("advil"), Model.advil)%>
<%=Html.Hidden3(Model.inputname("maalox"), Model.maalox)%>
<%=Html.Hidden3(Model.inputname("robitussin"), Model.robitussin)%>
<% }
   if (Model.org.AskParents == true)
   { %>
<%=Html.Hidden3(Model.inputname("mname"), Model.mname)%>
<%=Html.Hidden3(Model.inputname("fname"), Model.fname)%>
<% }
   if (Model.org.AskCoaching == true)
   { %>
<%=Html.Hidden3(Model.inputname("coaching"), Model.coaching)%>
<% }
   if (Model.org.AskChurch == true)
   { %>
<%=Html.Hidden3(Model.inputname("memberus"), Model.memberus)%>
<%=Html.Hidden3(Model.inputname("otherchurch"), Model.otherchurch)%>
<% }
   if (Model.org.AskTickets == true)
   { %>
<%=Html.Hidden3(Model.inputname("ntickets"), Model.ntickets)%>
<% }
   if(Model.org.AskOptions.HasValue())
   { %>
<%=Html.Hidden3(Model.inputname("option"), Model.option)%>
<% }
   if(Model.org.ExtraOptions.HasValue())
   { %>
<%=Html.Hidden3(Model.inputname("option2"), Model.option2)%>
<% }
   if(Model.org.GradeOptions.HasValue())
   { %>
<%=Html.Hidden3(Model.inputname("gradeoption"), Model.gradeoption)%>
<% }
   foreach (var a in Model.ExtraQuestions())
   { %>
<%=Html.Hidden3(Model.inputname("ExtraQuestion[" + a.n + "].Key"), a.question) %>
<%=Html.Hidden3(Model.inputname("ExtraQuestion[" + a.n + "].Value"), Model.ExtraQuestion[a.question]) %>
<% }
   foreach (var a in Model.YesNoQuestions())
   { %>
<%=Html.Hidden3(Model.inputname("YesNoQuestion[" + a.n + "].Key"), a.name) %>
<%=Html.Hidden3(Model.inputname("YesNoQuestion[" + a.n + "].Value"), Model.YesNoQuestion[a.name]) %>
<% }
   if (Model.Checkbox != null)
   {
        foreach (var a in Model.Checkbox)
        { %>
<%=Html.Hidden3(Model.inputname("Checkbox"), a)%>
<%      }
   }
   foreach (var i in Model.MenuItemsChosen())
   { %>
<%=Html.Hidden3(Model.inputname("MenuItem[" + i.n + "].Key"), i.sg) %>
<%=Html.Hidden3(Model.inputname("MenuItem[" + i.n + "].Value"), Model.MenuItemValue(i.sg)) %>
<% }
   if (Model.org.Deposit > 0)
   { %>
<%=Html.Hidden3(Model.inputname("paydeposit"), Model.paydeposit)%>
<% } %>
<%=Html.Hidden3(Model.inputname("CreatingAccount"), Model.CreatingAccount)%>