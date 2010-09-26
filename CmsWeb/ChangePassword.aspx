<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="CmsWeb.ChangePassword" validateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Change Password</title>
    <style type="text/css">
        .style1
        {
            height: 43px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ChangePassword ID="ChangePassword1" runat="server" BackColor="#EFF3FB" 
            BorderColor="#B5C7DE" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" 
            ContinueDestinationPageUrl="~/" Font-Names="Verdana" Font-Size="1em" 
            onchangedpassword="ChangePassword1_ChangedPassword" 
            ChangePasswordFailureText="Password incorrect or New Password invalid. New Password length minimum: {0}. Non-alphanumeric characters required (like punctuation): {1}." >
            <CancelButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" 
                BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" ForeColor="#284E98" />
            <PasswordHintStyle Font-Italic="True" ForeColor="#507CD1" />
            <ContinueButtonStyle BackColor="White" BorderColor="#507CD1" 
                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" 
                ForeColor="#284E98" />
            <ChangePasswordButtonStyle BackColor="White" BorderColor="#507CD1" 
                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" 
                ForeColor="#284E98" />
            <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="1em" 
                ForeColor="White" />
            <ChangePasswordTemplate>
                <table border="0" cellpadding="4" cellspacing="0" 
                    style="border-collapse:collapse" >
                    <tr>
                        <td align="left">
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2" 
                                        style="color:White;background-color:#507CD1;font-size:1em;font-weight:bold;">
                                        Change Your Password</td>
                                    <td align="center" 
                                        style="color:White;background-color:#507CD1;font-size:1em;font-weight:bold;">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="CurrentPasswordLabel" runat="server" 
                                            AssociatedControlID="CurrentPassword">Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CurrentPassword" runat="server" Font-Size="1em" 
                                            TextMode="Password"></asp:TextBox>
                                        <br />
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top" nowrap="nowrap" class="style1">
                                        <asp:Label ID="NewPasswordLabel" runat="server" 
                                            AssociatedControlID="NewPassword">New Password:</asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="NewPassword" runat="server" Font-Size="1em" 
                                            TextMode="Password"></asp:TextBox> &nbsp;<br />
                                        <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" 
                                            ControlToValidate="NewPassword" ErrorMessage="New Password is required." 
                                            ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td width="50%" valign="top">
                                        (at least 7 characters with 1 non-alphanumeric)</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="ConfirmNewPasswordLabel" runat="server" 
                                            AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ConfirmNewPassword" runat="server" Font-Size="1em" 
                                            TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" 
                                            ControlToValidate="ConfirmNewPassword" 
                                            ErrorMessage="Confirm New Password is required." 
                                            ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:CompareValidator ID="NewPasswordCompare" runat="server" 
                                            ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" 
                                            Display="Dynamic" 
                                            ErrorMessage="The Confirm New Password must match the New Password entry." 
                                            ValidationGroup="ChangePassword1"></asp:CompareValidator>
                                    </td>
                                    <td align="center">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="3" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                    <td align="left" style="color:Red;">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                    </td>
                                    <td colspan="2">
                                                                                <asp:Button ID="ChangePasswordPushButton" runat="server" BackColor="White" 
                                            BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" 
                                            CommandName="ChangePassword" Font-Names="Verdana" Font-Size="1em" 
                                            ForeColor="#284E98" Text="Change Password" ValidationGroup="ChangePassword1" />
&nbsp;<asp:Button ID="CancelPushButton" runat="server" BackColor="White" 
                                            BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" 
                                            CausesValidation="False" CommandName="Cancel" Font-Names="Verdana" 
                                            Font-Size="1em" ForeColor="#284E98" Text="Cancel" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ChangePasswordTemplate>
            <TextBoxStyle Font-Size="1em" />
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
        </asp:ChangePassword>    
    </div>
    </form>
</body>
</html>
