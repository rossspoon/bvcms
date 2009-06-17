<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMembersDialog.aspx.cs"
    Inherits="CMSWeb.EditMembersDialog" StylesheetTheme="Minimal" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register TagPrefix="user" TagName="QuickSearchParameters" Src="UserControls/QuickSearchParameters.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Edit Members Dialog</title>
</head>

<script type="text/javascript">
    function ToggleCallback(e) {
        var result = eval('(' + e + ')');
        $get(result.ControlId).checked = result.HasTag;
    }
    function Toggle(PeopleId, ControlId) {
        var gid = '<%= GroupId %>';
        var orgid = '<%= OrgId %>';
        PageMethods.ToggleTag(PeopleId, orgid, gid, ControlId, ToggleCallback);
        return false;
    }
</script>

<body>
    <form id="form1" runat="server">
    <div>
        <input type="hidden" id="retval" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <table class="modalPopup">
            <tr style="font-size: small">
                <td colspan="2">
                    <asp:LinkButton ID="ClearSearch" runat="server" OnClick="ClearSearch_Click">clear</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <th>
                    Tag:
                </th>
                <td colspan="2">
                    <asp:DropDownList ID="TagSearch" runat="server" DataTextField="Value" AutoPostBack="true"
                        DataValueField="Id" DataSourceID="UserTags" OnSelectedIndexChanged="Search_Changed">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Member Type:
                </th>
                <td colspan="2">
                    <asp:DropDownList ID="SearchMemberType" runat="server" AutoPostBack="true" DataSourceID="MemberTypeData"
                        DataTextField="Value" DataValueField="Id" AppendDataBoundItems="true" OnSelectedIndexChanged="Search_Changed">
                        <asp:ListItem Value="0">(unspecified)</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th>
                    Inactive Date:
                </th>
                <td valign="top">
                    <asp:TextBox ID="SearchInactiveDate" runat="server" Width="121px" AutoPostBack="true"
                        OnTextChanged="Search_Changed"></asp:TextBox>
                    <cc2:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" PopupPosition="TopLeft"
                        TargetControlID="SearchInactiveDate">
                    </cc2:CalendarExtender>
                </td>
                <td colspan="2" align="center">
                    &nbsp;
                </td>
            </tr>
        </table>
        <div id="EditSection" runat="server">
            <br />
            <asp:DropDownList ID="MemberType" runat="server" DataSourceID="MemberTypeData" DataTextField="Value"
                DataValueField="Id" AppendDataBoundItems="true">
                <asp:ListItem Value="-1">Drop</asp:ListItem>
            </asp:DropDownList>
            InActive Date:
            <asp:TextBox ID="InactiveDate" runat="server" ToolTip="Goes Inactive Date" Height="22px"
                Width="80px"></asp:TextBox>
            <cc2:CalendarExtender ID="InactiveDate_CalendarExtender" runat="server" Enabled="True"
                PopupPosition="TopLeft" TargetControlID="InactiveDate">
            </cc2:CalendarExtender>
            &nbsp;
            <asp:Button ID="UpdateSelectedMembers" runat="server" OnClick="UpdateSelectedMembers_Click"
                Text="Update Selected" />
        </div>
        <asp:CheckBox ID="SelectAll" Text="Select All" runat="server" AutoPostBack="True"
            OnCheckedChanged="SelectAll_CheckedChanged" />
        <div style="overflow: auto; height: 200px;">
            <asp:ListView ID="ListView1" runat="server" DataSourceID="MemberData" 
                DataKeyNames="PeopleId" onitemdatabound="ListView1_ItemDataBound">
                <LayoutTemplate>
                    <table id="Table1" runat="server">
                        <tr id="Tr1" runat="server">
                            <td id="Td1" runat="server" style="">
                                <asp:DataPager ID="pager" runat="server">
                                    <Fields>
                                        <cc1:PagerField NextPageImageUrl="~/images/arrow_right2.gif" PreviousPageImageUrl="~/images/arrow_left.gif" />
                                    </Fields>
                                </asp:DataPager>
                            </td>
                        </tr>
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
                                        <th id="Th6" runat="server">
                                            Groups
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
                            <asp:CheckBox ID="ck" runat="server" Checked='<%# (bool)Eval("HasTag") %>' 
                                onclick='<%# Eval("PeopleId", "return Toggle({0}, this.id);") %>'>
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
                        <td>
                            <%# Eval("Groups") %>
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
        <asp:ObjectDataSource ID="MemberData" runat="server" EnablePaging="True" SelectMethod="FetchOrgMemberList"
            TypeName="CMSPresenter.PersonSearchDialogController" SelectCountMethod="Count"
            SortParameterName="sortExpression">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="sortExpression" Type="String" />
                <asp:ControlParameter ControlID="SearchMemberType" Name="memtype" Type="Int32" />
                <asp:ControlParameter ControlID="TagSearch" Name="tag" Type="Int32" />
                <asp:ControlParameter ControlID="SearchInactiveDate" Name="inactive" Type="Datetime" />
                <asp:QueryStringParameter Name="orgid" QueryStringField="id" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="MemberTypeData" runat="server" SelectMethod="MemberTypeCodes2"
            TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="UserTags" runat="server" SelectMethod="UserTagsWithUnspecified"
            TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
