<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrganizationPage.MemberModel>" %>
<% ViewData["OrgMemberContext"] = true;
   Html.RenderPartial("ExportToolBar");
   ViewData["OrgMemberContext"] = false; 
%>
&nbsp;<div style="clear: both"></div>
<p>
Count: <strong><%=Model.Count() %></strong>&nbsp;&nbsp;
<a class="filtergroupslink" href="#">Filter by name/group<%= Model.isFiltered ? "<span style='color:red'>(filtered)</span>" : "" %></a>
<% if(Page.User.IsInRole("Edit"))
   { %>
| <a href="/SearchAdd/Index/<%=Model.OrganizationId %>?type=org" class="addmembers">add members</a>
| <a href="/Organization/AddFromTag/<%=Model.OrganizationId %>" id="addfromtaglink" title="Add Members From Tag">add from tag</a>
| <a id="currMembersUpdate" href="/OrgMembersDialog/Index/<%=Model.OrganizationId %>" title="Update Members" class="memberdialog">update members</a>
<% } %>
</p>
<% Html.RenderPartial("MemberGrid", Model); %>
