<%@ Control Language="C#" CodeBehind="DateCalendar_Edit.ascx.cs" Inherits="CMSWeb.DynamicData.FieldTemplates.DateCalendar_EditField" %>

<asp:TextBox ID="TextBox1" runat="server" Text='<%# FieldValueEditString %>' CssClass="droplist"></asp:TextBox>

<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" Enabled="false" />
<asp:DynamicValidator runat="server" ID="DynamicValidator1" CssClass="droplist" ControlToValidate="TextBox1" Display="Dynamic" />
<asp:Calendar ID="Calendar1" runat="server" 
  VisibleDate=
    '<%# (FieldValue != null) ? FieldValue : DateTime.Now %>'
  SelectedDate=
    '<%# (FieldValue != null) ? FieldValue : DateTime.Now %>'
  OnSelectionChanged="Calendar1_SelectionChanged" />