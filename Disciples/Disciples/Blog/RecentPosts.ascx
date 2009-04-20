<%@ Control Language="C#" AutoEventWireup="True" Inherits="RecentPosts" CodeBehind="RecentPosts.ascx.cs" %>
<%@ Register Src="../SideBarArea.ascx" TagName="SideBarArea" TagPrefix="uc2" %>
<%@ Register Src="Item.ascx" TagName="Item" TagPrefix="uc1" %>
<div class="CommonSidebar">
    <div class="CommonSidebarArea">
        <div class="CommonSidebarRoundTop">
            <div class="r1">
            </div>
            <div class="r2">
            </div>
            <div class="r3">
            </div>
            <div class="r4">
            </div>
        </div>
        <div class="CommonSidebarInnerArea">
            <h4 class="CommonSidebarHeader">
                Posts on this page
            </h4>
            <div class="CommonSidebarContent">
                <asp:DataList ID="DataList1" runat="server" DataSourceID="ObjectDataSource2">
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# Eval("Id", "~/Blog/{0}.aspx") %>'
                            Text='<%# Eval("Title") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
        <div class="CommonSidebarRoundBottom">
            <div class="r1">
            </div>
            <div class="r2">
            </div>
            <div class="r3">
            </div>
            <div class="r4">
            </div>
        </div>
    </div>
</div>
<asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="FetchTopNTitles"
    TypeName="DiscData.BlogPostController">
    <SelectParameters>
        <asp:Parameter Name="BlogId" Type="Int32" />
        <asp:Parameter Name="Page" Type="Int32" />
        <asp:Parameter Name="Count" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
