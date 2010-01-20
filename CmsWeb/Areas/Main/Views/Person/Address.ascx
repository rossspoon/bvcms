<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonModel.AddressInfo>" %>
<table>
    <tr>
        <td>
            <table class="Design2">
                <tr>
                    <th>Address:</th>
                    <td><%=Model.AddressLine1 %></td>
                    <th>Bad Address Flag:</th>
                    <td><input type="checkbox" <%=Model.BadAddress%> disabled="disabled" /></td>
                </tr>
                <tr>
                    <th>&nbsp;</th>
                    <td><%=Model.AddressLine2 %></td>
                    <th>Resident Code:</th>
                    <td><%=Model.ResCode %></td>
                </tr>
                <tr>
                    <th>City:</th>
                    <td><%=Model.City %></td>
                </tr>
                <tr>
                    <th>State:</th>
                    <td><%=Model.State %></td>
                </tr>
                <tr>
                    <th>Zip:</th>
                    <td><%=Model.Zip.FmtZip() %></td>
                </tr>
                <tr>
                    <th style="vertical-align: top">Effective Dates:</th>
                    <td colspan="3">
                        <table class="Design2">
                            <tr>
                                <th>From:</th>
                                <td><%=Model.FromDate.ToShortDateStr() %></td>
                            </tr>
                            <tr>
                                <th>To:</th>
                                <td><%=Model.ToDate.ToShortDateStr() %></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th>Preferred Address</th>
                    <td colspan="3">
                        <input type="radio" disabled="disabled" <%=Model.Preferred%> />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
