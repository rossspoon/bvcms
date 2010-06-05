<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MeetingGrid.ascx.cs" Inherits="CMSWeb.MeetingGrid" %>
<%@ Register Src="GridPager.ascx" TagName="GridPager" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:GridView ID="Grid" runat="server" PageSize="10" AllowPaging="True" DataKeyNames="MeetingId"
    PagerSettings-Position="Bottom"
    AllowSorting="True" AutoGenerateColumns="False" 
    onrowdeleting="Grid_RowDeleting" OnRowCommand="ttt" 
        CellPadding="4" ForeColor="#333333" GridLines="None" >
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#3e8cb5" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="MeetingId" 
            DataNavigateUrlFormatString="~/Meeting.aspx?id={0}" 
            DataTextField="MeetingDate" DataTextFormatString="{0:d}" HeaderText="Date" SortExpression="MeetingDate" />
        <asp:BoundField DataField="Time" HeaderText="Time" DataFormatString="{0:t}" />
        <asp:BoundField DataField="NumPresent" HeaderText="Present" SortExpression="NumPresent" />
        <asp:BoundField DataField="NumVisitors" HeaderText="Visitors" SortExpression="NumVisitors" />
        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
        <asp:BoundField DataField="Description" HeaderText="Description" />
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <cc1:LinkButtonConfirm ID="DeleteLink" Text="Delete" Confirm="Are you sure you want to delete this meeting (this action cannot be undone)?"
                    runat="server" CommandName="Delete">
                </cc1:LinkButtonConfirm>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerTemplate>
        <uc1:GridPager ID="GridPager1" runat="server" />
    </PagerTemplate>
</asp:GridView>
