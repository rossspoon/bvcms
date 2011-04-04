<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SavedQueries.aspx.cs"
    Inherits="CmsWeb.SavedQueries" Title="Saved Queries" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="UserControls/GridPager.ascx" TagName="GridPager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:CheckBox ID="OnlyMyQueries" runat="server" AutoPostBack="true" Text="Show only my queries" />
    <asp:CheckBox ID="ShowScratchPads" runat="server" AutoPostBack="true" Text="Show Scratch pad queries" /><br />
    <asp:GridView ID="GridView1" CssClass="aspgrid" runat="server" DataSourceID="ObjectDataSource1" AllowPaging="True"
        AutoGenerateColumns="False" AllowSorting="True" DataKeyNames="QueryId"
        PageSize="10"
        CellPadding="4" ForeColor="#333333" GridLines="None" >
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#4296CC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#4296CC" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:TemplateField ShowHeader="False">
                <EditItemTemplate>
                    <asp:LinkButton ID="UpdateLinkButton" runat="server" CausesValidation="True" CommandName="Update"
                        Text="Update"></asp:LinkButton>
                    &nbsp;<asp:LinkButton ID="CancelLinkButton" runat="server" CausesValidation="False"
                        CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:LinkButton ID="EditLinkButton" runat="server" CausesValidation="False" CommandName="Edit"
                        Text="Edit" Enabled='<%# CanEdit(Eval("User")) %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="User" HeaderText="User" SortExpression="User" />
            <asp:TemplateField HeaderText="Description" SortExpression="Description">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("QueryId", "/QueryBuilder/Main/{0}") %>'
                        Text='<%# Eval("Description", "{0}") %>'></asp:HyperLink>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Description") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField DataField="IsPublic" HeaderText="IsPublic" SortExpression="IsPublic" />
            <asp:BoundField DataField="LastUpdated" HeaderText="LastUpdated" SortExpression="LastUpdated"
                ReadOnly="True" />
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <cc1:LinkButtonConfirm ID="LinkButtonConfirm1" Text="Delete" CommandName="Delete"
                        Confirm='<%# Eval("Description", "Delete the query?: {0}") %>' 
                        Enabled='<%# CanEdit(Eval("User")) %>'
                        runat="server"></cc1:LinkButtonConfirm>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerTemplate>
            <uc1:GridPager ID="GridPager1" runat="server" />
        </PagerTemplate>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="FetchSavedQueries"
        TypeName="CMSPresenter.QueryController" UpdateMethod="Update" SelectCountMethod="SavedQueryCount"
        EnablePaging="True" SortParameterName="sortExpression" DeleteMethod="Delete">
        <DeleteParameters>
            <asp:Parameter Name="queryId" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="queryId" Type="Int32" />
            <asp:Parameter Name="Description" Type="String" />
            <asp:Parameter Name="IsPublic" Type="Boolean" />
            <asp:Parameter Name="User" Type="String" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter Name="onlyMine" Type="Boolean" ControlID="OnlyMyQueries" PropertyName="Checked" />
            <asp:ControlParameter Name="showscratchpads" Type="Boolean" ControlID="ShowScratchPads" PropertyName="Checked" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
