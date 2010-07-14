<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.SearchContactModel>" %>
<form id="searchform" action="/Task/SearchContact/" method="post">
<div>
    <table class="modalPopup">
        <tr>
            <th>Contactee Name:</th>
            <td>
                <input type="text" title="part of the contactee's name" name="ContacteeName" />
            </td>
            <td rowspan="7" valign="bottom">
                <input type="button" onclick="SearchClicked()" value="Search" style="margin-left: .5em" />
            </td>
        </tr>
        <tr>
            <th>Contactor Name:</th>
            <td><input type="text" title="part of the contactor's name" name="ContacteeName" /></td>
        </tr>
        <tr>
            <th>Contact Start Date:</th>
            <td>
                <input type="text" name="StartDate" class="datepicker" />
            </td>
        </tr>
        <tr>
            <th>Contact End Date:</th>
            <td>
                <input type="text" name="EndDate" class="datepicker" />
            </td>
        </tr>
        <tr>
            <th>Contact Type:</th>
            <td>
                <%=Html.DropDownList("TypeCode", ViewData.Model.ContactTypeCodes()) %>
            </td>
        </tr>
        <tr>
            <th>Contact Reason:</th>
            <td>
                <%=Html.DropDownList("ReasonCode", ViewData.Model.ReasonTypeCodes()) %>
            </td>
        </tr>
        <tr>
            <th>Ministry:</th>
            <td>
                <%=Html.DropDownList("MinistryCode", ViewData.Model.Ministries()) %>
            </td>
        </tr>
    </table>
    <input type="button" value="Add New Contact" />
</div>
<table id="contacts">
    <thead>
        <tr> 
            <td colspan="5" nowrap="nowrap">
                <table>
                <tr>
                    <td id="NumItems"></td>
                    <td><%=Html.DropDownList("PageSize", Html.PageSizes(), new { onchange = "SearchClicked()" })%></td>
                    <td class="pagination"></td>
                </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td></td>
            <th><a href="#" class="sortable">Date</a></th>
            <th><a href="#" class="sortable">Reason</a></th>
            <th><a href="#" class="sortable">Type</a></th>
            <th>Contactees</th>
        </tr>
    </thead>
    <tbody>
        <% Html.RenderPartial("SearchContactRows", ViewData.Model); %>
    </tbody>
</table>
</form>
