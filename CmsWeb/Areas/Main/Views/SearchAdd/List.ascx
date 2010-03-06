<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<% 
    Html.RenderPartial("HiddenModel", Model); 
    %>
<table id="people" cellpadding="0" cellspacing="0">
<% 
   int n = 0;
   CMSWeb.Models.SearchPersonModel p = null;
   for (; n < Model.List.Count; n++ )
   {
       p = Model.List[n];
       p.index = n;
%>
<tr <%=n % 2 == 1 ? "class='alt'" : "" %>><td>
<%
    Html.RenderPartial("PersonDisplay", p);
    Html.RenderPartial("HiddenPerson", p);
%>
</td></tr>    
<% } %>
<tr><td colspan="2">
        <a href="/SearchAdd/Complete/<%=Model.typeid %>" class="bt formlink default">Commit and Add</a>
        or <a href="/SearchAdd/SearchPerson/" class="bt formlink">Another Search</a>
        or <a href="/SearchAdd/FormAbbreviated/<%=p.FamilyId %>" class="bt formlink">Add to Last Family</a></td>
    </tr>
</table>