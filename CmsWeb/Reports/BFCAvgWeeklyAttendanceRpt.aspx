<%@ Page Language="C#" StylesheetTheme="Standard" AutoEventWireup="true" CodeBehind="BFCAvgWeeklyAttendanceRpt.aspx.cs"
    MasterPageFile="~/Reports/Reports.Master" Inherits="CMSWeb.Reports.BFCAvgWeeklyAttendanceRpt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            font-size: large;
        }
        .totalrow td
        {
            border-top: thin solid black;
            font-weight: bold;
        }
        .headerrow td, th
        {
            border-bottom: thin solid black;
        }
     </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center">
        <h1>
            BFC Division Average Attendance</h1>
        <table style="text-align: left" align="center">
            <tr>
                <td style="text-align: right">
                    From Date:
                </td>
                <td>
                    <asp:TextBox ID="FromDate" runat="server" AutoPostBack="false" Style="font-size: large"
                        Width="100"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    To Date:
                </td>
                <td>
                    <asp:TextBox ID="ToDate" runat="server" AutoPostBack="false" Style="font-size: large"
                        Width="100"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" ToolTip="Press to run report" CausesValidation="true"
                        runat="server" Text="Run" CssClass="noPrint" ValidationGroup="vgDates" OnClick="btnSubmit_Click" />
                </td>
            </tr>
        </table>
        <cc2:CalendarExtender ID="FromExtender" runat="server" TargetControlID="FromDate">
        </cc2:CalendarExtender>
        <cc2:CalendarExtender ID="ToDateExtender" runat="server" TargetControlID="ToDate">
        </cc2:CalendarExtender>
        <asp:CompareValidator ID="FromDateValidator" runat="server" ErrorMessage="CompareValidator"
            ControlToValidate="FromDate" ControlToCompare="ToDate" Operator="LessThanEqual"
            SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="From Date must be before To Date."
            CssClass="noPrint"></asp:CompareValidator><br />
        <asp:CompareValidator ID="ToDateValidator" runat="server" ErrorMessage="CompareValidator"
            ControlToValidate="ToDate" ControlToCompare="FromDate" Operator="GreaterThanEqual"
            SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="To Date must be after From Date."
            CssClass="noPrint"></asp:CompareValidator>
        <hr />
        <asp:ListView ID="ListView1" runat="server" DataSourceID="dsBFCWeeklyAttendance">
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" style="width: 500px" align="center">
                    <tr runat="server" class="headerrow">
                        <th align="left" runat="server">
                            Division Name
                        </th>
                        <th align="right" runat="server">
                            8:00
                        </th>
                        <th align="right" runat="server">
                            9:30
                        </th>
                        <th align="right" runat="server">
                            11:00
                        </th>
                        <th align="right" runat="server">
                            Total
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class='<%# Eval("Class") %>' style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'>
                    <td align="left">
                        <asp:Label ID="divisionnameLabel" runat="server" Text='<%# Eval("Name") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="count1Label" runat="server" Text='<%# Eval("Avg800", "{0:n0}") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="count2Label" runat="server" Text='<%# Eval("Avg930", "{0:n0}") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="count3Label" runat="server" Text='<%# Eval("Avg1100", "{0:n0}") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="totalLabel" runat="server" Text='<%# Eval("Total", "{0:n0}") %>' />
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
        </asp:ListView>
    </div>
    <asp:ObjectDataSource ID="dsBFCWeeklyAttendance" runat="server" SelectMethod="BFCAvgWeeklyAttendanceSummary"
        TypeName="CMSPresenter.BFCAttendSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDate" Name="fromDate" Type="DateTime" />
            <asp:ControlParameter ControlID="ToDate" Name="toDate" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
