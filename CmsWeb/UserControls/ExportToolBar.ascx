<%@ Import Namespace="System" %>
<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ExportToolBar.ascx.cs"
    Inherits="CMSWeb.ExportToolBar" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<ul class="sf-tab">
    <li class="headlink"><a href="#">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/Mail.png" />
        Email</a>
        <ul>
            <li>
                <asp:HyperLink ID="EmailLink" runat="server">
                    <asp:Image ID="Image5" ImageUrl="~/images/Mail.png" Width="16" runat="server" />
                    Individual</asp:HyperLink></li>
        </ul>
    </li>
    <li class="headlink"><a href="#">
        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/BulkMailing.png" />
        Export</a>
        <ul>
            <li>
                <asp:HyperLink ID="LabelsLink" ToolTip="Labels (pdf for label printer)" runat="server">
                    <asp:Image ID="Image14" ImageUrl="~/images/tags.png" runat="server" />
                    Labels</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="ExcelLink" ToolTip="For mail merge" runat="server">
                    <asp:Image ID="Image6" ImageUrl="~/images/Excel.png" runat="server" />
                    Excel</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="BulkMailLink" ToolTip="Comma separated values text file, opens in excel, for bulk mailings"
                    runat="server">
                    <asp:Image ID="Image7" ImageUrl="~/images/Excel.png" runat="server" />
                    Bulk (csv)</asp:HyperLink></li>
        </ul>
    </li>
    <li class="headlink"><a href="#">
        <asp:Image ID="Image3" runat="server" ImageUrl="~/images/Report.png" />&nbsp;Reports</a>
        <ul runat="server" id="ReportLinks">
            <li>
                <asp:HyperLink ID="ProspectLink" runat="server">
                    <asp:Image ID="Image8" ImageUrl="~/images/Report.png" runat="server" />
                    Prospect Form</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="InreachLink" runat="server">
                    <asp:Image ID="Image9" ImageUrl="~/images/Report.png" runat="server" />
                    Inreach Form</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="InvolvementLink" ToolTip="Personal, Contact and Enrollment Info"
                    runat="server">
                    <asp:Image ID="Image10" ImageUrl="~/images/Excel.png" runat="server" />
                    Involvement</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="AttendLink" ToolTip="Contains attendance information for their class"
                    runat="server">
                    <asp:Image ID="Image11" ImageUrl="~/images/Excel.png" runat="server" />
                    Attendance</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="ChildrenLink" ToolTip="Contains emergency contact, who brought child info"
                    runat="server">
                    <asp:Image ID="Image15" ImageUrl="~/images/Excel.png" runat="server" />
                    Children</asp:HyperLink></li>
            <li>
                <asp:HyperLink ID="ChurchLink" ToolTip="Includes Drop/Join/Other Church info"
                    runat="server">
                    <asp:Image ID="Image19" ImageUrl="~/images/Excel.png" runat="server" />
                    Other Churches</asp:HyperLink></li>
        </ul>
    </li>
    <li class="headlink"><a href="#">
        <asp:Image ID="Image4" runat="server" ImageUrl="~/images/Tag.png" />
        Tag</a>
        <ul>
            <li>
                <asp:LinkButton ID="TagAddAll" runat="server" OnClick="TagAddAll_Click">
                    <asp:Image ID="Image12" ImageUrl="~/images/Tag.png" runat="server" />
                    <asp:Label ID="TagAddLabel" runat="server">Add All</asp:Label></asp:LinkButton></li>
            <li>
                <asp:LinkButton ID="TagRemoveAll" runat="server" OnClick="TagRemoveAll_Click">
                    <asp:Image ID="Image13" ImageUrl="~/images/Tag.png" runat="server" />
                    <asp:Label ID="TagRemoveLabel" runat="server">Remove All</asp:Label></asp:LinkButton></li>
        </ul>
    </li>
    <li class="headlink"><a href="#">
        <asp:Image ID="Image16" runat="server" ImageUrl="~/images/Tag.png" />
        Other</a>
        <ul>
            <li>
                <cc2:LinkButtonConfirm ID="AddContact2" runat="server" Confirm="Are you sure you want to add a contact for all these people?"
                    OnClick="AddContact_Click"><asp:Image ID="Image17" ImageUrl="~/images/Tag.png" runat="server" />
                    <asp:Label ID="Label1" runat="server">Add Contact</asp:Label></cc2:LinkButtonConfirm>
                </li>
            <li>
                <asp:HyperLink ID="ChoirLink" ToolTip="Labels for Choir Attendance"
                    runat="server">
                    <asp:Image ID="Image18" ImageUrl="~/images/tags.png" runat="server" />
                    Choir Labels</asp:HyperLink></li>
            <li id="RollsheetItem" runat="server">
                <a title="Rollsheet Report" href="#" onclick="OpenRollsheet();return false;">
                    <asp:Image ID="Image20" ImageUrl="~/images/tags.png" runat="server" />Rollsheet Report</a></li>
            <li id="MemberItem" runat="server">
                <asp:HyperLink ID="MemberLink" ToolTip="Includes Org Member info"
                    runat="server">
                    <asp:Image ID="Image21" ImageUrl="~/images/Excel.png" runat="server" />
                    Member Export</asp:HyperLink></li>
        </ul>
    </li>
</ul>
<asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none;
    width: 350px; padding: 10px">
    <table>
        <tr>
            <td style="margin: 3px; border: solid thin black">
                <asp:RadioButtonList ID="Option" runat="server">
                    <asp:ListItem ToolTip="Addressed to individuals" Value="Individual" Selected="True">Individual</asp:ListItem>
                    <asp:ListItem ToolTip="Addressed as a family when there are children" Value="Family">Family</asp:ListItem>
                    <asp:ListItem ToolTip="Addressed to a couple, if both are in selection" Value="CouplesBoth">Couples (both)</asp:ListItem>
                    <asp:ListItem ToolTip="Addressed to a couple, even if both are not in selection"
                        Value="CouplesEither">Couples (either)</asp:ListItem>
                    <asp:ListItem ToolTip="Addressed to parents or parent" Value="ParentsOf">Parents Of</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            <td>
                <asp:CheckBox ID="UseTitle" runat="server" Text="Use Titles" />
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="cmdOK" OnClientClick="TBExportGo()" runat="server" Text="OK" />
                <asp:Button ID="cmdClose" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" />
<cc1:ModalPopupExtender ID="Panel1_ModalPopupExtender" runat="server" Enabled="True"
    TargetControlID="hiddenTargetControlForModalPopup" BackgroundCssClass="modalBackground"
    PopupControlID="Panel1" CancelControlID="cmdClose" OkControlID="cmdOK">
</cc1:ModalPopupExtender>
