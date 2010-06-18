<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OrgGroupsModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery.dimensions.js")
        .Add("/Content/js/jquery.tooltip.js")
        .Add("/Scripts/OrgGroups.js")
        .Render("/Content/OrgGroups_#.js")
            %>        
<form id="groupsform" action="/OrgGroups/Update" method="post">
<%=Html.Hidden("orgid") %>
<h2><a href="/Organization/Index/<%=Model.orgid %>"><%=Model.OrgName %></a></h2>
<table id="ManageGroups" class="modalPopup">
<% Html.RenderPartial("ManageGroups", Model); %>
</table>
<input id="SelectAll" type="checkbox" /> Select All
<%=Html.SubmitButton("AssignSelectedToTargetGroup", "Assign Selected To Target Group")%>
<%=Html.SubmitButton("RemoveSelectedFromTargetGroup", "Remove Selected From Target Group")%>
<div>
<table class="grid">
    <thead>
	<tr>
		<th>Select </th>
		<th align="left">Name </th>
		<th align="left">Address </th>
		<th align="left">CityStateZip </th>
		<th>Age </th>
		<th>Gender </th>
		<th width="5%"></th>
		<th align="left" width="25%">Groups </th>
	</tr>
	</thead>
	<tbody>
<% Html.RenderPartial("Rows", Model); %>
    </tbody>
</table>
</div>
</form>
</asp:Content>

