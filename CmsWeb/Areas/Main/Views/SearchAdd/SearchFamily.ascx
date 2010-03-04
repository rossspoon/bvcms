<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<table width="90%">
<tr>
<td>
    <table>
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
            <td><a href="/SearchAdd/ResultsFamily/" class="submitbutton formlink">Search</a></td>
        </tr>
    </table>
</td><td align="right" valign="top">
    <a href="/SearchAdd/SearchFamilyCancel/" class="formlink">cancel</a>
</td></tr>
</table>
<% 
    int n = 0;
    foreach (var p in Model.List)
    {
        p.index = n++;
        Html.RenderPartial("Hidden", p);
    }
%>
