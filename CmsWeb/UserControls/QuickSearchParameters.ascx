<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="QuickSearchParameters.ascx.cs"
    Inherits="QuickSearchParameters" %>
<table class="modalPopup">
    <tr style="font-size: small">
        <td colspan="2">
            <asp:LinkButton ID="ClearSearch" runat="server" OnClick="ClearSearch_Click">clear</asp:LinkButton>
        </td>
        <th>
            Campus:
        </th>
        <td colspan="2" nowrap="nowrap">
            <asp:DropDownList ID="CampusSearch" runat="server"
                DataTextField="Value" DataValueField="Id" Width="150px"
                DataSourceID="ODS_Campus">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            Name/ID#:
        </th>
        <td>
            <asp:TextBox ID="NameSearch" runat="server" Width="190px" ToolTip="Starting letters of First<space>Last or just Last"></asp:TextBox>
        </td>
        <th>
            Tags:
        </th>
        <td colspan="2">
            <asp:DropDownList ID="TagSearch" runat="server" DataTextField="Value" DataValueField="Id"
                DataSourceID="UserTags" style="width: 240px">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <th>
            Communication:
        </th>
        <td>
            <asp:TextBox ID="CommunicationSearch" runat="server" Width="190px" ToolTip="Any part of any phone or email"></asp:TextBox>
        </td>
        <th>
            Member:
        </th>
        <td>
            <asp:DropDownList ID="MemberSearch" runat="server" DataSourceID="MemberStatusData"
                DataTextField="Value" DataValueField="Id">
            </asp:DropDownList>
        </td>
        <th align="left">OrgId</th>
    </tr>
    <tr>
        <th>
            Address:
        </th>
        <td>
            <asp:TextBox ID="AddressSearch" runat="server" Width="190px" ToolTip="Any part of the address, city or zip">
            </asp:TextBox>
        </td>
        <th>
            Gender:
        </th>
        <td>
            <asp:DropDownList ID="GenderSearch" runat="server" DataSourceID="GenderCodes" DataTextField="Value"
                DataValueField="Id">
            </asp:DropDownList>
        </td>
        <td>
            <asp:TextBox ID="OrgIdSearch" runat="server" Width="50px" ToolTip="Organization Id"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <th>
            Date of Birth:
        </th>
        <td valign="top">
            <asp:TextBox ID="DOBSearch" runat="server" Width="190px" ToolTip="YYYY or MM or MM/DD or MM/DD/YY">
            </asp:TextBox>
        </td>
        <th>
            Married:
        </th>
        <td> <asp:DropDownList ID="MarriedSearch" runat="server" DataSourceID="MaritalCodes" DataTextField="Value"
             DataValueField="Id"></asp:DropDownList>
        </td>
        <td>
            <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click"
                TabIndex="6" />
        </td>
    </tr>
</table>
<asp:ObjectDataSource ID="MemberStatusData" runat="server" SelectMethod="MemberStatusCodes0"
    TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="UserTags" runat="server" SelectMethod="UserTagsWithUnspecified"
    TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="GenderCodes" runat="server" SelectMethod="GenderCodesWithUnspecified"
    TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="ODS_Campus" runat="server" SelectMethod="AllCampuses0"
    TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
<asp:ObjectDataSource ID="MaritalCodes" runat="server" SelectMethod="MaritalStatusCodes0"
    TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
