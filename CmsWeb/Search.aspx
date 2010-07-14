<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs"
    Inherits="CmsWeb.Search" Title="Basic Person Search" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="UserControls/PersonGrid.ascx" TagName="PersonGrid" TagPrefix="uc1" %>
<%@ Register Src="UserControls/QuickSearchParameters.ascx" TagName="QuickSearchParameters"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:LinkButton ID="NewSearch" runat="server" OnClick="NewSearch_Click">New Search (clear)</asp:LinkButton>
        <uc2:QuickSearchParameters ID="Parameters" runat="server" />
    </div>
    <uc1:PersonGrid ID="PersonGrid1" runat="server" DataSourceID="PeopleData" Visible="false" />
    <asp:ObjectDataSource ID="PeopleData" runat="server" EnablePaging="True" SelectMethod="FetchPeopleList"
        TypeName="CMSPresenter.PersonSearchController" SelectCountMethod="Count" SortParameterName="sortExpression">
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
            <asp:ControlParameter ControlID="Parameters" Name="orgid" PropertyName="OrgId" Type="Int32" />
            <asp:ControlParameter ControlID="Parameters" Name="campus" PropertyName="Campus" Type="Int32" />
            <asp:Parameter Name="usersonly" Type="Boolean" DefaultValue="false" />
            <asp:ControlParameter ControlID="Parameters" Name="marital" PropertyName="Married"
                    Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
