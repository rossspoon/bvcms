<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Reports.ascx.cs" Inherits="CMSWeb.UserControls.Reports" %>
<div style="overflow: auto; height: 125px;">
    <table>
        <tr>
            <td>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Reports/ChurchAttendanceSummaryRpt.aspx"
                    Target="_blank">Church Attendance Summary</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Reports/ChurchAttendanceRpt.aspx"
                    Target="_blank">Church Attendance</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Reports/BFCWeeklyAttendanceSummaryRpt.aspx"
                    Target="_blank">BFC Weekly Attendance Summary</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Reports/BFCAvgWeeklyAttendanceRpt.aspx"
                    Target="_blank">BFC Average Weekly Attendance</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HyperLink ID="DecisionSummary" runat="server" NavigateUrl="~/Reports/DecisionSummary.aspx"
                    Target="_blank">Decision Summary Report</asp:HyperLink>
            </td>
        </tr>
    </table>
</div>
