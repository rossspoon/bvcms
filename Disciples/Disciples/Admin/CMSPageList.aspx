<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Admin_CMSPageList" Title="CMS Pages" Codebehind="CMSPageList.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main" class="wide">
    <h1>All CMS Pages</h1>
    <a href='/view/newpage.aspx'>New</a>
    <asp:GridView ID="GridView1" SkinID="subsonicSkin" runat="server" AutoGenerateColumns="False" 
        DataSourceID="LinqDataSource1">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="PageId" 
                DataNavigateUrlFormatString="/Admin/cmspagelist.aspx?id={0}" 
                DataTextField="PageId" HeaderText="ID" />
            <asp:BoundField DataField="Title" HeaderText="Title" ReadOnly="True" 
                SortExpression="Title" />
            <asp:HyperLinkField DataNavigateUrlFields="PageId" 
                DataNavigateUrlFormatString="/Admin/cmspagelist.aspx?id={0}" 
                DataTextField="PageUrl" HeaderText="Url" />
        </Columns>
    </asp:GridView>
    <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
        ContextTypeName="DiscData.DiscDataContext" Select="new (PageID, Title, PageUrl)" 
        TableName="Pages">
    </asp:LinqDataSource>
    </div>
</asp:Content>
