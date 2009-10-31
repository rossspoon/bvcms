<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWebCommon.Models.SearchPeopleModel>" %>
<form id="searchform" method="post">
<%=Html.Hidden("entrypoint", Model.Origin) %>
<%=Html.Hidden("origin", Model.EntryPoint) %>
<div>
<table class="modalPopup">
    <tr style="font-size: small">
        <td colspan="2">
            <a id="ClearForm" href="#">clear</a>
        </td>
        <th>OrgId:</th>
        <td>
            <input name="OrgId" title="Organization Id" style="width:90px" type="text" class="clearable" />
        </td>
    </tr>
    <tr>
        <th>Name/ID#:</th>
        <td>
            <input name="Name" title="Starting letters of First<space>Last or just Last" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Tags:</th>
        <td>
            <%=Html.DropDownList("TagId", Model.UserTags())%>
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
    </tr>
    <tr>
        <th>Date of Birth:</th>
        <td valign="top">
            <input name="DateOfBirth" title="YYYY or MM or MM/DD or MM/DD/YY" style="width:190px" type="text" class="clearable" />
        </td>
        <td colspan="2">
            <input id="Search" type="button" tabindex="6" value="Search" />
        </td>
    </tr>
</table>
<div>
    <a id="AddNew" href="#">Add new person to new family</a> |
    <%=Html.CheckBox("AddToExisting") %> Add new person to existing family
</div>
</div>
<table id="people">
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
        <% Html.RenderPartial("Rows", ViewData.Model); %>
    </tbody>
</table>
</form>

