<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Forum_Default" Title="Bellevue Teacher Forums" Codebehind="Default.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:LinkButton ID="NewForum" runat="server" EnableViewState="false" OnClick="NewForum_Click"  CssClass="CommonImageTextButton Big CommonAddButton">New 
    Forum</asp:LinkButton><br />
    <br />
    <asp:Panel ID="Panel1" runat="server" EnableViewState="false">
        <table>
            <tr>
                <td>
                    Forum Name</td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    Group</td>
                <td>
                    <cc1:DropDownCC ID="DropDownCC1" runat="server">
                    </cc1:DropDownCC>
                </td>
            </tr>
        </table>
        &nbsp;
        <asp:Button ID="NewForumSave" runat="server" OnClick="NewForumSave_Click" Text="Create New Forum" /></asp:Panel>
    <br />
    <br />
    <asp:GridView ID="GridView1" runat="server" DataSourceID="ObjectDataSource1" AutoGenerateColumns="False"
        CellPadding="4" ForeColor="#333333" GridLines="None" DataKeyNames="id" EnableViewState="False"
        EmptyDataText="You are not a member of any forums" SkinID="subsonicSkin">
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Update"></asp:LinkButton>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel"
                        Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Edit"
                        Text="Edit" Visible='<%# Eval("IsAdmin") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("id", "~/Forum/{0}.aspx") %>'
                        Text='<%# Eval("Description") %>'></asp:HyperLink>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="GroupName" HeaderText="Group" ReadOnly="True" />
            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy"
                ReadOnly="True" />
            <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn"
                ReadOnly="True" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="delete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="Delete" Visible='<%# Eval("IsAdmin") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnableViewState="false" SelectMethod="FetchAllForUser"
        TypeName="DiscData.ForumController" DeleteMethod="Delete" UpdateMethod="Update">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Object" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Id" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
        </UpdateParameters>
    </asp:ObjectDataSource>
</asp:Content>
