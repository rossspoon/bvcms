<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.SearchModel>" %>
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<input type="hidden" name="m.dob" value="<%=Model.dob%>" />
<h4>Search for Family</h4>
<a class="helplink" target="_blank" href='<%=Model.HelpLink("SearchFamily") %>'>help</a>
<table width="100%" class="modalPopup searchenter">
    <tr>
    <td align="right" colspan="2"><a href="#" class="clear">clear</a></td>
    </tr>
    <tr>
        <td><label for="name">Last Name</label></td>
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
</table>
<p align="right">
<a href="/SearchAdd/ResultsFamily/" class="bt formlink default">Search</a>
<a href="/SearchAdd/SearchFamilyCancel/" class="bt formlink" title="back to person search">go back</a>
</p>
<% 
    int n = 0;
    foreach (var p in Model.List)
    {
        p.index = n++;
        Html.RenderPartial("HiddenPerson", p);
    }
%>
