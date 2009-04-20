<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="NewEntry" Title="Edit Forum Entry" ValidateRequest="false" Codebehind="NewEntry.aspx.cs" %>
<%@ Register TagPrefix="user" TagName="EditForumEntry" Src="EditForumEntry.ascx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
<user:EditForumEntry id="EditForumEntry1" runat="server" />
</asp:Content>
