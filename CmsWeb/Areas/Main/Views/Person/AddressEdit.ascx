<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PersonPage.AddressInfo>" %>
<a class="displayedit" href="/Person/AddressDisplay/<%=Model.PeopleId %>?type=<%=Model.Name %>">Cancel</a>
<table class="Design2">
    <tr>
        <th>Address:</th>
        <td><%=Html.TextBox("Address1") %></td>
    </tr>
    <tr>
        <th>Address2:</th>
        <td><%=Html.TextBox("Address2") %></td>
    </tr>
    <tr>
        <th>City:</th>
        <td><%=Html.TextBox("City") %></td>
    </tr>
    <tr>
        <th>State:</th>
        <td><%=Html.DropDownList("State", CmsWeb.Models.PersonPage.AddressInfo.States()) %></td>
    </tr>
    <tr>
        <th>Zip:</th>
        <td><%=Html.TextBox("Zip", Model.Zip.FmtZip()) %> <a id="verifyaddress" href="/Person/VerifyAddress/" class="button">Verify</a></td>
    </tr>
    <% if (Model.person.CanUserEditAll)
       { %>
    <tr>
        <th>Resident Code:</th>
        <td><%=Html.DropDownList("ResCodeId", CmsWeb.Models.PersonPage.AddressInfo.ResCodes()) %></td>
    </tr>
    <tr>
        <th>Bad Address Flag:</th>
        <td><%=Html.CheckBox("BadAddress")%></td>
    </tr>
    <tr>
        <th style="vertical-align: top">Effective Dates:</th>
        <td>
            <table class="Design2">
                <tr>
                    <th>From:</th>
                    <td><%=Html.TextBox("FromDt", Model.FromDt.FormatDate())%></td>
                </tr>
                <tr>
                    <th>To:</th>
                    <td><%=Html.TextBox("ToDt", Model.ToDt.FormatDate())%></td>
                </tr>
            </table>
        </td>
    </tr>
    <% } %>
    <tr>
        <th>Preferred Address</th>
        <td><%=Html.CheckBox("Preferred", Model.Preferred, Model.Preferred ? new { disabled = "disabled" } : null)%></td>
    </tr>
    <tr><td></td><td><a href="/Person/AddressUpdate/<%=Model.PeopleId %>?type=<%=Model.Name %>" class="submitbutton">Save Changes</a></td></tr>
</table>
