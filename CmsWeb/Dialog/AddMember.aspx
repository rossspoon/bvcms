<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="AddMember.aspx.cs"
    Inherits="CMSWeb.Dialog.AddMember" StylesheetTheme="Minimal" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="user" TagName="QuickSearchParameters" Src="~/UserControls/QuickSearchParameters.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Add Member Dialog</title>
</head>

<script type="text/javascript">
    function ToggleCallback(e) {
        var result = eval('(' + e + ')');
        $get(result.ControlId).checked = result.HasTag;
    }
</script>

<body>
    <form id="form1" runat="server">
    <div>
        <input type="hidden" id="retval" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <user:QuickSearchParameters ID="Parameters" runat="server" />
        <div id="AddNewSection" runat="server">
            <asp:Button ID="AddNew1" runat="server" Text="Add New Person" OnClick="AddNew1_Click" />
            <cc2:ConfirmButtonExtender ID="AddNew1_ConfirmButtonExtender" runat="server" 
                ConfirmText="Are you sure you want to add a new person?" Enabled="True" TargetControlID="AddNew1">
            </cc2:ConfirmButtonExtender>
            <asp:RadioButtonList ID="FamilyOption" RepeatDirection="Horizontal" RepeatLayout="Flow"
                runat="server">
                <asp:ListItem Value="0" Selected="True">Existing Family</asp:ListItem>
                <asp:ListItem Value="1">New Family</asp:ListItem>
                <asp:ListItem Value="2">Couple</asp:ListItem>
            </asp:RadioButtonList>
            <br />
            <asp:DropDownList ID="MemberType" runat="server" DataSourceID="MemberTypeData" DataTextField="Value"
                DataValueField="Id">
            </asp:DropDownList>
            &nbsp;Enrollment:
            <asp:TextBox ID="EnrollmentDate" runat="server" ToolTip="Enrollment Date" 
                Height="21px" Width="80px"></asp:TextBox>
            <cc2:CalendarExtender ID="EnrollmentDate_CalendarExtender" runat="server" Enabled="True"
                PopupPosition="TopLeft" TargetControlID="EnrollmentDate">
            </cc2:CalendarExtender>
        &nbsp; InActive:
            <asp:TextBox ID="InactiveDate" runat="server" ToolTip="Goes Inactive Date" 
                Height="22px" Width="80px"></asp:TextBox>
            <cc2:CalendarExtender ID="InactiveDate_CalendarExtender" runat="server" Enabled="True"
                PopupPosition="TopLeft" TargetControlID="InactiveDate">
            </cc2:CalendarExtender>
        </div>
        <asp:Button ID="AddSelectedMembers" runat="server" OnClick="AddSelectedMembers_Click"
            Text="Add Selected" /> <asp:CheckBox ID="SelectAll" Text="Select All" 
            runat="server" AutoPostBack="True" 
            oncheckedchanged="SelectAll_CheckedChanged" />
        &nbsp;<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Must select single member of existing family"></asp:CustomValidator>
        <div style="overflow: auto; height: 200px;">
            <asp:ListView ID="ListView1" runat="server" DataKeyNames="PeopleId"
                Visible="false">
                <LayoutTemplate>
                    <table id="Table1" runat="server">
                        <tr id="Tr2" runat="server">
                            <td id="Td2" runat="server">
                                <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                                    <tr id="Tr3" runat="server" style="">
                                        <th id="Th1" runat="server">
                                            Select
                                        </th>
                                        <th id="Th2" runat="server">
                                            Name
                                        </th>
                                        <th id="Th3" runat="server">
                                            Address
                                        </th>
                                        <th id="Th4" runat="server">
                                            CityStateZip
                                        </th>
                                        <th id="Th5" runat="server">
                                            Age
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr id="Tr4" style='background-color: <%# (Container.DataItemIndex % 2 == 0)?"#eee":"#fff" %>'
                        title='<%# Eval("ToolTip") %>'>
                        <td>
                            <asp:CheckBox ID="ck" runat="server" Visible='<%# (int)Eval("PeopleId")>0 %>' Checked='<%# (bool)Eval("HasTag") %>' onclick='<%# Eval("PeopleId", "PageMethods.ToggleTag({0}, this.id, ToggleCallback); return false;") %>'>
                            </asp:CheckBox>
                        </td>
                        <td>
                            <asp:Label ID="NameLabel" runat="server" Text='<%# Eval("Name") %>' />
                        </td>
                        <td>
                            <asp:Label ID="AddressLabel" runat="server" Text='<%# Eval("Address") %>' />
                        </td>
                        <td>
                            <asp:Label ID="CityStateZipLabel" runat="server" Text='<%# Eval("CityStateZip") %>' />
                        </td>
                        <td>
                            <asp:Label ID="AgeLabel" runat="server" Text='<%# Eval("Age") %>' />
                        </td>
                    </tr>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <table id="Table2" runat="server" style="">
                        <tr>
                            <td>
                                No data was returned.
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
        <asp:ObjectDataSource ID="MemberTypeData" runat="server" SelectMethod="MemberTypeCodes2"
            TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
