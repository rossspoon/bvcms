<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" Inherits="Disciples._Default"
    Title="Welcome to GO Disciples" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="front">
        <div id="thoughts">
            <h1>
                <asp:Label ID="Label1" runat="server" EnableViewState="false" Text=""></asp:Label>
            </h1>
            <cms:Paragraph ID="defaultTop" EnableViewState="false" runat="server" />
            <asp:Panel ID="Panel1" runat="server">
                <asp:Login ID="Login1" runat="server" LoginButtonText="Sign In" PasswordLabelText="Password"
                    TitleText="Sign In" RememberMeSet="True" UserNameLabelText="User Name" EnableViewState="False" 
                     onauthenticate="Login1_Authenticate" OnLoggedIn="Login1_LoggedIn">
                    <TextBoxStyle CssClass="CommonTextBig" />
                    <LoginButtonStyle CssClass="CommonTextButton Big" />
                    <TitleTextStyle CssClass="CommonMessageTitle" />
                    <LabelStyle CssClass="CommonFormFieldName" />
                    <LayoutTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" style="border-collapse: collapse">
                            <tr>
                                <td>
                                    <a name="login"></a>
                                    <table border="0" cellpadding="0">
                                        <tr>
                                            <td align="center" class="CommonMessageTitle" colspan="2">
                                                Sign In
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="CommonFormFieldName">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User 
                                                    Name</asp:Label>
                                            </td>
                                            <td>
                                                <cc1:DefaultButtonTextBox ID="UserName" runat="server" CssClass="CommonTextBig" ButtonId="LoginButton"></cc1:DefaultButtonTextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                    ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" class="CommonFormFieldName">
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password</asp:Label>
                                            </td>
                                            <td>
                                                <cc1:DefaultButtonTextBox ID="Password" runat="server" CssClass="CommonTextBig" TextMode="Password"
                                                    ButtonId="LoginButton"></cc1:DefaultButtonTextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                    ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." Checked="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:HyperLink ID="ForgotUsername" runat="server" 
                                                    ToolTip="Retrieve your username">Forgot Username</asp:HyperLink>
                                                ?</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:HyperLink ID="ForgotPassword" runat="server" ToolTip="Reset your password">Forgot Password</asp:HyperLink>
                                                ?</td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color: red">
                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" CssClass="CommonTextButton Big"
                                                    Text="Sign In" ValidationGroup="Login1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                </asp:Login>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSideBar" runat="server">
    <div id="extra" class="front">
        <div class="panel">
            <h4>
                Featured Items
            </h4>
            <cms:Paragraph ID="p1" ContentName="FeaturedItems" runat="server" />
        </div>
        <div class="panel">
            <h4>
                Places to go,<br />
                Things to do
            </h4>
            <cms:Paragraph ID="p2" ContentName="PlacesToGo" runat="server" />
        </div>
    </div>
</asp:Content>
