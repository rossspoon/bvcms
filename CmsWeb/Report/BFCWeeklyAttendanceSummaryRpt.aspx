<%@ Page Language="C#" StylesheetTheme="Standard" AutoEventWireup="true" CodeBehind="BFCWeeklyAttendanceSummaryRpt.aspx.cs"
    MasterPageFile="~/Report/Reports.Master" Inherits="CMSWeb.Reports.BFCWeeklyAttendanceSummaryRpt" %>

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
            BFC Division Weekly Attendance Summary</h1>
        Sunday Date:&nbsp;<asp:TextBox ID="SundayDate" runat="server" AutoPostBack="True"
            Style="font-size: large" Width="100"></asp:TextBox>
        <cc2:CalendarExtender ID="SundayDateExtender" runat="server" TargetControlID="SundayDate">
        </cc2:CalendarExtender>
        <hr />
        <asp:ListView ID="ListView1" runat="server" DataSourceID="dsBFCWeeklyAttendance">
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" style="width: 500px" align="center">
                    <tr id="Tr1" runat="server" class="headerrow">
                        <th id="Th1" align="left" runat="server">
                            Division Name
                        </th>
                        <th id="Th2" align="right" runat="server">
                            8:00
                        </th>
                        <th id="Th3" align="right" runat="server">
                            9:30
                        </th>
                        <th id="Th4" align="right" runat="server">
                            11:00
                        </th>
                        <th id="Th5" align="right" runat="server">
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
                        <asp:Label ID="count1Label" runat="server" Text='<%# Eval("Cnt800", "{0:n0}") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="count2Label" runat="server" Text='<%# Eval("Cnt930", "{0:n0}") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="count3Label" runat="server" Text='<%# Eval("Cnt1100", "{0:n0}") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="totalLabel" runat="server" Text='<%# Eval("Total", "{0:n0}") %>' />
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table id="Table1" runat="server" style="">
                    <tr>
                        <td>
                            No data was returned.
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
    <asp:ObjectDataSource ID="dsBFCWeeklyAttendance" runat="server" SelectMethod="BFCWeeklyAttendanceSummary"
        TypeName="CMSPresenter.BFCAttendSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SundayDate" Name="sunday" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
