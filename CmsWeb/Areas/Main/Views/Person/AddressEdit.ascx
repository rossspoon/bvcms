<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonModel.Address>" %>
<table>
    <tr>
        <td>
            <table class="Design2">
                <tr>
                    <th>Address:</th>
                    <td><%=Html.TextBox("Address1") %></td>
                </tr>
                <tr>
                    <th>Bad Address Flag:</th>
                    <td><%=Html.CheckBox("BadAddress") %></td>
                </tr>
                <tr>
                    <th>Address2:</th>
                    <td><%=Model.Address2 %></td>
                </tr>
                <tr>
                    <th>Resident Code:</th>
                    <td><%=Html.DropDownList("ResCode", Model.ResCode) %></td>
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
                                <td><%=Model.FromDt.FormatDate() %></td>
                            </tr>
                            <tr>
                                <th>To:</th>
                                <td><%=Model.ToDt.FormatDate() %></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th>Preferred Address</th>
                    <td colspan="3">
                        <%=Html.RadioButton("Preferred", Model.Name, Model.Preferred, new { disabled = "disabled" })%>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
