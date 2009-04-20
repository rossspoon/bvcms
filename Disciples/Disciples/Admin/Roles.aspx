<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/site.master" Inherits="AdminUsers_admin_roles"
    Title="Roles Administration" CodeBehind="Roles.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
        <br />
        <b><a href="users.aspx">Users</a></b> | <b>Groups</b><br />
        <br />
        <asp:GridView ID="GridView1" runat="server" SkinID="subsonicSkin" AutoGenerateColumns="False"
            DataSourceID="allGroups" EmptyDataText="There are no matching roles in the system."
            Font-Italic="False" DataKeyNames="Name">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Admin/Role_edit.aspx?id={0}"
                    HeaderText="Name" DataTextField="Name" />
                <asp:CheckBoxField DataField="HasWelcomeText" HeaderText="HasWelcomeText" ReadOnly="True"
                    SortExpression="HasWelcomeText">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:CheckBoxField>
                <asp:CommandField ShowDeleteButton="True" />
                <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Default.aspx?group={0}"
                    HeaderText="Edit Welcome Text" ShowHeader="False" Text="Edit Welcome Text" />
            </Columns>
            <EmptyDataRowStyle Font-Italic="True" />
        </asp:GridView>
        <br />
        <div>
            <table>
                <tr>
                    <td>
                        <asp:TextBox ID="RoleName" runat="server" ValidationGroup="new" CssClass="adminlabel" />
                        <asp:Button runat="server" ID="AddRole" Text="Add" ForeColor="black" ValidationGroup="new"
                            CssClass="frmbutton" OnClick="AddRole_Click" />
                        <asp:RequiredFieldValidator ID="RoleNameRequiredFieldValidator" runat="server" ControlToValidate="RoleName"
                            Display="Dynamic" EnableClientScript="true" ValidationGroup="new">required</asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <p>
                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/default.aspx?group=default">Edit Anonymous Welcome Text</asp:HyperLink></p>
            <p>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/default.aspx?group=default1">Edit New-User, no-groups Welcome Text</asp:HyperLink></p>
            <p>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/default.aspx?group=default2">Edit 
            Generic Welcome Text</asp:HyperLink></p>
        </div>
        <asp:ObjectDataSource ID="allGroups" runat="server" SelectMethod="FetchAllGroupsWhereAdmin"
            TypeName="DiscData.GroupController" DeleteMethod="DeleteGroup">
            <DeleteParameters>
                <asp:Parameter Name="Name" Type="String" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        <br />
        <h4>
            Invitations</h4>
        <asp:GridView ID="Invitations" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1"
            DataKeyNames="Password,GroupId" SkinID="subsonicSkin" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="Groupname" HeaderText="Groupname" SortExpression="Groupname" />
                <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="Password" />
                <asp:BoundField DataField="Expires" HeaderText="Expires" SortExpression="Expires" />
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="Delete"
            SelectMethod="FetchAll" TypeName="DiscData.InvitationController" 
            SortParameterName="sortExpression">
            <DeleteParameters>
                <asp:Parameter Name="Password" Type="String" />
                <asp:Parameter Name="GroupId" Type="Int32" />
            </DeleteParameters>
        </asp:ObjectDataSource>
        Password:
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;Group:
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList>
        &nbsp;<asp:Button ID="AddInvite" runat="server" OnClick="AddInvite_Click" Text="Add Invitation" /><br />
    </div>
</asp:Content>
