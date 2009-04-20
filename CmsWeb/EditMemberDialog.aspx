<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMemberDialog.aspx.cs"
    Inherits="CMSWeb.EditMemberDialog" StylesheetTheme="Minimal" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Edit Member Dialog</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="hidden" id="retval" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <table>
            <tr>
            <td></td>
                <td>
                    <asp:Label ID="Name" runat="server" Text='Name'></asp:Label>
                </td>
            </tr>
            <tr>
            <td>Member Type:</td>
                <td> 
                    <cc1:DisplayOrEditDropDown ID="MemberTypeId" BindingMode="TwoWay" BindingSource="OrgMember"
                        DataSourceID="MemberTypeData" DataTextField="Value" DataValueField="Id" runat="server">
                    </cc1:DisplayOrEditDropDown>
                </td>
            </tr>
            <tr>
            <td>Attendance:</td>
                <td> 
                    <asp:HyperLink ID="AttendString" runat="server" style="font-family: Courier New"></asp:HyperLink>
                </td>
            </tr>
            <tr>
                <td>Inactive Date:</td>
                <td>
                    <cc1:DisplayOrEditDate ID="InactiveDate" BindingSource="OrgMember" BindingMode="TwoWay" runat="server"></cc1:DisplayOrEditDate>
                    <cc2:CalendarExtender ID="CalendarExtender1" TargetControlID="InactiveDate" runat="server">
                    </cc2:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td>Enrollment Date:</td>
                <td>
                    <cc1:DisplayOrEditDate ID="EnrollmentDate" BindingSource="OrgMember" BindingMode="OneWay" runat="server"></cc1:DisplayOrEditDate>
                </td>
            </tr>
            <tr>
            <td>VIP Week:</td>
                <td>
                    
                    <cc1:DisplayOrEditCheckbox ID="VipWeek1" Text="1" BindingSource="OrgMember" runat="server" />
                    <cc1:DisplayOrEditCheckbox ID="VipWeek2" Text="2" BindingSource="OrgMember" runat="server" />
                    <cc1:DisplayOrEditCheckbox ID="VipWeek3" Text="3" BindingSource="OrgMember" runat="server" />
                    <cc1:DisplayOrEditCheckbox ID="VipWeek4" Text="4" BindingSource="OrgMember" runat="server" />
                    <cc1:DisplayOrEditCheckbox ID="VipWeek5" Text="5" BindingSource="OrgMember" runat="server" />
                </td>
            </tr>
            <tr>
            <td></td>
                <td>
                    <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" 
                        onclick="EditUpdateButton1_Click" />
                    <asp:ImageButton ID="Delete" runat="server" 
                        ImageUrl="~/images/delete.gif" 
                        onclick="Delete_Click" />
                    <cc2:ConfirmButtonExtender ID="Delete_ConfirmButtonExtender" 
                        runat="server" ConfirmText="Are you sure you want to delete?" Enabled="True" TargetControlID="Delete">
                    </cc2:ConfirmButtonExtender>
                </td>
            </tr>
        </table>
        <asp:ObjectDataSource ID="MemberTypeData" runat="server" SelectMethod="MemberTypeCodes"
            TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
