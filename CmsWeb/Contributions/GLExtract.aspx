<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="GLExtract.aspx.cs" Inherits="CmsWeb.Contributions.GLExtract" EnableEventValidation="false" %>

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
                    </td>
                    <td>
                        <asp:Button ID="btnSubmit" ToolTip="Click to download report" CausesValidation="true"
                            runat="server" Text="Download" CssClass="noPrint" 
                            OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Contentscr" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>