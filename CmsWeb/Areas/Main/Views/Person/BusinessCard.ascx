﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.PersonInfo>" %>
<table>
    <%if (Model.Deceased)
      { %>
    <tr>
        <td style="color: Red" colspan="2">Deceased: <%=Model.basic.DeceasedDate%></td>
    </tr>
    <% } %>
    <tr>
        <td><a href="http://www.google.com/maps?q=<%=Model.PrimaryAddr.AddrCityStateZip() %>" target="_blank">
                <%=Model.PrimaryAddr.Address1%></a>
        </td>
        <td><a id="clipaddr" href="#" title="copy name and address to clipboard">clipboard</a></td>
    </tr>
    <% if (Model.PrimaryAddr.Address2.HasValue())
       { %>
    <tr>
        <td colspan="2">
            <a href="http://www.google.com/maps?q=<%=Model.PrimaryAddr.Addr2CityStateZip() %>"
                target="_blank"><%=Model.PrimaryAddr.Address2%></a>
        </td>
    </tr>
    <% } %>
    <tr>
        <td><%=Model.PrimaryAddr.CityStateZip()%></td>
        <td><a href='http://www.google.com/maps?f=d&saddr=2000+Appling+Rd,+Cordova,+Tennessee+38016&pw=2&daddr=<%=Model.PrimaryAddr.AddrCityStateZip() %>'
                target="_blank">driving directions</a>
        </td>
    </tr>
    <tr>
        <td colspan="2"><a href="mailto:<%=Model.basic.EmailAddress %>"><%=Model.basic.EmailAddress%></a></td>
    </tr>
</table>
<textarea id="addrhidden" rows="5" cols="20" style="display: none"><%=Model.Name %>
    <%=Model.PrimaryAddr.Address1 %>
    <% if (Model.PrimaryAddr.Address2.HasValue())
       { %><%=Model.PrimaryAddr.Address2 %>
    <% } %><%=Model.PrimaryAddr.CityStateZip() %>
</textarea>