<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>
    <% CmsData.Organization o = Model.org; %>
<table>
<tr>
<td valign="top">
    <table class="Design2">
        <tr>
            <th>Name:</th>
            <td><%=o.OrganizationName %></td>
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
            <td><%=Html.CodeDesc("org.LeaderTypeId", Model.LeaderTypeList()) %></td>
        </tr>
    </table>
</td>
<td valign="top"><a class="displayedit2" href="/Organization/OrgInfoEdit/<%=Model.OrganizationId %>">Edit</a></td>
<% CMSWeb.Models.OrganizationPage.OrganizationModel m = Model;
   if (Page.User.IsInRole("Edit"))
   { %>
<% } 
   if (Page.User.IsInRole("ManageGroups"))
   { %>
<td>
    <form action="" method="post">
    <% Html.RenderPartial("ManageGroups", Model); %>
    </form>
</td>
<% } %>
</tr>
</table>
