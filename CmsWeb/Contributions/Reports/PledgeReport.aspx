<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Contributions/Reports/Reports.Master" CodeBehind="PledgeReport.aspx.cs" Inherits="CmsWeb.Contributions.Reports.PledgeReport" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center; font-size: 110%">
        <div>
            <h1>
                Pledge Report</h1>
            <table style="text-align: left">
                <tr>
                    <td style="text-align: right">
                        To Date:
                    </td>
                    <td>
                        <asp:TextBox ID="ToDt" runat="server" AutoPostBack="false" Style="font-size: 110%"
                            Width="100"></asp:TextBox>
                        <cc1:CalendarExtender ID="ToDateExtender" runat="server" TargetControlID="ToDt">
                        </cc1:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="Run" ToolTip="Click to run report" CausesValidation="true"
                            runat="server" Text="Run" CssClass="noPrint" ValidationGroup="vgDates" OnClick="Run_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <asp:GridView ID="GridView1" CssClass="aspgrid" runat="server" AutoGenerateColumns="False" 
            DataSourceID="ObjectDataSource1"
        CellPadding="4" ForeColor="#333333" GridLines="None" >
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#4296CC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#4296CC" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="FundId" HeaderText="FundId" 
                    SortExpression="FundId" />
                <asp:BoundField DataField="FundName" HeaderText="FundName" 
                    SortExpression="FundName" >
                <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Pledged" DataFormatString="{0:N}" 
                    HeaderText="Pledged" SortExpression="Pledged">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="ToPledge" DataFormatString="{0:N}" 
                    HeaderText="ToPledge" SortExpression="ToPledge">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="NotToPledge" DataFormatString="{0:N}" 
                    HeaderText="NotToPledge" SortExpression="NotToPledge">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="ToFund" DataFormatString="{0:N}" HeaderText="ToFund" 
                    SortExpression="ToFund">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="FetchPledgeData" 
            TypeName="CmsWeb.Contributions.Reports.PledgeReportODS">
            <SelectParameters>
                <asp:ControlParameter ControlID="ToDt" Name="dt" PropertyName="Text" 
                    Type="DateTime" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>