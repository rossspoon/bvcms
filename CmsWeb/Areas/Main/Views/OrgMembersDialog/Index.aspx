<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OrgMembersDialogModel>" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Index</title>
    <link href="/Content/style.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery-ui-1.8.1.custom.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery.tooltip.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="/Content/js/jquery-ui-1.8.1.custom.min.js" type="text/javascript"></script>    
    <script src="/Content/js/jquery.tooltip.min.js" type="text/javascript"></script>
    <script src="/Scripts/OrgMembersDialog.js" type="text/javascript"></script>
<form action="/OrgMembersDialog/Update" method="post">
<%=Html.Hidden("orgid") %>
<%=Html.Hidden("pendings") %>
<table class="modalPopup">
	<tr>
		<td colspan="2"><a id="ClearSearch" href="#">clear</a></td>
	</tr>
	<tr>
		<th>Tag: </th>
		<td><%=Html.DropDownList("tag", Model.Tags(), new { @class="filter" }) %></td>
	</tr>
	<tr>
		<th>Member Type:</th>
		<td><%=Html.DropDownList("memtype", Model.MemberTypeCodesWithNotSpecified(), new { @class = "filter" })%></td>
	</tr>
	<tr>
		<th>Inactive Date: </th>
		<td valign="top"><%=Html.TextBox("inactivedt", "", new { @class="filter datepicker" }) %></td>
	</tr>
</table>
<div id="EditSection">
    New Values: 
    Membertype <%=Html.DropDownList("MemberType", Model.MemberTypeCodesWithDrop())%>
    InActive Date <%=Html.TextBox("InactiveDate", "", new { @class = "datepicker", style = "width:100px" })%>
    Pending <%=Html.CheckBox("Pending")%>
</div>
<input id="SelectAll" type="checkbox" /> Select All
<%=Html.SubmitButton("Update", "Update Selected")%>
<div>
<table class="grid">
    <thead>
	<tr>
		<th>Select </th>
		<th>Name </th>
		<th>Address </th>
		<th>CityStateZip </th>
		<th>Age </th>
	</tr>
	</thead>
	<tbody>
<% Html.RenderPartial("Rows", Model); %>
    </tbody>
</table>
</div>
</form>
</body>
</html>
