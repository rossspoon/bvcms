<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="OrganizationSearch.aspx.cs"
    Inherits="CMSWeb.OrganizationSearch" Title="Organization Search" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register src="UserControls/GridPager.ascx" tagname="GridPager" tagprefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
    
    function ToggleTagCallback(e)
    {
        if (e == "")
            return;
        var result = eval('(' + e + ')');
        $('#' + result.ControlId).text(result.HasTag ? "Remove" : "Add");
    }

    function OpenRollsheet() {
        var modalPopupBehavior = $find('RollsheetPopupBehavior');
        modalPopupBehavior.show();
        $get('<%=RollsheetInputPanel.ClientID%>').focus();
    }

    $(function() {
        $('#PopupAttDetail').click(function() {
            var modalPopupBehavior = $find('AttDetailPopupBehavior');
            modalPopupBehavior.show();
            $get('<%=AttDetailInputPanel.ClientID%>').focus();
            return false;
        });
    });
    function OpenAttDetail() {
    }
    function ViewRollsheet2() {
        Page_ClientValidate(' ');
        if (Page_IsValid) {
            var did = '<%=OrgDivisions.SelectedValue %>';
            if (did == '0') {
                alert('must choose division');
                return false;
            }
            var sid = '<%=Schedule.SelectedValue %>';
            var nam = '<%=NameSearch.Text %>';
            var d = $get('<%=MeetingDate.ClientID %>').value;
            var t = $get('<%=MeetingTime.ClientID %>').value;
            var args = "?div=" + did +
                   "&schedule=" + sid +
                   "&name=" + nam +
                   "&dt=" + d + " " + t;

            var newWindowUrl = "Report/Rollsheet.aspx" + args
            window.open(newWindowUrl);
        }
        return Page_IsValid;
    }
    function ViewRosterRpt() {
        Page_ClientValidate(' ');
        if (Page_IsValid) {
            var did = '<%=OrgDivisions.SelectedValue %>';
            if (did == '0') {
                alert('must choose division');
                return false;
            }
            var sid = '<%=Schedule.SelectedValue %>';
            var args = "?div=" + did +
                   "&schedule=" + sid;

            var newWindowUrl = "Report/RosterReport.aspx" + args
            window.open(newWindowUrl);
        }
        return Page_IsValid;
    }
    function ViewAttDetail() {
        Page_ClientValidate(' ');
        if (Page_IsValid) {
            var did = '<%=OrgDivisions.SelectedValue %>';
            var nam = '<%=NameSearch.Text %>';
            var sid = '<%=Schedule.SelectedValue %>';
            var d1 = $get('<%=MeetingDate1.ClientID %>').value;
            var d2 = $get('<%=MeetingDate2.ClientID %>').value;
            var args = "?divid=" + did +
                   "&schedid=" + sid +
                   "&name=" + nam +
                   "&dt1=" + d1 +
                   "&dt2=" +d2;

            var newWindowUrl = "Report/AttendanceDetail.aspx" + args
            window.open(newWindowUrl);
        }
        return Page_IsValid;
    }
    </script>

    <div>
        <asp:LinkButton ID="NewSearch" runat="server" OnClick="NewSearch_Click">New Search (clear)</asp:LinkButton>
        <table width="100%">
            <tr>
                <td>
                    <table class="modalPopup">
                        <tr>
                            <th>
                                Name:
                            </th>
                            <td colspan="3">
                                <asp:TextBox ID="NameSearch" runat="server" ToolTip="OrganizationId, Location or part of Name (organization, leader, division)"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Division:
                            </th>
                            <td colspan="3">
                                <cc1:DropDownCC ID="OrgDivisions" runat="server" DataTextField="Value" DataSourceID="OrgTagData"
                                    DataValueField="Id">
                                </cc1:DropDownCC>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Schedule:
                            </th>
                            <td colspan="3">
                                <cc1:DropDownCC ID="Schedule" runat="server" DataTextField="Value" DataSourceID="ScheduleData"
                                    DataValueField="Id">
                                </cc1:DropDownCC>
                            </td>
                        </tr>
                        <tr>
                            <th>
                                Status:
                            </th>
                            <td>
                                <cc1:DropDownCC ID="Status" runat="server" DataTextField="Value" DataSourceID="OrganizationStatusData"
                                    DataValueField="Id">
                                </cc1:DropDownCC>
                            </td>
                            <th>
                                Campus:
                            </th>
                            <td>
                                <cc1:DropDownCC ID="Campus" runat="server"
                                    DataTextField="Value" DataValueField="Id" Width="150px"
                                    DataSourceID="ODS_Campus"
                                    MakeDefault0="True">
                                </cc1:DropDownCC>
                            </td>
                        </tr>
                    </table>
                </td>
                <td id="ManageOrgTags" runat="server">
                    <table>
                        <tr>
                            <th colspan="2">
                                Manage Divisions
                            </th>
                        </tr>
                        <tr>
                            <th align="right">
                                Change Active Division:
                            </th>
                            <td>
                                <asp:DropDownList ID="Tags" runat="server" DataTextField="Text" DataSourceID="OrgTagData2"
                                    DataValueField="Value" AutoPostBack="True" OnSelectedIndexChanged="Tags_SelectedIndexChanged">
                                </asp:DropDownList>
                                &nbsp;
                                <input type="checkbox" id="maindiv" /> Main Division
                                &nbsp;
                                <asp:LinkButton ID="DeleteTag" runat="server" OnClick="DeleteTag_Click">Delete Division</asp:LinkButton><br />
								<asp:Label ID="progdivid" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <th align="right">
                                New Division Name:
                            </th>
                            <td>
                                <asp:TextBox ID="TagName" runat="server" Width="225px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="TagNameValidator" runat="server" ErrorMessage="DivName is Required"
                                    ControlToValidate="TagName" ValidationGroup="TagName"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="right">
                                &nbsp;
                            </th>
                            <td>
                                <asp:LinkButton ID="MakeNewTag" runat="server" OnClick="MakeNewTag_Click" ValidationGroup="TagName">Make New 
        Division</asp:LinkButton>&nbsp;|
                                <asp:LinkButton ID="RenameTag" runat="server" OnClick="RenameTag_Click" ValidationGroup="TagName">Rename Division</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    &nbsp;
                </td>
            </tr>
        </table>
        <hr />
        <asp:Button ID="SearchButton" Style="margin-bottom: .5em; margin-left: .5em" runat="server"
            Text="Search" OnClick="SearchButton_Click" TabIndex="6" />
        <br />
    </div>
    Count:
    <asp:Label ID="GridCount" runat="server" Text="0"></asp:Label>
        <asp:GridView ID="OrganizationGrid" SkinID="GridViewSkin" runat="server" AllowPaging="True"
    PagerSettings-Position="Bottom" AutoGenerateColumns="False" PageSize="10" 
        AllowSorting="True" DataSourceID="OrganizationData" 
        onrowdatabound="OrganizationGrid_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Division" SortExpression="Division">
            <ItemTemplate>
                <asp:Label ID="DivLab" Text='<%#Eval("DivisionName")%>' ToolTip='<%#Eval("DivisionId")%>' runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Name" SortExpression="Name">
            <ItemTemplate>
                <asp:HyperLink ID="OrgLink" runat="server" NavigateUrl='<%# Eval("OrganizationId", "~/Organization.aspx?id={0}") %>'
                    Text='<%# Eval("OrganizationName") %>'></asp:HyperLink>
                <br />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Leader Name" SortExpression="LeaderName">
            <ItemTemplate>
                <asp:HyperLink ID="LeaderLink" runat="server" NavigateUrl='<%# Eval("LeaderId", "~/Person/Index/{0}") %>'
                    Text='<%# Eval("LeaderName") %>'></asp:HyperLink>
                <br />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="MemberCount" HeaderText="Members" SortExpression="MemberCount"
            ItemStyle-HorizontalAlign="Right">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Schedule" HeaderText="Schedule" SortExpression="Schedule" />
        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
        <asp:BoundField DataField="AllowSelfCheckIn" HeaderText="Self Check-in" SortExpression="SelfCheckIn" />
        <asp:TemplateField HeaderText="Tag" ShowHeader="False">
            <ItemTemplate>
                <asp:LinkButton ID="TagUntag" runat="server" CausesValidation="False" 
                    Text='<%# (bool)Eval("HasTag") ? "Remove" : "Add" %>' 
                    ToolTip="Add to/Remove from Active Division"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerTemplate>
        <uc1:GridPager ID="GridPager1" runat="server" />
    </PagerTemplate>
</asp:GridView>
<asp:HiddenField ID="ActiveTag" runat="server" />

    <asp:LinkButton ID="ExportExcel" runat="server" OnClick="ExportExcel_Click" Text="Export to Excel" /> &nbsp;|
    <asp:LinkButton ID="RollsheetRpt" runat="server" OnClientClick="OpenRollsheet(); return false;" Text="Create Roll Sheet(s)"/> &nbsp;|
    <asp:HyperLink ID="MeetingsLink" runat="server">Meetings</asp:HyperLink> |
    <asp:LinkButton ID="RosterRpt" runat="server" OnClientClick="ViewRosterRpt(); return false;">Roster</asp:LinkButton>&nbsp;|
    <a id="PopupAttDetail" href="#">Meetings Attendance</a>
&nbsp;<asp:UpdatePanel ID="RollsheetPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="TriggerRollsheetPopup" style="display:none" runat="server">LinkButton</asp:LinkButton>            
            <asp:Panel ID="RollsheetInputPanel" runat="server" CssClass="modalDiv" Style="display: none">
                <div style="text-align: left">
                    <p style="font-size:larger; font-weight:bold"> Please select a meeting date and time: </p>
                    Meeting Date: <asp:TextBox ID="MeetingDate" runat="server"></asp:TextBox><br />
                    Meeting Time: <asp:TextBox ID="MeetingTime" runat="server" ToolTip="Time in Format hh:mm am or pm"></asp:TextBox>
                    <cc2:CalendarExtender ID="MeetingDateExtender" runat="server" TargetControlID="MeetingDate"></cc2:CalendarExtender>
                    <span class="footer">
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false" Text="Create"
                             OnClientClick="ViewRollsheet2();" ValidationGroup="RollSheetValidatorGroup" />
                        <asp:LinkButton ID="RollsheetCancel" runat="server" CausesValidation="false" Text="Cancel" />
                    </span>
                    <br />
                </div>
                <asp:RegularExpressionValidator ID="MeetingTimeValidator" runat="server" 
                    ErrorMessage="Invalid time: Use format hh:mm am or pm." 
                    ControlToValidate="MeetingTime" 
                    ValidationExpression="^ *(1[0-2]|[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$" 
                    SetFocusOnError="True" ValidationGroup="RollSheetValidatorGroup"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="MeetingDateRequiredFieldValidator" runat="server" ErrorMessage="Please enter a Meeting Date." ControlToValidate="MeetingDate" SetFocusOnError="True" ValidationGroup="RollSheetValidatorGroup"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="MeetingTimeRequiredFieldValidator" runat="server" ErrorMessage="Please enter a meeting time." ControlToValidate="MeetingTime" SetFocusOnError="True" ValidationGroup="RollSheetValidatorGroup"></asp:RequiredFieldValidator>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="RollsheetPopup" BehaviorID="RollsheetPopupBehavior" runat="server"
                TargetControlID="TriggerRollsheetPopup" PopupControlID="RollsheetInputPanel"
                CancelControlID="RollsheetCancel" DropShadow="true" BackgroundCssClass="modalBackground">
            </cc2:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="AttDetailPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="TriggerAttDetailPopup" style="display:none" runat="server">LinkButton</asp:LinkButton>            
            <asp:Panel ID="AttDetailInputPanel" runat="server" CssClass="modalDiv" Style="display: none">
                <div style="text-align: left">
                    <p style="font-size:larger; font-weight:bold"> Please select a date range: </p>
                    Meetings Start Date: <asp:TextBox ID="MeetingDate1" runat="server"></asp:TextBox><br />
                    Meetings End Date: <asp:TextBox ID="MeetingDate2" runat="server"></asp:TextBox>
                    <cc2:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="MeetingDate1"></cc2:CalendarExtender>
                    <cc2:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="MeetingDate2"></cc2:CalendarExtender>
                    <span class="footer">
                        <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="false" Text="Run"
                             OnClientClick="ViewAttDetail();" />
                        <asp:LinkButton ID="LinkButton4" runat="server" CausesValidation="false" Text="Cancel" />
                    </span>
                    <br />
                </div>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="AttDetailPopup" BehaviorID="AttDetailPopupBehavior" runat="server"
                TargetControlID="TriggerAttDetailPopup" PopupControlID="AttDetailInputPanel"
                CancelControlID="LinkButton4" DropShadow="true" BackgroundCssClass="modalBackground">
            </cc2:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>     
    <asp:ObjectDataSource ID="OrganizationData" runat="server" EnablePaging="True" SelectMethod="FetchOrganizationList"
        TypeName="CMSPresenter.OrganizationSearchController" SelectCountMethod="Count"
        SortParameterName="sortExpression" OnSelected="OrganizationData_Selected" 
        EnableViewState="False">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="" />
            <asp:ControlParameter ControlID="NameSearch" Name="NameSearch" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="OrgDivisions" Name="OrgSubDivId" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="Schedule" Name="scheduleid" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="Status" Name="statusid" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="Campus" Name="campusid" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="Tags" Name="tagid" 
                PropertyName="SelectedValue" Type="string" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OrgTagData" runat="server" SelectMethod="AllOrgDivTags"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ScheduleData" runat="server" SelectMethod="Schedules0" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OrgTagData2" runat="server" SelectMethod="AllOrgDivTags2"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="OrganizationStatusData" runat="server" SelectMethod="OrganizationStatusCodes0"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODS_Campus" runat="server" SelectMethod="AllCampuses0"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
</asp:Content>
