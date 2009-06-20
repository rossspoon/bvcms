<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.SearchPeopleModel>" %>
<form id="searchform" action="/Task/SearchPeople/" method="post">
<div>
<script type="text/javascript">
    function ClearForm() {
        $('#searchform.clearable').clearFields();
    }
</script>
<table class="modalPopup">
    <tr style="font-size: small">
        <td colspan="2">
            <a href="javascript:ClearForm()">clear</a>
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
            <%=Html.DropDownList("TagId", ViewData.Model.UserTags())%>
        </td>
    </tr>
    <tr>
        <th>Communication:</th>
        <td>
            <input name="Communication" title="Any part of any phone or email" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Member:</th>
        <td>
            <%=Html.DropDownList("MemberStatusId", ViewData.Model.MemberStatusCodes())%>
        </td>
    </tr>
    <tr>
        <th>Address:</th>
        <td>
            <input name="Address" title="Any part of the address, city or zip" style="width:190px" type="text" class="clearable" />
        </td>
        <th>Gender:</th>
        <td>
            <%=Html.DropDownList("GenderId", ViewData.Model.GenderCodes())%>
        </td>
    </tr>
    <tr>
        <th>Date of Birth:</th>
        <td valign="top">
            <input name="DateOfBirth" title="YYYY or MM or MM/DD or MM/DD/YY" style="width:190px" type="text" class="clearable" />
        </td>
        <td colspan="2" align="left">
            <input type="button" tabindex="6" onclick="SearchClicked()" value="Search" />
        </td>
    </tr>
</table>
    <input type="button" value="Add New Person" />
</div>
<table id="people">
    <thead>
        <tr> 
            <td colspan="5" nowrap="nowrap">
                <table>
                <tr>
                    <td id="NumItems"></td>
                    <td><%=Html.DropDownList("PageSize", Util.PageSizes(), new { onchange = "SearchClicked()" })%></td>
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
        <% Html.RenderPartial("SearchPeopleRows", ViewData.Model); %>
    </tbody>
</table>
</form>

