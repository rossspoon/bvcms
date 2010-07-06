<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<h4>Search for Person</h4>
<table class="modalPopup" width="100%" class="searchenter">
    <tr>
        <td align="right" colspan="2"><a href="#" class="clear">clear</a></td>
    </tr>
    <tr>
        <td><label for="name">Name</label></td>
        <td><input id="name" type="text" name="m.name" value="<%=Model.name%>" /></td>
    </tr>
    <tr>
        <td><label for="phone">Communication</label></td>
        <td><input id="phone" type="text" name="m.phone" value="<%=Model.phone%>" /></td>
    </tr>
    <tr>
        <td><label for="address">Address</label></td>
        <td><input id="address" type="text" name="m.address" value="<%=Model.address%>" /></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td><input id="dob" type="text" name="m.dob" value="<%=Model.dob%>" class="dob" title="" /></td>
    </tr>
</table>
<p align="right">
<a href="/SearchAdd/Results/" class="bt formlink default">Search</a>
<% if (Model.type == "family")
   { %>
    <a href="/SearchAdd/FormAbbreviated/<%=Model.typeid %>" class="bt formlink" title="shame on you for even thinking about doing this">Skip Search, Add New</a>
<% }
   else
   { %>
    <a href="/SearchAdd/FormFull/" class="bt formlink" title="shame on you for even thinking about doing this">Skip Search, Add New</a>
<% } %>
    <a href="/SearchAdd/SearchCancel/" class="bt formlink" title="<%=Model.List.Count > 0 ? "back to selections" : "quit the dialog"%>"><%=Model.List.Count > 0 ? "go back" : "close"%></a></td>
  
</p>
<% 
    int n = 0;
    foreach (var p in Model.List)
    {
        p.index = n++;
        Html.RenderPartial("HiddenPerson", p);
    }
%>
