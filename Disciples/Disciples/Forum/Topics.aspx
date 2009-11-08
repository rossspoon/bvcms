<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Forum_Topics" Title="GO Disciples Forum Topics" Codebehind="Topics.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:HyperLink ID="AddEntry" runat="server" NavigateUrl="~/Forum/Edit.aspx" EnableViewState="false"
        CssClass="CommonImageTextButton Big CommonPostButton">Write a new Post</asp:HyperLink><br />
    <br />
    <asp:GridView ID="GridView1" runat="server" SkinID="subsonicSkin" EnableViewState="false"
        DataSourceID="ObjectDataSource1" AutoGenerateColumns="False">
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="id" DataNavigateUrlFormatString="~/Forum/Thread/{0}.aspx"
                DataTextField="Title" DataTextFormatString="{0}" HeaderText="Title" />
            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" SortExpression="CreatedBy">
                <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" SortExpression="CreatedOn" />
            <asp:BoundField DataField="ModifiedOn" HeaderText="ModifiedOn" SortExpression="ModifiedOn" />
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnableViewState="false"
        SelectMethod="GetTopLevelEntriesForForum" TypeName="DiscData.ForumEntryController">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="1" Name="ForumId" QueryStringField="ForumId"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
