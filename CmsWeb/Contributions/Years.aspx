<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Years.aspx.cs" Inherits="CmsWeb.Contributions.Years" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function CreateStatement() {
            Page_ClientValidate();
            if (Page_IsValid) {
            
                var id = '<%=id.ToString()%>';
                var fd = $get('<%=FromDate.ClientID%>').value;
                var td = $get('<%=ToDate.ClientID %>').value;
                var rb1 = $get('ctl00_ContentPlaceHolder1_rblType_0').checked;
                var rb2 = $get('ctl00_ContentPlaceHolder1_rblType_1').checked;
                var typ = 1;
                if (rb2) typ = 2;
                var args = "?id=" + id + "&fd=" + fd + "&td=" + td + "&typ=" + typ;
                var newWindowUrl = "Reports/ContributionStatementRpt.aspx" + args;
                tb_remove();
                window.open(newWindowUrl);
            }
            return Page_IsValid;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:HyperLink ID="PersonLink" runat="server"></asp:HyperLink>
    <asp:Literal runat="server" id="litContributionStatement" Visible="false">&nbsp; | &nbsp;</asp:Literal> 
    <asp:HyperLink ID="hlContributionStatement" runat="server" 
                   NavigateUrl="#TB_inline?height=100&width=300&inlineId=divCSForm&modal=true" 
                   CssClass="thickbox2">
    </asp:HyperLink><br />
    <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1">
        <LayoutTemplate>
            <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                <tr id="Tr1" runat="server" style="">
                    <th id="Th1" runat="server">
                        Year
                    </th>
                    <th id="Th2" align="right" runat="server">
                        Count
                    </th>
                    <th id="Th3" align="right" runat="server">
                        Amount
                    </th>
                </tr>
                <tr id="itemPlaceholder" runat="server">
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr style="">
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Year") %>' />
                </td>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Count", "{0:n0}") %>' />
                </td>
                <td align="right">
                    <asp:HyperLink ID="HyperLink1"  Text='<%# Eval("Amount", "{0:n}") %>' 
                    NavigateUrl='<%# Eval("PeopleId", "~/Contributions/Individual.aspx?id={0}").ToString() + Eval("Year", "&year={0}") %>'
                       runat="server"></asp:HyperLink>
                </td>
           </tr>
        </ItemTemplate>
        <EmptyDataTemplate>
            <table id="Table1" runat="server" style="">
                <tr>
                    <td>
                        No data was returned.
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:ListView>
    <div id="divCSForm" style="display:none; vertical-align:middle">
        <table class="modalDiv" width="100%" >
            <tr> 
                <td style="text-align:right">From Date:</td>
                <td><asp:TextBox ID="FromDate" runat="server" AutoPostBack="false" style="font-size: large" Width="100"></asp:TextBox></td>
            </tr>
                <tr>
                    <td style="text-align:right">To Date:</td>
                    <td><asp:TextBox ID="ToDate" runat="server" AutoPostBack="false" style="font-size: large" Width="100"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                    <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Enabled="true" Selected="True" Text="Individual" Value="1"></asp:ListItem> 
                        <asp:ListItem Enabled="false" Text="Family" Value="2"></asp:ListItem> 
                    </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center">
                        <asp:Button ID="btnSubmit" ToolTip="Press to create statement" CausesValidation="true" runat="server" 
                                    Text="Run" ValidationGroup="vgDates" OnClientClick="CreateStatement()"/>
                        <asp:Button ID="btnCancel" ToolTip="Press to Cancel" CausesValidation="false" runat="server" 
                                    Text="Cancel" OnClientClick="tb_remove()" />
                    </td>
                </tr>
        </table>
        <asp:CompareValidator ID="FromDateValidator" runat="server" ErrorMessage="CompareValidator" ControlToValidate="FromDate" ControlToCompare="ToDate" Operator="LessThanEqual" SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="From Date must be before To Date." CssClass="noPrint"></asp:CompareValidator><br />
        <asp:CompareValidator ID="ToDateValidator" runat="server" ErrorMessage="CompareValidator" ControlToValidate="ToDate" ControlToCompare="FromDate" Operator="GreaterThanEqual" SetFocusOnError="True" ValidationGroup="vgDates" Type="Date" Text="To Date must be after From Date." CssClass="noPrint"></asp:CompareValidator>
    </div>
    <cc2:CalendarExtender ID="FromExtender" runat="server" TargetControlID="FromDate"></cc2:CalendarExtender>
    <cc2:CalendarExtender ID="ToDateExtender" runat="server" TargetControlID="ToDate"></cc2:CalendarExtender>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="FetchYearlyContributions"
        TypeName="CMSPresenter.BundleController">
        <SelectParameters>
            <asp:QueryStringParameter Name="peopleId" DefaultValue="0" QueryStringField="Id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
