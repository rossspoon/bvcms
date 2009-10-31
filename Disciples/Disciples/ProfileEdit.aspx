<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Disciples.ProfileEdit"
    Title="Edit Profile" CodeBehind="ProfileEdit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
    <asp:Panel ID="Panel1" runat="server" GroupingText="Groups">
        <blockquote>
            Enter group invitation code:
            <cc1:DefaultButtonTextBox ID="SecretCode" runat="server" ButtonId="btnJoinGroup" 
                TabIndex="1" ValidationGroup="Group"></cc1:DefaultButtonTextBox>
            <asp:Button ID="btnJoinGroup" runat="server" OnClick="JoinGroup_Click" 
                Text="Join Group" ValidationGroup="Group" TabIndex="2" />
            <br />
            <asp:CustomValidator ID="SecretCodeValidator" ValidationGroup="Group" runat="server" 
                ErrorMessage="CustomValidator"></asp:CustomValidator>
            <cms:ResultMessage ID="GroupResult" EnableViewState="false" runat="server" />
            Select Default Group Startup Message:
            <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="ObjectDataSource1"
                DataTextField="Name" OnDataBound="DropDownList1_DataBound" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
                AutoPostBack="True" TabIndex="3">
            </asp:DropDownList>
            <asp:ObjectDataSource ID="ObjectDataSource1" EnableViewState="false" runat="server"
                SelectMethod="FetchUserGroups" TypeName="DiscData.GroupController"></asp:ObjectDataSource>
        </blockquote>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" GroupingText="Password">
        <blockquote>
            <cms:ResultMessage ID="ChangePasswordResult" EnableViewState="false" runat="server" />
            <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="~/Default.aspx"
                OnChangedPassword="PasswordWasChanged" ContinueDestinationPageUrl="~/Default.aspx"
                ChangePasswordTitleText="" PasswordLabelText="Current Password:" 
                TabIndex="4">
            </asp:ChangePassword>
        </blockquote>
    </asp:Panel>
    <asp:Panel ID="Panel3" runat="server" GroupingText="Email">
        <blockquote>
            <cms:ResultMessage ID="EmailChangedResult" EnableViewState="false" runat="server" />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtEmail" Text="Email Address: "></asp:Label>
                    </td>
                    <td>
                        <cc1:DefaultButtonTextBox ID="txtEmail" runat="server" Columns="30" MaxLength="128"
                            ValidationGroup="UpdateEmail" ButtonId="SaveEmail" TabIndex="9"></cc1:DefaultButtonTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmail"
                            Display="Dynamic" EnableClientScript="true" ValidationGroup="UpdateEmail">required</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="NotifyEnabled" runat="server" Text="Notifications Enabled" 
                            TabIndex="10" />
                        <br />
                        <asp:CheckBox ID="NotifyAll" runat="server" 
                            Text="Notify for all content changes" TabIndex="11" />
                    </td>
                </tr>
            </table>
            <asp:Button ID="SaveEmail" runat="server" Text="Save Email Changes" Width="164px"
                ValidationGroup="UpdateEmail" OnClick="SaveEmail_Click" TabIndex="12" />
        </blockquote>
    </asp:Panel>
    <asp:Panel ID="Panel5" runat="server" GroupingText="Birthday">
        <blockquote>
            <cms:ResultMessage ID="BirthdayResult" EnableViewState="false" runat="server" />
            <table>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtEmail" Text="Birthday: "></asp:Label>
                    </td>
                    <td>
                        <cc1:DefaultButtonTextBox ID="txtBirthday" runat="server" Columns="30" MaxLength="128"
                            ValidationGroup="Birthday" ButtonId="SaveBirthday" TabIndex="9"></cc1:DefaultButtonTextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtBirthday"
                            Display="Dynamic" EnableClientScript="true" ValidationGroup="Birthday">required</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="BirthdayValid" runat="server" ErrorMessage="Bad date" ValidationGroup="Birthday"
                            onservervalidate="BirthdayValid_ServerValidate"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
            <asp:Button ID="SaveBirthday" runat="server" Text="Save Birthday" Width="164px"
                ValidationGroup="Birthday" OnClick="SaveBirthday_Click" TabIndex="12" />
        </blockquote>
    </asp:Panel>
</div>
</asp:Content>
