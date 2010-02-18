<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTags.aspx.cs"
    Inherits="CMSWeb.MyTags" Title="Tag Management" %>

<%@ Register Src="UserControls/PersonGrid.ascx" TagName="PersonGrid" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="UserControls/ExportToolBar.ascx" TagName="ExportToolBar" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript">
        $(function() {
            tb_init('a.thickbox2');
            imgLoader = new Image();
            imgLoader.src = tb_pathToImage;
        });
        function ShareWith(countstring) {
            tb_remove();
            $('<%= "#" + ShareLink.ClientID %>').text(countstring);
        }
    </script>

   <div>
        <table>
            <tr><td colspan="3"><h2>Tag List</h2></td></tr>
            <tr>
                <th align="right">
                    Change Active Tag:
                </th>
                <td>
                    <asp:DropDownList ID="Tags" runat="server" DataTextField="Value" DataSourceID="UserTags"
                        DataValueField="Code" AutoPostBack="True" OnSelectedIndexChanged="Tags_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;
                </td>
                <td>
                    <asp:LinkButton ID="delTag" runat="server" OnClick="delTag_Click">Delete Tag</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <th align="right">
                    New Tag Name:
                </th>
                <td>
                    <span class="footer">
                        <asp:TextBox ID="TagName" runat="server" Width="225px" MaxLength="50" ></asp:TextBox>
                    </span>
                </td>
                <td>
                    <asp:LinkButton ID="setNewTag" runat="server" OnClick="setNewTag_Click" ValidationGroup="TagName">Make New 
                    Tag</asp:LinkButton>&nbsp;|
                    <asp:LinkButton ID="renameTag" runat="server" OnClick="renameTag_Click" ValidationGroup="TagName">Rename Tag</asp:LinkButton>
                    <asp:RequiredFieldValidator ID="TagNameValidator" runat="server" ErrorMessage="TagName is Required"
                        ControlToValidate="TagName" ValidationGroup="TagName"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator
                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="TagName" ValidationGroup="TagName" ErrorMessage="no colon allowed" ValidationExpression="[^:]*"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <th>
                </th>
                <td>
                    <asp:HyperLink nodisable="true" runat="server" ID="ShareLink" class="thickbox2" NavigateUrl="~/Dialog/AddTagShareds.aspx?TB_iframe=true&height=450&width=600"></asp:HyperLink>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <asp:Button ID="Refresh" runat="server" Text="Refresh" OnClick="Refresh_Click" />
        <br />
    </div>
    <br />
    <uc2:ExportToolBar ID="ExportToolBar1" runat="server" />
    <div style="clear: both">
        <uc1:PersonGrid ID="PersonGrid1" runat="server" DataSourceID="PeopleData" />
    </div>
    <asp:ObjectDataSource ID="PeopleData" runat="server" EnablePaging="True" SelectMethod="FetchPeopleList"
        TypeName="CMSPresenter.TagController" SelectCountMethod="Count" SortParameterName="sortExpression">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="" />
            <asp:ControlParameter ControlID="Tags" Name="tag" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="UserTags" runat="server" SelectMethod="UserTags"
        TypeName="CMSPresenter.TagController">
        <SelectParameters>
            <asp:QueryStringParameter DbType="Int32" QueryStringField="pid" Name="pid" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
