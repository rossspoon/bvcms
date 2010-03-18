<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsData.Organization>" %>
<%  CmsData.Organization o = Model; %>
<a class="displayedit" href="/Organization/Settings/<%=o.OrganizationId %>">cancel</a>
<table class="Design2">
    <tr>
        <th>Schedule:</th>
        <td><%=Html.DropDownList("SchedDay", CMSWeb.Models.OrganizationPage.OrganizationModel.DaysOfWeek()) %>
         <%=Html.TextBox("SchedTime", Model.SchedTime.ToString2("h:mm tt")) %></td>
    </tr>
    <tr>
        <th>Allow Attendance Overlap:</th>
        <td><%=Html.CheckBox("AllowAttendOverlap")%></td>
    </tr>
    <tr>
        <th>Class Filled:</th>
        <td><%=Html.CheckBox("ClassFilled") %></td>
    </tr>
    <tr>
        <th>Online Catalog Sort:</th>
        <td><%=Html.TextBox("OnLineCatalogSort") %></td>
    </tr>
    <tr>
        <th>Max Limit:</th>
        <td><%=Html.TextBox("Limit") %></td>
    </tr>
    <tr>
        <th>Online Notify Emails:</th>
        <td><%=Html.TextBox("EmailAddresses") %></td>
    </tr>
    <tr>
        <th>Online Reg Type:</th>
        <td><%=Html.TextBox("RegType")%></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <th>Allow Self Check-In:</th>
        <td><%=Html.CheckBox("CanSelfCheckin")%></td>
    </tr>
    <tr>
        <th>Allow Non-Campus Check-In:</th>
        <td><%=Html.CheckBox("AllowNonCampusCheckin")%></td>
    </tr>
    <tr>
        <th>Number of CheckIn Labels:</th>
        <td><%=Html.TextBox("NumCheckInLabels")%></td>
    </tr>
    <tr>
        <th>Number of Worker CheckIn Labels:</th>
        <td><%=Html.TextBox("NumWorkerCheckInLabels")%></td>
    </tr>
    <tr>
        <th>First Meeting Date:</th>
        <td><%=Html.TextBox("FirstMeetingDate", o.FirstMeetingDate.FormatDate2(), new { @class = "datepicker date" })%></td>
    </tr>
    <tr>
        <th>Last Meeting Date:</th>
        <td><%=Html.TextBox("LastMeetingDate", o.LastMeetingDate.FormatDate2(), new { @class = "datepicker date" })%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th>Attendance Tracking Level:</th>
        <td><%=Html.DropDownList("AttendTrkLevelId", CMSWeb.Models.OrganizationPage.OrganizationModel.AttendTrkLevelList()) %></td>
    </tr>
    <tr>
        <th>Attendance Classification:</th>
        <td><%=Html.DropDownList("AttendClassificationId", CMSWeb.Models.OrganizationPage.OrganizationModel.AttendClassificationList())%></td>
    </tr>
    <tr>
        <th>Entry Point:</th>
        <td><%=Html.DropDownList("EntryPointId", CMSWeb.Models.OrganizationPage.OrganizationModel.EntryPointList()) %></td>
    </tr>
    <tr>
        <th>Security Type:</th>
        <td><%=Html.DropDownList("SecurityTypeId", CMSWeb.Models.OrganizationPage.OrganizationModel.SecurityTypeList())%></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <th>Rollsheet Visitor Weeks:</th>
        <td><%=Html.TextBox("RollSheetVisitorWks")%></td>
    </tr>
    <tr>
        <th>Start Grade/Age:</th>
        <td><%=Html.TextBox("GradeAgeStart")%></td>
    </tr>
    <tr>
        <th>End Grade/Age:</th>
        <td><%=Html.TextBox("GradeAgeEnd")%></td>
    </tr>
    <tr>
        <th>Gender:</th>
        <td><%=Html.DropDownList("GenderId", CMSWeb.Models.OrganizationPage.OrganizationModel.GenderList()) %></td>
    </tr>
    <tr>
        <th>Fee:</th>
        <td><%=Html.TextBox("Fee")%></td>
    </tr>
    <tr>
        <th>Deposit:</th>
        <td><%=Html.TextBox("Deposit")%></td>
    </tr>
    <tr>
        <th>Shirt Fee:</th>
        <td><%=Html.TextBox("ShirtFee")%></td>
    </tr>
    <tr>
        <th>Extra Fee:</th>
        <td><%=Html.TextBox("ExtraFee")%></td>
    </tr>
    <tr>
        <th>Pending Location:</th>
        <td><%=Html.TextBox("PendingLoc")%></td>
    </tr>
    <tr><td></td><td><a href="/Organization/SettingsUpdate/<%=o.OrganizationId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
