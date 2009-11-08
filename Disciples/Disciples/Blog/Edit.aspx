<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Blog_Edit"
    Title="Edit Blog Post" ValidateRequest="false" CodeBehind="Edit.aspx.cs" %>

<%@ Register TagPrefix="user" TagName="BlogEdit" Src="BlogEdit.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main">
        <user:BlogEdit ID="EditBlogPost" runat="server" EnableViewState="false"></user:BlogEdit>
    </div>
</asp:Content>
