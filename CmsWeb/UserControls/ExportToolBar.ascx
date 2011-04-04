<%@ Import Namespace="System" %>
<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ExportToolBar.ascx.cs"
    Inherits="CmsWeb.ExportToolBar" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<table id="toolbar">
<tr>
    <td class="dropdown">
        <div><a href="#"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/Mail.png" />
        Email</a></div>
        <div class="sublinks">
            <asp:HyperLink ID="EmailLink" runat="server">
                <asp:Image ID="Image5" ImageUrl="~/images/Mail.png" Width="16" runat="server" />
                Individuals</asp:HyperLink>
            <asp:HyperLink ID="EmailLinkParents" runat="server">
                <asp:Image ID="Image33" ImageUrl="~/images/Mail.png" Width="16" runat="server" />
                Parents</asp:HyperLink>
        </div>
    </td>
    <td class="dropdown">
        <div><a href="#"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/BulkMailing.png" />
        Export</a></div>
        <div class="sublinks">
            <asp:HyperLink ID="ExcelLink" class="ChooseLabelType" ToolTip="For mail merge" runat="server">
                <asp:Image ID="Image6a" ImageUrl="~/images/Excel.png" runat="server" />
                Excel</asp:HyperLink>
            <asp:HyperLink ID="ExcelFamLink" class="ChooseLabelType" ToolTip="For mail merge" runat="server">
                <asp:Image ID="Image6b" ImageUrl="~/images/Excel.png" runat="server" />
                Excel Family</asp:HyperLink>
            <asp:HyperLink ID="ExcelPicLink" ToolTip="For picture directory word merge" runat="server">
                <asp:Image ID="Image6" ImageUrl="~/images/Excel.png" runat="server" />
                Excel Pictures</asp:HyperLink>
            <asp:HyperLink ID="ExcelUpdateLink" ToolTip="For Doing a Mass Update"
                runat="server">
                <asp:Image ID="Image9" ImageUrl="~/images/Excel.png" runat="server" />
                Excel Update</asp:HyperLink>
            <asp:HyperLink ID="BulkMailLink" class="ChooseLabelType" ToolTip="Comma separated values text file, opens in excel, for bulk mailings"
                runat="server">
                <asp:Image ID="Image7" ImageUrl="~/images/Excel.png" runat="server" />
                Bulk (csv)</asp:HyperLink>
        </div>
    </td>
    <td class="dropdown">
        <div><a href="#"><asp:Image ID="Image3" runat="server" ImageUrl="~/images/Report.png" />
            Reports</a></div>
        <div class="sublinks">
            <asp:HyperLink ID="ProspectLink" runat="server">
                <asp:Image ID="Image8" ImageUrl="~/images/Report.png" runat="server" />
                Inreach/Outreach</asp:HyperLink>
            <asp:HyperLink ID="ContactsLink" ToolTip="For calling contacts"
                runat="server">
                <asp:Image ID="Image25" ImageUrl="~/images/Report.png" runat="server" />
                Contact Report</asp:HyperLink>
            <asp:HyperLink ID="AttendanceLink" ToolTip="General Attendance Stats"
                runat="server">
                <asp:Image ID="Image31" ImageUrl="~/images/Report.png" runat="server" />
                Weekly Attend</asp:HyperLink>
            <asp:HyperLink ID="InvolvementLink" ToolTip="Personal, Contact and Enrollment Info"
                runat="server">
                <asp:Image ID="Image10" ImageUrl="~/images/Excel.png" runat="server" />
                Involvement</asp:HyperLink>
            <asp:HyperLink ID="FamilyLink" ToolTip="Family Information"
                runat="server">
                <asp:Image ID="Image32" ImageUrl="~/images/Report.png" runat="server" />
                Family Report</asp:HyperLink>
            <asp:HyperLink ID="VolunteerLink" ToolTip="Volunteer Information"
                runat="server">
                <asp:Image ID="Image18" ImageUrl="~/images/Report.png" runat="server" />
                Volunteer Report</asp:HyperLink>
            <asp:HyperLink ID="RegistrationLink" ToolTip="One Page Per Registratant"
                runat="server">
                <asp:Image ID="Image24" ImageUrl="~/images/Report.png" runat="server" />
                Registration Report</asp:HyperLink>
            <asp:HyperLink ID="AttendLink" ToolTip="Contains attendance information for their class"
                runat="server">
                <asp:Image ID="Image11" ImageUrl="~/images/Excel.png" runat="server" />
                BF Attendance</asp:HyperLink>
            <asp:HyperLink ID="ChildrenLink" ToolTip="Contains emergency contact, who brought child info"
                runat="server">
                <asp:Image ID="Image15" ImageUrl="~/images/Excel.png" runat="server" />
                Children</asp:HyperLink>
            <asp:HyperLink ID="ChurchLink" ToolTip="Includes Drop/Join/Other Church info"
                runat="server">
                <asp:Image ID="Image19" ImageUrl="~/images/Excel.png" runat="server" />
                Other Churches</asp:HyperLink>
        </div>
    </td>
    <td class="dropdown">
        <div><a href="#"><asp:Image ID="Image26" runat="server" ImageUrl="~/images/Tag.png" />
        Labels</a></div>
        <div class="sublinks">
            <asp:HyperLink ID="LabelsLink" class="ChooseLabelType" ToolTip="Labels (pdf for Datamax label printer)" runat="server">
                <asp:Image ID="Image14" ImageUrl="~/images/tags.png" runat="server" />
                Roll Labels</asp:HyperLink>
            <asp:HyperLink ID="BarCodeLabels" ToolTip="Labels for Choir Attendance"
                runat="server">
                <asp:Image ID="Image28" ImageUrl="~/images/tags.png" runat="server" />
                BarCode Labels</asp:HyperLink>
            <asp:HyperLink ID="AveryLabels" ToolTip="Avery Name Labels"
                runat="server">
                <asp:Image ID="Image20" ImageUrl="~/images/tags.png" runat="server" />
                Avery Labels</asp:HyperLink>
            <asp:HyperLink ID="AveryLabels3" ToolTip="Avery 3 Across Labels (person per row)"
                runat="server">
                <asp:Image ID="Image27" ImageUrl="~/images/tags.png" runat="server" />
                Avery Labels 3</asp:HyperLink>
            <asp:HyperLink ID="AveryAddressLabels" class="ChooseLabelType" ToolTip="Avery Address Labels" runat="server">
                <asp:Image ID="Image30" ImageUrl="~/images/tags.png" runat="server" />
                Avery Address</asp:HyperLink>
        </div>
    </td>
    <td class="dropdown">
        <div><a href="#"><asp:Image ID="Image4" runat="server" ImageUrl="~/images/Tag.png" />
        Tag</a></div>
        <div class="sublinks">
            <asp:LinkButton ID="TagAddAll" runat="server" OnClick="TagAddAll_Click">
                <asp:Image ID="Image12" ImageUrl="~/images/Tag.png" runat="server" />
                <asp:Label ID="TagAddLabel" runat="server">Add All</asp:Label></asp:LinkButton>
            <asp:LinkButton ID="TagRemoveAll" runat="server" OnClick="TagRemoveAll_Click">
                <asp:Image ID="Image13" ImageUrl="~/images/Tag.png" runat="server" />
                <asp:Label ID="TagRemoveLabel" runat="server">Remove All</asp:Label></asp:LinkButton>
        </div>
    </td>
    <td class="dropdown">
        <div><a href="#"><asp:Image ID="Image16" runat="server" ImageUrl="~/images/Tag.png" />
        Other</a></div>
        <div class="sublinks">
            <cc2:LinkButtonConfirm ID="AddContact2" runat="server" Confirm="Are you sure you want to add a contact for all these people?"
                OnClick="AddContact_Click"><asp:Image ID="Image17" ImageUrl="~/images/Tag.png" runat="server" />
                <asp:Label ID="Label1" runat="server">Add Contact</asp:Label></cc2:LinkButtonConfirm>
        </div>
    </td>
</tr>
</table>
