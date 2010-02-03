<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="MemberGrid.ascx.cs"
    Inherits="CMSWeb.MemberGrid" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
Org Members: <asp:HyperLink ID="AddMember" runat="server" CssClass="thickbox3" ToolTip='Add Organization Member'>Add</asp:HyperLink>
<asp:HyperLink ID="UpdateMembers" runat="server" CssClass="thickbox3" ToolTip='Batch Update Members'>Update</asp:HyperLink>
<div style="clear: both">
</div>
<asp:UpdatePanel ID="MemberPanel" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:DataPager ID="pager" PagedControlID="MemberGrid1" runat="server">
            <Fields>
                <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
            </Fields>
        </asp:DataPager>
        <div class="members">
            <asp:ListView ID="MemberGrid1" runat="server" DataSourceID="MembersData" DataKeyNames="PeopleId"
                OnItemDataBound="MemberGrid1_ItemDataBound" EnableViewState="false">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" border="0" style="border: solid 1 black">
                        <tr>
                            <th>
                            </th>
                            <th>
                            </th>
                            <th align="left">
                                <asp:LinkButton ID="lb4a" CommandName="Sort" CommandArgument="MemberStatus" ToolTip="Sort by Church Member Status"
                                    runat="server">Church</asp:LinkButton>
                            </th>
                            <th>
                            </th>
                            <th>
                            </th>
                            <th colspan="2">
                                Attendance
                            </th>
                            <th>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                <asp:LinkButton ID="lb2" CommandName="Sort" CommandArgument="Name" runat="server">Name</asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lb3" CommandName="Sort" CommandArgument="MemberType" ToolTip="Sort by Organization Member Type"
                                    runat="server">Member</asp:LinkButton>
                            </th>
                            <th align="left">
                                <asp:LinkButton ID="lb3a" CommandName="Sort" CommandArgument="Age" runat="server">Age</asp:LinkButton>
                                -
                                <asp:LinkButton ID="lb4" CommandName="Sort" CommandArgument="DOB" ToolTip="Sort by Birthday"
                                    runat="server">Bday</asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lb5" CommandName="Sort" CommandArgument="Address" runat="server">Primary Address</asp:LinkButton>
                            </th>
                            <th>
                                Communication
                            </th>
                            <th>
                                <asp:LinkButton ID="lb7" CommandName="Sort" CommandArgument="AttendPct" runat="server">Pct</asp:LinkButton>
                            </th>
                            <th>
                                <asp:LinkButton ID="lb8" CommandName="Sort" CommandArgument="LastAttended" runat="server">Last</asp:LinkButton>
                            </th>
                            <th>
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
                            <asp:HyperLink ID="MemberType" CssClass="thickbox2" Text='<%# Eval("MemberType") %>'
                                runat="server"></asp:HyperLink>
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
                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("AttendPct", "{0:N1}%") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("LastAttended", "{0:d}") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:HyperLink ID="HyperLink1" runat="server" ToolTip="Add to/Remove from Active Tag"><%# (bool)Eval("HasTag") ? "Remove" : "Add" %></asp:HyperLink>
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table runat="server" style="">
                        <tr>
                            <td>
                                No data was returned.
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
        <asp:DataPager ID="pager2" PagedControlID="MemberGrid1" runat="server">
            <Fields>
                <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
            </Fields>
        </asp:DataPager>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:ObjectDataSource ID="MembersData" runat="server" EnablePaging="True" SelectCountMethod="Count"
    SelectMethod="OrgMembers" TypeName="CMSPresenter.OrganizationController" SortParameterName="sortExpression"
    OnSelecting="MembersData_Selecting" EnableViewState="false">
    <SelectParameters>
        <asp:QueryStringParameter Name="OrganizationId" QueryStringField="id" Type="Int32" />
        <asp:Parameter Name="Select" Type="Int32" />
        <asp:Parameter Name="GroupId" Type="Int32" />
        <asp:Parameter Name="sortExpression" Type="String" />
        <asp:Parameter Name="maximumRows" Type="Int32" />
        <asp:Parameter Name="startRowIndex" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:ObjectDataSource ID="MemberTypeData" runat="server" SelectMethod="MemberTypeCodes2"
    TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
