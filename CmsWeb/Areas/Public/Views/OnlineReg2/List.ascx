<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel2>" %>
<% //================================MODEL DATA=====================================================
 %>
<input type="hidden" name="m.divid" value="<%=Model.divid %>" />
<input type="hidden" name="m.orgid" value="<%=Model.orgid %>" />
<input type="hidden" name="m.testing" value="<%=Model.testing %>" />
<input type="hidden" name="m.URL" value="<%=Model.URL %>" />
<input type="hidden" name="m.UserPeopleId" value="<%=Model.UserPeopleId %>" />

<% //==================================LOGIN=====================================================
    
   if (Model.List.Count == 1 && !Model.UserPeopleId.HasValue && !Model.IsCreateAccount())
   { %>
<span class="blue">You can register by logging in</span>
<div style="padding-left: 1em">
<table class="login" cellspacing="6" style="padding-left:1em; border-width: 1px;border-color: #D3D3D3; border-style: solid; width: auto" >
    <tr>
        <td><label for="username">Username</label></td>
        <td><input id="username" type="text" name="m.username" value="<%=Model.username%>"  /></td>
        <td valign="bottom">forgot username? <%= Html.ValidationMessage("username") %></td>
    </tr>
    <tr>
        <td><label for="password">Password</label></td>
        <td><input id="password" type="password" name="m.password" value="<%=Model.password%>" /></td>
        <td>forgot password? <%= Html.ValidationMessage("password") %></td>
    </tr>
    <tr>
        <td colspan="2" align="center"><a href="/OnlineReg2/Login/" class="submitbutton">Login</a></td>
        <td>
            <a href="/CreateAccount">don't have an account? create one now</a><br />
            <a target="_blank" href="http://www.bvcms.com/Page/WhyMemberAccount">why do I need this?</a>
        </td>
    </tr>
</table>
</div>
<%= Html.ValidationMessage("authentication") %>
<div class="blue">Or you can fill in the form below</div>
<% }
    //=================================PARTICIPANT LIST========================================== 
  %>
<table cellpadding="0" cellspacing="2" width="100%" style="padding-left:1em">
<% 
    for (var i = 0; i < Model.List.Count; i++)
    {
        var p = Model.List[i];
        p.index = i;
        p.LastItem = i == (Model.List.Count - 1);
        if (Model.UserPeopleId.HasValue && p.LastItem && !p.ShowDisplay())
        { %>
<tr><td>
           <% Html.RenderPartial("FamilyList", Model); %>
</td></tr>
     <% }
%>
<tr><td style="border-width: 1px;border-color: #778899; border-style: solid;">
<input type="hidden" name="m.List.index" value="<%=p.index %>" />
<%=Html.Hidden3("m.List[" + p.index + "].orgid", p.orgid)%>
<%=Html.Hidden3("m.List[" + p.index + "].divid", p.divid)%>
<%=Html.Hidden3("m.List[" + p.index + "].ShowAddress", p.ShowAddress)%>
<%
    if (p.UserSelectsOrganization())
        Html.RenderPartial("ChooseClass", p);
        
    if (p.ShowDisplay())
    {
       Html.RenderPartial("PersonDisplay", p);
       if (p.OtherOK)
           Html.RenderPartial("OtherDisplay", p);
       else
           Html.RenderPartial("OtherEdit", p);
    }
    else if (!Model.IsEnded())
       if (p.IsFamily)
           Html.RenderPartial("PersonDisplay", p);
       else
           Html.RenderPartial("PersonEdit", p);
%>
</td>
    <% if (!Model.IsCreateAccount())
       { %>
<td><a class="cancel" href="/OnlineReg2/Cancel/<%=p.index %>">remove<br />participant</a></td>
    <% } %>
</tr>
<%      } %>
</table>
<% //=================================BUTTONS================================================= 
    
   if (Model.last != null && Model.last.OtherOK && Model.last.ShowDisplay())
   {   
       if (!Model.OnlyOneAllowed())
       { %>
        <span class="blue">OK, now you can add another household member</span>
        <blockquote><a href="/OnlineReg2/AddAnotherPerson/" class="submitbutton">Add another participant</a></blockquote>
        <span class="blue">Or you can complete your registration</span>
    <% }
       else if(Model.ChoosingSlots())
       { %>
        <span class="blue">OK, now you can choose your committments</span>
    <% } 
       else
       { %>
        <span class="blue">OK, Now you can complete your registration</span>
    <% } %>
       <blockquote>
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
        </blockquote>
<% } %>
