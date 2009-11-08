<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Blog_New" Title="Creating New Blog Post"  ValidateRequest="false" Codebehind="New.aspx.cs" %>

<%@ Register Src="BlogEdit.ascx" TagName="BlogEdit" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main"><uc1:BlogEdit ID="BlogEdit1" EnableViewState="false" runat="server" /></div>
</asp:Content>
