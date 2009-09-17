<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs"
    Inherits="CMSWeb.Contact" Title="Person" EnableEventValidation="false" %>

<%@ Register Src="UserControls/GridPager.ascx" TagName="GridPager" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
         $(function() {
            if ('<%=EditUpdateButton1.Editing?"true":"false"%>' == 'true')
                $('a.thickbox2').unbind("click")
            else
                tb_init('a.thickbox2');
            imgLoader = new Image();
            imgLoader.src = tb_pathToImage;
        });
       function AddSelected() {
            tb_remove();
            $('#<%= RefreshGrids.ClientID %>').click();
        }
    </script>

    <asp:Button ID="RefreshGrids" runat="server" style="display:none" OnClick="RefreshGrids_Click" />      
    <h1>
        Contacts</h1>
    <table>
        <tr>
            <th align="right">
                Contact Date:
            </th>
            <td>
                <cc1:DisplayOrEditDate ID="ContactDate" runat="server" BindingMember="ContactDate"
                    BindingMode="TwoWay" BindingSource="contact">
                </cc1:DisplayOrEditDate>
                <cc2:CalendarExtender ID="TextBox2_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="ContactDate">
                </cc2:CalendarExtender>
            </td>
        </tr>
        <tr runat="server" id="TaskRow">
            <th align="right">
                Completed Task:
            </th>
            <td>
                <asp:HyperLink ID="TaskLink" runat="server"></asp:HyperLink>
            </td>
        </tr>
        <tr>
            <th align="right">
                Ministry:
            </th>
            <td>
                <cc1:DisplayOrEditDropDown ID="MinistryId" runat="server" BindingMember="MinistryId"
                    BindingMode="TwoWay" BindingSource="contact" DataTextField="Value" DataValueField="Id"
                    MakeDefault0="True" Width="250px" DataSourceID="ODSMinistryId" AppendDataBoundItems="false"
                    ChangedStatus="False">
                </cc1:DisplayOrEditDropDown>
            </td>
        </tr>
        <tr>
            <th align="right">
                Type:
            </th>
            <td>
                <cc1:DisplayOrEditDropDown ID="ContactTypeId" runat="server" BindingMember="ContactTypeId"
                    BindingMode="TwoWay" BindingSource="contact" DataTextField="Value" DataValueField="Id"
                    MakeDefault0="False" Width="250px" DataSourceID="ODSContactTypeId">
                </cc1:DisplayOrEditDropDown>
            </td>
        </tr>
        <tr>
            <th align="right">
                Reason:
            </th>
            <td>
                <cc1:DisplayOrEditDropDown ID="ContactReasonId" runat="server" BindingMember="ContactReasonId"
                    BindingMode="TwoWay" BindingSource="contact" DataTextField="Value" DataValueField="Id"
                    MakeDefault0="False" Width="250px" DataSourceID="ODSContactReasonId">
                </cc1:DisplayOrEditDropDown>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <cc1:DisplayOrEditCheckbox ID="NotAtHome" runat="server" BindingSource="contact"
                                Text="Not At Home" BindingMode="TwoWay" /><br />
                            <cc1:DisplayOrEditCheckbox ID="LeftDoorHanger" runat="server" BindingSource="contact"
                                Text="Left Door Hanger" BindingMode="TwoWay" /><br />
                            <cc1:DisplayOrEditCheckbox ID="LeftMessage" runat="server" BindingSource="contact"
                                Text="Left Message" BindingMode="TwoWay" /><br />
                        </td>
                        <td>
                            <cc1:DisplayOrEditCheckbox ID="ContactMade" runat="server" BindingSource="contact"
                                Text="Contact Made" BindingMode="TwoWay" /><br />
                            <cc1:DisplayOrEditCheckbox ID="GospelShared" runat="server" BindingSource="contact"
                                Text="Gospel Shared" BindingMode="TwoWay" /><br />
                            <br />
                        </td>
                        <td>
                            <cc1:DisplayOrEditCheckbox ID="PrayerRequest" runat="server" BindingSource="contact"
                                Text="Prayer Request Received" BindingMode="TwoWay" /><br />
                            <cc1:DisplayOrEditCheckbox ID="GiftBagGiven" runat="server" BindingSource="contact"
                                Text="Gift Bag Given" BindingMode="TwoWay" /><br />
                            <br />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table class="Design2">
                    <tr>
                        <td height="100%" width="100%">
                            <span id="CommentsSection" runat="server"><strong>Comments:</strong>
                                <br />
                                <cc1:DisplayOrEditText ID="Comments" runat="server" BindingSource="contact" BindingMember="Comments"
                                    BindingMode="TwoWay" TextMode="MultiLine" Height="150" Width="500"></cc1:DisplayOrEditText>
                            </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" CheckRole="false" OnClick="EditUpdateButton1_Click" />
            </td>
            <td>
                <cc1:LinkButtonConfirm ID="DeleteButton" Text="Delete" Confirm="Are you sure you want to delete this contact?"
                    runat="server" CommandName="Delete" OnClick="DeleteButton_Click">
                </cc1:LinkButtonConfirm>
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td style="vertical-align: top">
                <strong>Ministry Team:</strong>
                <table style="vertical-align: top">
                    <tr style="vertical-align: top">
                        <td style="vertical-align: top">
                            <asp:GridView ID="ContactorGrid" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CellPadding="3" EmptyDataText="No Contactors Found"
                                PageSize="10" SkinID="GridViewSkin" DataSourceID="ContactorList" DataKeyNames="ContactId, PeopleId">
                                <Columns>
                                    <asp:BoundField DataField="PeopleId" HeaderText="PeopleId" SortExpression="Id" Visible="False">
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
                                    <asp:CommandField CausesValidation="False" ShowEditButton="True" Visible="False" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <cc1:LinkButtonConfirm ID="btnRemoveContactor" Text="Remove" Confirm="Are you sure you want to remove contactor from the list?"
                                                runat="server" CommandName="Delete">
                                            </cc1:LinkButtonConfirm>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; border-top-style: solid">
                            <asp:HyperLink runat="server" ID="AddContactorLink" CssClass="thickbox2" ToolTip="Person doing ministry">Add Contactor</asp:HyperLink>
                            <p>
                                <asp:LinkButton ID="AddTeamContact" runat="server" OnClick="AddTeamContact_Click">Add another Contact for this team</asp:LinkButton>
                            </p>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
            <td style="vertical-align: top">
                <strong>Persons ministered to:</strong>
                <table style="vertical-align: top">
                    <tr style="vertical-align: top">
                        <td style="vertical-align: top">
                            <asp:GridView ID="ContacteeGrid" runat="server" AllowPaging="True" AllowSorting="True"
                                AutoGenerateColumns="False" CellPadding="3" EmptyDataText="No Conactees Found"
                                PageSize="10" SkinID="GridViewSkin" DataSourceID="ContacteeList" DataKeyNames="ContactId,PeopleId"
                                OnRowCreated="ContacteeGrid_RowCreated">
                                <Columns>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="UpdateLinkButton" runat="server" CausesValidation="True" CommandName="Update"
                                                Text="Update"></asp:LinkButton>
                                            &nbsp;<asp:LinkButton ID="CancelLinkButton" runat="server" CausesValidation="False"
                                                CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="EditLinkButton" runat="server" CausesValidation="False" CommandName="Edit"
                                                Text="Edit" Enabled='<%# CanEdit() %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle ForeColor="Black" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="PeopleId" HeaderText="PeopleId" SortExpression="Id" Visible="False">
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
                                    <asp:CheckBoxField DataField="ProfessionOfFaith" HeaderText="Profession Of Faith"
                                        SortExpression="ProfessionOfFaith">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:CheckBoxField>
                                    <asp:CheckBoxField DataField="PrayedForPerson" HeaderText="Prayed For Person" SortExpression="PrayedForPerson">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:CheckBoxField>
                                    <asp:CommandField CausesValidation="False" ShowEditButton="True" Visible="False" />
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <cc1:LinkButtonConfirm ID="btnRemoveContactee" Text="Remove" Confirm="Are you sure you want to remove contactee from the list?"
                                                runat="server" CommandName="Delete">
                                            </cc1:LinkButtonConfirm>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="AddTask" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="AddTask" runat="server" CommandName="addtask" OnCommand="AddTask_Command"
                                                Visible='<%# Eval("NoTask") %>'>Add task</asp:LinkButton>
                                            <asp:HyperLink ID="GoTask" runat="server" Visible='<%# Eval("HasTask") %>' NavigateUrl='<%# Eval("TaskId", "/Task/List/{0}") %>'>Go task</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; border-top-style: solid">
                          <asp:HyperLink runat="server" ID="AddContacteeLink" CssClass="thickbox2" ToolTip="Person ministered to">Add Contactee</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ContacteeList" runat="server" EnablePaging="True" SelectCountMethod="ContacteeCount"
        SelectMethod="ContacteeList" SortParameterName="sortExpression" TypeName="CMSPresenter.ContactController"
        DeleteMethod="DeleteContactee" UpdateMethod="UpdateContactee">
        <DeleteParameters>
            <asp:QueryStringParameter Name="ContactId" QueryStringField="id" Type="Int32" />
            <asp:Parameter Name="PeopleId" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:QueryStringParameter Name="ContactId" QueryStringField="id" Type="Int32" />
            <asp:Parameter Name="PeopleId" Type="Int32" />
            <asp:Parameter Name="ProfessionOfFaith" Type="Boolean" />
            <asp:Parameter Name="PrayedForPerson" Type="Boolean" />
        </UpdateParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="cid" QueryStringField="id" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Date DESC" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ContactorList" runat="server" EnablePaging="True" SelectCountMethod="ContactorCount"
        SelectMethod="ContactorList" SortParameterName="sortExpression" TypeName="CMSPresenter.ContactController"
        DeleteMethod="DeleteContactor">
        <DeleteParameters>
            <asp:QueryStringParameter Name="ContactId" QueryStringField="id" Type="Int32" />
            <asp:Parameter Name="PeopleId" Type="Int32" />
        </DeleteParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="cid" QueryStringField="id" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Date DESC" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSMinistryId" runat="server" SelectMethod="Ministries0"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSContactTypeId" runat="server" SelectMethod="ContactTypeCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSContactReasonId" runat="server" SelectMethod="ContactReasonCodes"
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
    <asp:LinkButton ID="HiddenDoNothingButton" runat="server" Style="display: none"></asp:LinkButton>
</asp:Content>
