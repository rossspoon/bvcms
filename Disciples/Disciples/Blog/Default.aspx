<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Blog_Default"
    Title="GO Disciples Blogs" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main">
        <asp:LinkButton ID="NewBlog" runat="server" CssClass="CommonImageTextButton Big CommonAddButton"
            OnClick="NewBlog_Click" EnableViewState="false">New Blog</asp:LinkButton><br />
        <br />
        <asp:Panel ID="Panel1" runat="server" EnableViewState="false">
            <table>
                <tr>
                    <td>
                        Blog Short Name
                    </td>
                    <td>
                        <asp:TextBox ID="BlogName" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Blog Name must not have any spaces"
                            ValidationExpression='\w+' ControlToValidate="BlogName"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Blog Title
                    </td>
                    <td>
                        <asp:TextBox ID="BlogTitle" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="height: 26px">
                        Description (subtitle)&nbsp;
                    </td>
                    <td style="height: 26px">
                        <asp:TextBox ID="BlogDesc" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Group
                    </td>
                    <td>
                        <cc1:DropDownCC ID="DropDownCC1" runat="server">
                        </cc1:DropDownCC>
                    </td>
                </tr>
            </table>
            <asp:Button ID="NewBlogSave" runat="server" Text="Create New Blog" OnClick="NewBlogSave_Click" /></asp:Panel>
        <br />
        <br />
        <asp:GridView ID="GridView1" runat="server" EnableViewState="False" DataSourceID="ObjectDataSource1"
            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None"
            DataKeyNames="id" EmptyDataText="You are not a member of any blogs" >
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
                <asp:TemplateField HeaderText="Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Name") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# "~/Blog/" + Eval("name") + ".aspx" %>'
                            Text='<%# Bind("Name") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Description" HeaderText="Description"></asp:BoundField>
                <asp:BoundField DataField="GroupName" HeaderText="Group" ReadOnly="True" />
                <asp:BoundField DataField="Owner" HeaderText="Owner" SortExpression="Owner" ReadOnly="True" />
                <asp:CheckBoxField DataField="IsPublic" HeaderText="Public" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="delete" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="Delete" Enabled='<%# Eval("CanDelete") %>'></asp:LinkButton>
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
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnableViewState="False"
            SelectMethod="FetchAllForUser2" TypeName="DiscData.BlogController" UpdateMethod="Update"
            DeleteMethod="Delete">
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="Id" Type="Int32" />
                <asp:Parameter Name="Description" Type="String" />
                <asp:Parameter Name="Name" Type="String" />
                <asp:Parameter Name="Title" Type="String" />
            </UpdateParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
