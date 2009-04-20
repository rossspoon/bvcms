<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Role_edit.aspx.cs" Inherits="BellevueTeachers.Admin.Role_edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="main" class="wide">
        <asp:Button ID="Button2" runat="server" Text="Save" onclick="SaveClick" />
        <table cellspacing="0" cellpadding="0" border="0" width="750">
            <tbody>
                <tr align="left" valign="top">
                    <td width="62%" height="100%" class="lbBorders">
                        <table class="bodyText" cellspacing="0" width="100%" cellpadding="0" border="0">
                            <tr class="callOutStyleLowLeftPadding">
                                <td colspan="4">
                                    <div style="padding-bottom: 5px">
                                        Group | <b><a href="users.aspx">Users</a></b> | <b><a href="roles.aspx">Groups</a></b>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trResultRow" runat="server" visible="false">
                                <td colspan="4">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1"
                                        DataKeyNames="UserName">
                                        <Columns>
                                            <asp:HyperLinkField DataNavigateUrlFields="UserName" HeaderText="UserName" SortExpression="user" DataTextField="UserName" DataNavigateUrlFormatString="~/Admin/Users_edit.aspx?username={0}" />
                                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="name" />
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
                                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetUsers"
                                        TypeName="DiscData.GroupController">
                                        <SelectParameters>
                                            <asp:QueryStringParameter QueryStringField="id" Type="Int32" Name="groupid" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Button ID="Button1" runat="server" Text="Save" onclick="SaveClick" />
    </div>
</asp:Content>
