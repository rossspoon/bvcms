<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Prayer.Models.SearchPeopleModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>ViewPage1</title>
    <link href="/Content/pager.css?v=1" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<form id="form" action="/Signup/SearchPeople/" method="post">
<div>
<script src="/Scripts/jquery-1.2.6.min-vsdoc.js" type="text/javascript"></script>
<script src="/Scripts/jquery.form2.js" type="text/javascript"></script>
<script src="/Scripts/jquery.blockUI.js" type="text/javascript"></script>
<script type="text/javascript">
    function ClearForm() {
        $('#searchform.clearable').clearFields();
    }
    var queryString = "";
    function SelectPerson(id) { }

    function ChangePage(page, pager) {
        $.post('/Signup/SearchPeople/' + page, queryString, function(ret) {
            $('#people > tbody').html(ret);
        });
        return false;
    }
    function RefreshList() {
        var q = $('#form').formSerialize();
        $.navigate("/Signup/SearchPeople", q);
    }
    function GotoPage(pg) {
        var q = $('#form').formSerialize();
        q = q.appendQuery("Page=" + pg);
        $.navigate("/Signup/SearchPeople", q);
    }
    function SetPageSize(sz) {
        var q = $('#form').formSerialize();
        q = q.appendQuery("PageSize=" + sz);
        $.navigate("/Signup/SearchPeople", q);
    }
    function SearchClicked() {
        var q = $('#form').formSerialize();
        $.navigate("/Signup/SearchPeople", q);
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
        <td colspan="2" align="center">
            <input type="button" tabindex="6" onclick="SearchClicked()" value="Search" />
        </td>
    </tr>
</table>
    <input type="button" value="Add New Person" />
</div>
<table id="people">
    <thead>
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
<% Html.RenderPartial("Pager", ViewData.Model.pagerModel()); %>
</form>

</asp:Content>