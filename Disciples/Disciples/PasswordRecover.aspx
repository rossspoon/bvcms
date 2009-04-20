<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="BellevueTeachers.PasswordRecover" Title="Recover Password" Codebehind="PasswordRecover.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="leftcontent">
    </div>
    <div id="centercontent">
        <h4>
            Recover Your Password</h4>
        <p>
            <asp:PasswordRecovery ID="PasswordRecovery1" runat="server">
            </asp:PasswordRecovery>
            &nbsp;</p>
    </div>
    <div id="rightcontent">
    </div>
</asp:Content>
