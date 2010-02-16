<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RecRegModel>" %>
    <tr>
        <td><label for="addr">Address</label></td>
        <td><%=Model.address %>
        </td>
    </tr>
    <tr>
        <td><label for="zip">Zip</label></td>
        <td><%=Model.zip.FmtZip() %>
        </td>
    </tr>
    <tr>
        <td><label for="city">City</label></td>
        <td><%=Model.city %>
        </td>
    </tr>
    <tr>
        <td><label for="state">State</label></td>
        <td><%=Model.state %>
        </td>
    </tr>
    <tr>
        <td><label for="gender">Gender</label></td>
        <td><%=Model.genderdisplay %> 
        </td>
    </tr>
    <tr>
        <td><label for="marrieddisplay">Marital Status</label></td>
        <td><%=Model.marrieddisplay %>
        </td>
    </tr>
