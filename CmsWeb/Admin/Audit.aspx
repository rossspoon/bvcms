<%@ Page Language="C#" StylesheetTheme="Standard" AutoEventWireup="true" CodeBehind="Audit.aspx.cs" Inherits="CMSWeb.Audit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Audit Trail</title>
    <style type="text/css">
        h4
        {
            margin-bottom: 1px;
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h4>
            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/">Home</asp:HyperLink>
        </h4>
        <h4>
            View Audits - Select an Audit to View Changes</h4>
        <asp:GridView ID="gridAudits" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataSourceID="LinqDataSource1" ForeColor="#333333" GridLines="None" DataKeyNames="Id"
            EnableViewState="False" AllowPaging="True" AllowSorting="True" 
            PageSize="20">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Action" HeaderText="Action" 
                    SortExpression="Action" />
                <asp:BoundField DataField="TableName" HeaderText="TableName" 
                    SortExpression="TableName" />
                <asp:BoundField DataField="TableKey" HeaderText="TableKey" 
                    SortExpression="TableKey" />
                <asp:BoundField DataField="UserName" HeaderText="UserName" 
                    SortExpression="UserName" />
                <asp:BoundField DataField="AuditDate" HeaderText="AuditDate" 
                    SortExpression="AuditDate" />
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="CMSModel.CMSDataContext"
            TableName="Audits" EnableViewState="False">
        </asp:LinqDataSource>
        <h4>
            View Modified Values</h4>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
            DataSourceID="LinqDataSource2" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="MemberName" HeaderText="MemberName" ReadOnly="True" SortExpression="MemberName" />
                <asp:BoundField DataField="OldValue" HeaderText="OldValue" ReadOnly="True" SortExpression="OldValue" />
                <asp:BoundField DataField="NewValue" HeaderText="NewValue" ReadOnly="True" SortExpression="NewValue" />
            </Columns>
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#999999" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <br />
        <asp:LinqDataSource ID="LinqDataSource2" runat="server" ContextTypeName="CMSModel.CMSDataContext"
            Select="new (MemberName, OldValue, NewValue, AuditId)" TableName="AuditValues"
            Where="AuditId == @AuditId">
            <WhereParameters>
                <asp:ControlParameter ControlID="gridAudits" Name="AuditId" PropertyName="SelectedValue"
                    Type="Int32" DefaultValue="1" />
            </WhereParameters>
        </asp:LinqDataSource>
    </div>
    </form>
</body>
</html>
