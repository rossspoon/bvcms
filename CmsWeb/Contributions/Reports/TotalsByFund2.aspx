<%@ Page Title="" Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Contributions/Reports/Reports.Master"
    AutoEventWireup="true" CodeBehind="TotalsByFund2.aspx.cs" Inherits="CMSWeb.Contributions.Reports.TotalsByFund2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center; font-size: large">
        <div>
            <h1>
                <asp:Label ID="Label1" runat="server" Text="Totals By Fund"></asp:Label></h1>
            <table style="text-align: left">
                <tr>
                    <td style="text-align: right">
                        CampusId:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="CampusId" DataSourceID="CampusIds"  DataTextField="Value"
                        DataValueField="Id"></asp:DropDownList>
                    </td>
                        <td>
                            &nbsp;
                        </td>
                </tr>
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
                        <asp:Button ID="btnSubmit" ToolTip="Click to run report" CausesValidation="true"
                            runat="server" Text="Run" CssClass="noPrint" ValidationGroup="vgDates" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
            <asp:CompareValidator ID="FromDateValidator" runat="server" ErrorMessage="CompareValidator"
                ControlToValidate="FromDate" ControlToCompare="ToDate" Operator="LessThanEqual"
                SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="From Date must be before To Date."
                CssClass="noPrint"></asp:CompareValidator><br />
        </div>
        <hr />
        <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1" OnDataBound="ListView1_DataBound"
            Visible="False" onitemdatabound="ListView1_ItemDataBound">
            <ItemTemplate>
                <tr style="">
                    <td align="left">
                        <asp:HyperLink ID="FundNameLink" runat="server" 
                            Text='<%# Eval("FundName") %>'></asp:HyperLink>
                    </td>
                    <td align="right">
                        <asp:Label ID="TotalLabel" runat="server" Text='<%# Eval("Total", "{0:#,###.00}") %>' />
                    </td>
                    <td align="right">
                        <asp:Label ID="CountLabel" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
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
            <LayoutTemplate>
                <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                    <tr id="Tr1" runat="server" style="">
                        <th id="Th1" runat="server">
                            FundName
                        </th>
                        <th id="Th2" runat="server">
                            Total
                        </th>
                        <th id="Th3" runat="server">
                            Count
                        </th>
                    </tr>
                    <tr id="itemPlaceholder" runat="server">
                    </tr>
                    <tr style="border-top: 2px solid black">
                        <th align="left">
                            Total
                        </th>
                        <th align="right">
                            <asp:Label ID="Total" runat="server" />
                        </th>
                        <th align="right">
                            <asp:Label ID="Count" runat="server" />
                        </th>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="TotalsByFund" TypeName="CMSPresenter.BundleController" OnObjectCreated="ObjectDataSource1_ObjectCreated">
            <SelectParameters>
                <asp:ControlParameter Name="dt1" ControlID="FromDate" Type="DateTime" />
                <asp:ControlParameter Name="dt2" ControlID="ToDate" Type="DateTime" />
                <asp:QueryStringParameter Name="Pledges" Type="Boolean" QueryStringField="pledged"
                    DefaultValue="false" />
            <asp:ControlParameter Name="CampusId" ControlID="CampusId" Type="Int32" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="CampusIds" runat="server" SelectMethod="AllCampuses0" TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    </div>
</asp:Content>
