<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Organization.aspx.cs"
    Inherits="CMSWeb.Organization" Title="Organization" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="UserControls/MemberGrid.ascx" TagName="MemberGrid" TagPrefix="uc2" %>
<%@ Register Src="UserControls/MeetingGrid.ascx" TagName="MeetingGrid" TagPrefix="uc3" %>
<%@ Register Src="UserControls/ExportToolBar.ascx" TagName="ExportToolBar" TagPrefix="uc1" %>
<%@ Register Src="UserControls/VisitorGrid.ascx" TagName="VisitorGrid" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        $(function() {
            if ('<%=EditUpdateButton1.Editing?"true":"false"%>' == 'true') {
                $('a.thickbox2').unbind("click")
                $('a.thickbox3').unbind("click")
            }
            else {
                tb_init('a.thickbox2');
                tb_init('a.thickbox3');
            }
            imgLoader = new Image();
            imgLoader.src = tb_pathToImage;

            var $maintabs = $("#main-tab").tabs();
            var t = $.cookie('maintab3');
            if (t) {
                $maintabs.tabs('select', parseInt(t));
                if (t == "4")
                    $get('<%=ShowMeetings.ClientID%>').click();
            }
            $("#main-tab > ul > li > a").click(function() {
                var selected = $maintabs.data('selected.tabs');
                $.cookie('maintab3', selected);
            });
            $("#meetings-link").click(function() {
                if (!$get('<%=MeetingGrid1.GridClientID%>'))
                    $get('<%=ShowMeetings.ClientID%>').click();
            });
        });
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                if ('<%=EditUpdateButton1.Editing?"true":"false"%>' == 'true')
                    $('div.members a.thickbox2').unbind("click")
                else
                    tb_init('a.thickbox2');
            }
        }

        function OpenRollsheet() {
            $find('<%=RollsheetPopup.ClientID%>').show();
        }

        function ViewRollsheet2() {
            var re = /^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$/;
            var d = $get('<%=MeetingDate.ClientID %>').value;
            var t = $get('<%=MeetingTime.ClientID %>').value;
            if (!re.test(t)) {
                alert('enter valid time'); 
                return;
            }
            if (!d) {
                alert('enter valid date');
                return;
            }
            var args = "?org=curr&dt=" + d + " " + t;
            var newWindowUrl = "Report/Rollsheet.aspx" + args
            window.open(newWindowUrl);
        }

        function OpenNewMeeting() {
            $find('<%=NewMeetingPopup.ClientID%>').show();
        }
    </script>

    <table class="PersonHead" border="0">
        <tr>
            <td>
                <cc1:DisplayLabel ID="OrganizationNameInHeader" runat="server" BindingMember="OrganizationName"
                    BindingSource="organization"></cc1:DisplayLabel>
            </td>
            <td>
                <cc1:DisplayLabel ID="LeaderNameInHeader" runat="server" BindingMember="LeaderName"
                    BindingSource="organization" CssClass="leadername"></cc1:DisplayLabel>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <table class="Design2">
                    <tr>
                        <th>
                            Name:
                        </th>
                        <td>
                            <cc1:DisplayOrEditText ID="OrganizationName" runat="server" BindingSource="organization"
                                Width="250px">
                            &nbsp;
                            </cc1:DisplayOrEditText>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Main Division:
                        </th>
                        <td>
                            <cc1:DisplayOrEditDropDown ID="DivisionId" runat="server" BindingMode="TwoWay" BindingSource="organization"
                                Width="250px" DataSourceID="ODS_Divisions" DataTextField="Value" DataValueField="Id">
                            </cc1:DisplayOrEditDropDown>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Divisions:
                        </th>
                        <td>
                            <cc1:DisplayOrEditDropCheck ID="TagString" runat="server" BindingMode="TwoWay" BindingSource="organization"
                                BindingMember="TagString" Width="250px">
                            </cc1:DisplayOrEditDropCheck>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Leader:
                        </th>
                        <td>
                            <cc1:DisplayOrEditText ID="LeaderName" runat="server" BindingSource="organization"
                                BindingMode="OneWay" Width="250px">
                            &nbsp;
                            </cc1:DisplayOrEditText>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Location:
                        </th>
                        <td>
                            <cc1:DisplayOrEditText ID="Location" runat="server" BindingSource="organization">
                            &nbsp;
                            </cc1:DisplayOrEditText>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table class="Design2">
                    <tr>
                        <th>
                            Status:
                        </th>
                        <td>
                            <cc1:DisplayOrEditDropDown ID="OrganizationStatusId" runat="server" BindingMode="TwoWay"
                                BindingSource="organization" DataTextField="Value" DataValueField="Id" Width="150px"
                                BindingMember="OrganizationStatusId" DataSourceID="ODS_OrganizationStatusId"
                                MakeDefault0="False">
                            </cc1:DisplayOrEditDropDown>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Leader Type:
                        </th>
                        <td>
                            <cc1:DisplayOrEditDropDown ID="LeaderMemberTypeId" runat="server" BindingMode="TwoWay"
                                BindingSource="organization" DataTextField="Value" MakeDefault0="True" DataValueField="Id"
                                Width="150px" BindingMember="LeaderMemberTypeId" AppendDataBoundItems="true"
                                DataSourceID="ODS_LeaderMemberTypeId">
                                <asp:ListItem Value="0">(not specified)</asp:ListItem>
                            </cc1:DisplayOrEditDropDown>
                        </td>
                    </tr>
                </table>
            </td>
            <td id="ManageGroups" runat="server">
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
        </tr>
    </table>
    <table>
        <tr>
            <td style="margin-bottom: 20px">
                <asp:HyperLink ID="RecentAttendRpt" runat="server" Target="_blank">Recent Attendance Report</asp:HyperLink>&nbsp
                | &nbsp
                <cc1:LinkButtonConfirm ID="CloneOrg1" OnClick="CloneOrg_Click" Confirm="This will make a copy of the org. Are you sure?"
                    runat="server">Copy this Organization</cc1:LinkButtonConfirm>
            </td>
        </tr>
    </table>
    <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" OnClick="EditUpdateButton1_Click" />
    <asp:ImageButton ID="DeleteOrg" runat="server" ImageUrl="~/images/delete.gif"
        OnClientClick="return confirm('Are you sure you want to delete?')" OnClick="DeleteOrg_Click" />
    <asp:CustomValidator ID="ValidateDelete" runat="server" Display="Dynamic" ErrorMessage="Too many relationships remain"></asp:CustomValidator>
    <div id="main-tab" class="ui-tabs">
        <ul>
            <li><a href="#Members-tab"><span>Members</span></a></li>
            <li><a href="#Inactive-tab"><span>Inactive</span></a></li>
            <li><a href="#Pending-tab"><span>Pending</span></a></li>
            <li><a href="#Priors-tab"><span>Previous</span></a></li>
            <li><a href="#Visitors-tab"><span>Visitors</span></a></li>
            <li><a href="#Schedule-tab"><span>Settings</span></a></li>
            <li><a id="meetings-link" href="#Meetings-tab"><span>Meetings</span></a></li>
        </ul>
        <div id="Members-tab" class="ui-tabs-hide">
            <uc1:ExportToolBar ID="MemberToolbar" runat="server" />
            <asp:DropDownList ID="GroupFilter" runat="server" DataSourceID="GroupData2" 
                DataTextField="Name" DataValueField="Id"
                AutoPostBack="True" OnSelectedIndexChanged="Group_SelectedIndexChanged" 
                ondatabound="GroupFilter_DataBound">
            </asp:DropDownList>
            <uc2:MemberGrid ID="Members" runat="server" Select="Active" />
        </div>
        <div id="Inactive-tab" class="ui-tabs-hide">
            <uc1:ExportToolBar ID="InactiveToolbar" runat="server" />
            <uc2:MemberGrid ID="Inactives" runat="server" Select="Inactive" />
        </div>
        <div id="Pending-tab" class="ui-tabs-hide">
            <uc1:ExportToolBar ID="PendingToolbar" runat="server" />
            <uc2:MemberGrid ID="Pendings" runat="server" Select="Pending" />
        </div>
        <div id="Priors-tab" class="ui-tabs-hide">
            <uc1:ExportToolBar ID="PriorsToolbar" runat="server" />
            <uc2:MemberGrid ID="Priors" runat="server" Select="Previous" />
        </div>
        <div id="Visitors-tab" class="ui-tabs-hide">
            <uc1:ExportToolBar ID="VisitorToolbar" runat="server" />
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
        </div>
        <div id="Schedule-tab" class="ui-tabs-hide">
            <table class="Design2">
                <tr>
                    <th>
                        Schedule:
                    </th>
                    <td>
                        <cc1:DisplayOrEditDropDown ID="ScheduleId" runat="server" BindingMode="TwoWay" BindingSource="organization"
                            DataTextField="Value" DataValueField="Id" MakeDefault0="True" Width="350px" BindingMember="ScheduleId"
                            DataSourceID="ODS_ScheduleId" AppendDataBoundItems="True">
                            <asp:ListItem Value="0">(not specified)</asp:ListItem>
                        </cc1:DisplayOrEditDropDown>
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc1:DisplayOrEditCheckbox ID="AllowAttendOverlap" runat="server" BindingSource="organization"
                            Text="Allow Attendance Overlap" TextIfChecked="Attendance Overlap Allowed" BindingMode="TwoWay"
                            TextIfNotChecked="Attendance Overlap Not Allowed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &#160;&nbsp
                    </td>
                </tr>
                <tr>
                    <th>
                        First Meeting Date:
                    </th>
                    <td>
                        <cc1:DisplayOrEditDate ID="FirstMeetingDate" runat="server" BindingSource="organization"
                            BindingMode="TwoWay" BindingMember="FirstMeetingDate"></cc1:DisplayOrEditDate>
                        <cc2:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="FirstMeetingDate"
                            Enabled="True">
                        </cc2:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <th>
                        Last Meeting Date:
                    </th>
                    <td>
                        <cc1:DisplayOrEditDate ID="LastMeetingDate" runat="server" BindingSource="organization"
                            BindingMode="TwoWay" BindingMember="LastMeetingDate"></cc1:DisplayOrEditDate>
                        <cc2:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="LastMeetingDate"
                            Enabled="True">
                        </cc2:CalendarExtender>
                    </td>
                </tr>
            </table>
            <table class="Design2">
                <tr>
                    <th>
                        Attendance Tracking Level:
                    </th>
                    <td>
                        <cc1:DisplayOrEditDropDown ID="AttendTrkLevelId" runat="server" BindingMode="TwoWay"
                            BindingSource="organization" DataTextField="Value" DataValueField="Id" Width="200px"
                            BindingMember="AttendTrkLevelId" DataSourceID="ODS_AttendTrkLevelId" MakeDefault0="False">
                        </cc1:DisplayOrEditDropDown>
                    </td>
                </tr>
                <tr>
                    <th>
                        Attendance Classification:
                    </th>
                    <td>
                        <cc1:DisplayOrEditDropDown ID="AttendClassificationId" runat="server" BindingMode="TwoWay"
                            BindingSource="organization" DataTextField="Value" DataValueField="Id" Width="200px"
                            BindingMember="AttendClassificationId" DataSourceID="ODS_AttendClassificationId"
                            MakeDefault0="False">
                        </cc1:DisplayOrEditDropDown>
                    </td>
                </tr>
                <tr>
                    <th>
                        Entry Point:
                    </th>
                    <td>
                        <cc1:DisplayOrEditDropDown ID="EntryPointId" runat="server" BindingMode="TwoWay"
                            BindingSource="organization" DataTextField="Value" DataValueField="Id" Width="200px"
                            BindingMember="EntryPointId" DataSourceID="ODS_EntryPointId"
                            MakeDefault0="True">
                        </cc1:DisplayOrEditDropDown>
                    </td>
                </tr>
                <tr>
                    <th>
                        Security Type:
                    </th>
                    <td>
                        <cc1:DisplayOrEditDropDown ID="SecurityTypeId" runat="server" BindingMode="TwoWay"
                            BindingSource="organization" DataTextField="Value" DataValueField="Id" Width="200px"
                            BindingMember="SecurityTypeId" DataSourceID="ODS_SecurityTypeId" MakeDefault0="False">
                        </cc1:DisplayOrEditDropDown>
                    </td>
                </tr>
                <tr>
                    <td>
                        &#160;&nbsp;
                    </td>
                </tr>
                <tr>
                    <th>
                        Rollsheet Visitor Weeks:
                    </th>
                    <td>
                        <cc1:DisplayOrEditText ID="RollSheetVisitorWks" runat="server" BindingSource="organization"
                            BindingMember="RollSheetVisitorWks" BindingMode="TwoWay" ChangedStatus="False"
                            Width="135px"></cc1:DisplayOrEditText>
                    </td>
                </tr>
                <tr>
                    <th>
                        Grade:
                    </th>
                    <td>
                        <cc1:DisplayOrEditText ID="GradeRangeStart" runat="server" BindingSource="organization"
                            BindingMember="GradeRangeStart" BindingMode="TwoWay" ChangedStatus="False" Width="135px"></cc1:DisplayOrEditText>
                    </td>
                </tr>
            </table>
            <table class="Design2">
            </table>
        </div>
        <div id="Meetings-tab" class="ui-tabs-hide">
            <asp:UpdatePanel ID="MeetingsPanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton ID="NewMeetingLink" runat="server" OnClientClick="OpenNewMeeting();return false;"
                        Text="Create New Meeting" />
                    <br />
                    <uc3:MeetingGrid ID="MeetingGrid1" runat="server" DataSourceID="MeetingData" Visible="false" />
                    <div style="visibility: hidden">
                        <asp:Button ID="ShowMeetings" runat="server" OnClick="ShowMeetings_Click" Text="Button" /></div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ShowMeetings" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
            <asp:Button ID="TriggerRollsheetPopup2" Style="display: none" runat="server"></asp:Button>
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
    <asp:ObjectDataSource ID="MeetingData" runat="server" EnablePaging="True" SelectCountMethod="MeetingCount"
        DeleteMethod="DeleteMeeting" SortParameterName="sortExpression" SelectMethod="Meetings"
        TypeName="CMSPresenter.MeetingController">
        <DeleteParameters>
            <asp:Parameter Name="MeetingId" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="orgid" QueryStringField="id" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="VisitorData" runat="server" EnablePaging="True" SelectCountMethod="VisitorCount"
        SortParameterName="sortExpression" SelectMethod="Visitors" TypeName="CMSPresenter.OrganizationController">
        <SelectParameters>
            <asp:QueryStringParameter Name="orgid" QueryStringField="id" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_Divisions" runat="server" SelectMethod="AllOrgDivTags"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_OrganizationStatusId" runat="server" SelectMethod="OrganizationStatusCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_OrganizationTypeId" runat="server" SelectMethod="OrganizationTypes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_ScheduleId" runat="server" SelectMethod="Schedules"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_LeaderMemberTypeId" runat="server" SelectMethod="MemberTypeCodes2"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_SecurityTypeId" runat="server" SelectMethod="SecurityTypeCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_AttendTrkLevelId" runat="server" SelectMethod="AttendanceTrackLevelCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_EntryPointId" runat="server" SelectMethod="EntryPoints"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_AttendClassificationId" runat="server" SelectMethod="AttendanceClassifications"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GroupData" runat="server" 
        SelectMethod="FetchMemberGroups" 
        TypeName="CMSPresenter.OrganizationController">
        <SelectParameters>
            <asp:QueryStringParameter Name="oid" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="GroupData2" runat="server" 
        SelectMethod="FetchMemberGroups2" 
        TypeName="CMSPresenter.OrganizationController">
        <SelectParameters>
            <asp:QueryStringParameter Name="oid" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
