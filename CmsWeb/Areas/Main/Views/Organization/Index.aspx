<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="/Content/jquery.multiSelect.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>

    <script src="/Content/js/jquery.validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/Pager.js" type="text/javascript"></script>
    <script src="/Content/js/jquery.multiSelect.js" type="text/javascript"></script>
    <script src="/Scripts/Organization.js" type="text/javascript"></script>

    <% CmsData.Organization o = Model.org; %>
    <%=Html.Hidden("OrganizationId") %>
    <div class="PersonHead">
        <%=o.OrganizationName %>,
        <%=o.LeaderName %>
    </div>
    <form action="/Organization/OrgInfoUpdate/<%=Model.OrganizationId %>" class="DisplayEdit" method="post">
        <% Html.RenderPartial("OrgInfo", Model); %>
    </form>
    <div style="margin-bottom: 20px">
<% if (Page.User.IsInRole("Admin"))
   { %>
<a id="deleteorg" href="/Organization/Delete/<%=o.OrganizationId %>">
            <img border="0" src="/images/delete.gif" align="middle" /></a> |
<% } %>
        <a id="RecentAttendRpt" href="/Report/PastAttendeeRpt.aspx?id=<%=o.OrganizationId %>" target="_blank">Recent Attendance Report</a> |
        <a id="AttendanceRpt" href="/Reports/Attendance/<%=o.OrganizationId %>">Attendance Percentages</a> | 
        <a id="VolunteerCalendar" href="/Volunteers/Calendar/<%=o.OrganizationId %>">Volunteer Calendar</a> | 
        <a class="CreateAndGo" href="/Organization/Clone/<%=o.OrganizationId %>" confirm="This will make a copy of the org. Are you sure?"> Copy this Organization</a>
<% if (Page.User.IsInRole("Admin"))
   { %>
        | <a href="/Organization/CopySettings/<%=o.OrganizationId %>">Copy Settings</a>
<% } %>
    </div>
    <div id="main-tab" class="ui-tabs">
        <ul class="ui-tabs-nav">
            <li><a href="#Members-tab"><span>Members</span></a></li>
            <li><a id="inactive-link" href="#Inactive-tab"><span>Inactive</span></a></li>
            <li><a id="pending-link" href="#Pending-tab"><span>Pending</span></a></li>
            <li><a id="priors-link" href="#Priors-tab"><span>Previous</span></a></li>
            <li><a id="visitors-link" href="#Visitors-tab"><span>Visitors</span></a></li>
            <li><a href="#Schedule-tab"><span>Settings</span></a></li>
            <li><a id="meetings-link" href="#Meetings-tab"><span>Meetings</span></a></li>
        </ul>
        <div id="Members-tab" class="ui-tabs-panel ui-tabs-hide">
            <form action="/Organization/CurrMemberGrid/<%=Model.OrganizationId %>" method="post">
            <% Html.RenderPartial("CurrMemberGrid", Model.MemberModel); %>
            </form>
        </div>
        <div id="Inactive-tab" class="ui-tabs-panel ui-tabs-hide">
            <form action="/Organization/InactiveMemberGrid/<%=Model.OrganizationId %>" method="post">
            </form>
        </div>
        <div id="Pending-tab" class="ui-tabs-panel ui-tabs-hide">
            <form action="/Organization/PendingMemberGrid/<%=Model.OrganizationId %>" method="post">
            </form>
        </div>
        <div id="Priors-tab" class="ui-tabs-panel ui-tabs-hide">
            <form action="/Organization/PrevMemberGrid/<%=Model.OrganizationId %>" method="post">
            </form>
        </div>
        <div id="Visitors-tab" class="ui-tabs-panel ui-tabs-hide">
            <form action="/Organization/VisitorGrid/<%=Model.OrganizationId %>" method="post">
            </form>
        </div>
        <div id="Schedule-tab" class="ui-tabs-panel ui-tabs-hide">
            <form id="settingsForm" class="DisplayEdit" action="">
            <% Html.RenderPartial("Settings", Model); %>
            </form>
        </div>
        <div id="Meetings-tab" class="ui-tabs-panel ui-tabs-hide">
            <form action="/Organization/MeetingGrid/<%=Model.OrganizationId %>" method="post">
            <%=Html.Hidden("future", false) %>
            </form>
        </div>
    </div>
    <div id="NewMeetingDialog" class="modalDiv" style="display:none">
    <table>
        <tr>
            <th colspan="2" style="font-size: larger; font-weight: bold">
                Please select a meeting date and time:
            </th>
        </tr>
        <tr>
            <th>Meeting Date:</th>
            <td><%=Html.DatePicker("NewMeetingDate") %></td>
        </tr>
        <tr>
            <th>Meeting Time:</th>
            <td><%=Html.TextBox("NewMeetingTime", Model.NewMeetingTime, new { title = "Time in Format hh:mm am or pm" })%></td>
        </tr>
        <tr>
            <th id="grouplabel"></th>
            <td><%=Html.CheckBox("group") %></td>
        </tr>
    </table>
    </div>
    <div id="AddFromTag" class="modalDiv" style="display:none">
        <%=Html.DropDownList("addfromtagid", Model.Tags()) %>
    </div>
</asp:Content>
