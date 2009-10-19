<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Users.aspx.cs" Inherits="Admin_admin_users" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User Management</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/" runat="server">HomePage</asp:HyperLink>
            <asp:TextBox runat="server" ID="TextBox1" BorderStyle="Solid" />
            <asp:Button ID="Button1" runat="server" Text="Search for Users" OnClick="SearchForUsers" /><br />
    <br />
    <asp:DataPager ID="pager1" runat="server" PagedControlID="ListView1">
        <Fields>
            <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif">
            </cc1:PagerField>
        </Fields>
    </asp:DataPager>
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="UserId" DataSourceID="ObjectDataSourceMembershipUser"
                OnItemCommand="ListView1_ItemCommand" OnItemDeleted="ListView1_ItemDeleted"
                OnPagePropertiesChanging="ListView1_PagePropertiesChanging" OnSorting="ListView1_Sorting"
                >
                <ItemTemplate>
                    <tr style="background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>">
                        <td>
                            <asp:Label ID="pid" runat="server" Text='<%# Eval("PeopleId") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:HyperLink ID="HyperLink2" runat="server" ToolTip='<%#Eval("UserId")%>' NavigateUrl='<%#Eval("UserName", "~/Admin/Users_edit.aspx?username={0}") %>'
                                Text='<%# Eval("Username") %>'></asp:HyperLink>
                        </td>
                        <td>
                            <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                        <td>
                            <asp:CheckBox ID="ApprovedCheckBox" runat="server" Checked='<%# Eval("IsApproved") %>'
                                Enabled="false" />
                        </td>
                        <td>
                            <asp:CheckBox ID="ChgPwdCheckBox" runat="server" Checked='<%# Eval("MustChangePassword") %>'
                                Enabled="false" />
                        </td>
                        <td>
                            <asp:CheckBox ID="LockedCheckBox" runat="server" Checked='<%# Eval("IsLockedOut") %>'
                                Enabled="false" />
                        </td>
                        <td>
                            <asp:CheckBox ID="OnlineCheckBox" runat="server" Checked='<%# Eval("IsOnLine") %>'
                                Enabled="false" />
                        </td>
                        <td>
                            <asp:HyperLink ID="EmailAddressLink" runat="server" NavigateUrl='<%# "mailto:" + Eval("Name") + "<" + Eval("EmailAddress") + ">" %>'
                                Text='<%# Eval("EmailAddress") %>' />
                        </td>
                        <td>
                            <asp:Label ID="LastActivityLabel" runat="server" Text='<%# Eval("LastVisit", "{0:MM/dd/yy h:mm t}") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>
                                No data was returned.
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton6" CommandName="Sort" CommandArgument="Pid" runat="server">Pid</asp:LinkButton>
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton0" CommandName="Sort" CommandArgument="Username" runat="server">User</asp:LinkButton>
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton1" CommandName="Sort" CommandArgument="Name" runat="server">Name</asp:LinkButton>
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton2" CommandName="Sort" CommandArgument="IsApproved"
                                    runat="server">Appr</asp:LinkButton>
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton3" CommandName="Sort" CommandArgument="MustChangePassword"
                                    runat="server">Pwd</asp:LinkButton>
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton4" CommandName="Sort" CommandArgument="IsLockedOut"
                                    runat="server">Lock</asp:LinkButton>
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton5" CommandName="Sort" CommandArgument="IsOnLine" runat="server">Online</asp:LinkButton>
                            </th>
                            <th runat="server">
                                Email
                            </th>
                            <th runat="server">
                                <asp:LinkButton ID="LinkButton7" CommandName="Sort" CommandArgument="LastActivityDate"
                                    runat="server">Activity</asp:LinkButton>
                            </th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
            </asp:ListView>
    <asp:DataPager ID="pager2" runat="server" PagedControlID="ListView1">
        <Fields>
            <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
        </Fields>
    </asp:DataPager>
    <asp:HyperLink ID="HyperLink3" NavigateUrl="~/Admin/Users_edit.aspx" runat="server">New User</asp:HyperLink>
    <asp:ObjectDataSource ID="ObjectDataSourceMembershipUser" runat="server"
        SelectMethod="GetUsers" TypeName="UserController"
        SortParameterName="sortExpression"
        EnableViewState="False" EnablePaging="True" SelectCountMethod="Count" StartRowIndexParameterName="startIndex">
        <SelectParameters>
            <asp:ControlParameter ControlID="TextBox1" Name="name" Type="String" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="startIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</form>
</body>
</html>
