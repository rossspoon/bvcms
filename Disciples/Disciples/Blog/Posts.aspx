<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Blog_Posts"
    Title="Blog Posts" CodeBehind="Posts.aspx.cs" %>

<%@ Register Src="Item.ascx" TagName="Item" TagPrefix="uc1" %>
<%@ Register Src="Archives.ascx" TagName="Archives" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main"><div id="thoughts">
        <asp:ListView ID="Posts" runat="server" EnableViewState="false">
            <LayoutTemplate>
                <div id="itemPlaceHolder" runat="server" />
            </LayoutTemplate>
            <ItemTemplate>
                <uc1:Item ID="Item1" runat="server" PostId='<%# Eval("Id") %>' />
            </ItemTemplate>
        </asp:ListView>
        <div style="clear: both">
        </div>
    </div></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphSideBar" runat="server">
    <div id="extra">
        <div id="SideStep" class="panel" runat="server">
            <h4>
                Sidestep</h4>
            <ul>
                <li id="CategoryPaging2" runat="server">
              <asp:Label ID="PageOf" runat="server" Text="Page x Of y in the z category"></asp:Label>
                  </li>
                <li id="NextListItem" class="next" runat="server">
                    <asp:HyperLink ID="NewerPosts" runat="server">Newer Posts</asp:HyperLink></li>
                <li id="PrevListItem" class="prev" runat="server">
                    <asp:HyperLink ID="OlderPosts" runat="server">Older Posts</asp:HyperLink></li>
            </ul>
        </div>
        <div class="panel" id="sideblog">
            <h4>
                Posts on this page</h4>
            <ul>
                <asp:ListView ID="PostsOnPage" runat="server">
                    <LayoutTemplate>
                        <li id="itemPlaceHolder" runat="server"></li>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>
                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("Id", "~/Blog/{0}.aspx") %>'
                                Text='<%# Eval("Title") %>'></asp:HyperLink></li>
                    </ItemTemplate>
                </asp:ListView>
            </ul>
        </div>
        <uc3:Archives ID="Archives1" runat="server" />
        <div class="panel">
            <asp:HyperLink ID="AddEntry" runat="server" EnableViewState="false" NavigateUrl="~/Blog/Edit.aspx">Write a new Post</asp:HyperLink></div>
    </div>
</asp:Content>
