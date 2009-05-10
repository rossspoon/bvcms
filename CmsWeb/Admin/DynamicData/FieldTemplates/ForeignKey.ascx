<%@ Control Language="C#" CodeBehind="ForeignKey.ascx.cs" Inherits="CMSWeb.ForeignKeyField" AutoEventWireup="True" %>

<asp:HyperLink ID="HyperLink1" runat="server"
    Text="<%# GetDisplayString() %>"
    NavigateUrl="<%# GetNavigateUrl() %>"  />