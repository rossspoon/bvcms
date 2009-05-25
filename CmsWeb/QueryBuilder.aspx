<%@ Page Language="C#" StylesheetTheme="Standard" MasterPageFile="~/Site.Master" AutoEventWireup="True" CodeBehind="QueryBuilder.aspx.cs"
    Inherits="CMSWeb.QueryBuilder" Title="Query Builder" ViewStateEncryptionMode="Never"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="UserControls/PersonGrid.ascx" TagName="PersonGrid" TagPrefix="uc2" %>
<%@ Register Src="UserControls/ExportToolBar.ascx" TagName="ExportToolBar" TagPrefix="uc1" %>
<%@ Register Src="UserControls/QueryConditions.ascx" TagName="QueryConditions" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function OpenQuery() { $find('<%=OpenQueryPopup.ClientID%>').show(); }
        function SaveQuery() { $find('<%=SaveQueryPopup.ClientID%>').show(); }
        function SelectCondition() {
            jQuery("#tabber").tabs();
            $find('<%=ConditionSelectPopupBehavior.ClientID%>').show();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="up_container" style="clear: both; margin-top: 10px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Label ID="SavedQueryName" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
                <table class="modalPopup">
                    <tr class="QBConditionTitle">
                        <td colspan="3">
                            Configure the query condition
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table>
                                <tr>
                                    <th colspan="2">
                                        Left Side
                                    </th>
                                </tr>
                                <tr id="CategoryDiv2" runat="server" visible="true">
                                    <td>
                                      <input type="button" value="Select Condition" onclick="SelectCondition()" />
                                    </td>
                                    <td>
                                        <asp:Label ID="CurrentCondition" runat="server" Style="font-size: large" Text="Group"></asp:Label>
                                        <asp:HiddenField ID="ConditionName" runat="server" Value="Group" />
                                    </td>
                                </tr>
                                <tr id="DaysDiv" runat="server" visible="false">
                                    <td>
                                        Days:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Days" runat="server"></asp:TextBox>
                                        <asp:CustomValidator ID="ValidateDays" runat="server" ErrorMessage="# Days required"
                                            OnServerValidate="ValidateDays_ServerValidate" ControlToValidate="Days" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="WeekDiv" runat="server" visible="false">
                                    <td>
                                        Week:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Week" runat="server"></asp:TextBox>
                                        <asp:CustomValidator ID="ValidateWeek" runat="server" ErrorMessage="Week # required"
                                            OnServerValidate="ValidateWeek_ServerValidate" ControlToValidate="Week" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="QuartersDiv" runat="server" visible="false">
                                    <td>
                                        Quarters:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Quarters" runat="server"></asp:TextBox><asp:CustomValidator ID="ValidateQuarters"
                                            runat="server" ErrorMessage="Quarters required: 1,2,3,4" OnServerValidate="ValidateQuarters_ServerValidate"
                                            ControlToValidate="Quarters" ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="TagDiv" runat="server" visible="false">
                                    <td>
                                        Tags:
                                    </td>
                                    <td>
                                        <cc1:DropCheck ID="Tags" runat="server" DataTextField="Value" DataValueField="Code"
                                            Width="225px">
                                        </cc1:DropCheck>
                                        <asp:CustomValidator ID="ValidateTags" runat="server" ErrorMessage="Must Choose Tags"
                                            OnServerValidate="ValidateTags_ServerValidate"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="SavedQueryDiv" runat="server" visible="false">
                                    <td>
                                        Saved Query:
                                    </td>
                                    <td>
                                        <cc1:DropDownCC ID="SavedQueryDesc" runat="server" DataTextField="Value" DataValueField="IdValue">
                                        </cc1:DropDownCC>
                                        <asp:CustomValidator ID="ValidateSavedQuery" runat="server" ErrorMessage="Must Choose a Query"
                                            OnServerValidate="ValidateSavedQuery_ServerValidate"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="StartDiv" runat="server" visible="false">
                                    <td>
                                        Start Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="StartDate" runat="server"></asp:TextBox>
                                        <cc2:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="StartDate">
                                        </cc2:CalendarExtender>
                                        <asp:CustomValidator ID="ValidateStartDate" runat="server" ErrorMessage="Date not valid"
                                            ControlToValidate="StartDate" EnableViewState="False" OnServerValidate="ValidateStartDate_ServerValidate"
                                            ValidateEmptyText="True"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="EndDiv" runat="server" visible="false">
                                    <td>
                                        End Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="EndDate" runat="server"></asp:TextBox><asp:CustomValidator ID="ValidateEndDate"
                                            runat="server" ErrorMessage="Date not valid" OnServerValidate="ValidateEndDate_ServerValidate"
                                            ControlToValidate="EndDate"></asp:CustomValidator><cc2:CalendarExtender ID="CalendarExtender2"
                                                runat="server" TargetControlID="EndDate">
                                            </cc2:CalendarExtender>
                                    </td>
                                </tr>
                                <tr id="ProgDiv" runat="server" visible="false">
                                    <td>
                                        Program:
                                    </td>
                                    <td>
                                        <cc1:DropDownCC ID="DivOrg" runat="server" AutoPostBack="True" DataTextField="Value"
                                            DataValueField="Id" OnSelectedIndexChanged="DivOrg_SelectedIndexChanged" AppendDataBoundItems="True">
                                        </cc1:DropDownCC>
                                    </td>
                                </tr>
                                <tr id="DivDiv" runat="server" visible="false">
                                    <td>
                                        Division:
                                    </td>
                                    <td>
                                        <cc1:DropDownCC ID="SubDivOrg" runat="server" AutoPostBack="True" DataTextField="Value"
                                            DataValueField="Id" OnSelectedIndexChanged="SubDivOrg_SelectedIndexChanged">
                                        </cc1:DropDownCC>
                                    </td>
                                </tr>
                                <tr id="OrgDiv" runat="server" visible="false">
                                    <td>
                                        Organization:
                                    </td>
                                    <td>
                                        <cc1:DropDownCC ID="Organization" runat="server" DataTextField="Value" DataValueField="Id">
                                        </cc1:DropDownCC>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <th>
                                        Comparison
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <cc1:DropDownCC ID="Comparison" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Comparison_SelectedIndexChanged">
                                        </cc1:DropDownCC>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td id="RightPanel" runat="server" valign="top">
                            <table>
                                <tr>
                                    <th>
                                        Right Side
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <cc1:DropDownCC ID="ValueCode" runat="server" Visible="false">
                                        </cc1:DropDownCC>
                                        <cc1:DropCheck ID="ValueCheckCodes" runat="server" Visible="False" Width="225px">
                                        </cc1:DropCheck>
                                        <asp:TextBox ID="ValueDate" runat="server"></asp:TextBox>
                                        <cc2:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="ValueDate">
                                        </cc2:CalendarExtender>
                                        <asp:TextBox ID="ValueText" runat="server" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CustomValidator ID="ValidateRightSide" runat="server" OnServerValidate="ValidateRightSide_ServerValidate"></asp:CustomValidator>
                                        <asp:HiddenField ID="HiddenTextType" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                            <asp:Button ID="Update" runat="server" OnClick="Update_Click" Text="Update" Width="96px"
                                Enabled="false" />
                            <asp:Button ID="Add" runat="server" OnClick="Add_Click" Text="Add" Width="96px" Enabled="false" />
                            <asp:Button ID="AddToGroup" runat="server" OnClick="AddToGroup_Click" Text="Add To Group"
                                Width="96px" />
                            <asp:Button ID="Delete" runat="server" OnClick="Remove_Click" Text="Remove" Width="96px"
                                Enabled="false" />
                        </td>
                    </tr>
                </table>
                <table class="modalPopup" style="margin-top: 6px">
                    <tr>
                        <td colspan="2" style="background-color: #a9cfe2; border: solid 1px Gray; color: Black;
                            font-size: medium">
                            Conditions
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="ConditionGrid" runat="server" AutoGenerateColumns="False" BorderColor="#3e8cb5"
                                BorderStyle="Solid" BorderWidth="3px" DataKeyNames="Id" DataSourceID="QueryData"
                                Font-Size="Large" OnRowCommand="QueryGridCommand" ShowHeader="False">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Image ID="TabLevel" runat="server" Height="1px" ImageUrl="~/images/1pix.gif"
                                                Width='<%#Eval("Level")%>' />
                                            <asp:Image ID="DownRight" runat="server" ImageUrl="~/images/DownRight.gif" Height="12px"
                                                Width="19px" />
                                            <asp:Panel ID="PopupMenu" runat="server" CssClass="popupMenu">
                                                <div>
                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                        CommandName="insgroup" Text="Insert Group Above" />
                                                </div>
                                                <div>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%#Eval("Id")%>'
                                                        CommandName="copyasnew" Text="Copy As New" />
                                                </div>
                                            </asp:Panel>
                                            <asp:LinkButton ID="EditLink" runat="server" CommandArgument='<%#Eval("Id")%>' CommandName="edit"
                                                CssClass='<%# (int)(Eval("Id")) == SelectedId? "SelectedRow" : "" %>' Text='<%# Eval("ClauseHtml") %>'
                                                ToolTip="Click to Edit/Select" />
                                            <cc2:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopDelay="25" PopupControlID="PopupMenu"
                                                PopupPosition="Left" TargetControlID="DownRight">
                                            </cc2:HoverMenuExtender>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td align="center" valign="bottom">
                            <div>
                                <asp:ImageButton ID="RunQuery" runat="server" AlternateText="Run Query" EnableViewState="False"
                                    ImageUrl="~/images/Run.png" OnClick="RunQuery_Click" OnClientClick="$.blockUI()" /></div>
                            <div>
                                Run</div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:HiddenField ID="SavedSelectedId" runat="server" />
                        </td>
                    </tr>
                </table>
               <asp:LinkButton ID="LinkButton2" Style="display: none" runat="server">LinkButton</asp:LinkButton>
             <asp:Panel ID="ConditionSelectPanel1" runat="server" CssClass="modalPopup" Style="margin-top: 6px;
                    width: 700px; height: 500px; display: none">
                    <span style="float: left; font-size: medium">Select a Condition</span> <span style="float: right">
                        <asp:LinkButton ID="ConditionCancel" runat="server">Cancel</asp:LinkButton></span>
                    <div style="clear: both">
                    </div>
                    <uc3:QueryConditions ID="ConditionSelection" runat="server" />
                </asp:Panel>
                <cc2:ModalPopupExtender ID="ConditionSelectPopupBehavior" runat="server"
                    TargetControlID="LinkButton2" PopupControlID="ConditionSelectPanel1" CancelControlID="ConditionCancel"
                    DropShadow="true" BackgroundCssClass="modalBackground">
                </cc2:ModalPopupExtender>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DivOrg" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="SubDivOrg" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="Update" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="Add" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="AddToGroup" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="Delete" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="ConditionGrid" EventName="RowCommand" />
                <asp:PostBackTrigger ControlID="RunQuery" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <cc2:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" runat="server"
        TargetControlID="UpdatePanel1">
        <Animations>
            <OnUpdating>
                    <FadeOut AnimationTarget="up_container" Duration="0" minimumOpacity=".2" />
            </OnUpdating>
            <OnUpdated>
                    <FadeIn AnimationTarget="up_container" Duration="0" minimumOpacity=".2" />
            </OnUpdated>
        </Animations>
    </cc2:UpdatePanelAnimationExtender>
    <div id="Results" runat="server" visible="false" style="margin-top: 6px">
        <uc1:ExportToolBar ID="ExportToolBar1" runat="server" />
        <div style="clear: both">
            <uc2:PersonGrid ID="PersonGrid1" runat="server" DataSourceID="PersonData" />
        </div>
    </div>
    <asp:Button ID="TriggerOpenQueryPopup" Style="display: none" runat="server" />
    <asp:Panel ID="OpenQueryInputPanel" runat="server" CssClass="modalDiv" Style="display: none2">
        <div style="text-align: left">
            <asp:DropDownList ID="ExistingQueries" runat="server" DataTextField="Value" 
                DataValueField="Id" DataSourceID="dsExistingQueries" />
            <span class="footer">
                <asp:LinkButton ID="OpenQueryLoad" runat="server" CausesValidation="false" Text="Open"
                    OnClick="OpenQuery" />
                <asp:LinkButton ID="OpenQueryCancel" runat="server" CausesValidation="false" Text="Cancel" />
            </span>
            <br />
        </div>
    </asp:Panel>
    <cc2:ModalPopupExtender ID="OpenQueryPopup" runat="server"
        TargetControlID="TriggerOpenQueryPopup" PopupControlID="OpenQueryInputPanel"
        CancelControlID="OpenQueryCancel" DropShadow="true" BackgroundCssClass="modalBackground">
    </cc2:ModalPopupExtender>
    <asp:UpdatePanel ID="SaveQueryPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:LinkButton ID="TriggerSaveQueryPopup" Style="display: none" runat="server">LinkButton</asp:LinkButton>
            <asp:Panel ID="SaveQueryInputPanel" runat="server" CssClass="modalDiv" Style="display: none">
                <div style="text-align: left">
                    Query Description:
                    <asp:TextBox ID="QueryDescription" runat="server" ValidationGroup="SaveQueryGroup"
                        Width="274px"></asp:TextBox>
                    &nbsp;
                    <asp:CheckBox ID="PublicCheck" runat="server" Text="Is Public" />
                    <span class="footer">
                        <asp:LinkButton ID="SaveQuerySave" runat="server" CausesValidation="false" Text="Save"
                            OnClick="SaveQuery" />
                        <asp:LinkButton ID="SaveQueryCancel" runat="server" CausesValidation="false" Text="Cancel" />
                    </span>
                    <br />
                </div>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="SaveQueryPopup" runat="server"
                TargetControlID="TriggerSaveQueryPopup" PopupControlID="SaveQueryInputPanel"
                CancelControlID="SaveQueryCancel" DropShadow="true" BackgroundCssClass="modalBackground">
            </cc2:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ObjectDataSource ID="QueryData" runat="server" SelectMethod="ConditionList"
        TypeName="CMSPresenter.QueryBuilderController" OnObjectCreating="ObjectCreating">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PersonData" runat="server" SelectMethod="FetchPeopleList"
        TypeName="CMSPresenter.QueryBuilderController" EnablePaging="True" SelectCountMethod="FetchPeopleListCount"
        SortParameterName="sortExpression" OnObjectCreating="ObjectCreating">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" />
            <asp:SessionParameter Name="QueryId" SessionField="QueryId" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="dsExistingQueries" runat="server" 
                SelectMethod="UserQueries" 
                TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
</asp:Content>
