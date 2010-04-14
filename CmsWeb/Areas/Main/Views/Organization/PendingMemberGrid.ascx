<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.MemberModel>" %>
<% Html.RenderPartial("ExportToolBar"); %>
&nbsp;<div style="clear: both"></div>
<p>
Count: <strong><%=Model.Count() %></strong>
<a href="/SearchAdd/Index/<%=Model.OrganizationId %>?type=pending" class="memberdialog">add pending members</a> |
<a href="/Organization/AddFromTag/<%=Model.OrganizationId %>?pending=true" id="addfromtaglink" title="Add Pendings From Tag">add from tag</a> |
<a href="/OrgMembersDialog/Index/<%=Model.OrganizationId %>?pendings=true" title="Update Pending Members" class="memberdialog">update pending members</a>
</p>
<% Html.RenderPartial("MemberGrid", Model); %>