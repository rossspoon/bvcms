<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.MemberModel>" %>
<% Html.RenderPartial("ExportToolBar"); %>
&nbsp;<div style="clear: both"></div>
<p>
Count: <strong><%=Model.Count() %></strong>&nbsp;&nbsp;
Small Groups: <%=Html.DropDownList("smallgroupid", Model.SmallGroups()) %>
<a href="/SearchAdd/Index/<%=Model.OrganizationId %>?type=org" class="addmembers">add members</a> |
<a href="/Organization/AddFromTag/<%=Model.OrganizationId %>" id="addfromtaglink" title="Add Members From Tag">add from tag</a> |
<a id="currMembersUpdate" href="/OrgMembersDialog/Index/<%=Model.OrganizationId %>" title="Update Members" class="memberdialog">update members</a>
</p>
<% Html.RenderPartial("MemberGrid", Model); %>
