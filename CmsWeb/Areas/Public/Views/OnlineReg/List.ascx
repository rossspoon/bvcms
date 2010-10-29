<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<% //================================MODEL DATA=====================================================
 %>
<input type="hidden" name="m.divid" value="<%=Model.divid %>" />
<input type="hidden" name="m.orgid" value="<%=Model.orgid %>" />
<input type="hidden" name="m.testing" value="<%=Model.testing %>" />
<input type="hidden" name="m.URL" value="<%=Model.URL %>" />
<input type="hidden" name="m.UserPeopleId" value="<%=Model.UserPeopleId %>" />
<input type="hidden" name="m.nologin" value="<%=Model.nologin %>" />

<% //==================================LOGIN=====================================================

if (Model.DisplayLogin())
{ %>
<% Html.RenderPartial("Login", Model); %>
<% 
}
else
{ %>
<%=Model.Instructions%>
<table cellpadding="6" width="100%">
<% if (!Model.OnlyOneAllowed() && Model.List.Count > 1)
   { %>
<tr><td><div class="instruct">Registrants</div></td></tr>
<% }
   //=================================PARTICIPANT LIST========================================== 
   for (var i = 0; i < Model.List.Count; i++)
   {
    var p = Model.List[i];
    p.index = i;
    p.LastItem = i == (Model.List.Count - 1);

    if (p.LastItem && !p.ShowDisplay())
    {
        //---------------------CHOOSE A GROUP--------------------------------
        
        if (Model.UserSelectsOrganization())
        { %>
<tr><td><div class="instruct">Choose a group/event...</div></td></tr>
<tr><td><div class="box" style="padding:1em">
   <% Html.RenderPartial("ChooseClass", Model); %></div>
</td></tr>
     <% }
        //---------------------CHOOSE FAMILY MEMBER--------------------------------

        if (Model.UserPeopleId.HasValue && Model.FamilyMembers().Count() > 0)
        { %>
<tr><td><div class="instruct">Choose a family member to register...</div></td></tr>
<tr><td><div class="box" style="padding:1em">
    <% Html.RenderPartial("FamilyList", Model); %></div>
        <%= Html.ValidationMessage("findf")%>
</td></tr>
<tr><td><div class="instruct">Or register a guest (or new family member) below</div></td></tr>
     <% }
        else
        { %>
<tr><td><div class="instruct">Fill out the form to <%=Model.IsCreateAccount() ? "create an account" : "register" %></div></td></tr>
     <% }
    } %>
<tr><td><div class="box">
<%   //-----------------------REMOVE REGISTRATION---------------------------
   if (!Model.IsCreateAccount())
   { %>
<a href="/OnlineReg/Cancel/<%=p.index %>" class="close submitlink"><img src="/images/delete.gif" border="0" alt="cancel" title="cancel this registration" /></a>
<% } %>
<input type="hidden" name="m.List.index" value="<%=p.index %>" />
<% 
       //------------------------HIDDEN DATA-------------------------------
       
   Html.RenderPartial("PersonMetaHidden", p);
   if (p.ShowDisplay())
   {
       Html.RenderPartial("PersonHidden", p);
       if (p.OtherOK)
           Html.RenderPartial("OtherHidden", p);
   }
       //-----------------------DETAILS TOGGLE------------------------------
   if (p.Finished())
   { %>
<div class="personheader"><%=p.first + " " + p.last %> <span class="blue" style="font-size:80%">(show or hide <a class="toggle" href="#">details</a>)</span></div>
<% } %>
<span><%= Html.ValidationMessage("findn") %></span>
<table class="particpant" style='<%=!p.Finished() ? "" : "display: none" %>'>
<%
      //-----------------------PERSON IDENTIFIED------------------------------
   if (p.ShowDisplay())
   {
       Html.RenderPartial("PersonDisplay", p);
       if (p.OtherOK)
           Html.RenderPartial("OtherDisplay", p);
       else if (p.AnyOtherInfo())
       { %>
           <tr><td colspan="5">
                <p class="instruct">OK, we <%=p.IsNew ? "have your new" : "found your"%> record, please continue below.</p>
           </td></tr>
        <% Html.RenderPartial("OtherEdit", p);
       }
   }
   else if (!Model.IsEnded())
       if (p.IsFamily)
           Html.RenderPartial("PersonDisplay", p);
           
       else  //-------------------FIND PERSON OR ADD NEW--------------------------
           
           Html.RenderPartial("PersonEdit", p);
%>
</table>
</div></td>
</tr>
<% } %>
</table>
<% //=================================BUTTONS================================================= 
    
   if (Model.last != null && Model.last.OtherOK && Model.last.ShowDisplay())
   { %> 
<div class="instruct">
<%   if (!Model.OnlyOneAllowed())
       { %>
        <a href="/OnlineReg/AddAnotherPerson/" class="submitbutton">Register another</a> Or
    <% } %>
    <% var amt = Model.Amount();
       if (amt > 0)
       { %>
        <input id="submitit" type="submit"
             class="submitbutton" value='Complete Registration and Pay <%=amt.ToString("c") %>' />
    <% }
       else if(Model.Terms.HasValue())
       { %>
        <input id="submitit" type="submit"
             class="submitbutton" value='Complete Registration and Read Terms' />
    <% }
       else if(Model.ChoosingSlots())
       { %>
        <input id="submitit" type="submit"
             class="submitbutton" value='Choose Committment Times' />
    <% }
       else
       { %>
        <input id="submitit" type="submit"
             class="submitbutton" value='Complete Registration' />
    <% } %>
</div>
<% }
} %>