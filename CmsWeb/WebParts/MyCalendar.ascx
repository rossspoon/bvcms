<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyCalendar.ascx.cs" Inherits="CMSWeb.WebParts.MyCalendar" %>
<h5>Current Date and Time</h5>
<div style="font:verdana">
<span style="width: 140px">Date: </span>
<%= Util.Now.ToShortDateString() %>
</div>
<div style="font:verdana">
<span style="width: 140px">Time: </span>
<%= Util.Now.ToShortTimeString() %>
</div>
<h4 style="font:verdana; color:#2b637d">
    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Task/List/" runat="server">Tasks</asp:HyperLink> 
</h4>
<div style="overflow:auto;height:522px;">
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    ShowHeader="False" SkinID="GridViewSkin" 
    DataSourceID="ObjectDataSource1" >
    <Columns>
        <asp:HyperLinkField DataNavigateUrlFields="Id" 
            DataNavigateUrlFormatString="~/Task/List/{0}" DataTextField="Description" 
            HeaderText="Task" />
        <asp:BoundField DataField="Who" HeaderText="Who" />
    </Columns>
</asp:GridView>
</div>


<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    DeleteMethod="DeleteTask" OldValuesParameterFormatString="original_{0}" 
    SelectMethod="FetchContactTasks" TypeName="CMSWeb.Models.TaskModel" 
    UpdateMethod="UpdateTask">
    <DeleteParameters>
        <asp:Parameter Name="TaskId" Type="Int32" />
        <asp:Parameter Name="notify" Type="Object" />
    </DeleteParameters>
    <UpdateParameters>
        <asp:Parameter Name="Description" Type="String" />
        <asp:Parameter Name="Due" Type="String" />
        <asp:Parameter Name="Location" Type="String" />
        <asp:Parameter Name="Notes" Type="String" />
        <asp:Parameter Name="Priority" Type="Int32" />
        <asp:Parameter Name="Project" Type="String" />
        <asp:Parameter Name="StatusId" Type="Int32" />
        <asp:Parameter Name="Id" Type="Int32" />
        <asp:Parameter Name="notify" Type="Object" />
    </UpdateParameters>
</asp:ObjectDataSource>



