<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Admin_History" Title="Untitled Page" Codebehind="History.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <asp:GridView ID="grid" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False"
        PageSize="200"
        DataSourceID="LinqDataSource1">
        <PagerSettings Position="TopAndBottom" />
    <Columns>
        <asp:TemplateField HeaderText="Name" SortExpression="Name">
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Eval("FirstName") + " " + Eval("LastName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="PageUrl" HeaderText="PageUrl" ReadOnly="True" 
            SortExpression="PageUrl" />
        <asp:BoundField DataField="PageTitle" HeaderText="PageTitle" ReadOnly="True" 
            SortExpression="PageTitle" />
        <asp:BoundField DataField="VisitTime" HeaderText="VisitTime" 
            ReadOnly="True" SortExpression="VisitTime" />
    </Columns>
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
<asp:LinqDataSource ID="LinqDataSource1" runat="server" 
    ContextTypeName="DiscData.DiscDataContext" EnableViewState="False" OrderBy="VisitTime desc" 
    Select="new (FirstName, LastName, PageTitle, CreatedOn, PageUrl, VisitTime)" 
    StoreOriginalValuesInViewState="False" TableName="ViewViewPageVisits">
</asp:LinqDataSource>
</asp:Content>

