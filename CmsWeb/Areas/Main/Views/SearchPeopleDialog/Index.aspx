<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchPeopleDialogModel>" %>
<html>
<head>
    <link href="/Content/style.css" rel="stylesheet" type="text/css" />
    <link href="/Content/jquery-ui-1.7.2.custom.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <script src="/Content/js/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="/Content/js/jquery-ui-1.7.2.custom.min.js" type="text/javascript"></script>    
    <script src="/Scripts/SearchDialog.js" type="text/javascript"></script>
    <script src="/Scripts/Pager.js" type="text/javascript"></script>

<form class="DisplayEdit" action="/SearchPeopleDialog/Search/" method="post">
<%=Html.Hidden("entrypoint", Model.Origin) %>
<%=Html.Hidden("origin", Model.EntryPoint) %>
<table style="font-size: 12px" class="modalPopup">
    <tr>
        <td colspan="2">
            <a id="ClearForm" href="#">clear</a>
        </td>
        <th>Campus:</th>
        <td colspan="2" nowrap="nowrap">
            <%=Html.DropDownList("CampusId", Model.CampusCodes(), new { style = "width: 150px" })%>
        </td>
    </tr>
    <tr>
        <th>Name/ID#:</th>
        <td>
            <input name="Name" title="Starting letters of First<space>Last or just Last" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Tags:</th>
        <td colspan="2">
            <%=Html.DropDownList("TagId", Model.UserTags(), new {style="width: 240px"})%>
        </td>
    </tr>
    <tr>
        <th>Communication:</th>
        <td>
            <input name="Communication" title="Any part of any phone or email" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Member:</th>
        <td>
            <%=Html.DropDownList("MemberStatusId", Model.MemberStatusCodes())%>
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
            <%=Html.DropDownList("GenderId", Model.GenderCodes())%>
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
        <td><%=Html.DropDownList("MaritalStatusId", Model.MaritalStatusCodes()) %></td>
        <td>
            <a href="/SearchPeopleDialog/Search/" class="submitbutton">Search</a>
        </td>
    </tr>
</table>
</form>
<div>
    <a id="AddNew" href="#">Add new person to new family</a> |
    <%=Html.CheckBox("AddToExisting") %> Add new person to existing family
</div>
<table id="people" class="grid" style="font-size:12px">
    <thead>
        <tr> 
            <td colspan="5" nowrap="nowrap">
                <table>
                <tr>
                    <td id="NumItems"></td>
                    <td><%=Html.DropDownList("PageSize", Html.PageSizes())%></td>
                    <td class="pagination"></td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <th><a href="#" class="sortable">Id</a></th>
            <th><a href="#" class="sortable">Name</a></th>
            <th>Address</th>
            <th><a href="#" class="sortable">CityStateZip</a></th>
            <th><a href="#" class="sortable">Age</a></th>
        </tr>
    </thead>
    <tbody>
    </tbody>
</table>
</body>
</html>