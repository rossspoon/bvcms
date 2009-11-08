<%@ Page Language="C#" StylesheetTheme="Default" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Disciples.Register"
    EnableViewState="false" Title="GO Disciples Registration" CodeBehind="Register.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">

    <script language="javascript" type="text/javascript">
        function ValidateTermsOfUse(source, args) {
            args.IsValid = document.getElementById('<%= chkAgree.ClientID %>').checked;
        } 
    </script>

    <div id="main" class="wide">
        <h1>
            Registration</h1>
        <table width=580 style='table-layout:fixed'>
             <col width=140>
             <col width=140>
             <col width=300>
             <tr>
                <td align="right" valign="top">
                    First Name:
                </td>
                <td>
                    <asp:TextBox ID="txtFirst" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFirst"
                        Display="Dynamic">Required</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    Last Name:
                </td>
                <td>
                    <asp:TextBox ID="txtLast" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLast"
                        Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    Birthday:
                </td>
                <td>
                    <asp:TextBox ID="txtBirthday" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtBirthday"
                        Display="Dynamic" EnableClientScript="true">Required</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="BirthdayValid" runat="server" ControlToValidate="txtBirthday"
                        OnServerValidate="BirthdayValid_ServerValidate" Display="Dynamic">Bad Date</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    Email address:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                </td>
                <td>
                    (we use this to notify you of updated content, and to reset your password
                    if you forget it)<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="txtEmail" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="emailValidator" runat="server" 
                        OnServerValidate="emailValidator_ServerValidate" Display="Dynamic"><br />Invalid email address</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    User Name:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtLogin" runat="server"></asp:TextBox>
                </td>
                <td>
                    (you will use this to logon to the system. NOTE: don't use your email address)<asp:RequiredFieldValidator
                        ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLogin" Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="usernameValidator" runat="server" Display="Dynamic"><br />Need to use another login - this one's taken</asp:CustomValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="^[^ ]*$"
                        ControlToValidate="txtLogin" Display="Dynamic"><br />No spaces allowed</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    Password:
                </td>
                <td valign="top">
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    (at least four characters)
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPassword"
                        Display="Dynamic"><br />Required</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="validatePassword" runat="server" Display="Dynamic"><br />Invalid password. Needs to be 4 or more letters/numbers</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="right" valign="top">
                    Confirm:
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    (type your password again)<asp:CompareValidator ID="CompareValidator1" runat="server"
                        ControlToCompare="txtPassword" ControlToValidate="TextBox1" 
                        Display="Dynamic"><br />Passwords don't match</asp:CompareValidator>
                </td>
            </tr>
            <%--            <tr>
                <td>
                    Password Reminder Question (we'll ask you this if you forget your password)<br />
                    <asp:DropDownList ID="ddQ" runat="server">
                    <asp:ListItem>What is your favorite food?</asp:ListItem>
                    <asp:ListItem>What was your favorite place to visit as a child?</asp:ListItem>
                    <asp:ListItem>Who is your favorite actor, musician, or artist?</asp:ListItem>
                    <asp:ListItem>What phone number do you remember as a child?</asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="txtQ" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtQ"
                        Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    Password Reminder Answer (you will need to answer this exactly to retrieve your
                    password)<br />
                    <asp:TextBox ID="txtA" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtA"
                        Display="Dynamic" ErrorMessage="Required"></asp:RequiredFieldValidator>
                </td>
            </tr>
--%>
            <tr>
                <td align="right" nowrap="nowrap" valign="top">
                    Group Invitation Code:
                </td>
                <td valign="top">
                    <asp:TextBox ID="SecretCode" runat="server"></asp:TextBox>
                </td>
                <td>
                    (<b>optional</b>, if you have been given one of these to join a group)<asp:CustomValidator
                        ID="SecretCodeValidator" runat="server" ControlToValidate="SecretCode" 
                        OnServerValidate="SecretCodeValidator_ServerValidate" Display="Dynamic"><br />Code expired or invalid, (you can clear this out to register without joining a group)</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <p>
                        <br />
                        Scroll down to read and agree to the Terms of Use (check the box),
                        <br />
                        then click Register at the bottom of the page.</p>
                    <h3>
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </h3>
                    <cms:Paragraph ID="Paragraph1" runat="server" ContentName="termsofuse" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="2">
                    <asp:CheckBox Text="I agree to the terms of use" ID="chkAgree" runat="server" />
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateTermsOfUse"
                        ErrorMessage="You must agree to terms"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td colspan="2">
                    <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
