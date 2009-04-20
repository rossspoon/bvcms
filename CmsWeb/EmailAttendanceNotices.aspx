<%@ Page Title="" Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="EmailAttendanceNotices.aspx.cs" Inherits="CMSWeb.EmailAttendanceNotices" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
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
            <td class="style1">
                Attendance Date:
            </td>
            <td class="style1">
                <asp:TextBox ID="EndDate" runat="server"></asp:TextBox><cc2:calendarextender id="CalendarExtender2"
                        runat="server" targetcontrolid="EndDate">
                                            </cc2:calendarextender>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="EndDate" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Program:
            </td>
            <td>
                <cc1:DropDownCC ID="DivOrg" runat="server" AutoPostBack="True" DataTextField="Value"
                    DataValueField="Id" OnSelectedIndexChanged="DivOrg_SelectedIndexChanged" AppendDataBoundItems="True">
                </cc1:DropDownCC>
                <asp:CustomValidator ID="CustomValidator1" runat="server" 
                    ErrorMessage="Must select" onservervalidate="CustomValidator2_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                Division:
            </td>
            <td>
                <cc1:DropDownCC ID="SubDivOrg" runat="server" AutoPostBack="True" DataTextField="Value"
                    DataValueField="Id" OnSelectedIndexChanged="SubDivOrg_SelectedIndexChanged">
                </cc1:DropDownCC>
                <asp:CustomValidator ID="CustomValidator2" runat="server" 
                    ErrorMessage="Must select" onservervalidate="CustomValidator2_ServerValidate"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                Organization:
            </td>
            <td>
                <cc1:DropDownCC ID="Organization" runat="server" DataTextField="Value" DataValueField="Id">
                </cc1:DropDownCC>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                    Text="Send Email Notice" />
            </td>
        </tr>
    </table>
                <asp:Label ID="Label1" runat="server" Text="Email has been sent" Font-Size="Large"
                    ForeColor="#58DF55" Height="30px" Visible="False"></asp:Label>
 </asp:Content>
