<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="ForumEntryDisplay" Codebehind="ForumEntryDisplay.ascx.cs" %>
<div class="ForumPostContentArea" id="PostDisplayContainer">
    <div class="ForumThreadPostBody">
        <div class="ForumThreadPostHeader" id="PostDisplayHeader">
            <div class="ForumPostTitle">
                <asp:Label ID="PostTitle" runat="server" EnableViewState="False"></asp:Label>
            </div>
            <div class="ForumPostPubDate">
                <asp:Label ID="PubDate" runat="server"></asp:Label>
            </div>
            <div class="ForumPostUserName">
                <asp:Label ID="UserName" runat="server"></asp:Label>
            </div>
        </div>
        <div id="PostDisplayBody1">
            <asp:Literal ID="PostBody" runat="server" EnableViewState="false"></asp:Literal>
        </div>
    </div>
</div>
