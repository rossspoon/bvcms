<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel>" %>
<%=Html.Hidden("m.divid", Model.divid) %>
<%=Html.Hidden("m.orgid", Model.orgid) %>
<%=Html.Hidden("m.testing", Model.testing) %>
<%=Html.Hidden("m.URL", Model.URL) %>
<table cellpadding="0" cellspacing="2" width="100%">
<% 
    for (var i = 0; i < Model.List.Count; i++)
    {
        var p = Model.List[i];
        p.index = i;
%>
<tr><td class="alt<%=p.index % 2 %>">
<%=Html.Hidden("m.List.index", p.index) %>
<%=Html.Hidden3("m.List[" + p.index + "].orgid", p.orgid) %>
<%=Html.Hidden3("m.List[" + p.index + "].divid", p.divid) %>
<%=Html.Hidden3("m.List[" + p.index + "].ShowAddress", p.ShowAddress) %>
<%=Html.Hidden3("m.List[" + p.index + "].TryCount", p.TryCount) %>
<%
        p.LastItem = i == (Model.List.Count - 1);
       
        if (p.ShowDisplay())
        {
            Html.RenderPartial("PersonDisplay", p);
            if (p.OtherOK)
                Html.RenderPartial("OtherDisplay", p);
            else
                Html.RenderPartial("OtherEdit", p);
        }
        else if (!Model.IsEnded())
            Html.RenderPartial("PersonEdit", p);
%>
</td>
<td><a class="cancel" href="/OnlineReg/Cancel/<%=p.index %>">remove<br />participant</a></td>
</tr>
<% }
   var last = Model.List[Model.List.Count - 1];
   last.LastItem = true;
   if (last.OtherOK && last.ShowDisplay())
   { %>
<tr><td colspan="2">
    <% if (!Model.OnlyOneAllowed())
       { %>
        <span class="blue">OK, now you can add another household member</span>
        <blockquote><a href="/OnlineReg/AddAnotherPerson/" class="submitbutton">Add another household member</a></blockquote>
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
</td></tr>
<% } %>
</table>