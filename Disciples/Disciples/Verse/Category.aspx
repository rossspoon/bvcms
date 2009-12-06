<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Category" Title="Manage Verse Categories" Codebehind="Category.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
   <asp:HiddenField ID="UserId" runat="server" EnableViewState="False" />
    <h4>
        Maintain Verse Categories</h4>
    <asp:CheckBox ID="includeAdmin" runat="server" AutoPostBack="True" Text="Include Other Users Categories" /><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="AddNewCategory" OnClick="AddNewCategory_Click"
                    runat="server" EnableViewState="false" CssClass="CommonImageTextButton Big CommonAddButton">New Category</asp:LinkButton><br />
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                ForeColor="#333333" GridLines="None" DataSourceID="ObjectDataSource1" DataKeyNames="id"
                EnableViewState="False" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" SkinID="subsonicSkin">
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="Editing">
                        <EditItemTemplate>
                            <asp:LinkButton ID="Update" runat="server" CausesValidation="True" CommandName="Update"
                                Text="Update"></asp:LinkButton>
                            <asp:LinkButton ID="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="Rename" runat="server" ImageUrl="~/images/icons/editdoc.png"
                                CausesValidation="False" CommandName="Edit" />
                            <asp:ImageButton ID="Delete" runat="server" ImageUrl="~/images/icons/xdoc.png" CausesValidation="False"
                                CommandName="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Copy">
                        <ItemTemplate>
                            <asp:ImageButton ID="LinkButton1" runat="server" ImageUrl="~/images/icons/copydoc.png" CausesValidation="False"
                                CommandName="Copy"  CommandArgument='<%# Bind("id") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Name" SortExpression="name">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="DisplayVerses" Enabled='<%# Eval("Username").ToString() == User.Identity.Name %>' runat="server" NavigateUrl='<%# Eval("id", "~/Verse/Default.aspx?cat={0}") %>'
                                Text='<%# Eval("name") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Username" HeaderText="Owner" ReadOnly="True" />
                    <asp:BoundField DataField="VerseCount" HeaderText="Count" SortExpression="VerseCount"
                        ReadOnly="True" />
                </Columns>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <EditRowStyle BackColor="#999999" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="includeAdmin" EventName="CheckedChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DeleteMethod="DeleteDeep"
        SelectMethod="GetCategoriesForOwner" TypeName="CmsData.VerseCategoryController"
        UpdateMethod="Update" EnableViewState="False">
        <UpdateParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="id" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter ControlID="includeAdmin" Name="includeAdmin" PropertyName="Checked"
                Type="Boolean" />
            <asp:ControlParameter ControlID="UserId" Name="Owner" PropertyName="Value" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="id" Type="Int32" />
        </DeleteParameters>
    </asp:ObjectDataSource>
</div>
</asp:Content>
