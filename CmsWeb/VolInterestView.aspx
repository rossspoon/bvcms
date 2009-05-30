<%@ Page Title="" Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VolInterestView.aspx.cs" Inherits="CMSWeb.VolInterestView" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
        DataSourceID="ObjectDataSource1" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="Opportunity" HeaderText="Opportunity" 
                SortExpression="Opportunity" />
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundField DataField="Created" HeaderText="Created" 
                SortExpression="Created" />
            <asp:BoundField DataField="Interest" HeaderText="Interest" 
                SortExpression="Interest" />
            <asp:BoundField DataField="CVAStatus" HeaderText="CVAStatus" 
                SortExpression="CVAStatus" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    EnablePaging="True" EnableViewState="False" 
    OldValuesParameterFormatString="original_{0}" SelectCountMethod="Count" 
    SelectMethod="FetchList" TypeName="CMSPresenter.VolInterestController">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
