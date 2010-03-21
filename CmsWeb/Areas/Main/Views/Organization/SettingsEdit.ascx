<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>
<a class="displayedit" href="/Organization/Settings/<%=Model.OrganizationId %>">cancel</a>
<table class="Design2">
    <tr>
        <th>Schedule:</th>
        <td><%=Html.DropDownList("org.SchedDay", Model.DaysOfWeek()) %>
         <%=Html.TextBox("org.SchedTime", Model.org.SchedTime.ToString2("h:mm tt")) %></td>
    </tr>
    <tr>
        <th>Allow Attendance Overlap:</th>
        <td><%=Html.CheckBox("org.AllowAttendOverlap")%></td>
    </tr>
    <tr>
        <th>Class Filled:</th>
        <td><%=Html.CheckBox("org.ClassFilled") %></td>
    </tr>
    <tr>
        <th>Online Catalog Sort:</th>
        <td><%=Html.TextBox("org.OnLineCatalogSort")%></td>
    </tr>
    <tr>
        <th>Max Limit:</th>
        <td><%=Html.TextBox("org.Limit")%></td>
    </tr>
    <tr>
        <th>Online Notify Emails:</th>
        <td><%=Html.TextBox("org.EmailAddresses")%></td>
    </tr>
    <tr>
        <th>Online Reg Type:</th>
        <td><%=Html.TextBox("org.RegType")%></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <th>Allow Self Check-In:</th>
        <td><%=Html.CheckBox("org.CanSelfCheckin")%></td>
    </tr>
    <tr>
        <th>Allow Non-Campus Check-In:</th>
        <td><%=Html.CheckBox("org.AllowNonCampusCheckin")%></td>
    </tr>
    <tr>
        <th>Number of CheckIn Labels:</th>
        <td><%=Html.TextBox("org.NumCheckInLabels")%></td>
    </tr>
    <tr>
        <th>Number of Worker CheckIn Labels:</th>
        <td><%=Html.TextBox("org.NumWorkerCheckInLabels")%></td>
    </tr>
    <tr>
        <th>First Meeting Date:</th>
        <td><%=Html.DatePicker("org.FirstMeetingDate")%></td>
    </tr>
    <tr>
        <th>Last Meeting Date:</th>
        <td><%=Html.DatePicker("org.LastMeetingDate")%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th>Attendance Tracking Level:</th>
        <td><%=Html.DropDownList("org.AttendTrkLevelId", Model.AttendTrkLevelList())%></td>
    </tr>
    <tr>
        <th>Attendance Classification:</th>
        <td><%=Html.DropDownList("org.AttendClassificationId", Model.AttendClassificationList())%></td>
    </tr>
    <tr>
        <th>Entry Point:</th>
        <td><%=Html.DropDownList("org.EntryPointId", Model.EntryPointList())%></td>
    </tr>
    <tr>
        <th>Security Type:</th>
        <td><%=Html.DropDownList("org.SecurityTypeId", Model.SecurityTypeList())%></td>
    </tr>
    <tr>
        <th>Pending Location:</th>
        <td><%=Html.TextBox("org.PendingLoc")%></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <th>Rollsheet Visitor Weeks:</th>
        <td><%=Html.TextBox("org.RollSheetVisitorWks")%></td>
    </tr>
    <tr>
        <th>Start Grade/Age:</th>
        <td><%=Html.TextBox("org.GradeAgeStart")%></td>
    </tr>
    <tr>
        <th>End Grade/Age:</th>
        <td><%=Html.TextBox("org.GradeAgeEnd")%></td>
    </tr>
    <tr>
        <th>Gender:</th>
        <td><%=Html.DropDownList("org.GenderId", Model.GenderList())%></td>
    </tr>
    <tr>
        <th>Fee:</th>
        <td><%=Html.TextBox("org.Fee")%></td>
    </tr>
    <tr>
        <th>Deposit:</th>
        <td><%=Html.TextBox("org.Deposit")%></td>
    </tr>
    <tr>
        <th>Shirt Fee:</th>
        <td><%=Html.TextBox("org.ShirtFee")%></td>
    </tr>
    <tr>
        <th>Extra Fee:</th>
        <td><%=Html.TextBox("org.ExtraFee")%></td>
    </tr>
    <tr>
        <th>Last Day Before Extra:</th>
        <td><%=Html.DatePicker("org.LastDayBeforeExtra")%></td>
    </tr>
    <tr>
        <th>Ask About Allergies:</th>
        <td><%=Html.CheckBox("org.AskAllergies")%></td>
    </tr>
    <tr>
        <th>Ask About Tylenol, Etc:</th>
        <td><%=Html.CheckBox("org.AskTylenolEtc")%></td>
    </tr>
    <tr>
        <th>Ask About Shirt Size:</th>
        <td><%=Html.CheckBox("org.AskShirtSize")%></td>
    </tr>
    <tr>
        <th>Ask About Request:</th>
        <td><%=Html.CheckBox("org.AskRequest")%></td>
    </tr>
    <tr>
        <th>Ask For Parents:</th>
        <td><%=Html.CheckBox("org.AskParents")%></td>
    </tr>
    <tr>
        <th>Ask For Emergency Contact:</th>
        <td><%=Html.CheckBox("org.AskEmContact")%></td>
    </tr>
    <tr>
        <th>Ask About Medical:</th>
        <td><%=Html.CheckBox("org.AskMedical")%></td>
    </tr>
    <tr>
        <th>Ask For Doctor:</th>
        <td><%=Html.CheckBox("org.AskDoctor")%></td>
    </tr>
    <tr>
        <th>Ask For Insurance:</th>
        <td><%=Html.CheckBox("org.AskInsurance")%></td>
    </tr>
    <tr>
        <th>Allow Last Year Shirt:</th>
        <td><%=Html.CheckBox("org.AllowLastYearShirt")%></td>
    </tr>
    <tr>
        <th>Ask About Coaching:</th>
        <td><%=Html.CheckBox("org.AskCoaching")%></td>
    </tr>
    <tr>
        <th>Ask About Church:</th>
        <td><%=Html.CheckBox("org.AskChurch")%></td>
    </tr>

    <tr><td></td><td><a href="/Organization/SettingsUpdate/<%=Model.OrganizationId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
