<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<% Html.RenderPartial("HiddenModel", Model); %>
<table width="100%" class="modalPopup">
<tr><th align="left">Search for Family</th>
    <td align="right">
        <a href="/SearchAdd/SearchFamilyCancel/" class="formlink" title="back to person search">go back</a></td>
</tr>
<tr><td colspan="2">
    <table width="100%">
        <tr>
            <td><label for="name">Last Name</label></td>
            <td><input type="text" name="m.name" value="<%=Model.name%>" /></td>
        </tr>
        <tr>
            <td><label for="phone">Communication</label></td>
            <td><input type="text" name="m.phone" value="<%=Model.phone%>" /></td>
        </tr>
        <tr>
            <td><label for="address">Address</label></td>
            <td><input type="text" name="m.address" value="<%=Model.address%>" /></td>
        </tr>
        <tr><td></td>
            <td><a href="/SearchAdd/ResultsFamily/" class="bt formlink default">Search</a></td>
        </tr>
    </table>
</td></tr>
</table>
<% 
    int n = 0;
    foreach (var p in Model.List)
    {
        p.index = n++;
        Html.RenderPartial("HiddenPerson", p);
    }
%>
