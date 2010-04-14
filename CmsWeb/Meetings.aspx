<%@ Page Title="" Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Meetings.aspx.cs" Inherits="CMSWeb.Meetings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Meetings</h1>
    Meeting Date:
    <asp:TextBox ID="MeetingDate" runat="server" AutoPostBack="True" Width="100" Style="font-size: large"></asp:TextBox>
    <cc2:CalendarExtender ID="MeetingDateExtender" runat="server" TargetControlID="MeetingDate">
    </cc2:CalendarExtender>
    <hr />
    <asp:ListView ID="ListView1" runat="server" DataSourceID="dsMeetings">
        <LayoutTemplate>
            <table id="itemPlaceholderContainer" runat="server" border="0" cellspacing="0">
                <tr>
                    <th>
                        <asp:LinkButton ID="LinkButton2" CommandName="Sort" CommandArgument="Division" runat="server">Division</asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="LinkButton3" CommandName="Sort" CommandArgument="Organization"
                            runat="server">Organization</asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="LinkButton5" CommandName="Sort" CommandArgument="Time" runat="server">Time</asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="LinkButton4" CommandName="Sort" CommandArgument="Attended" runat="server">Attended</asp:LinkButton>
                    </th>
                    <th>
                        <asp:LinkButton ID="LinkButton1" CommandName="Sort" CommandArgument="Leader" runat="server">Leader</asp:LinkButton>
                    </th>
                </tr>
                <tr id="itemPlaceholder" runat="server">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Division") %>' ToolTip='<%# Eval("Program") %>'></asp:Label>
                </td>
                <td>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("OrganizationId", "/Organization/Index/{0}") %>'
                        Text='<%# Eval("Organization") %>' ToolTip='<%# Eval("Tracking") %>'></asp:HyperLink>
                </td>
                <td>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("MeetingId", "~/Meeting.aspx?id={0}") %>'
                        Text='<%# Eval("Time", "{0:t}") %>'></asp:HyperLink>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Attended", "{0:n0}") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("Leader") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        No data was returned.
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>
    <asp:ObjectDataSource ID="dsMeetings" runat="server" SelectMethod="MeetingsForDate"
        TypeName="CMSPresenter.MeetingController" SortParameterName="SortOn" SelectCountMethod="MeetingsCount">
        <SelectParameters>
            <asp:ControlParameter ControlID="MeetingDate" Name="MeetingDate" Type="DateTime" />
            <asp:QueryStringParameter QueryStringField="name" Name="Name" Type="String" />
            <asp:QueryStringParameter QueryStringField="progid" Name="ProgId" Type="Int32" DefaultValue="0" />
            <asp:QueryStringParameter QueryStringField="divid" Name="DivId" Type="Int32" DefaultValue="0" />
            <asp:QueryStringParameter QueryStringField="schedid" Name="SchedId" Type="Int32" DefaultValue="0" />
            <asp:QueryStringParameter QueryStringField="campusid" Name="CampusId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
