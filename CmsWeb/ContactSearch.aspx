<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="ContactSearch.aspx.cs"
    Inherits="CmsWeb.ContactSearch" Title="Contact Search" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register src="UserControls/GridPager.ascx" tagname="GridPager" tagprefix="uc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="roundbox floatleft">
        <h2>Contacts</h2>
        <table cellspacing="5">
            <tr>
                <th align="right">
                    Contactee Name:
                </th>
                <td>
                    <asp:TextBox ID="ContacteeNameSearch" runat="server" ToolTip="Part of the Contactees Name"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    Contactor Name:
                </th>
                <td>
                    <asp:TextBox ID="ContactorNameSearch" runat="server" ToolTip="Part of the Contactors Name"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th align="right">
                    Contact Start Date: 
                </th>
                <td>
                    <asp:TextBox ID="startDate" runat="server"></asp:TextBox>
                    <cc2:CalendarExtender ID="TextBox1_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="startDate">
                    </cc2:CalendarExtender>
                </td>
            </tr>
            <tr>
                <th align="right">
                    Contact End Date: 
                </th>
                <td>
                    <asp:TextBox ID="endDate" runat="server"></asp:TextBox>
                    <cc2:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="endDate">
                    </cc2:CalendarExtender>
                </td>
            </tr>
            <tr>
                <th align="right">
                    Contact Type:
                </th>
                <td>
                    <cc1:DropDownCC ID="TypeList" runat="server" DataTextField="Value" DataSourceID="TypeListData"
                        DataValueField="Id" AppendDataBoundItems="True">
                        <asp:ListItem Value="0">(not specified)</asp:ListItem>
                    </cc1:DropDownCC>
                </td>
            </tr>
            <tr>
                <th align="right">
                    Contact Reason:
                </th>
                <td>
                    <cc1:DropDownCC ID="ReasonList" runat="server" DataTextField="Value" DataSourceID="ReasonListData"
                        DataValueField="Id" AppendDataBoundItems="True">
                        <asp:ListItem Value="0">(not specified)</asp:ListItem>
                    </cc1:DropDownCC>
                </td>
            </tr>
            <tr>
                <th align="right">
                    Ministry:
                </th>
                <td>
                    <cc1:DropDownCC ID="MinistryList" runat="server" DataTextField="Value" DataSourceID="MinistryListData"
                        DataValueField="Id" AppendDataBoundItems="False">
                    </cc1:DropDownCC>
                </td>
            </tr>
            <tr>
                <td>Count:<asp:Label ID="GridCount" style="font-weight:bold" runat="server" Text="0"></asp:Label>
                            
                </td>
                <td align="right">
                    <asp:LinkButton ID="NewSearch" runat="server" OnClick="NewSearch_Click" CssClass="bt">Clear</asp:LinkButton>
                    <asp:Button ID="SearchButton" Style="margin-bottom: .5em; margin-left: .5em" runat="server"
                        Text="Search" OnClick="SearchButton_Click" TabIndex="6"  CssClass="bt"/>
                </td>
            </tr>
                    </table>
    </div>
<br /><br /><br /><br />
&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="QuerySearch" runat="server" ToolTip="Query results are the Contactors, not the Contactees"
                                onclick="QuerySearch_Click" CssClass="bt">Convert to Query</asp:LinkButton>
    <div class="floatclear"></div>

    <asp:GridView ID="ContactGrid" CssClass="aspgrid" runat="server" AllowPaging="True"
        PagerSettings-Position="Bottom" AutoGenerateColumns="False" PageSize="10" 
        AllowSorting="True" DataSourceID="ContactData"
        CellPadding="4" ForeColor="#333333" GridLines="None" >
        <PagerSettings Position="TopAndBottom" />
        <FooterStyle BackColor="#4296CC" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle CssClass="pagerstyle" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#4296CC" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#EFF3FB"/>
        <AlternatingRowStyle BackColor="White" />
    <Columns>
        <asp:TemplateField HeaderText="Date" SortExpression="Date">
            <ItemTemplate>
                <asp:HyperLink ID="ContactLink" runat="server" NavigateUrl='<%# Eval("ContactId", "~/Contact.aspx?id={0}") %>'
                    Text='<%# Eval("ContactDate", "{0:d}") %>'></asp:HyperLink>
                <br />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ContactReason" HeaderText="Reason" SortExpression="Reason" />
        <asp:BoundField DataField="TypeOfContact" HeaderText="Type" SortExpression="Type" />
        <asp:TemplateField HeaderText="Contactees">
            <ItemTemplate>
                <asp:Label runat="server" ID="ContacteeList" Text='<%# GetContacteeList((int)Eval("ContactId")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerTemplate>
        <uc1:GridPager ID="GridPager1" runat="server" />
    </PagerTemplate>
    </asp:GridView>
    <asp:ObjectDataSource ID="ContactData" runat="server" EnablePaging="True" SelectMethod="FetchContactList"
        TypeName="CMSPresenter.ContactSearchController" SelectCountMethod="Count" OnSelected="ContactData_Selected"
        SortParameterName="sortExpression" EnableViewState="False">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Date DESC" />
            <asp:ControlParameter ControlID="ContacteeNameSearch" Name="contacteeNameSearch" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ContactorNameSearch" Name="contactorNameSearch" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="startDate" Name="startDate" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="endDate" Name="endDate" PropertyName="Text" Type="String" />
            <asp:ControlParameter ControlID="ReasonList" Name="reasonCode" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="TypeList" Name="typeCode" PropertyName="SelectedValue" Type="Int32" />
            <asp:ControlParameter ControlID="MinistryList" Name="ministryCode" PropertyName="SelectedValue" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="TypeListData" runat="server" SelectMethod="ContactTypeCodes" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ReasonListData" runat="server" SelectMethod="ContactReasonCodes" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="MinistryListData" runat="server" SelectMethod="Ministries0" TypeName="CMSPresenter.CodeValueController">
    </asp:ObjectDataSource>
</asp:Content>

<asp:Content ID="Contentscr" ContentPlaceHolderID="scripts" runat="server">
<script type="text/javascript">
    $(function () {
        $(".bt").button();
        $(".roundbox select").css("width", "100%");
    });
</script>
</asp:Content>