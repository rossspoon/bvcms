<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SideBarArea.ascx.cs" Inherits="BellevueTeachers.SideBarArea" %>
<div class="CommonSidebarArea">
    <div class="CommonSidebarInnerArea">
        <h4 class="CommonSidebarHeader">
            <asp:Literal EnableViewState="false" ID="header" runat="server"></asp:Literal>
        </h4>
        <div class="CommonSidebarContent">
        <cms:Paragraph ID="Paragraph1" runat="server" />
        </div>
    </div>
</div>
