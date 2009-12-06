<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="All" Title="Select From All Verses" Codebehind="All.aspx.cs" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
    <h4>
        Choose from all cataloged veres for category: <asp:Label ID="Category" runat="server" Text="Label"></asp:Label></h4>
    <asp:Button ID="SaveChanges1" runat="server" OnClick="SaveChanges_Click" Text="Save Changes"
        Width="126px" EnableViewState="False" /><br />
    <br />
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        PageSize="100" EnableViewState="False" CellPadding="4" ForeColor="#333333" GridLines="None"
        DataSourceID="ObjectDataSource1" SkinID="subsonicSkin">
        <Columns>
            <asp:TemplateField HeaderText="In
            Category">
                <ItemTemplate>
                    <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Bind("InCategory") %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="Reference" HeaderText="Reference" ReadOnly="True" SortExpression="Reference" />
            <asp:BoundField DataField="ShortText" HeaderText="ShortText" ReadOnly="True" SortExpression="ShortText" />
            <asp:BoundField DataField="CategoryCount" HeaderText="Category
            Count" SortExpression="CategoryCount">
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle HorizontalAlign="Center" />
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <EditRowStyle BackColor="#999999" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <PagerSettings Position="TopAndBottom" />
    </asp:GridView>
    <br />
    <asp:Button ID="SaveChanges2" runat="server" OnClick="SaveChanges_Click" Text="Save Changes"
        Width="126px" EnableViewState="False" />
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" SelectMethod="Select"
        SelectCountMethod="GetCount" TypeName="CmsData.VerseSummaryController" 
        EnableViewState="False" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>
</asp:Content>
