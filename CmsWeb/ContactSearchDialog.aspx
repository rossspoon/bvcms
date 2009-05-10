<%@ Page Language="C#" StylesheetTheme="Standard" AutoEventWireup="true" CodeBehind="ContactSearchDialog.aspx.cs"
    Inherits="CMSWeb.ContactSearchDialog" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="hidden" id="retval" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <table class="modalPopup">
            <tr>
                <th>
                    Contactee Name:
                </th>
                <td>
                    <asp:TextBox ID="ContacteeNameSearch" runat="server" ToolTip="Part of the Contactees Name"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    Contactor Name:
                </th>
                <td>
                    <asp:TextBox ID="ContactorNameSearch" runat="server" ToolTip="Part of the Contactors Name"></asp:TextBox>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    Contact Start Date:
                </th>
                <td>
                    <asp:TextBox ID="startDate" runat="server"></asp:TextBox>
                    <cc2:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="startDate">
                    </cc2:CalendarExtender>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    Contact End Date:
                </th>
                <td>
                    <asp:TextBox ID="endDate" runat="server"></asp:TextBox>
                    <cc2:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" Enabled="True"
                        TargetControlID="endDate">
                    </cc2:CalendarExtender>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    Contact Type:
                </th>
                <td>
                    <cc1:DropDownCC ID="TypeList" runat="server" DataTextField="Value" DataSourceID="TypeListData"
                        DataValueField="Id" AppendDataBoundItems="True">
                        <asp:ListItem Value="0" runat="server">(not specified)</asp:ListItem>
                    </cc1:DropDownCC>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th>
                    Contact Reason:
                </th>
                <td>
                    <cc1:DropDownCC ID="ReasonList" runat="server" DataTextField="Value" DataSourceID="ReasonListData"
                        DataValueField="Id" AppendDataBoundItems="True">
                        <asp:ListItem Value="0">(not specified)</asp:ListItem>
                    </cc1:DropDownCC>
                </td>
                <td>
                    <asp:Button ID="SearchButton" Style="margin-bottom: .5em; margin-left: .5em" runat="server"
                        Text="Search" OnClick="SearchButton_Click" TabIndex="6" />
                </td>
            </tr>
            <tr>
                <th>
                    Ministry:
                </th>
                <td>
                    <cc1:DropDownCC ID="MinistryList" runat="server" DataTextField="Value" DataSourceID="MinistryListData"
                        DataValueField="Id" AppendDataBoundItems="False">
                    </cc1:DropDownCC>
                </td>
            </tr>
        </table>
        <asp:Button ID="AddNew1" runat="server" Text="Add New Contact" OnClientClick="parent.AddSelectedContact(-1)" />
    </div>
    <asp:DataPager ID="DataPager2" PagedControlID="ListView1" runat="server">
        <Fields>
            <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
        </Fields>
    </asp:DataPager>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="ContactData">
        <LayoutTemplate>
            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                <tr runat="server" style="">
                    <th runat="server">
                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Date">Date</asp:LinkButton>
                    </th>
                    <th runat="server">
                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Reason">Reason</asp:LinkButton>
                    </th>
                    <th id="Th1" runat="server">
                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="Type">Type</asp:LinkButton>
                    </th>
                    <th id="Th2" runat="server">
                        Contactees
                    </th>
                </tr>
                <tr id="itemPlaceholder" runat="server">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'>
                <td>
                    <asp:LinkButton ID="select" runat="server" OnClientClick='<%# Eval("ContactId", "parent.AddSelectedContact({0})") %>'>select</asp:LinkButton>
                </td>
                <td>
                    <asp:Label ID="ContactDateLabel" runat="server" Text='<%# Eval("ContactDate", "{0:d}") %>' />
                </td>
                <td>
                    <asp:Label ID="ContactReasonLabel" runat="server" Text='<%# Eval("ContactReason") %>' />
                </td>
                <td>
                    <asp:Label ID="TypeOfContactLabel" runat="server" Text='<%# Eval("TypeOfContact") %>' />
                </td>
                <td>
                    <asp:Label runat="server" ID="ContacteeList" Text='<%# GetContacteeList((int)Eval("ContactId")) %>' />
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
    <asp:ObjectDataSource ID="ContactData" runat="server" EnablePaging="True" SelectMethod="FetchContactList"
        TypeName="CMSPresenter.ContactSearchController" SelectCountMethod="Count" SortParameterName="sortExpression">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="" />
            <asp:ControlParameter ControlID="ContacteeNameSearch" Name="contacteeNameSearch"
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ContactorNameSearch" Name="contactorNameSearch"
                PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="startDate" Name="startDate" PropertyName="Text"
                Type="String" />
            <asp:ControlParameter ControlID="endDate" Name="endDate" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ReasonList" Name="reasonCode" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="TypeList" Name="typeCode" PropertyName="SelectedValue"
                Type="Int32" />
            <asp:ControlParameter ControlID="MinistryList" Name="ministryCode" PropertyName="SelectedValue"
                Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="TypeListData" runat="server" SelectMethod="ContactTypeCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ReasonListData" runat="server" SelectMethod="ContactReasonCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="MinistryListData" runat="server" SelectMethod="Ministries"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    </form>
</body>
</html>
