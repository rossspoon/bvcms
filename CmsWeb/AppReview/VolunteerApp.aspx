<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VolunteerApp.aspx.cs"
    Inherits="CmsWeb.VolunteerApp" Title="Volunteer Application Review" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:DisplayHyperlink ID="Name" runat="server" BindingMember="Person.Name" BindingMode="OneWay"
        BindingSource="vol" BindingUrlFormat="~/Person/Index/{0}" BindingUrlMember="PeopleId">
        [Name]</cc1:DisplayHyperlink>
    <br />
    <table class="Design2" style="border-style: groove; border-width: thin;">
        <tr>
            <th>
                Approvals:
            </th>
            <td>
                <cc1:DisplayOrEditCheckbox ID="Standard" runat="server" BindingSource="vol" Text="Standard (references only)" /><br />
                <cc1:DisplayOrEditCheckbox ID="Leader" runat="server" BindingSource="vol" Text="Leadership (background check)" />
            </td>
            <th>
                Comments:
            </th>
            <td width="90%">
                <cc1:DisplayOrEditText ID="Comments" runat="server" BindingSource="vol" Rows="10"
                    Width="100%" TextMode="MultiLine"></cc1:DisplayOrEditText>
            </td>
        </tr>
        <tr>
            <th>
                Approval Date:
            </th>
            <td>
                <cc1:DisplayOrEditDate ID="ProcessedDate" runat="server" BindingSource="vol"></cc1:DisplayOrEditDate>
                <cc2:CalendarExtender ID="ProcessedDate_CalendarExtender" runat="server" Enabled="True"
                    TargetControlID="ProcessedDate">
                </cc2:CalendarExtender>
            </td>
            <th>
                Status Code:
            </th>
            <td>
                <cc1:DisplayOrEditDropDown ID="StatusId" runat="server" BindingSource="vol" DataTextField="Value"
                    DataValueField="Id" MakeDefault0="True" BindingMember="StatusId"  
                    BindingMode="TwoWay" DataSourceID="ODSVolunteerStatusApplicationCodes">
                </cc1:DisplayOrEditDropDown>
            </td>
        </tr>
    </table>
    <br />
    <cc1:EditUpdateButton ID="EditUpdateButton1" runat="server" CheckRole="false" OnClick="EditUpdateButton1_Click" />
    <br />
    <br />
    <asp:DataList ID="DataList1" runat="server" DataSourceID="ObjectDataSource1" GridLines="Both"
        RepeatLayout="Flow" RepeatDirection="Horizontal" 
        onitemdatabound="DataList1_ItemDataBound" 
        onitemcommand="DataList1_ItemCommand">
        <ItemTemplate>
            <asp:HyperLink ID="HyperLink1" runat="server" ImageUrl='<%# Eval("ThumbId", "~/Image.aspx?id={0}") %>'
                NavigateUrl='<%# Eval("Id", "~/AppReview/FormImage.aspx?id={0}") %>' ToolTip='<%# Eval("AppDate", "Uploaded: {0}") %>'
                BorderStyle="Solid" BorderWidth="1px"></asp:HyperLink><br />
            <asp:LinkButton ID="delete" CommandName="delete" CommandArgument='<%# Eval("Id") %>' runat="server">delete</asp:LinkButton>
            <cc2:ConfirmButtonExtender ID="delete_ConfirmButtonExtender" runat="server" 
                Enabled="True" ConfirmText="Are you sure you want to delete?" TargetControlID="delete">
            </cc2:ConfirmButtonExtender>
        </ItemTemplate>
    </asp:DataList><br />
    <div id="UploadDiv" runat="server">
        Select Image File To Upload:
        <asp:FileUpload ID="ImageFile" runat="server" />
        &nbsp;&nbsp;
        <asp:Button ID="Upload" runat="server" Text="Submit" OnClick="Upload_Click"></asp:Button>
        <asp:CustomValidator ID="CheckImage" runat="server" ErrorMessage="Invalid Image File"></asp:CustomValidator>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="AppForms"
        TypeName="CMSPresenter.VolunteerAppController">
        <SelectParameters>
            <asp:QueryStringParameter Name="pid" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSVolunteerStatusApplicationCodes" runat="server" 
        SelectMethod="VolApplicationStatusCodes" 
        TypeName="CMSPresenter.CodeValueController"></asp:ObjectDataSource>
</asp:Content>

<asp:Content ID="Contentscr" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>