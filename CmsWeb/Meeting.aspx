<%@ Page Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Meeting.aspx.cs" Inherits="CmsWeb.Meeting"
    Title="Meeting" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var $editing;
        $(function() {
            $editing = $('#<%= EditUpdateButton1.ClientID %>')[0].value == 'Update';
            $('#visitorDialog').dialog({
                title: 'Add Visitors Dialog',
                bgiframe: true,
                autoOpen: false,
                width: 712,
                height: 630,
                modal: true,
                overlay: {
                    opacity: 0.5,
                    background: "black"
                }, close: function() {
                    $('iframe', this).attr("src", "");
                }
            });
            $('#AddVisitorLink').live("click", function(e) {
                e.preventDefault();
                var d = $('#visitorDialog');
                $('iframe', d).attr("src", this.href);
                d.dialog("option", "title", this.title);
                d.dialog("open");
            });
        });
        function ToggleCallback(e) {
            var result = eval('(' + e + ')');
            $get(result.ControlId).checked = result.HasTag;
        }
        function CallToggleAttendance(peopleId, controlId) {
            PageMethods.ToggleAttendance(peopleId, controlId, ToggleCallback);
            return true;
        }
        function ToggleAttendee(e, text) {
            var key = window.event ? e.keyCode : e.which;
            if (key != 13)
                return true;
            var tb = $get('<%= TextBox1.ClientID %>');
            tb.value = "";
            if (text.substring(2, 0) == "M.") {
                if ($editing)
                    $('#<%= EditUpdateButton1.ClientID %>')[0].click();
                else
                    __doPostBack("CreateMeeting", text);
                return false;
            }
            var s = '[pid=' + text + '] input';
            var cb = $(s)[0];
            if (cb) {
                cb.scrollIntoView();
                cb.checked = !cb.checked;
            }
            return false;
        }
        function AddSelected(ret) {
            var d = $('#visitorDialog');
            d.dialog("close");
            if (ret.error)
                alert(ret.error);
            else
                $('#<%= AddedSelectedVisitors.ClientID %>').click();
        }
        function ChangeMeetingDate() {
            $find('<%=MeetingPopup.ClientID%>').show();
        }

    </script>

    <table class="PersonHead" border="0" width="100%">
        <tr>
            <td>
                <cc1:DisplayHyperlink ID="OrgName" runat="server"
                    BindingUrlMember="OrganizationId" BindingMember="OrganizationName" BindingSource="meeting.Organization"
                    BindingMode="OneWay" ForeColor="White">[OrgName]</cc1:DisplayHyperlink>
            </td>
            <td>
                <cc1:DisplayLabel ID="MeetingDateHeader" runat="server" BindingMember="MeetingDate"
                    BindingSource="meeting"></cc1:DisplayLabel>
                <asp:LinkButton ID="MeetingDateChange" runat="server" ToolTip="change meeting date and time" OnClientClick="ChangeMeetingDate();return false;"
                        Text="e" />

            </td>
        </tr>
    </table>
    <table class="Design2">
        <tr>
            <th colspan="2" align="center">
                Meeting Description
            </th>
            <td>
                <cc1:DisplayOrEditText ID="Description" MaxLength="99" runat="server" BindingSource="meeting" BindingMode="TwoWay" />
            </td>
        </tr>
        <tr>
            <th colspan="2" align="center">
                Counts
            </th>
            <td valign="bottom" rowspan="6">
                <asp:HyperLink ID="MeetingSummaryLink" runat="server" Target="_blank">
                    <asp:Image ID="Image2" ImageUrl="~/images/pdficon_small.gif" runat="server" />
                    Visits/Absents Report</asp:HyperLink><br />
                <asp:HyperLink ID="MeetingAttendanceLink" runat="server" Target="_blank">
                    <asp:Image ID="Image1" ImageUrl="~/images/pdficon_small.gif" runat="server" />
                    Attendee Report</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table class="Design2">
                    <tr id="TR_NumPresent" runat="server">
                        <th>
                            Number Present:
                        </th>
                        <td>
                            <span style="font-weight:bold"><cc1:DisplayOrEditText ID="NumPresent" runat="server" BindingSource="meeting" BindingMode="TwoWay"
                                AssociatedRowId="TR_NumPresent" /></span>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton ID="QuerySearch" runat="server" 
        onclick="QuerySearch_Click">Convert to Query</asp:LinkButton> 
                        </td>
                    </tr>
                    <tr id="TR_NumMembers" runat="server">
                        <th>
                            Members:
                        </th>
                        <td>
                            <cc1:DisplayOrEditText ID="NumMembers" runat="server" BindingSource="meeting" BindingMode="OneWay"
                                AssociatedRowId="TR_NumMembers" />
                        </td>
                    </tr>
                    <tr id="TR_NumVstMembers" runat="server">
                        <th>
                            VstMembers:
                        </th>
                        <td>
                            <cc1:DisplayOrEditText ID="NumVstMembers" runat="server" BindingSource="meeting"
                                BindingMode="OneWay" AssociatedRowId="TR_NumVstMembers" />
                        </td>
                    </tr>
                    <tr id="TR_NumRepeatVst" runat="server">
                        <th>
                            RepeatVst:
                        </th>
                        <td>
                            <cc1:DisplayOrEditText ID="NumRepeatVst" runat="server" BindingSource="meeting" BindingMode="OneWay"
                                AssociatedRowId="TR_NumRepeatVst" />
                        </td>
                    </tr>
                    <tr id="TR_NumNewVisit" runat="server">
                        <th>
                            NewVisit:
                        </th>
                        <td>
                            <cc1:DisplayOrEditText ID="NumNewVisit" runat="server" BindingSource="meeting" BindingMode="OneWay"
                                AssociatedRowId="TR_NumNewVisit" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            Group Meeting:
                        </th>
                        <td>
                            <cc1:DisplayOrEditCheckbox ID="GroupMeetingFlag" runat="server" BindingSource="meeting" BindingMode="TwoWay" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" OnClick="EditUpdateButton1_Click"
        CheckRole="false" />
    <asp:TextBox ID="TextBox1" runat="server" onkeypress="return ToggleAttendee(event,value)"></asp:TextBox>
    <br />
    <asp:Label ID="AlreadyAttendErrors" runat="server" Font-Bold="False" ForeColor="Red"
        Text="Label" Visible="False"></asp:Label>
    <br />
    <asp:HiddenField ID="ShowAttendanceFlag1" runat="server" />
    <% if (User.IsInRole("Attendance"))
       { %>
    <a id="AddVisitorLink" href="/SearchAdd/Index/<%=meeting.MeetingId %>?type=visitor">Add Visitor</a>
    <% } %>
    <br />
    Count:
    <asp:Label ID="GridCount" runat="server" Text="0"></asp:Label>
    <asp:Button ID="AddedSelectedVisitors" runat="server" Style="display: none" OnClick="AddedSelectedVisitors_Click" />
    <asp:ListView ID="ListView1" runat="server" DataSourceID="AttendData" DataKeyNames="PeopleId">
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:CheckBox ID="ck" pid='<%# Eval("PeopleId") %>' runat="server" Checked='<%# (bool)Eval("AttendFlag") %>'
                        Visible='<%# ShowAttendanceFlag1.Value == "True" %>'></asp:CheckBox>
                </td>
                <td>
                    <asp:Label ID="Label1" Visible='<%# ShowAttendanceFlag1.Value == "True" %>' runat="server"
                        Text='<%# Eval("Name") %>' ToolTip='<%# Eval("Age") %>'></asp:Label>
                    <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# Eval("PeopleId", "~/Person/Index/{0}") %>'
                        Visible='<%# ShowAttendanceFlag1.Value != "True" %>' Text='<%# Eval("Name") %>'
                        runat="server">HyperLink</asp:HyperLink>
                </td>
                <td>
                    <asp:Label ID="MemberTypeLabel" runat="server" Text='<%# Eval("MemberType") %>' />
                </td>
                <td>
                    <asp:Label ID="AttendTypeLabel" runat="server" Text='<%# Eval("AttendType") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table runat="server" style="">
                <tr>
                    <td>
                        No data was returned.
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server">
                                    &nbsp;
                                </th>
                                <th runat="server">
                                    Name
                                </th>
                                <th runat="server">
                                    MemberType
                                </th>
                                <th runat="server">
                                    AttendType
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
    </asp:ListView>
    <asp:HyperLink ID="rollsheetlink" Target="_blank" runat="server">Meeting Rollsheet of Attendees</asp:HyperLink>
    <br />
    <asp:Button ID="TriggerMeetingPopup2" Style="display: none" runat="server"></asp:Button>
    <asp:Panel ID="MeetingInputPanel" runat="server" CssClass="modalDiv" Style="display: none">
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
                    <asp:RegularExpressionValidator ID="MeetingTimeValidator" runat="server" ErrorMessage="Invalid time: Use format hh:mm am or pm."
                        ControlToValidate="MeetingTime" ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$"
                        SetFocusOnError="True" ValidationGroup="MeetingValidatorGroup"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td class="footer">
                    <asp:LinkButton ID="ChangeMeeting" runat="server" CausesValidation="false" Text="Change Meeting Time"
                        OnClick="ChangeMeetingDate" ValidationGroup="MeetingValidatorGroup" />
                    <asp:LinkButton ID="MeetingCancel" runat="server" CausesValidation="false" Text="Cancel" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="MeetingPopup" runat="server" TargetControlID="TriggerMeetingPopup2"
        PopupControlID="MeetingInputPanel" CancelControlID="MeetingCancel" DropShadow="true"
        BackgroundCssClass="modalBackground">
    </cc2:ModalPopupExtender>
    <div id="visitorDialog">
    <iframe style="width:99%;height:99%"></iframe>
    </div>
    <asp:ObjectDataSource ID="AttendData" runat="server" SelectCountMethod="Count" SelectMethod="Attendees"
        TypeName="CMSPresenter.AttendController" OnSelected="AttendData_Selected">
        <SelectParameters>
            <asp:QueryStringParameter Name="meetid" QueryStringField="id" Type="Int32" />
            <asp:ControlParameter Name="inEditMode" ControlID="ShowAttendanceFlag1" PropertyName="Value"
                Type="Boolean" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
