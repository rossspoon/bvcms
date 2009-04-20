<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyInvolvement.ascx.cs" Inherits="CMSWeb.WebParts.MyInvolvement" %>
<div style="overflow:auto;height:105px;">
<asp:GridView ID="grdMyInvolvement" runat="server" AutoGenerateColumns="False" DataSourceID="dsMyInvolvement"
    SkinID="GridViewSkin" AllowPaging="true" AllowSorting="true" Visible="true"
    EmptyDataText="No Current Enrollments Found." ShowHeader="false" ShowFooter="false">
    <Columns>
        <asp:TemplateField SortExpression="Name">
            <ItemTemplate>
                <asp:HyperLink ID="OrgLink" runat="server" NavigateUrl='<%# Eval("Id", "~/Organization.aspx?id={0}") %>'
                    Text='<%# Eval("Name") %>'></asp:HyperLink></ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="MemberType"/>
    </Columns>
</asp:GridView>
</div>
    <asp:ObjectDataSource ID="dsMyInvolvement" runat="server" SelectMethod="EnrollData" TypeName="CMSPresenter.PersonController"
        EnablePaging="true" SortParameterName="sortExpression" SelectCountMethod="EnrollDataCount">
        <SelectParameters>
            <asp:Parameter Name="pid" Type="Int32" />
            <asp:Parameter Name="sortExpression" Type="String" DefaultValue="Organization" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:Parameter Name="startRowIndex" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
