<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Bundles.aspx.cs" Inherits="CmsWeb.Contributions.Bundles" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="font-size: 110%">
        <asp:LinkButton ID="NewBundle" runat="server" OnClick="NewBundle_Click" CssClass="bt">Create New Bundle</asp:LinkButton>
    </div>
    <asp:ListView ID="ListView1" runat="server" DataKeyNames="BundleId" DataSourceID="ObjectDataSource1">
        <LayoutTemplate>
            <table id="itemPlaceholderContainer" runat="server" border="0" class="grid">
                <thead>
                <tr runat="server">
                    <th id="Th3" runat="server">
                        <asp:LinkButton ID="LinkButton2" CommandName="Sort" CommandArgument="BundleId" runat="server">BundleId</asp:LinkButton>
                    </th>
                    <th id="Th4" runat="server">
                        <asp:LinkButton ID="LinkButton3" CommandName="Sort" CommandArgument="HeaderType"
                            runat="Server">Header Type</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton CommandName="Sort" CommandArgument="DepositDate" runat="Server">Deposit Date</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton CommandName="Sort" CommandArgument="TotalCash" runat="Server">Total Bundle</asp:LinkButton>
                    </th>
                    <th id="Th5" runat="server">
                        Total Items
                    </th>
                    <th id="Th1" runat="server">
                        <asp:LinkButton ID="LinkButton1" CommandName="Sort" CommandArgument="Fund" runat="Server">Fund</asp:LinkButton>
                    </th>
                    <th id="Th2" runat="server">
                        <asp:LinkButton ID="LinkButton4" CommandName="Sort" CommandArgument="Status" runat="server">Status</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton CommandName="Sort" CommandArgument="PostingDate" runat="Server">Posting Date</asp:LinkButton>
                    </th>
                    <th></th>
                </tr>
                </thead>
                <tr id="itemPlaceholder" runat="server">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'>
                <td align="right">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# Eval("BundleId", "~/Contributions/Bundle.aspx?id={0}") %>'
                        Text='<%# Eval("BundleId") %>' runat="server">HyperLink</asp:HyperLink>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("HeaderType") %>' />
                </td>
                <td>
                    <asp:HyperLink ID="DepositDateLink" Target="_blank" NavigateUrl='<%# Eval("DepositDate", "~/Contributions/Deposit.aspx?dt={0:d}") %>'
                        Text='<%# Eval("DepositDate", "{0:d}") %>' runat="server"></asp:HyperLink>
                    <asp:Label ID="DepositDateLabel" runat="server" />
                </td>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("TotalBundle", "{0:N2}") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("TotalItems", "{0:N2}") %>' />
                </td>
                <td>
                    <asp:Label ID="FundLabel" runat="server" ToolTip='<%# Eval("FundId") %>' Text='<%# Eval("Fund") %>' />
                </td>
                <td>
                    <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                </td>
                <td>
                    <asp:Label ID="PostingDateLabel" runat="server" Text='<%# Eval("PostingDate", "{0:d}") %>' />
                </td>
                <td>
                    <asp:HyperLink ID="HyperLink2" Visible=<%# (bool)Eval("open") %> NavigateUrl='<%# Eval("BundleId", "~/PostBundle/Index/{0}") %>'
                        runat="server">edit</asp:HyperLink>
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
    <asp:DataPager ID="DataPager1" runat="server" PagedControlID="ListView1">
        <Fields>
            <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
        </Fields>
    </asp:DataPager>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
        SelectCountMethod="Count" SelectMethod="FetchBundles" SortParameterName="sortExpression"
        TypeName="CMSPresenter.BundleController">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

<asp:Content ID="Contentscr" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>