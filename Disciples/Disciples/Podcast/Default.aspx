<%@ Page Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True"
    Inherits="Podcast_Default" Title="Bellevue Teacher Podcasts" Codebehind="Default.aspx.cs" %>
<asp:Content ID="Content3" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main"><asp:GridView ID="GridView1" runat="server" EnableViewState="false" AutoGenerateColumns="False"
        DataSourceID="ObjectDataSource1" SkinID="subsonicSkin">
        <Columns>
            <asp:TemplateField HeaderText="PodcastIt" ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# ITunesLink(Eval("Username")) %>'>
                        <asp:Image ID="Image1" ImageUrl="~/images/itunes.gif" runat="server" /></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Rss Feed" ShowHeader="False">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# RssLink(Eval("Username")) %>'>
                        <asp:Image ID="Image1" ImageUrl="~/images/rssxml.gif" runat="server" /></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Username" HeaderText="Podcaster" SortExpression="Username" />
            <asp:BoundField DataField="LastPosted" HeaderText="LastPosted" SortExpression="LastPosted" />
            <asp:BoundField DataField="Count" HeaderText="Count" SortExpression="Count" />
        </Columns>
    </asp:GridView></div>
    <asp:ObjectDataSource ID="ObjectDataSource1" EnableViewState="false" runat="server" OldValuesParameterFormatString="original_{0}"
        SelectMethod="FetchSummary" TypeName="DiscData.PodCastController"></asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphSideBar" runat="server">
    <div id="extra" class="front">
        <div class="panel">
            <h4>
                Featured Items
            </h4>
            <cms:Paragraph ID="postit" ContentName="PostPodcast" runat="server" />
        </div>
    </div>
</asp:Content>
