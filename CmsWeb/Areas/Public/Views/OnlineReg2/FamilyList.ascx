<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OnlineRegModel2>" %>
<% var family = Model.FamilyMembers();
   if (family.Count() > 0)
   {%>
   <span class="blue">Choose a family member to register...</span>
<div style="padding-left:1em">
    <% foreach (var fm in family)
       { %>
    <a href="/OnlineReg2/Register/<%=fm.PeopleId %>" class="submitlink">Register <%=fm.Name %> (<%=fm.Age %>)</a><br />
    <% } %>
</div>
   <span class="blue">Or register a guest below</span>
<% } %>