<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Admin_History" Title="Untitled Page" Codebehind="History.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" Runat="Server">
    <asp:GridView ID="grid" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False"
        PageSize="200"
        DataSourceID="LinqDataSource1">
        <PagerSettings Position="TopAndBottom" />
    <Columns>
        <asp:BoundField DataField="Name2" HeaderText="Name" 
            SortExpression="Name2" ReadOnly="True" />
        <asp:BoundField DataField="PageUrl" HeaderText="PageUrl" 
            SortExpression="PageUrl" ReadOnly="True" />
        <asp:BoundField DataField="PageTitle" HeaderText="PageTitle" ReadOnly="True" 
            SortExpression="PageTitle" />
        <asp:BoundField DataField="VisitTime" HeaderText="VisitTime" ReadOnly="True" 
            SortExpression="VisitTime" />
    </Columns>
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
    <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
        ContextTypeName="CmsData.CMSDataContext" OrderBy="VisitTime desc" 
        Select="new (User.Name2, PageUrl, PageTitle, VisitTime)" TableName="PageVisits">
    </asp:LinqDataSource>
</asp:Content>

