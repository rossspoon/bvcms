<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<table id="people" cellpadding="0" cellspacing="0">
<% 
   int n = 0;
   foreach(var p in Model.List)
   {
       p.index = n++;
%>
<tr <%=n % 2 == 1 ? "class='alt'" : "" %>><td>
<%
    Html.RenderPartial("PersonDisplay", p);
    Html.RenderPartial("Hidden", p);
%>
</td></tr>    
<% } %>
<tr><td colspan="2"><input id="submitit" type="submit" class="submitbutton" value='Add Complete' />
        or <a href="/SearchAdd/SearchPerson/" class="submitbutton formlink">Another Search</a>
        or <a href="/SearchAdd/AddToFamily/" class="submitbutton formlink">Add to Last Family</a></td>
    </tr>
</table>