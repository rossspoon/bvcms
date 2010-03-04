<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.SearchPersonModel>" %>
<table width="90%">
<tr>
<td>
    <table>
        <tr>
            <td><label for="first">First Name</label></td>
            <td><%=Model.first %></td>
        </tr>
        <tr>
            <td><label for="last">Last Name</label></td>
            <td><%=Model.last %>
            </td>
        </tr>
         <tr>
            <td><label for="dob">Date of Birth</label></td>
            <td><%=Model.birthday.ToShortDateString() %> <span><%=Model.age %></span>
            </td>
        </tr>
        <tr>
            <td><label for="phone">Phone</label></td>
            <td><%=Model.phone.FmtFone(Model.homecell) %>
            </td>
        </tr>
        <tr>
            <td><label for="email">Email</label></td>
            <td><%=Model.email %>
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
    </table>
</td>
<td align="right" valign="top"><a class="formlink" href="/SearchAdd/PersonCancel/<%=Model.index %>">cancel</a></td>
</tr>
</table>