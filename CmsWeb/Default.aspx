<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CMSWeb.Default" Title="CMS2 Home" %>
<%@ Register src="WebParts/MyCalendar.ascx" tagname="MyCalendar" tagprefix="uc2" %>
<%@ Register src="WebParts/News.ascx" tagname="News" tagprefix="uc1" %>
<%@ Register src="WebParts/Birthdays.ascx" tagname="Birthdays" tagprefix="uc3" %>
<%@ Register src="WebParts/Reports.ascx" tagname="Reports" tagprefix="uc4" %>
<%@ Register src="WebParts/MyInvolvement.ascx" tagname="MyInvolvement" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:WebPartManager ID="WebPartManager1" runat="server" />
        <table>
            <tr valign="top">
                <td>
                    <asp:WebPartZone ID="LeftZone" runat="server" CloseVerb-Visible="false" MinimizeVerb-Visible="false" Width="100%">
                    <CloseVerb Visible="False"></CloseVerb>
                        <ZoneTemplate>
                            <uc3:Birthdays ID="Birthdays1" Title="Birthdays" runat="server"/>
                        </ZoneTemplate>

                        <MinimizeVerb Visible="False"></MinimizeVerb>
                    </asp:WebPartZone>
                    <br />
                    <asp:WebPartZone ID="LeftZone2" runat="server" CloseVerb-Visible="false" MinimizeVerb-Visible="false" Width="100%">
                        <CloseVerb Visible="False"></CloseVerb>
                        <ZoneTemplate>
                            <uc4:Reports ID="Reports1" runat="server" Title="Reports"/>
                        </ZoneTemplate>

                        <MinimizeVerb Visible="False"></MinimizeVerb>
                    </asp:WebPartZone>
                    <br />
                    <asp:WebPartZone ID="LeftZone3" runat="server" CloseVerb-Visible="false" MinimizeVerb-Visible="false" Width="100%">
                        <CloseVerb Visible="False"></CloseVerb>
                        <ZoneTemplate>
                            <uc5:MyInvolvement ID="ucMyInvolvement" runat="server" Title="My Involvement" />
                        </ZoneTemplate>
                        <MinimizeVerb Visible="False"></MinimizeVerb>
                    </asp:WebPartZone>
                </td>
                <td>
                    <asp:WebPartZone ID="MiddleZone" runat="server" CloseVerb-Visible="false" MinimizeVerb-Visible="false">
                        <CloseVerb Visible="False"></CloseVerb>
                        <ZoneTemplate>
                            <uc1:News ID="News1" Title="News and Announcements" runat="server"/>
                        </ZoneTemplate>

                        <MinimizeVerb Visible="False"></MinimizeVerb>
                    </asp:WebPartZone>
                </td>
                <td>
                    <asp:WebPartZone ID="RightZone" runat="server" CloseVerb-Visible="false" MinimizeVerb-Visible="false">
                        <CloseVerb Visible="False"></CloseVerb>
                        <ZoneTemplate>
                            <uc2:MyCalendar Title="Tasks" ID="MyCalendar1" runat="server" />
                        </ZoneTemplate>

                    <MinimizeVerb Visible="False"></MinimizeVerb>
                    </asp:WebPartZone>
                </td>
                <td style="width:auto">
                    <asp:EditorZone ID="EditorZone1" runat="server">
                        <ZoneTemplate>
                            <asp:LayoutEditorPart ID="LayoutEditorPart1" runat="server" />
                        </ZoneTemplate>
                    </asp:EditorZone>
                    <asp:CatalogZone ID="CatalogZone1" runat="server">
                        <ZoneTemplate>
                            <asp:PageCatalogPart ID="PageCatalogPart1" runat="server" />
                        </ZoneTemplate>
                    </asp:CatalogZone>
                    <asp:ConnectionsZone ID="ConnectionsZone1" runat="server"></asp:ConnectionsZone>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
