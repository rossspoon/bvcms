<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="admin_user_edit" Title="Edit User" Codebehind="Users_edit.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
       <div id="main" class="wide">
 <table cellspacing="0" cellpadding="0" border="0" width="750">
        <tbody>
            <tr align="left" valign="top">
                <td width="62%" height="100%" class="lbBorders">
                    <table class="bodyText" cellspacing="0" width="100%" cellpadding="0" border="0">
                        <tr class="callOutStyleLowLeftPadding">
                            <td colspan="4">
                                <div style="padding-bottom: 5px">
                                    <b>
                                        <asp:Label ID="ActionTitle" runat="server" Text="Edit User"></asp:Label></b> 
                                    | <b><a href="users.aspx">Users</a></b> | <b><a href="roles.aspx">Groups</a></b>
                                </div>
                            </td>
                        </tr>
                        <tr id="trResultRow" runat="server" visible="false">
                            <td colspan="4">
                                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class="bodyText" bordercolor="#ccddef">
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="UserID" Text="Username: "
                                                CssClass="adminlabel" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="UserID" MaxLength="255" TabIndex="101" Columns="30"
                                                CssClass="adminlabel" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserID"
                                                Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                                            &nbsp;<asp:Label ID="LastLogin" runat="server" Text="Label"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label2" runat="server" AssociatedControlID="Email" Text="Email Address: "
                                                CssClass="adminlabel" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="Email" MaxLength="128" TabIndex="102" Columns="30"
                                                CssClass="adminlabel" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Email"
                                                Display="Dynamic" EnableClientScript="true">required</asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            First Name:</td>         <td>
                                            <asp:TextBox runat="server" ID="FirstName" MaxLength="128" TabIndex="102" Columns="30"
                                                CssClass="adminlabel" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Last Name:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="LastName" MaxLength="128" TabIndex="102" Columns="30"
                                                CssClass="adminlabel" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Birthday:</td>
                                        <td>
                                            <asp:TextBox runat="server" ID="Birthday" MaxLength="128" TabIndex="102" Columns="30"
                                                CssClass="adminlabel" /></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            PeopleId:</td>
                                        <td>
                                            <asp:TextBox ID="pid" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNewPassword" runat="server" AssociatedControlID="pw" Text="New Password: " />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="pw" TextMode="Password" MaxLength="50" TabIndex="103"
                                                Columns="20" autocomplete="off" />&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="NotifyEnabled" runat="server" Text="Notification Enabled" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="NotifyAll" runat="server" Text="Notify for all content" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="ActiveUser" Text="Active User" TabIndex="106" Checked="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button runat="server" ID="unlockUser" Text="Unlock Account" TabIndex="107" OnClick="unlockAccount_Click"
                                                CssClass="frmbutton" /></td>
                                    </tr>
                                    <tr valign="top">
                                        <td class="userDetailsWithFontSize" height="100%" colspan="2">
                                            <br />
                                            <h2>
                                                <asp:Label runat="server" ID="SelectRolesLabel" Text="Select User Roles" />
                                            </h2>
                                            <p>
                                                <asp:CheckBox ID="SiteAdministrator" runat="server" Text="Site Administrator" /><br />
                                                <asp:CheckBox ID="BlogAdministrator" runat="server" Text="Blog Administrator" />
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1"
                                                DataKeyNames="Name">
                                                <Columns>
                                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                    <asp:TemplateField HeaderText="IsMember" SortExpression="IsMember">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbMember" runat="server" Checked='<%# Bind("IsMember") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IsBlogger" SortExpression="IsBlogger">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbBlogger" runat="server" Checked='<%# Bind("IsBlogger") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="IsAdmin" SortExpression="IsAdmin">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbAdmin" runat="server" Checked='<%# Bind("IsAdmin") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server"
                                                SelectMethod="FetchAllGroups" TypeName="CmsData.GroupController">
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <h2>
                                                Blog Notifications OptOuts</h2>
                                            <p>
                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                                                    DataKeyNames="BlogId" DataSourceID="ObjectDataSource2">
                                                    <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="optout" runat="server" Checked='<%# Bind("Checked") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                        <asp:BoundField DataField="Title" HeaderText="Blog" />
                                                        <asp:BoundField DataField="Poster" HeaderText="Poster" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" 
                                                    SelectMethod="FetchOptOuts" 
                                                    TypeName="UserController">
                                                    <SelectParameters>
                                                        <asp:QueryStringParameter QueryStringField="username" DbType="String" Name="username" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                            <asp:Button runat="server" ID="SaveButton" OnClick="SaveClick" Text="Save" Width="100"
                                                CssClass="frmbutton" />
                                        </td>
                                        <td>
                                            <br />
                                            <asp:Button runat="server" ID="Delete" OnClick="DeleteClick" Text="Delete" Width="100"
                                                CssClass="frmbutton" />
                                             <ajx:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server"
                                                 TargetControlID="Delete" ConfirmText="Are you sure you want to delete?">
                                                </ajx:ConfirmButtonExtender>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</div>
</asp:Content>
