<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SavedQuery.aspx.cs"
    Inherits="CMSWeb.SavedQuery" Title="Saved Query" %>

<%@ Register Src="UserControls/PersonGrid.ascx" TagName="PersonGrid" TagPrefix="uc1" %>
<%@ Register Src="UserControls/ExportToolBar.ascx" TagName="ExportToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table><tr><td><asp:Label ID="QueryDesc" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label></td></tr></table>
    <uc2:ExportToolBar ID="ExportToolBar1" runat="server" />
    <div style="clear: both">
        <uc1:PersonGrid ID="PersonGrid1" runat="server" DataSourceID="PersonData" />
    </div>
    <asp:ObjectDataSource ID="PersonData" runat="server" SelectMethod="FetchPeopleList"
        TypeName="CMSPresenter.QueryController" EnablePaging="True" SelectCountMethod="Count"
        SortParameterName="sortExpression" EnableViewState="False"
        OnObjectCreating="ObjectCreating">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:QueryStringParameter Name="QueryId" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
