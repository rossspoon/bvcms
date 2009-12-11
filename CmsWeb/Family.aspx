<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="Family.aspx.cs"
    Inherits="CMSWeb.FamilyPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="UserControls/Address.ascx" TagName="address" TagPrefix="uc1" %>
<%@ Register Src="UserControls/GridPager.ascx" TagName="GridPager" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
         $(function() {
            if (<%=EditUpdateButton1.Editing?"true":"false"%>)
                $('a.thickbox2').unbind("click")
            else
                tb_init('a.thickbox2');
            imgLoader = new Image();
            imgLoader.src = tb_pathToImage;

            var $maintabs = $("#main-tab").tabs();
            var $addrtabs = $("#Address-tab").tabs();
            var t = $.cookie('maintab2');
            if (t)
                $maintabs.tabs('select', parseInt(t));
            $("#main-tab > ul > li > a").click(function() {
                var selected = $maintabs.data('selected.tabs');
                $.cookie('maintab2', selected);
            });
        });
       function AddSelected() {
            tb_remove();
            $('#<%= RefreshGrids.ClientID %>').click();
        }
        function pageLoad(sender, args) {
            if (args.get_isPartialLoad()) {
                if (<%=EditUpdateButton1.Editing?"true":"false"%>)
                    $('a.thickbox2').unbind("click")
                else
                    tb_init('a.thickbox2');
            }
        }
    </script>

    <asp:Button ID="RefreshGrids" runat="server" Style="display: none" OnClick="RefreshGrids_Click" />
    <table class="PersonHead" border="0">
        <tr>
            <td>
                <cc1:DisplayLabel ID="FamilyName" runat="server" BindingSource="family" />
            </td>
            <td align="right">
                <b>Members:</b>
                <cc1:DisplayLabel ID="MemberCount" runat="server" BindingSource="family" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td valign="top">
                <div>
                    <asp:LinkButton ID="TagUnTag" runat="server"></asp:LinkButton></div>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <cc1:DisplayHyperlink ID="AddressLineOne" runat="server" BindingSource="family" BindingUrlFormat="http://www.google.com/maps?q={0}"
                                BindingUrlMember="AddrCityStateZip" Target="_blank" />
                            &nbsp;
                        </td>
                        <td>
                            <asp:LinkButton ToolTip="copy name and address to clipboard" ID="CopyToClipboard"
                                runat="server">clipboard</asp:LinkButton>
                        </td>
                    </tr>
                    <tr runat="server" id="trAddressLineTwo">
                        <td>
                            <cc1:DisplayHyperlink ID="AddressLineTwo" runat="server" BindingSource="family" BindingUrlFormat="http://www.google.com/maps?q={0}"
                                BindingUrlMember="Addr2CityStateZip" Target="_blank">[Address]</cc1:DisplayHyperlink>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <cc1:DisplayLabel ID="CityStateZip" BindingSource="family" runat="server"></cc1:DisplayLabel>
                        </td>
                        <td>
                            <a href='<%="http://www.google.com/maps?f=d&saddr=2000+Appling+Rd,+Cordova,+Tennessee+38016&pw=2&daddr=" + family.AddrCityStateZip %>'
                                target="_blank">driving directions</a>
                        </td>
                    </tr>
                    <%--                        <tr>
                            <td>
                                <cc1:DisplayHyperlink ID="DisplayHyperlink1" runat="server" BindingMember="EmailAddress"
                                    BindingUrlFormat="mailto:{0}" BindingUrlMember="EmailAddress" BindingSource="person">[EmailAddress]</cc1:DisplayHyperlink>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
--%>
                    <tr>
                        <td>
                            <cc1:DisplayOrEditMaskedText ID="HomePhone0" runat="server" BindingSource="Family"
                                BindingMember="HomePhone" BindingMode="OneWay" Height="18px" Width="135px" MaskType="Phone"></cc1:DisplayOrEditMaskedText>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" OnClick="EditUpdateButton1_Click"
                                Changes="0" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
            <td valign="top">
            </td>
        </tr>
    </table>
    <div id="main-tab" class='ui-tabs'>
        <ul class="ui-tabs-nav">
            <li><a href="#Members-tab"><span>Members</span></a></li>
            <li><a href="#Address-tab"><span>Addresses</span></a></li>
            <li><a href="#Relations-tab"><span>Family Relations</span></a></li>
        </ul>
        <div id="Members-tab" class="ui-tabs-panel ui-tabs-hide">
            <table>
                <tr>
                    <td>
                        <asp:GridView ID="FamilyGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            EmptyDataText="No Family Members" OnRowDataBound="GridView1_RowDataBound"
                            SkinID="GridViewSkin" DataSourceID="FamilyMembersODS" 
                            DataKeyNames="PeopleId">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="namelink" runat="server" NavigateUrl='<%# Eval("PeopleId", "~/Person.aspx?Id={0}") %>'
                                            Text='<%# Eval("Name") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Position In Family">
                                    <ItemTemplate>
                                        <asp:Label ID="PositionInFamily" runat="server" Text='<%# Bind("PositionInFamily") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="editPositionInFamilyId" runat="server" DataSourceID="FamilyPositionCodes"
                                            DataTextField="Value" DataValueField="Id" SelectedValue='<%#Bind("PositionInFamilyId")%>'>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Age" HeaderText="Age" SortExpression="Age" ReadOnly="True">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Member Status" SortExpression="MemberStatus">
                                    <ItemTemplate>
                                        <asp:Label ID="MemberStatus" runat="server" Text='<%# Eval("MemberStatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:CheckBoxField DataField="Deceased" HeaderText="Deceased" ReadOnly="True" SortExpression="Deceased">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:CheckBoxField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Edit" runat="server" CausesValidation="False" 
                                            CommandName="Edit" Text="Edit"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="Update" runat="server" CausesValidation="True" 
                                            CommandName="Update" Text="Update"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="Cancel" runat="server" CausesValidation="False" 
                                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ControlStyle ForeColor="#336699" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <cc1:LinkButtonConfirm ID="btnSplit" Text="Split" Confirm="Are you sure you want to remove member and create a new Family? Clicking Ok will take you to the new Family."
                                            runat="server" OnClick="btnSplit_Click"></cc1:LinkButtonConfirm>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <uc1:GridPager ID="GridPager1" runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; border-top-style: solid">
                        <asp:HyperLink ID="AddMembers2" CssClass="thickbox2" runat="server">Add Member(s)</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Address-tab" class="ui-tabs-panel ui-tabs-hide">
            <ul>
                <li><a href="#Family1-tab"><span>Family</span></a></li>
                <li><a href="#Family2-tab"><span>Family Alternate</span></a></li>
            </ul>
            <div id="Family1-tab" class="ui-tabs-hide">
                <uc1:address ID="FamilyPrimaryAddr" runat="server" AddressType="Family" />
            </div>
            <div id="Family2-tab" class="ui-tabs-hide">
                <uc1:address ID="FamilyAltAddr" runat="server" AddressType="FamilyAlternate" />
            </div>
        </div>
        <div id="Relations-tab" class="ui-tabs-panel ui-tabs-hide">
            <table>
                <tr>
                    <td>
                        <asp:GridView ID="RelatedFamilyGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                            CellPadding="3" EmptyDataText="No Related Families Found" 
                            OnRowDataBound="RelatedFamilyGrid_RowDataBound" SkinID="GridViewSkin" 
                            DataKeyNames="FamilyId,RelatedFamilyId" DataSourceID="RelatedFamiliesODS">
                            <Columns>
                                <asp:BoundField DataField="FamilyId" HeaderText="Id" SortExpression="Id" Visible="False">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="familylink" runat="server" NavigateUrl='<%# Eval("RelatedFamilyId", "~/Family.aspx?Id={0}") %>'
                                            Text='<%# Eval("RelatedFamilyId") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="FamilyRelationshipDescLbl" runat="server" Text='<%# Bind("FamilyRelationshipDesc") %>'></asp:Label>
                                        &nbsp;&nbsp;&nbsp;
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="FamilyRelationshipDescTxt" runat="server" Width="300" Text='<%# Bind("FamilyRelationshipDesc") %>'></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;
                                    </EditItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Edit" runat="server" CausesValidation="False" 
                                            CommandName="Edit" Text="Edit"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="Update" runat="server" CausesValidation="True" 
                                            CommandName="Update" Text="Update"></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="Cancel" runat="server" CausesValidation="False" 
                                            CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ControlStyle ForeColor="#336699" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        &nbsp;&nbsp;&nbsp;
                                        <cc1:LinkButtonConfirm ID="btnRemoveRelation" runat="server" Confirm="Are you sure you want to remove Related Family?"
                                            OnClick="btnRemoveRelation_Click" Text="Remove"></cc1:LinkButtonConfirm>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <uc1:GridPager ID="RelatedFamilyGridPager" runat="server" />
                            </PagerTemplate>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; border-top-style: solid">
                        <asp:HyperLink ID="AddRelatedFamily2" CssClass="thickbox2" runat="server">Add Related Family</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:ObjectDataSource ID="FamilyMembersODS" runat="server" SelectMethod="GetFamilyMembers"
        TypeName="CMSPresenter.FamilyController" UpdateMethod="UpdateFamilyMember">
        <UpdateParameters>
            <asp:Parameter Name="PeopleId" Type="Int32" />
            <asp:Parameter Name="PositionInFamilyId" Type="Int32" />
        </UpdateParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="id" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="RelatedFamiliesODS" runat="server" SelectMethod="GetRelatedFamilies"
        TypeName="CMSPresenter.FamilyController" UpdateMethod="UpdateFamilyRelation">
        <UpdateParameters>
            <asp:Parameter Name="FamilyId" Type="Int32" />
            <asp:Parameter Name="RelatedFamilyId" Type="Int32" />
            <asp:Parameter Name="FamilyRelationshipDesc" Type="String" />
        </UpdateParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="id" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="FamilyPositionCodes" runat="server" SelectMethod="FamilyPositionCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:LinkButton ID="HiddenDoNothingButton" runat="server" Style="display: none"></asp:LinkButton>
</asp:Content>
