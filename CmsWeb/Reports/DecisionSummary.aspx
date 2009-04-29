<%@ Page Title="" Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="DecisionSummary.aspx.cs" Inherits="CMSWeb.StaffOnly.DecisionSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            font-size: large;
        }
        .TotalLine
        {
            border-top-style: solid;
            border-top-width: thin;
            border-top-color: Black;
            font-weight: bold;
        }
        .HeaderLine
        {
            border-bottom-style: solid;
            border-bottom-width: thin;
            border-bottom-color: Black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center">
    <h1>
        Decision Summary Report</h1>
        <table style="font-size: large; text-align: left" align="center">
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
        <asp:CompareValidator ID="ToDateValidator" runat="server" ErrorMessage="CompareValidator"
            ControlToValidate="ToDate" ControlToCompare="FromDate" Operator="GreaterThanEqual"
            SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="To Date must be after From Date."
            CssClass="noPrint"></asp:CompareValidator>
        <hr />
        <table cellpadding="4" align="center">
            <tr>
                <td colspan="2">
                    <asp:ListView ID="DecisionsView" runat="server" DataSourceID="ODSDecisions" 
                        onitemcommand="ItemCommand">
                        <LayoutTemplate>
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="" align="center">
                                <tr id="Tr7" runat="server" style="">
                                    <th class="HeaderLine" id="Th13" runat="server" colspan="2">
                                        Decisions
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left" class='<%# Eval("CssClass") %>'>
                                    <asp:LinkButton ID="DetailButton" CommandName="ForDecisionType" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Desc") %>' runat="server"></asp:LinkButton>
                                </td>
                                <td align="right" class='<%# Eval("CssClass") %>'>
                                    <asp:Label ID="CountLabel" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table id="Table1" runat="server" style="" align="center">
                                <tr>
                                    <th>
                                        No Decisions
                                    </th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:ListView ID="BaptismsByGenderView" runat="server" 
                        DataSourceID="ODSBaptismsByGender" onitemcommand="ItemCommand">
                        <LayoutTemplate>
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr id="Tr9" runat="server" style="">
                                    <th class="HeaderLine" id="Th15" runat="server" colspan="2">
                                        Baptisms by Gender
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left" class='<%# Eval("CssClass") %>'>
                                    <asp:LinkButton ID="DetailButton" CommandName="ForBaptismGender" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Desc") %>' runat="server"></asp:LinkButton>
                                </td>
                                <td align="right" class='<%# Eval("CssClass") %>'>
                                    <asp:Label ID="CountLabel" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table id="Table3" runat="server" style="">
                                <tr>
                                    <th>
                                        No Baptisms
                                    </th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
                <td valign="top">
                    <asp:ListView ID="BaptismsByTypeView" runat="server" 
                        DataSourceID="ODSBaptismsByType" onitemcommand="ItemCommand">
                        <LayoutTemplate>
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr id="Tr10" runat="server" style="">
                                    <th class="HeaderLine" id="Th16" runat="server" colspan="2">
                                        Baptisms by Type
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left" class='<%# Eval("CssClass") %>'>
                                    <asp:LinkButton ID="DetailButton" CommandName="ForBaptismType" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Desc") %>' runat="server"></asp:LinkButton>
                                </td>
                                <td align="right" class='<%# Eval("CssClass") %>'>
                                    <asp:Label ID="CountLabel" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:ListView ID="NewMemberView" runat="server" 
                        DataSourceID="ODSNewMemberByType" onitemcommand="ItemCommand">
                        <LayoutTemplate>
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr id="Tr11" runat="server" style="">
                                    <th class="HeaderLine" id="Th17" runat="server" colspan="2">
                                        New Members by Type
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left" class='<%# Eval("CssClass") %>'>
                                    <asp:LinkButton ID="DetailButton" CommandName="ForNewMemberType" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Desc") %>' runat="server"></asp:LinkButton>
                                </td>
                                <td align="right" class='<%# Eval("CssClass") %>'>
                                    <asp:Label ID="CountLabel" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table id="Table4" runat="server" style="">
                                <tr>
                                    <th>
                                        No New Members
                                    </th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
                <td valign="top">
                    <asp:ListView ID="DroppedMemberView" runat="server" 
                        DataSourceID="ODSDroppedMemberByType" onitemcommand="ItemCommand">
                        <LayoutTemplate>
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr id="Tr12" runat="server" style="">
                                    <th class="HeaderLine" id="Th18" runat="server" colspan="2">
                                        Dropped Members by Type
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left" class='<%# Eval("CssClass") %>'>
                                    <asp:LinkButton ID="DetailButton" CommandName="ForDropType" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("Desc") %>' runat="server"></asp:LinkButton>
                                </td>
                                <td align="right" class='<%# Eval("CssClass") %>'>
                                    <asp:Label ID="CountLabel" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <table id="Table5" runat="server" style="">
                                <tr>
                                    <th>
                                        No Dropped Members
                                    </th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ListView ID="DroppedMemberChurchView" runat="server" 
                        DataSourceID="ODSDroppedMembersByChurch" onitemcommand="ItemCommand">
                        <LayoutTemplate>
                            <table id="itemPlaceholderContainer" runat="server" border="0" style="" align="center">
                                <tr id="Tr13" runat="server" style="">
                                    <th class="HeaderLine" id="Th19" runat="server" colspan="2">
                                        Dropped Members by Church
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="left" class='<%# Eval("CssClass") %>'>
                                    <asp:LinkButton ID="DetailButton" CommandName="DroppedForChurch" CommandArgument='<%# Eval("Desc") %>' Text='<%# Eval("Desc") %>' runat="server"></asp:LinkButton>
                                </td>
                                <td align="right" class='<%# Eval("CssClass") %>'>
                                    <asp:Label ID="CountLabel" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </div>
    <asp:ObjectDataSource ID="ODSDecisions" runat="server" SelectMethod="DecisionsByType"
        TypeName="CMSPresenter.DecisionSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDate" Name="dt1" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ToDate" Name="dt2" PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSBaptismsByGender" runat="server" SelectMethod="BaptismsByGender"
        TypeName="CMSPresenter.DecisionSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDate" Name="dt1" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ToDate" Name="dt2" PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSBaptismsByType" runat="server" SelectMethod="BaptismsByType"
        TypeName="CMSPresenter.DecisionSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDate" Name="dt1" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ToDate" Name="dt2" PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSNewMemberByType" runat="server" SelectMethod="NewMemberByType"
        TypeName="CMSPresenter.DecisionSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDate" Name="dt1" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ToDate" Name="dt2" PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSDroppedMemberByType" runat="server" SelectMethod="DroppedMemberByType"
        TypeName="CMSPresenter.DecisionSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDate" Name="dt1" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ToDate" Name="dt2" PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSDroppedMembersByChurch" runat="server" SelectMethod="DroppedMemberByChurch"
        TypeName="CMSPresenter.DecisionSummaryController">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDate" Name="dt1" PropertyName="Text" Type="DateTime" />
            <asp:ControlParameter ControlID="ToDate" Name="dt2" PropertyName="Text" Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
