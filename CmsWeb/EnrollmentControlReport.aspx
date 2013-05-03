<%@ Page Title="Enrollment Control" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EnrollmentControlReport.aspx.cs" Inherits="CmsWeb.EnrollmentControlReport" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Contentscr" ContentPlaceHolderID="scripts" runat="server">
    <script type="text/javascript">
        function Report() {
            var div = $get('<%= DivOrg.ClientID %>').value;
            if (div == "0") {
                alert('select a program');
                return false;
            }
            url = '/Reports/EnrollmentControl/'
                + "?div=" + $get('<%= DivOrg.ClientID %>').value
                + "&subdiv=" + $get('<%= SubDivOrg.ClientID %>').value
                + "&schedule=" + $get('<%= Schedule.ClientID %>').value;
            window.open(url, "_blank");
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 29px;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="modalPopup">
        <tr>
            <td>
                Program:
            </td>
            <td>
                <cc1:DropDownCC ID="DivOrg" runat="server" AutoPostBack="True" DataTextField="Value"
                    DataValueField="Id" OnSelectedIndexChanged="DivOrg_SelectedIndexChanged" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">(not specified)</asp:ListItem>
                </cc1:DropDownCC>
            </td>
        </tr>
        <tr>
            <td>
                Division:
            </td>
            <td>
                <cc1:DropDownCC ID="SubDivOrg" runat="server" DataTextField="Value"
                    DataValueField="Id" AutoPostBack="true"
                    onselectedindexchanged="SubDivOrg_SelectedIndexChanged">
                </cc1:DropDownCC>
            </td>
        </tr>
        <tr>
            <td>
                Schedule:
            </td>
            <td>
                <cc1:DropDownCC ID="Schedule" runat="server" DataTextField="Value" DataValueField="Id">
                </cc1:DropDownCC>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="Run" runat="server"  OnClientClick="Report()" Text="Run Report" />
            </td>
        </tr>
    </table>
</asp:Content>
