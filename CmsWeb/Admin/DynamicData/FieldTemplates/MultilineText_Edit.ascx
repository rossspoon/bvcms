<%@ Control Language="C#" CodeBehind="MultilineText_Edit.ascx.cs" Inherits="CMSWeb.MultilineText_EditField" AutoEventWireup="True" %>

<asp:TextBox ID="TextBox1" runat="server" CssClass="droplist" TextMode="MultiLine" Text='<%# FieldValueEditString %>' Columns="80" Rows="5"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" id="DynamicValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" />