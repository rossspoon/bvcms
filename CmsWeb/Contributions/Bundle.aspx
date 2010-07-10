<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Bundle.aspx.cs" Inherits="CMSWeb.Contributions.Bundle" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HyperLink ID="BundlesLink" NavigateUrl="~/Contributions/Bundles.aspx" runat="server">Bundles</asp:HyperLink>
    <table class="Design2">
        <tr>
            <th>
                Bundle Id
            </th>
            <td>
                <asp:Label ID="BundleId" runat="server">1001</asp:Label>
            </td>
        </tr>
        <tr id="TR_BundleStatusId" runat="server">
            <th>
                Bundle Status:
            </th>
            <td>
                <cc1:DisplayOrEditDropDown ID="BundleStatusIdDropDown" runat="server" BindingMember="BundleStatusId"
                    BindingSource="bundleheader" BindingMode="TwoWay" DataTextField="Value" DataValueField="Id"
                    AssociatedRowId="TR_BundleStatusId" DataSourceID="StatusTypesODS" MakeDefault0="False" />
            </td>
        </tr>
        <tr id="TR_ContributionDate" runat="server">
            <th>
                Contribution Date:
            </th>
            <td>
                <cc1:DisplayOrEditDate ID="ContributionDate" runat="server" BindingSource="bundleheader"
                    BindingMode="TwoWay" AssociatedRowId="TR_ContributionDate" />
                <cc2:CalendarExtender ID="ContributionDate_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="ContributionDate">
                </cc2:CalendarExtender>
            </td>
        </tr>
        <tr id="TR_BundleHeaderTypeId" runat="server">
            <th>
                Bundle Type:
            </th>
            <td>
                <cc1:DisplayOrEditDropDown ID="BundleHeaderTypeIdDropDown" runat="server" BindingMember="BundleHeaderTypeId"
                    BindingSource="bundleheader" BindingMode="TwoWay" DataTextField="Value" DataValueField="Id"
                    AssociatedRowId="TR_BundleHeaderTypeId" DataSourceID="HeaderTypesODS" MakeDefault0="False" />
            </td>
        </tr>
        <tr>
            <th>
                Deposit Date:
            </th>
            <td>
                <cc1:DisplayOrEditDate ID="DepositDate" runat="server" BindingSource="bundleheader"
                    BindingMode="TwoWay" />
                <cc2:CalendarExtender ID="DepositDate_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="DepositDate">
                </cc2:CalendarExtender>
            </td>
        </tr>
        <tr>
            <th>
                Total Cash:
            </th>
            <td>
                <cc1:DisplayOrEditText ID="TotalCash" runat="server" BindingSource="bundleheader"
                    BindingMode="TwoWay" />
                <asp:RequiredFieldValidator ID="ReqTotalCash" runat="server" 
                    ErrorMessage="required" ControlToValidate="TotalCash"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="ValTotalCash" runat="server" ControlToValidate="TotalCash"
                    ErrorMessage="amount invalid" ValidationExpression="^\d*(\.\d{0,2})?$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                Total Checks:
            </th>
            <td>
                <cc1:DisplayOrEditText ID="TotalChecks" runat="server" BindingSource="bundleheader"
                    BindingMode="TwoWay" />
                <asp:RequiredFieldValidator ID="ReqTotalChecks" runat="server" 
                    ErrorMessage="required" ControlToValidate="TotalChecks"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TotalChecks"
                    ErrorMessage="amount invalid" ValidationExpression="^\d*(\.\d{0,2})?$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                Total Coin:
            </th>
            <td>
                <cc1:DisplayOrEditText ID="TotalEnvelopes" runat="server" BindingSource="bundleheader"
                    BindingMode="TwoWay" />
                <asp:RequiredFieldValidator ID="ReqTotalEnv" runat="server" 
                    ErrorMessage="required" ControlToValidate="TotalEnvelopes"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TotalEnvelopes"
                    ErrorMessage="amount invalid" ValidationExpression="^\d*(\.\d{0,2})?$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr id="TR_FundId" runat="server">
            <th>
                Fund:
            </th>
            <td>
                <cc1:DisplayOrEditDropDown ID="FundIdDropDown" runat="server" BindingMember="FundId"
                    BindingSource="bundleheader" BindingMode="TwoWay" DataTextField="Value" DataValueField="Id"
                    AssociatedRowId="TR_FundId" DataSourceID="FundsODS" MakeDefault0="True" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">(not specified)</asp:ListItem>
                </cc1:DisplayOrEditDropDown>
                <asp:HyperLink ID="FundsLink" runat="server" Target="_blank">fund totals</asp:HyperLink>
            </td>
        </tr>
    </table>
    <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" OnClick="EditUpdateButton1_Click" />
    <asp:ImageButton ID="Delete" runat="server" ImageUrl="~/images/delete.gif"
        OnClick="Delete_Click" OnClientClick="return confirm('Are you sure you want to delete?')" />
    &nbsp;|
    <asp:LinkButton ID="NewBundle" runat="server" OnClick="NewBundle_Click">Create New Bundle</asp:LinkButton>
    | <a href="/PostBundle/Index/<%=bundleheader.BundleHeaderId %>">PostBundle (edit)</a>
    <table style="font-size: large">
        <tr>
            <td>
                Total Header:
            </td>
            <td>
                <asp:Label ID="TotalHeader" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                Total Items:
            </td>
            <td>
                <asp:Label ID="TotalItems" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="ContributionsODS" OnItemCreated="ListView1_ItemCreated">
        <LayoutTemplate>
            <table runat="server">
                <tr runat="server">
                    <td runat="server">
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server">
                                    Fund
                                </th>
                                <th runat="server">
                                    Type
                                </th>
                                <th runat="server">
                                    Name
                                </th>
                                <th runat="server">
                                    Date
                                </th>
                                <th runat="server">
                                    Amount
                                </th>
                                <th runat="server">
                                    Status
                                </th>
                                <th runat="server">
                                    Pledge
                                </th>
                                <th runat="server">
                                    Notes
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server">
                    <td runat="server" style="">
                    </td>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="FundLabel" runat="server" Text='<%# Eval("Fund") %>' />
                </td>
                <td>
                    <asp:Label ID="ContributionTypeLabel" runat="server" Text='<%# Eval("ContributionType") %>' />
                </td>
                <td>
                    <asp:HyperLink ID="NameLink" NavigateUrl='<%# Eval("PeopleId", "~/Contributions/Years.aspx?id={0}") %>'
                        Text='<%# Eval("Name") %>' runat="server"></asp:HyperLink>
                </td>
                <td>
                    <asp:Label ID="ContributionDateLabel" runat="server" Text='<%# Eval("ContributionDate", "{0:d}") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="ContributionAmountLabel" runat="server" Text='<%# Eval("ContributionAmount", "{0:c}") %>' />
                </td>
                <td>
                    <asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' />
                </td>
                <td>
                    <asp:CheckBox ID="PledgeCheckBox" runat="server" Checked='<%# Eval("Pledge") %>'
                        Enabled="false" />
                </td>
                <td>
                    <asp:Label ID="NotesLabel" runat="server" Text='<%# Eval("Description") %>' />
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
    <asp:ObjectDataSource ID="ContributionsODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="FetchBundleDetails" TypeName="CMSPresenter.BundleController">
        <SelectParameters>
            <asp:QueryStringParameter Name="bundleId" QueryStringField="Id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="StatusTypesODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="BundleStatusTypes" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="HeaderTypesODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="BundleHeaderTypes" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="FundsODS" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="OpenFunds" TypeName="CMSPresenter.BundleController"></asp:ObjectDataSource>
</asp:Content>
