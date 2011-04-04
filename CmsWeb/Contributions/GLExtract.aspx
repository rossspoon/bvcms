<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GLExtract.aspx.cs" Inherits="CmsWeb.Contributions.GLExtract" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align: center">
        <div style="text-align: center">
            <h1>
                General Ledger Extract</h1>
            <table style="font-size: 110%; text-align: left">
                <tr>
                    <td style="text-align: right">
                        From Date:
                    </td>
                    <td>
                        <asp:TextBox ID="FromDate" runat="server" AutoPostBack="false" Style="font-size: 110%"
                            Width="100"></asp:TextBox>
                        <cc2:CalendarExtender ID="FromExtender" runat="server" TargetControlID="FromDate">
                        </cc2:CalendarExtender>
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
                        <asp:TextBox ID="ToDate" runat="server" AutoPostBack="false" Style="font-size: 110%"
                            Width="100"></asp:TextBox>
                        <cc2:CalendarExtender ID="ToDateExtender" runat="server" TargetControlID="ToDate">
                        </cc2:CalendarExtender>
                    </td>
                    <td>
                        <asp:Button ID="btnSubmit" ToolTip="Click to download report" CausesValidation="true"
                            runat="server" Text="Download" CssClass="noPrint" 
                            ValidationGroup="vgDates" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
            <asp:CompareValidator ID="ToDateValidator" runat="server" ErrorMessage="CompareValidator"
                ControlToValidate="ToDate" ControlToCompare="FromDate" Operator="GreaterThanEqual"
                SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="To Date must be on or after From Date."
                CssClass="noPrint"></asp:CompareValidator>
        </div>
    </div>
</asp:Content>
