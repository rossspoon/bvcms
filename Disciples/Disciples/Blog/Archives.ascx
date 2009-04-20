<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Archives.ascx.cs" Inherits="BellevueTeachers.Blog.Archives" %>
<div class="panel">
    <h4>
        Archives
    </h4>
    <asp:ListView ID="ArchiveList" runat="server">
        <LayoutTemplate>
            <ul>
                <li id="itemPlaceHolder" runat="server"></li>
            </ul>
        </LayoutTemplate>
        <ItemTemplate>
            <li>
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl='<%# "~/Blog/" + blog.Name + ".aspx" + Eval("Month", "?mon={0:M-1-yy}") %>'
                    Text='<%# Eval("Month", "{0:MMMM yyyy}") + Eval("Count", " ({0})") %>'></asp:HyperLink></li>
        </ItemTemplate>
    </asp:ListView>
</div>
