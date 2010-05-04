<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>
    <% CmsData.Organization o = Model.org; %>
<table class="Design2">
    <tr>
        <th>Name:</th>
        <td><%=o.OrganizationName %></td>
        <td align="right">&nbsp;&nbsp;<a class="displayedit2" href="/Organization/OrgInfoEdit/<%=Model.OrganizationId %>">Edit</a></td>
    </tr>
    <tr>
        <th>Schedule:</th>
        <td><%="{0:dddd h:mm tt}".Fmt(o.MeetingTime) %></td>
    </tr>
    <tr>
        <th>Main Division:</th>
        <td><span title="<%=Model.DivisionsTitle() %>"><%=o.Division.Name %></span></td>
    </tr>
    <tr>
        <th>Location:</th>
        <td><%=o.Location %></td>
    </tr>
    <tr>
        <th>Campus:</th>
        <td><%=Html.CodeDesc("org.CampusId", Model.CampusList()) %></td>
    </tr>
    <tr>
        <th>Status:</th>
        <td><%=Html.CodeDesc("org.OrganizationStatusId", Model.OrgStatusList()) %></td>
    </tr>
    <tr>
        <th>Leader Type:</th>
        <td><%=Html.CodeDesc("org.LeaderMemberTypeId", Model.LeaderTypeList())%></td>
    </tr>
</table>
