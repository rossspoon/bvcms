<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OrganizationPage.OrganizationModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>

    <script src="/Content/js/jquery.validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/Pager.js" type="text/javascript"></script>
    <script src="/Scripts/Organization.js" type="text/javascript"></script>

    <% CmsData.Organization o = Model.org; %>
    <div class="PersonHead">
        <%=o.OrganizationName %>,
        <%=o.LeaderName %>
    </div>
    <table>
        <tr>
            <td>
                <table class="Design2">
                    <tr>
                        <th>
                            Name:
                        </th>
                        <td>
                            <span>
                                <%=o.OrganizationName %></span>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Main Division:
                        </th>
                        <td>
                            <span title="<%=Model.DivisionsTitle() %>">
                                <%=o.Division.Name %></span>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Location:
                        </th>
                        <td>
                            <%=o.Location %>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table class="Design2">
                    <tr>
                        <th>
                            Campus:
                        </th>
                        <td>
                            <%=Model.Campus %>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Status:
                        </th>
                        <td>
                            <%=Model.OrgStatus%>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Leader Type:
                        </th>
                        <td>
                            <%=Model.LeaderType%>
                        </td>
                    </tr>
                </table>
            </td>
            <%--            <td id="ManageGroups" runat="server">
                <table>
                    <tr>
                        <th colspan="2">
                            Manage Groups
                        </th>
                    </tr>
                    <tr>
                        <th align="right">
                            Change Active Group:
                        </th>
                        <td>
                            <asp:DropDownList ID="Groups" runat="server"
                            AutoPostBack="True" OnSelectedIndexChanged="Groups_SelectedIndexChanged" 
                                DataSourceID="GroupData" DataTextField="Name" DataValueField="Id" 
                                ondatabound="Groups_DataBound">
                            </asp:DropDownList>
                            &nbsp;
                            <asp:LinkButton ID="DeleteGroup" runat="server" OnClick="DeleteGroup_Click">Delete Group</asp:LinkButton> |
                            <asp:HyperLink ID="UpdateGroup" runat="server" CssClass="thickbox3" ToolTip='Update Group Members'>Group Members</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <th align="right">
                            New Group Name:
                        </th>
                        <td>
                            <asp:TextBox ID="GroupName" runat="server" Width="225px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="GroupNameValidator" runat="server" ErrorMessage="GroupName is Required"
                                ControlToValidate="GroupName" ValidationGroup="GroupName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th align="right">
                            &nbsp;
                        </th>
                        <td>
                            <asp:LinkButton ID="MakeNewGroup" runat="server" 
                                OnClick="MakeNewGroup_Click" 
                                ValidationGroup="GroupName">Make New Group</asp:LinkButton>&nbsp;|
                            <asp:LinkButton ID="RenameGroup" runat="server" 
                                OnClick="RenameGroup_Click" 
                                ValidationGroup="GroupName">Rename Group</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
--%>
        </tr>
    </table>
    <div style="margin-bottom: 20px">
        <% if (Page.User.IsInRole("Admin"))
           { %>
        <a id="deleteorg" href="/Organization/Delete/<%=o.OrganizationId %>">
            <img border="0" src="/images/delete.gif" align="middle" /></a> |
        <% } %>
        <a id="RecentAttendRpt" href="#" target="_blank">Recent Attendance Report</a> |
        <a id="AttendanceRpt" href="#">Attendance Percentages</a> | <a id="VolunteerCalendar"
            href="#">Volunteer Calendar</a> | <a id="CloneOrg1" href="#" confirm="This will make a copy of the org. Are you sure?">
                Copy this Organization</a> | <a href="/Organization.aspx?id=<%=o.OrganizationId %>">
                    Use Old Organization Page</a>
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
            <%--            <uc1:ExportToolBar ID="MemberToolbar" runat="server" />
            <asp:DropDownList ID="GroupFilter" runat="server" DataSourceID="GroupData2" 
                DataTextField="Name" DataValueField="Id"
                AutoPostBack="True" OnSelectedIndexChanged="Group_SelectedIndexChanged" 
                ondatabound="GroupFilter_DataBound">
            </asp:DropDownList>--%>
            <div class="members">
            <% Html.RenderPartial("MemberGrid", Model.MemberModel); %>
            </div>
        </div>
        <div id="Inactive-tab" class="ui-tabs-panel ui-tabs-hide">
            <%--            <uc1:ExportToolBar ID="InactiveToolbar" runat="server" />
            <uc2:MemberGrid ID="Inactives" runat="server" Select="Inactive" />
--%>
        </div>
        <div id="Pending-tab" class="ui-tabs-panel ui-tabs-hide">
            <%--            <uc1:ExportToolBar ID="PendingToolbar" runat="server" />
            <uc2:MemberGrid ID="Pendings" runat="server" Select="Pending" />
--%>
        </div>
        <div id="Priors-tab" class="ui-tabs-panel ui-tabs-hide">
            <%--            <uc1:ExportToolBar ID="PriorsToolbar" runat="server" />
            <uc2:MemberGrid ID="Priors" runat="server" Select="Previous" />
--%>
        </div>
        <div id="Visitors-tab" class="ui-tabs-panel ui-tabs-hide">
            <%--            <uc1:ExportToolBar ID="VisitorToolbar" runat="server" />
            &nbsp;Visitor Lookback Days:
            <asp:TextBox ID="VisitLookbackDays" runat="server" Width="34px"></asp:TextBox>
            <asp:LinkButton ID="SetDays" runat="server" OnClick="SetDays_Click">Set</asp:LinkButton>
            <div style="clear: both">
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc4:VisitorGrid ID="Visitors" runat="server" DataSourceID="VisitorData" />
                </ContentTemplate>
            </asp:UpdatePanel>
--%>
        </div>
        <div id="Schedule-tab" class="ui-tabs-panel ui-tabs-hide">
            <form id="settingsForm" class="DisplayEdit" action="">
            <% Html.RenderPartial("Settings", Model); %>
            </form>
        </div>
        <div id="Meetings-tab" class="ui-tabs-panel ui-tabs-hide">
            <%--            <asp:UpdatePanel ID="MeetingsPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="NewMeetingLink" runat="server" OnClientClick="OpenNewMeeting();return false;"
                        Text="Create New Meeting" />
                    <asp:CheckBox ID="Future" runat="server" AutoPostBack="true" OnCheckedChanged="ShowFuture_Click" Text="Future" />
                    <br />
                    <uc3:MeetingGrid ID="MeetingGrid1" runat="server" DataSourceID="MeetingData" Visible="false" />
                    <div style="visibility: hidden">
                        <asp:Button ID="ShowMeetings" runat="server" OnClick="ShowMeetings_Click" Text="Button" /></div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ShowMeetings" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
--%>
        </div>
    </div>
    <%--            <asp:Button ID="TriggerRollsheetPopup2" Style="display: none" runat="server"></asp:Button>
           <asp:Panel ID="RollsheetInputPanel" runat="server" CssClass="modalDiv" Style="display: none">
                <table>
                    <tr>
                        <th colspan="2" style="font-size: larger; font-weight: bold">
                            Please select a meeting date and time:
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Meeting Date:
                        </th>
                        <td>
                            <asp:TextBox ID="MeetingDate" runat="server"></asp:TextBox>
                            <cc2:CalendarExtender ID="MeetingDateExtender" runat="server" TargetControlID="MeetingDate">
                            </cc2:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Meeting Time:
                        </th>
                        <td>
                            <asp:TextBox ID="MeetingTime" runat="server" ToolTip="Time in Format hh:mm am or pm"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="footer">
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" Text="Create"
                                OnClientClick="ViewRollsheet2();" ValidationGroup="NewMeetingValidatorGroup" />
                            <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false" Text="By Group"
                                OnClientClick="ViewRollsheet2('grouped');" ValidationGroup="NewMeetingValidatorGroup" />
                            <asp:LinkButton ID="RollsheetCancel" runat="server" CausesValidation="false" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="RollsheetPopup" runat="server"
                TargetControlID="TriggerRollsheetPopup2" PopupControlID="RollsheetInputPanel"
                CancelControlID="RollsheetCancel" DropShadow="true" BackgroundCssClass="modalBackground">
            </cc2:ModalPopupExtender>
            <asp:Button ID="TriggerNewMeetingPopup2" Style="display: none" runat="server"></asp:Button>
            <asp:Panel ID="NewMeetingInputPanel" runat="server" CssClass="modalDiv" Style="display: none">
                <table>
                    <tr>
                        <th colspan="2" style="font-size: larger; font-weight: bold">
                            Please select a meeting date and time:
                        </th>
                    </tr>
                    <tr>
                        <th>
                            Meeting Date:
                        </th>
                        <td>
                            <asp:TextBox ID="NewMeetingDate" runat="server"></asp:TextBox>
                            <cc2:CalendarExtender
                                    ID="NewMeetingDateExtender" runat="server" TargetControlID="NewMeetingDate">
                            </cc2:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Meeting Time:
                        </th>
                        <td>
                            <asp:TextBox ID="NewMeetingTime" runat="server" ToolTip="Time in Format hh:mm am or pm"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                    ID="NewMeetingTimeValidator" runat="server" ErrorMessage="Invalid time: Use format hh:mm am or pm."
                                    ControlToValidate="NewMeetingTime" ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$"
                                    SetFocusOnError="True" ValidationGroup="NewMeetingValidatorGroup"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td class="footer">
                            <asp:LinkButton ID="NewMeeting" runat="server" CausesValidation="false" Text="New Meeting"
                                OnClick="CreateMeeting" ValidationGroup="NewMeetingValidatorGroup" />
                            <asp:LinkButton ID="NewGroupMeeting" runat="server" CausesValidation="false" Text="New Group Meeting"
                                OnClick="CreateGroupMeeting" ValidationGroup="NewMeetingValidatorGroup" />
                            <asp:LinkButton ID="NewMeetingCancel" runat="server" CausesValidation="false" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="NewMeetingPopup"
                runat="server" TargetControlID="TriggerNewMeetingPopup2" PopupControlID="NewMeetingInputPanel"
                CancelControlID="NewMeetingCancel" DropShadow="true" BackgroundCssClass="modalBackground">
            </cc2:ModalPopupExtender>
--%>
</asp:Content>
