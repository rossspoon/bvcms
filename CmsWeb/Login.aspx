<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CmsWeb.Login" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CMS2 Login Page</title>
</head>
<body>
    <form id="form1" runat="server" style="font-family:Arial">
    <div>
        <h2>
            Terms of use
        </h2>
        <asp:Label ID="TermsLabel" runat="server">
        </asp:Label>
        <asp:Login ID="Login1" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderPadding="4"
            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="1em" ForeColor="#333333"
            DisplayRememberMe="False" TitleText="Log In to CMS" OnLoggedIn="Login1_LoggedIn"
            OnLoginError="Login1_LoginError" OnAuthenticate="Login1_Authenticate" 
            PasswordRecoveryText="forgot password?" 
            PasswordRecoveryUrl="/Account/ForgotPassword">
            <TextBoxStyle Font-Size="0.8em" />
            <LoginButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px"
                Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
            <LayoutTemplate>
                <table border="0" cellpadding="4" cellspacing="0" 
                    style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table border="0" cellpadding="0">
                                <tr>
                                    <td align="center" colspan="2" 
                                        style="color:White;background-color:#507CD1;font-size:0.9em;font-weight:bold;">
                                        Log In to CMS</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                            ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Password is required." 
                                            ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Button ID="LoginButton" runat="server" BackColor="White" 
                                            BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" 
                                            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" Text="Log In" 
                                            ValidationGroup="Login1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:HyperLink ID="PasswordRecoveryLink" runat="server" 
                                            NavigateUrl="/Account/ForgotPassword">forgot password?</asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:HyperLink ID="HyperLink1" runat="server" 
                                            NavigateUrl="/Account/ForgotUsername">forgot username?</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
        </asp:Login>
    </div>
    </form>
</body>
</html>
