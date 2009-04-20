<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Forum_Edit" Title="Edit Forum Entry" ValidateRequest="false" Codebehind="Edit.aspx.cs" %>

<%@ Register Src="EditForumEntry.ascx" TagName="EditForumEntry" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <uc1:EditForumEntry ID="EditForumEntry1" EnableViewState="false" runat="server" />
</asp:Content>
