<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PersonGrid.ascx.cs"
    Inherits="CmsWeb.PersonGrid" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<script type="text/javascript">
    function Cancel() {
        $.unblockUI();
    }
    function ToggleTagCallback(e) {
        var result = eval('(' + e + ')');
        $('#' + result.ControlId).text(result.HasTag ? "Remove" : "Add");
    }
</script>

<asp:DataPager ID="pager" PagedControlID="ListView1" runat="server">
    <Fields>
             <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
    </Fields>
</asp:DataPager>
<asp:ListView ID="ListView1" runat="server" OnDataBound="ListView1_DataBound" 
    DataKeyNames="PeopleId" onitemdatabound="ListView1_ItemDataBound">
    <LayoutTemplate>
        <table id="itemPlaceHolderContainer" border="0" runat="server">
            <tr runat="server">
                <th id="Th1" runat="server">
                    <asp:LinkButton ID="lb2" CommandName="Sort" CommandArgument="Name" runat="server">Name</asp:LinkButton>
                </th>
                <th id="Th3" runat="server">
                    <asp:LinkButton ID="lb3" CommandName="Sort" CommandArgument="Member" runat="server">Status</asp:LinkButton>/Age
                    -
                    <asp:LinkButton ID="lb4" CommandName="Sort" CommandArgument="DOB" runat="server">DOB</asp:LinkButton>
                </th>
                <th id="Th4" runat="server">
                    <asp:LinkButton ID="lb5" CommandName="Sort" CommandArgument="Address" runat="server">Primary Address</asp:LinkButton>
                </th>
                <th id="Th5" runat="server">
                    Communication
                </th>
                <th id="Th6" runat="server">
                    <asp:LinkButton ID="lb7" CommandName="Sort" CommandArgument="BFTeacher" runat="server">Fellowship Leader</asp:LinkButton>
                </th>
                <th id="Th7" runat="server">
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
                <%# Eval("MemberStatus") %><br />
                <%# Eval("Age") %>
                -
                <%# Eval("BirthDate") %>
            </td>
            <td>
                <asp:HyperLink ID="AddressLink" runat="server" Target="_blank" NavigateUrl='<%# "http://www.google.com/maps?q=" + Eval("Address") + ",+" + Eval("CityStateZip") %>'
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
                <asp:HyperLink ID="HyperLink1" runat="server" 
                    ToolTip="Add to/Remove from Active Tag"><%# (bool)Eval("HasTag") ? "Remove" : "Add" %></asp:HyperLink>
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
<asp:DataPager ID="pager2" PagedControlID="ListView1" runat="server">
    <Fields>
        <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
    </Fields>
</asp:DataPager>
