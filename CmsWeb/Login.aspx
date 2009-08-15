<%@ Page Language="C#" StylesheetTheme="Standard" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CMSWeb.Login" %>

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
            <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
            <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
        </asp:Login>
    </div>
    </form>
</body>
</html>
