<%@ Control Language="C#" AutoEventWireup="True" Inherits="Blog_Item" CodeBehind="Item.ascx.cs" %>
<div class="entry">
    <h3>
        <a href='<%= PermaLink %>'><%=BlogPost.Title %></a></h3>
    <ul class="meta">
        <li class="poster">
            <a href='mailto:<%=BlogPost.PosterEmail %>'><%=BlogPost.PosterName %></a></li>
        <li class="date">
            <%=BlogPost.EntryDate.Value.ToString("ddd d MMM yyyy")%></li>
        <li class="time">
            <%=BlogPost.EntryDate.Value.ToString("HHmm") %></li>
        <li class="cat">
            <asp:ListView ID="CategoryList" runat="server">
                <LayoutTemplate>
                    <a id="itemPlaceHolder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>
                    <asp:HyperLink ID="CatLink" runat="server" NavigateUrl='<%# BlogLink0 + "/" + Container.DataItem + ".aspx" %>'
                        Text='<%# Container.DataItem %>'></asp:HyperLink>
                </ItemTemplate>
                <ItemSeparatorTemplate>
                    <br />
                </ItemSeparatorTemplate>
            </asp:ListView>
        </li>
        <li class="cmt">
            <a href='<%= PermaLink + "#comments"  %>' title="click to read comments or write a comment"><%=BlogPost.CommentCount %> Responses</a>
        </li>
        <li>
            <asp:HyperLink ID="Edit" runat="server"
                EnableViewState="False">Edit</asp:HyperLink></li>
    </ul>
    <div class="text">
        <%=BlogPost.Post%>
    </div>
</div>
