<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.OrganizationPage.OrganizationModel>" %>
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
        <th>Organization Phone Number:</th>
        <td><%=Html.TextBox("org.PhoneNumber")%></td>
    </tr>
    <tr>
        <td></td>
    </tr>
    <tr>
        <th>Allow Self Check-In:</th>
        <td><%=Html.CheckBox("org.CanSelfCheckin")%></td>
    </tr>
    <tr>
        <th>Allow Kiosk Registration:</th>
        <td><%=Html.CheckBox("org.AllowKioskRegister") %></td>
    </tr>
    <tr>
        <th>Allow Non-Campus Check-In:</th>
        <td><%=Html.CheckBox("org.AllowNonCampusCheckin")%></td>
    </tr>
    <tr>
        <th>No security label required:</th>
        <td><%=Html.CheckBox("org.NoSecurityLabel") %></td>
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
        <th>Rollsheet Visitor Weeks:</th>
        <td><%=Html.TextBox("org.RollSheetVisitorWks")%></td>
    </tr>
    <tr>
        <th>Days to ignore history on drop:</th>
        <td><%=Html.TextBox("org.DaysToIgnoreHistory")%></td>
    </tr>
    <tr>
        <td></td>
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
        <th>Start Grade/Age:</th>
        <td><%=Html.TextBox("org.GradeAgeStart")%></td>
    </tr>
    <tr>
        <th>End Grade/Age:</th>
        <td><%=Html.TextBox("org.GradeAgeEnd")%></td>
    </tr>
    <tr>
        <th>Start Birthday:</th>
        <td><%=Html.DatePicker("org.BirthDayStart")%></td>
    </tr>
    <tr>
        <th>End Birthday:</th>
        <td><%=Html.DatePicker("org.BirthDayEnd")%></td>
    </tr>
    <tr>
        <th>Gender:</th>
        <td><%=Html.DropDownList("org.GenderId", Model.GenderList())%></td>
    </tr>
    <tr>
        <th>Registration Type:</th>
        <td><%=Html.DropDownList("org.RegistrationTypeId", Model.RegistrationTypes())%></td>
    </tr>
    <tr>
        <th>HTML Shell<br />(blank for default)</th>
        <td><%=Html.TextBox("org.Shell")%></td>
    </tr>
    <tr>
        <th>Fee:</th>
        <td><%=Html.TextBox("org.Fee", Model.org.Fee.ToString2("n2"))%></td>
    </tr>
    <tr>
        <th>Deposit:</th>
        <td><%=Html.TextBox("org.Deposit", Model.org.Deposit.ToString2("n2"))%></td>
    </tr>
    <tr>
        <th>Shirt Fee:</th>
        <td><%=Html.TextBox("org.ShirtFee", Model.org.ShirtFee.ToString2("n2"))%></td>
    </tr>
    <tr>
        <th>Extra Fee:</th>
        <td><%=Html.TextBox("org.ExtraFee", Model.org.ExtraFee.ToString2("n2"))%></td>
    </tr>
    <tr>
        <th>Maximum Fee:</th>
        <td><%=Html.TextBox("org.MaximumFee", Model.org.MaximumFee.ToString2("n2"))%></td>
    </tr>
    <tr>
        <th>Org Member Fees:</th>
        <td><%=Html.TextBox("org.OrgMemberFees")%></td>
    </tr>
    <tr>
        <th>Ask How Many Items:</th>
        <td><%=Html.CheckBox("org.AskTickets")%></td>
    </tr>
    <tr>
        <th>Allow Only One:</th>
        <td><%=Html.CheckBox("org.AllowOnlyOne")%></td>
    </tr>
    <tr>
        <th>Ask Options:</th>
        <td><%=Html.TextBox("org.AskOptions")%> 
        <%= Html.ValidationMessage("askoptions")%></td>
    </tr>
    <tr>
        <th>Options Label:</th>
        <td><%=Html.TextBox("org.OptionsLabel")%></td>
    </tr>
    <tr>
        <th>Extra Options:</th>
        <td><%=Html.TextBox("org.ExtraOptions")%> 
        <%= Html.ValidationMessage("extraoptions")%></td>
    </tr>
    <tr>
        <th>Extra Options Label:</th>
        <td><%=Html.TextBox("org.ExtraOptionsLabel")%></td>
    </tr>
    <tr>
        <th>Grade Options:</th>
        <td><%=Html.TextBox("org.GradeOptions")%> 
        <%= Html.ValidationMessage("gradeoptions")%></td>
    </tr>
    <tr>
        <th>Age Fees:</th>
        <td><%=Html.TextBox("org.AgeFee")%> 
        <%= Html.ValidationMessage("agefee")%></td>
    </tr>
    <tr>
        <th>Age Groups:</th>
        <td><%=Html.TextBox("org.AgeGroups")%> 
        <%= Html.ValidationMessage("agegroups")%></td>
    </tr>
    <tr>
        <th>Last Day Before Extra:</th>
        <td><%=Html.DatePicker("org.LastDayBeforeExtra")%></td>
    </tr>
    <tr>
        <th>Shirt Sizes<br />(blank for default)</th>
        <td><%=Html.TextBox("org.ShirtSizes")%> 
        <%= Html.ValidationMessage("shirtsizes")%></td>
    </tr>
    <tr>
        <th>Yes No Questions:</th>
        <td><%=Html.TextBox("org.YesNoQuestions")%> 
        <%= Html.ValidationMessage("yesnoquestions")%></td>
    </tr>
    <tr>
        <th>Extra Questions:</th>
        <td><%=Html.TextBox("org.ExtraQuestions")%></td>
    </tr>
    <tr>
        <th>Menu Items:</th>
        <td><%=Html.TextArea("org.MenuItems", new { style = "height:200px;width:200px" })%></td>
    </tr>
    <tr>
        <th>Validate Member in OrgIds:</th>
        <td><%=Html.TextBox("org.ValidateOrgs")%></td>
    </tr>
    <tr>
        <th>Link Groups from Orgs:</th>
        <td><%=Html.TextBox("org.LinkGroupsFromOrgs")%></td>
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
        <th>Ask For Grade:</th>
        <td><%=Html.CheckBox("org.AskGrade")%></td>
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
        <th>Request Label:</th>
        <td><%=Html.TextBox("org.RequestLabel")%></td>
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
    <tr>
        <th>Members Only:</th>
        <td><%=Html.CheckBox("org.MemberOnly")%></td>
    </tr>
    <% if (Page.User.IsInRole("Admin"))
       { %>
    <tr>
        <th>Give OrgMemberOnly Access:</th>
        <td><%=Html.CheckBox("org.GiveOrgMembAccess")%></td>
    </tr>
    <tr>
        <th>Group to Join:</th>
        <td><%=Html.TextBox("org.GroupToJoin")%></td>
    </tr>
    <% } %>

    <tr><td></td><td><a href="/Organization/SettingsUpdate/<%=Model.OrganizationId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
