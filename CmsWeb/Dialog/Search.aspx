<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Search.aspx.cs" Inherits="CMSWeb.Dialog.Search"
    StylesheetTheme="Minimal" %>

<%@ Register Src="~/UserControls/GridPager.ascx" TagName="GridPager" TagPrefix="uc1" %>
<%@ Register TagPrefix="user" TagName="QuickSearchParameters" Src="~/UserControls/QuickSearchParameters.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <base target="_self" />
    <title>Search Dialog</title>
    </head>

<script type="text/javascript">
    function ToggleCallback(e)
    {
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
        Count:
        <asp:Label ID="GridCount" runat="server" Text="0"></asp:Label>
        &nbsp;<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Must select single member of existing family"></asp:CustomValidator>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
            DataSourceID="PeopleData" AutoGenerateColumns="False" 
            SkinID="GridViewSkin" DataKeyNames="PeopleId"
            Visible="False" OnRowDataBound="GridView1_RowDataBound" PageSize="10" 
            onrowcommand="GridView1_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Select" ShowHeader="False">
                    <ItemTemplate>
                        <asp:CheckBox ID="ck" runat="server" Checked='<%# (bool)Eval("HasTag") %>' onclick='<%# Eval("PeopleId", "PageMethods.ToggleTag({0}, this.id, ToggleCallback); return false;") %>' Visible='<%# !selectSingle %>'>
                        </asp:CheckBox>
                        <asp:LinkButton ID="select" runat="server" CommandName="select" CommandArgument='<%# Eval("PeopleId") %>' Visible='<%# selectSingle %>'>select</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                <asp:BoundField DataField="Address" HeaderText="Address" SortExpression="Address" />
                <asp:BoundField DataField="CityStateZip" HeaderText="CityStateZip" SortExpression="CityStateZip" />
                <asp:BoundField DataField="Age" HeaderText="Age" SortExpression="Age" />
            </Columns>
            <PagerTemplate>
                <uc1:GridPager ID="GridPager1" runat="server" />
            </PagerTemplate>
        </asp:GridView>
        <asp:HiddenField ID="UsersOnly" runat="server" />        
        <asp:ObjectDataSource ID="PeopleData" runat="server" EnablePaging="True" SelectMethod="FetchPeopleList"
            TypeName="CMSPresenter.PersonSearchController" SelectCountMethod="Count" SortParameterName="sortExpression"
            OnSelected="PeopleData_Selected" OnObjectCreating="PeopleData_ObjectCreating">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:Parameter Name="sortExpression" Type="String" DefaultValue="" />
                <asp:ControlParameter ControlID="Parameters" Name="namesearch" PropertyName="Name"
                    Type="String" />
                <asp:ControlParameter ControlID="Parameters" Name="commsearch" PropertyName="Comm"
                    Type="String" />
                <asp:ControlParameter ControlID="Parameters" Name="addrsearch" PropertyName="Addr"
                    Type="String" />
                <asp:ControlParameter ControlID="Parameters" Name="memstatus" PropertyName="Member"
                    Type="Int32" />
                <asp:ControlParameter ControlID="Parameters" Name="tag" PropertyName="Tag" Type="Int32" />
                <asp:ControlParameter ControlID="Parameters" Name="dob" PropertyName="DOB" Type="String" />
                <asp:ControlParameter ControlID="Parameters" Name="gender" PropertyName="Gender"
                    Type="Int32" />
                <asp:ControlParameter ControlID="Parameters" Name="orgid" PropertyName="OrgId"
                    Type="Int32" />
                <asp:ControlParameter ControlID="Parameters" Name="campus" PropertyName="Campus"
                    Type="Int32" />
                    <asp:ControlParameter ControlID="UsersOnly" Name="usersonly" PropertyName="Value" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
