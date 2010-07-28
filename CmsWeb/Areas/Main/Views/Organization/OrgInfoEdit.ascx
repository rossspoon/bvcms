<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrganizationPage.OrganizationModel>" %>
<table class="Design2" style="margin-bottom: 10px">
    <tr><td colspan="2" align='right'><a class="displayedit2" href="/Organization/OrgInfo/<%=Model.OrganizationId %>">cancel</a></td></tr>
    <tr>
        <th>Name:</th>
        <td><%=Html.TextBox("org.OrganizationName") %></td>
    </tr>
    <tr>
        <th>Main Division:</th>
        <td><%=Html.DropDownList("org.DivisionId", Model.Divisions()) %></td>
    </tr>
    <tr>
        <th>Multiple Divisions:</th>
        <td><select id="DivisionsList" name="DivisionsList" multiple="multiple">
<% foreach(var s in Model.DivisionPickList())
   { %>
            <option <%=s.Selected ? "selected='selected'" : "" %> value="<%=s.Text %>"><%=s.Text %></option>
<% } %>
            </select>
        </td>
    </tr>
    <tr>
        <th>Location:</th>
        <td><%=Html.TextBox("org.Location") %></td>
    </tr>
    <tr>
        <th>Campus:</th>
        <td><%=Html.DropDownList("org.CampusId", Model.CampusList()) %></td>
    </tr>
    <tr>
        <th>Status:</th>
        <td><%=Html.DropDownList("org.OrganizationStatusId", Model.OrgStatusList())%></td>
    </tr>
    <tr>
        <th>Leader Type:</th>
        <td><%=Html.DropDownList("org.LeaderMemberTypeId", Model.LeaderTypeList())%></td>
    </tr>
    <tr>
        <th>Main Fellowship:</th>
        <td><%=Html.CheckBox("org.IsBibleFellowshipOrg")%></td>
    </tr>
    <tr><td></td><td align="right"><a href="/Organization/OrgInfoUpdate/<%=Model.OrganizationId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
