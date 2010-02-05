<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.RetreatModel>" %>
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
        <input type="hidden" name="gender" value="<%=Model.gender%>" />
        </td>
    </tr>
    <tr>
        <td><label for="marrieddisplay">Marital Status</label></td>
        <td><%=Model.marrieddisplay %>
        <input type="hidden" name="married" value="<%=Model.married%>" />
        </td>
    </tr>
