<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" CodeBehind="ChurchAttReportIds.aspx.cs" Inherits="CMSWeb.Admin.ChurchAttReportIds" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:DynamicDataManager ID="DynamicDataManager1" runat="server"/>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateEditButton="true" 
            AutoGenerateColumns="false" DataSourceID="LinqDataSource1" AllowPaging="True" 
            AllowSorting="True" PageSize="200">
  <Columns>
    <asp:DynamicField DataField="Name" />
    <asp:DynamicField DataField="Id" />
  </Columns>
        </asp:GridView>    
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
            ContextTypeName="CmsData.CMSDataContext" EnableUpdate="True" 
            TableName="ChurchAttReportIds">
        </asp:LinqDataSource>
</asp:Content>
