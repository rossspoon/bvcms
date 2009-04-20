<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PastAttendeeRpt.aspx.cs" Inherits="CMSWeb.PastAttendeeRpt" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Past Attendees Report</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="PastAttendees"
        TypeName="CMSPresenter.AttendenceController">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="" Name="orgid" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetOrganizationInfo"
        TypeName="CMSPresenter.AttendenceController">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="" Name="orgid" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    </form>
</body>
</html>
