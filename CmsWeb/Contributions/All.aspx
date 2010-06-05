<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="All.aspx.cs" Inherits="CMSWeb.Contributions.All" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="modalPopup">
        <tr>
            <th>
                Name:
            </th>
            <td colspan="3">
                <asp:HyperLink ID="PersonLink" runat="server"></asp:HyperLink><br />
            </td>
        </tr>
        <tr>
            <th>
                Type:
            </th>
            <td>
                <asp:DropDownList ID="TypeSearch" runat="server" DataSourceID="TypesODS" DataTextField="Value"
                    DataValueField="Id" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">(not specified)</asp:ListItem>
                </asp:DropDownList>
            </td>
            <th>
                Fund:
            </th>
            <td>
                <asp:DropDownList ID="FundSearch" runat="server" DataTextField="Value" DataValueField="Id"
                    DataSourceID="FundsODS" AppendDataBoundItems="True">
                    <asp:ListItem Value="0">(not specified)</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                Status:
            </th>
            <td>
                <asp:DropDownList ID="StatusSearch" runat="server" DataSourceID="StatusesODS" DataTextField="Value"
                    DataValueField="Id" AppendDataBoundItems="True">
                    <asp:ListItem Value="99">(not specified)</asp:ListItem>
                </asp:DropDownList>
            </td>
            <th>
                Year:
            </th>
            <td>
                <asp:DropDownList ID="YearSearch" runat="server" DataSourceID="YearsODS" AppendDataBoundItems="True"
                    OnDataBound="YearSearch_DataBound">
                    <asp:ListItem Value="0">(not specified)</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
            </td>
            <td colspan="2" align="center">
                <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click"
                    TabIndex="6" Width="114px" />
            </td>
        </tr>
    </table>
    <asp:DataPager ID="DataPager2" PagedControlID="ListView1" runat="server">
        <Fields>
            <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
        </Fields>
    </asp:DataPager>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1">
        <LayoutTemplate>
            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                <tr runat="server" style="">
                    <th id="Th1" runat="server">
                        <asp:LinkButton ID="LinkButton1" CommandName="Sort" CommandArgument="Name" runat="server">Name</asp:LinkButton>
                    </th>
                    <th id="Th2" runat="server">
                        <asp:LinkButton ID="LinkButton7" CommandName="Sort" CommandArgument="Fund" runat="server">Fund</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton ID="LinkButton2" CommandName="Sort" CommandArgument="Type" runat="server">Type</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton ID="LinkButton3" CommandName="Sort" CommandArgument="Date" runat="server">Date</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton ID="LinkButton4" CommandName="Sort" CommandArgument="Amount" runat="server">Amount</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton ID="LinkButton5" CommandName="Sort" CommandArgument="Status" runat="server">Status</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton ID="LinkButton6" CommandName="Sort" CommandArgument="Pledge" runat="server">Pledge</asp:LinkButton>
                    </th>
                </tr>
                <tr id="itemPlaceholder" runat="server">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:HyperLink ID="HyperLink2" NavigateUrl='<%# Eval("PeopleId", "~/Contributions/Years.aspx?id={0}") %>'
                        Text='<%# Eval("Name") %>' runat="server"></asp:HyperLink>
                </td>
                <td>
                    <asp:Label ID="FundLabel" runat="server" Text='<%# Eval("Fund") %>' />
                </td>
                <td>
                    <asp:Label ID="ContributionTypeLabel" runat="server" Text='<%# Eval("ContributionType") %>' />
                </td>
                <td>
                    <asp:Label ID="ContributionDateLabel" runat="server" ToolTip='<%# Eval("ContributionId") %>'
                        Text='<%# Eval("ContributionDate", "{0:d}") %>' />
                </td>
                <td align="right">
                    <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# Eval("BundleId", "~/Contributions/Bundle.aspx?id={0}") %>'
                        Text='<%# Eval("ContributionAmount", "{0:n2}") %>' runat="server"></asp:HyperLink>
                </td>
                <td>
                    <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="PledgeCheckBox" runat="server" Checked='<%# Eval("Pledge") %>'
                        Enabled="false" />
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
    <asp:DataPager ID="DataPager1" PagedControlID="ListView1" runat="server">
        <Fields>
            <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
        </Fields>
    </asp:DataPager>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" EnablePaging="True" OldValuesParameterFormatString="original_{0}"
        SelectCountMethod="CountContributions" SelectMethod="FetchContributions" SortParameterName="sortExpression"
        TypeName="CMSPresenter.BundleController">
        <SelectParameters>
            <asp:ControlParameter ControlID="YearSearch" Name="year" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:ControlParameter ControlID="StatusSearch" Name="statusid" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="TypeSearch" Name="typeid" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="FundSearch" Name="fundid" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="StatusesODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="ContributionStatuses" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="TypesODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="ContributionTypes" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="FundsODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="Funds" TypeName="CMSPresenter.BundleController">
        <SelectParameters>
            <asp:Parameter Name="PeopleId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="YearsODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="Years" TypeName="CMSPresenter.BundleController">
        <SelectParameters>
            <asp:Parameter Name="PeopleId" Type="Int32" DefaultValue="0" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
