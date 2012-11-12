<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChurchAttendanceSummaryRpt.aspx.cs"
    MasterPageFile="~/Report/Reports.Master" Inherits="CmsWeb.ChurchAttendanceSummary" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            font-size: 110%;
        }
        .totalrow td
        {
            border-top: 2px solid black;
        }
        .headerrow td
        {
            border-bottom: 2px solid black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align:center">
        <h1>
            Church Attendance Summary</h1>
        <span>Sunday Date:&nbsp;</span><asp:TextBox ID="SundayDate" runat="server" AutoPostBack="True"
            Style="font-size: 110%" Width="100"></asp:TextBox>
        <cc2:CalendarExtender ID="SundayDateExtender" runat="server" TargetControlID="SundayDate">
        </cc2:CalendarExtender>
        <hr />
<%--        <asp:ListView ID="ListView1" runat="server" DataSourceID="dsAttendanceSummary">
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" cellpadding="2"
                    style="width: 400px" align="center">
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'>
                    <td align="left">
                        <asp:HyperLink ID="hlStatDetail" runat="server" Visible='<%# Eval("UseLink") %>'
                            NavigateUrl='<%# Eval("Link")%>' Text='<%# Eval("Name") %>' Style="font-size: large"
                            ToolTip="Right Click to open in a new tab"></asp:HyperLink>
                        <asp:Label ID="Label1" runat="server" Visible='<%# Eval("NoLink") %>' Text='<%# Eval("Name") %>'></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblCount" runat="server" Text='<%# Eval("Count","{0:#,0}")%>' Style="font-size: large"></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table id="Table1" runat="server" style="" align="center">
                    <tr>
                        <td>
                            No data was returned.
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
        <br />
--%>        <h2>
            Information Source</h2>
        <asp:ListView ID="InterestPointInfo" runat="server" DataSourceID="dsInterestPointInfo">
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" cellpadding="2"
                    style="width: 400px" align="center">
                    <tr class="headerrow">
                        <th>
                            &nbsp;
                        </th>
                        <th align="right">
                            Guests
                        </th>
                        <th align="right">
                            Prospects
                        </th>
                        <th align="right">
                            %
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr <%# (string)Eval("Interest") == "Total" ? "class='totalrow'" : "" %> style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'>
                    <td align="left">
                        <asp:Label ID="lblSource" runat="server" Text='<%# Bind("Interest")%>'></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblGuestSourceCount" runat="server" Text='<%# Eval("GuestCount","{0:#,0}")%>'></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblProspectSourceCount" runat="server" Text='<%# Eval("ProspectCount","{0:#,0}")%>'></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblProspectSourcePct" runat="server" Text='<%# Eval("Pct","{0:N1}")%>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table id="Table1" runat="server" style="" align="center">
                    <tr>
                        <td>
                            No data was returned.
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
        <h2>
            Guest Central Flow</h2>
        <asp:ListView ID="ListView2" runat="server" DataSourceID="dsGuestCentral">
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0" cellpadding="2"
                    style="width: 400px" align="center">
                    <tr class="headerrow">
                        <th>
                            &nbsp;
                        </th>
                        <th align="right">
                            Metro
                        </th>
                        <th align="right">
                            Outside Metro
                        </th>
                        <th align="right">
                            Total
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr <%# (string)Eval("HourLabel") == "Total" ? "class='totalrow'" : "" %> style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'>
                    <td align="left">
                        <asp:Label ID="lblHour" runat="server" Text='<%# Eval("HourLabel")%>'></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblMetroMarginalCount" runat="server" Text='<%# Eval("MetroCount")%>'></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblExMetroMarginalCount" runat="server" Text='<%# Eval("OutsideCount")%>'></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblTotalCount" runat="server" Text='<%# Eval("Total")%>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table id="Table1" runat="server" style="" align="center">
                    <tr>
                        <td>
                            No data was returned.
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
<%--    <asp:ObjectDataSource ID="dsAttendanceSummary" runat="server" SelectMethod="AttendanceSummary"
        TypeName="CMSPresenter.ChurchAttendanceSummaryController" 
        onobjectcreating="ODS_ObjectCreating">
        <SelectParameters>
            <asp:ControlParameter Name="sunday" Type="DateTime" ControlID="SundayDate" />
        </SelectParameters>
    </asp:ObjectDataSource>
--%>    <asp:ObjectDataSource ID="dsInterestPointInfo" runat="server" SelectMethod="GuestInterestPoints"
        TypeName="CMSPresenter.ChurchAttendanceSummaryController" onobjectcreating="ODS_ObjectCreating">
        <SelectParameters>
            <asp:ControlParameter Name="sunday" Type="DateTime" ControlID="SundayDate" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="dsGuestCentral" runat="server" SelectMethod="GuestCentral"
        TypeName="CMSPresenter.ChurchAttendanceSummaryController" onobjectcreating="ODS_ObjectCreating">
        <SelectParameters>
            <asp:ControlParameter Name="sunday" Type="DateTime" ControlID="SundayDate" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
