<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<input type="hidden" name="m.type" value="<%=Model.type%>" />
<input type="hidden" name="m.typeid" value="<%=Model.typeid%>" />
<input type="hidden" name="m.from" value="<%=Model.from%>" />
<table width="100%" class="modalPopup">
<tr><th align="left" colspan="2">Search for Person</th>
    <td align="right">
        <a href="/SearchAdd/SearchCancel/" class="formlink" title="<%=Model.List.Count > 0 ? "back to selections" : "quit the dialog"%>"><%=Model.List.Count > 0 ? "go back" : "close"%></a></td>
</tr>
<tr><td colspan="2">
    <table width="100%">
        <tr>
            <td><label for="name">Name</label></td>
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
         <tr>
            <td><label for="dob">Date of Birth</label></td>
            <td><input type="text" name="m.dob" value="<%=Model.dob%>" class="dob" title="" /></td>
        </tr>
        <tr><td></td>
            <td><a href="/SearchAdd/Results/" class="bt formlink default">Search</a></td>
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
