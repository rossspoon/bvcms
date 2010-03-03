<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchPeopleDialogModel>" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="/Content/Dialog.css" rel="stylesheet" type="text/css" />
    <link href="/Content/Pager.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery.cluetip.css" rel="stylesheet" type="text/css" />
    <script src="/Content/js/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="/Content/js/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>    
    <script src="/Scripts/SearchDialog.js" type="text/javascript"></script>
    <script src="/Scripts/pager.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.form.js" type="text/javascript"></script>    
    <script src="/Content/js/jquery.cluetip.min.js" type="text/javascript"></script>
</head>
<body>
<form id="searchForm" class="DisplayEdit" action="/SearchPeopleDialog/Rows/" method="post">
<%=Html.Hidden("entrypoint", Model.Origin) %>
<%=Html.Hidden("origin", Model.EntryPoint) %>
<table style="font-size: 12px" class="modalPopup">
    <tr>
        <td colspan="2">
            <a id="ClearForm" href="#">clear</a>
        </td>
        <th>Campus:</th>
        <td colspan="2" nowrap="nowrap">
            <%=Html.DropDownList("CampusId", Model.CampusCodes(), new { style = "width: 150px", @class="clearable" })%>
        </td>
    </tr>
    <tr>
        <th>Name/ID#:</th>
        <td>
            <input name="Name" title="Starting letters of First<space>Last or just Last" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Tags:</th>
        <td colspan="2">
            <%=Html.DropDownList("TagId", Model.UserTags(), new { style = "width: 240px", @class = "clearable" })%>
        </td>
    </tr>
    <tr>
        <th>Communication:</th>
        <td>
            <input name="Communication" title="Any part of any phone or email" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Member:</th>
        <td>
            <%=Html.DropDownList("MemberStatusId", Model.MemberStatusCodes(), new { @class="clearable"})%>
        </td>
        <th align="left">OrgId</th>
    </tr>
    <tr>
        <th>Address:</th>
        <td>
            <input name="Address" title="Any part of the address, city or zip" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Gender:</th>
        <td>
            <%=Html.DropDownList("GenderId", Model.GenderCodes(), new { @class = "clearable" })%>
        </td>
        <td>
            <input name="OrgId" title="Organization Id" style="width:50px" type="text" class="clearable" />
        </td>
    </tr>
    <tr>
        <th>Date of Birth:</th>
        <td valign="top">
            <input name="DateOfBirth" title="YYYY or MM or MM/DD or MM/DD/YY" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Married:</th>
        <td><%=Html.DropDownList("MaritalStatusId", Model.MaritalStatusCodes(), new { @class = "clearable" })%></td>
        <td>
            <a href="/SearchPeopleDialog/Search/" class="submitbutton">Search</a>
        </td>
    </tr>
</table>
<div>
    <a id="AddNew" href="#">Add new person to new family</a> |
    <%=Html.CheckBox("AddToExisting") %> Add new person to existing family
</div>
<div id="peoplediv"></div>
</form>
</body>
</html>
