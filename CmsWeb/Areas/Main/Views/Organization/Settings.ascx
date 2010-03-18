<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>
<% CmsData.Organization o = Model.org;
   if (Page.User.IsInRole("Edit"))
   { %>
<a class="displayedit" href="/Organization/SettingsEdit/<%=o.OrganizationId %>">Edit</a>
<% } %>
<div style="float: left">
    <table class="Design2">
        <tr>
            <th>Schedule:</th>
            <td><%=Model.SchedDay %> <%=Model.SchedTime %></td>
        </tr>
        <tr>
            <th>Allow Attendance Overlap:</th>
            <td><input type="checkbox" <%=Model.AttendanceOverlap %> disabled="disabled" /></td>
        </tr>
        <tr>
            <th>Class Filled:</th>
            <td><input type="checkbox" <%=Model.ClassFilled %> disabled="disabled" /></td>
        </tr>
        <tr>
            <th>Online Catalog Sort:</th>
            <td><%=o.OnLineCatalogSort %></td>
        </tr>
        <tr>
            <th>Max Limit:</th>
            <td><%=o.Limit %></td>
        </tr>
        <tr>
            <th>Online Notify Emails:</th>
            <td><%=o.EmailAddresses %></td>
        </tr>
        <tr>
            <th>Online Reg Type:</th>
            <td><%=o.RegType %></td>
        </tr>
        <tr>
            <th>Edit Online messages</th>
            <td><a id="emailmessagelink" href="/Display/OrgContent/<%=o.OrganizationId %>?what=message">
                   registration notification</a><br />
                <a id="instructionslink" href="/Display/OrgContent/<%=o.OrganizationId %>?what=instructions">
                    registration instructions</a>
            </td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <th>Allow Self Check-In:</th>
            <td><input type="checkbox" <%=Model.CanSelfCheckin %> disabled="disabled" /></td>
        </tr>
        <tr>
            <th>Allow Non-Campus Check-In:</th>
            <td><input type="checkbox" <%=Model.AllowNonCampusCheckin %> disabled="disabled" /></td>
        </tr>
        <tr>
            <th>Number of CheckIn Labels:</th>
            <td><%=o.NumCheckInLabels %></td>
        </tr>
        <tr>
            <th>Number of Worker CheckIn Labels:</th>
            <td><%=o.NumWorkerCheckInLabels %></td>
        </tr>
        <tr>
            <th>First Meeting Date:</th>
            <td><%=o.FirstMeetingDate.FormatDate2() %></td>
        </tr>
        <tr>
            <th>Last Meeting Date:</th>
            <td><%=o.LastMeetingDate.FormatDate2() %></td>
        </tr>
    </table>
</div>
<div style="float: left">
    <table class="Design2">
        <tr>
            <th>Attendance Tracking Level:</th>
            <td><%=Model.AttendTrkLevel %></td>
        </tr>
        <tr>
            <th>Attendance Classification:</th>
            <td><%=Model.AttendClassification %></td>
        </tr>
        <tr>
            <th>Entry Point:</th>
            <td><%=Model.EntryPoint %></td>
        </tr>
        <tr>
            <th>Security Type:</th>
            <td><%=Model.SecurityType %></td>
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <th>Rollsheet Visitor Weeks:</th>
            <td><%=o.RollSheetVisitorWks %></td>
        </tr>
        <tr>
            <th>Start Grade/Age:</th>
            <td><%=o.GradeAgeStart %></td>
        </tr>
        <tr>
            <th>End Grade/Age:</th>
            <td><%=o.GradeAgeEnd %></td>
        </tr>
        <tr>
            <th>Gender:</th>
            <td><%=Model.Gender %></td>
        </tr>
        <tr>
            <th>Fee:</th>
            <td><%=(o.Fee ?? 0).ToString("n2") %></td>
        </tr>
        <tr>
            <th>Deposit:</th>
            <td><%=(o.Deposit ?? 0).ToString("n2") %></td>
        </tr>
        <tr>
            <th>Shirt Fee:</th>
            <td><%=(o.ShirtFee ?? 0).ToString("n2") %></td>
        </tr>
        <tr>
            <th>Extra Fee:</th>
            <td><%=(o.ExtraFee ?? 0).ToString("n2") %></td>
        </tr>
        <tr>
            <th>Pending Location:</th>
            <td><%=o.PendingLoc %></td>
        </tr>
    </table>
</div>
<div style="clear: both">
</div>
