<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VisitorGrid.ascx.cs"
    Inherits="CMSWeb.UserControls.VisitorGrid" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="GridPager.ascx" TagName="GridPager" TagPrefix="uc1" %>
<asp:DataPager ID="pager" PagedControlID="ListView1" runat="server">
    <Fields>
        <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
    </Fields>
</asp:DataPager>
<asp:ListView ID="ListView1" runat="server" DataKeyNames="PeopleId" 
    OnItemDataBound="ListView1_ItemDataBound" onitemcommand="ListView1_ItemCommand">
    <LayoutTemplate>
        <table id="itemPlaceHolderContainer" border="0" runat="server">
            <tr runat="server">
                <th runat="server">
                    <asp:LinkButton ID="lb1" CommandName="Sort" CommandArgument="Name" runat="server">Name</asp:LinkButton>
                </th>
                <th>
                </th>
                <th runat="server">
                    <asp:LinkButton ID="lb3" CommandName="Sort" CommandArgument="Member" runat="server">Status</asp:LinkButton>/Age
                    -
                    <asp:LinkButton ID="lb4" CommandName="Sort" CommandArgument="DOB" runat="server">DOB</asp:LinkButton>
                </th>
                <th runat="server">
                    <asp:LinkButton ID="lb5" CommandName="Sort" CommandArgument="Address" runat="server">Primary Address</asp:LinkButton>
                </th>
                <th runat="server">
                    Communication
                </th>
                <th runat="server">
                    <asp:LinkButton ID="lb7" CommandName="Sort" CommandArgument="BFTeacher" runat="server">BF Teacher</asp:LinkButton>
                </th>
                <th runat="server">
                    <asp:LinkButton ID="lb8" CommandName="Sort" CommandArgument="LastAttended" runat="server">Last Attended</asp:LinkButton>
                </th>
                <th runat="server">
                    Tag
                </th>
            </tr>
            <tr id="itemPlaceholder" runat="server">
            </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'>
            <td>
                <asp:Image ID="PersonIcon" ImageUrl="~/images/individual.gif" Width="10px" Height="12px"
                    runat="server" />
                <asp:HyperLink ID="PersonLink" runat="server" NavigateUrl='<%# Eval("PeopleId", "~/Person/Index/{0}") %>'
                    Text='<%# Eval("Name") %>'></asp:HyperLink>
            </td>
            <td>
                <cc1:LinkButtonConfirm ID="JoinLink" CommandName="Join"
                    CommandArgument='<%# Eval("PeopleId") %>' Confirm="Are you sure you want to make this person a member"
                    runat="server">Join</cc1:LinkButtonConfirm>
            </td>
            <td>
                <%# Eval("MemberStatus") %><br />
                <%# Eval("Age") %>
                -
                <%# Eval("BirthDate") %>
            </td>
            <td>
                <asp:HyperLink ID="AddressLink" runat="server" Target="_blank" NavigateUrl='<%# "http://www.bing.com/maps?q=" + Eval("Address") + ",+" + Eval("CityStateZip") %>'
                    Text='<%# Eval("Address") %>'></asp:HyperLink>
                <br />
                <%# Eval("CityStateZip") %>
            </td>
            <td>
                <asp:Repeater ID="Phones" runat="server" DataSource='<%# Eval("Phones") %>'>
                    <ItemTemplate>
                        <%# Container.DataItem %><br />
                    </ItemTemplate>
                </asp:Repeater>
                <cc1:EmailHyperlink ID="EmailLink" runat="server" Text='<%# Eval("Email") %>' Email='<%# Eval("Email") %>'
                    Name='<%# Eval("Name") %>'></cc1:EmailHyperlink>
            </td>
            <td>
                <asp:HyperLink ID="BFTeacherLink" runat="server" NavigateUrl='<%# Eval("BFTeacherId", "~/Person/Index/{0}") %>'
                    Text='<%# Eval("BFTeacher") %>'></asp:HyperLink>
            </td>
            <td>
                <asp:Label ID="Label3" runat="server" Text='<%# Eval("LastAttended", "{0:d}") %>'></asp:Label>
            </td>
            <td>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="#" ToolTip="Add to/Remove from Active Tag"><%# (bool)Eval("HasTag") ? "Remove" : "Add" %></asp:HyperLink>
            </td>
        </tr>
    </ItemTemplate>
    <EmptyDataTemplate>
        <table id="Table1" runat="server" style="">
            <tr>
                <td>
                    <h2 style="color: Red">
                        No data was returned.</h2>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:ListView>
