<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Activity.aspx.cs" Inherits="CmsWeb.Admin.Activity" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Activity</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink ID="HyperLink1" NavigateUrl="~/" runat="server">Home</asp:HyperLink> |
        <asp:HyperLink ID="HyperLink2" NavigateUrl="~/Admin/Activity.aspx" runat="server">All Activity</asp:HyperLink> |
        <asp:HyperLink ID="HyperLink4" NavigateUrl="~/Admin/LastActivity.aspx" runat="server">Last Activity</asp:HyperLink> |
        <asp:HyperLink ID="HyperLink3" NavigateUrl="~/Admin/Users.aspx" runat="server">All Users</asp:HyperLink>
        <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1">
            <ItemTemplate>
                <tr style="background-color:#DCDCDC;color: #000000;">
                    <td>
                        <asp:HyperLink ID="UserLink" runat="server" NavigateUrl='<%# Eval("UserId", "~/Admin/Activity.aspx?uid={0}") %>' Text='<%# Eval("Username") %>'></asp:HyperLink>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("Name") %>' />
                    </td>
                    <td>
                        <asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>' />
                    </td>
                    <td>
                        <asp:Label ID="ActivityLink" runat="server" Text='<%# Eval("Activity") %>'></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table runat="server" 
                    style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;">
                    <tr>
                        <td>
                            No data was returned.</td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <table runat="server">
                    <tr runat="server">
                        <td runat="server">
                            <table ID="itemPlaceholderContainer" runat="server" border="1" 
                                style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;font-family: Verdana, Arial, Helvetica, sans-serif;">
                                <tr runat="server" style="background-color:#DCDCDC;color: #000000;">
                                    <th runat="server">
                                        User</th>
                                    <th runat="server">
                                        Name</th>
                                    <th runat="server">
                                        Date</th>
                                    <th runat="server">
                                        Activity</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td runat="server" 
                            style="text-align: center;background-color: #CCCCCC;font-family: Verdana, Arial, Helvetica, sans-serif;color: #000000;">
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="200">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" 
                                        ShowLastPageButton="True" />
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:ListView>    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="Activity" 
            TypeName="CMSPresenter.ActivityController" EnablePaging="True" 
            EnableViewState="False" SelectCountMethod="Count">
            <SelectParameters>
                <asp:QueryStringParameter DefaultValue="0" Name="uid" QueryStringField="uid" 
                    Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="startRowIndex" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
