<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchModel>" %>
<div>
<table>
    <tr>
        <td><label for="name">Name</label></td>
        <td><input type="text" name="name" value="<%=Model.name%>" /></td>
    </tr>
    <tr>
        <td><label for="phone">Communication</label></td>
        <td><input type="text" name="phone" value="<%=Model.phone%>" /></td>
    </tr>
    <tr>
        <td><label for="address">Address</label></td>
        <td><input type="text" name="address" value="<%=Model.address%>" /></td>
    </tr>
     <tr>
        <td><label for="dob">Date of Birth</label></td>
        <td><input type="text" name="dob" value="<%=Model.dob%>" class="dob" title="" /></td>
    </tr>
    <tr><td></td>
        <td><a href="/SearchAdd/Search/" class="submitbutton">Search</a></td>
    </tr>
</table>
</div>
