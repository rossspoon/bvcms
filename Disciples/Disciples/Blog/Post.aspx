<%@ Page StylesheetTheme="Default" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="True" Inherits="Blog_Post"
    Title="Blog Post" CodeBehind="Post.aspx.cs" %>

<%@ Register Src="Item.ascx" TagName="Item" TagPrefix="uc1" %>
<%@ Register Src="Archives.ascx" TagName="Archives" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <div id="main">
        <div id="thoughts">
            <div id="entry">
            <uc1:Item ID="Item1" EnableViewState="false" SingleItem="true" runat="server" />
            </div>
            <div id="comments">
                <a name="comments"></a>
                <h3>
                    <%=Item1.BlogPost.CommentCount %>
                    Responses<a href="#postcomment" title="Leave a comment">»</a></h3>
                <ol>
                    <asp:ListView ID="ListView2" runat="server" DataSourceID="ObjectDataSource1">
                        <LayoutTemplate>
                            <li id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li class='response <%# (bool)Eval("IsAuthor")? "kahuna" : "alt" %>'>
                                <ul class="meta">
                                    <li class="poster">
                                        <%# Eval("PosterName") %></li>
                                    <li class="date">
                                        <%# Eval("DatePosted", "{0:ddd d MMM yyyy}")%></li>
                                    <li class="time">
                                        <%# Eval("DatePosted","{0:HHmm}") %></li>
                                    <li>
                                        <asp:HyperLink ID="EditComment" runat="server" NavigateUrl='<%# Eval("Id","~/Blog/EditComment.aspx?id={0}") %>' Visible='<%# Eval("CanEdit") %>'
                                            EnableViewState="False">Edit</asp:HyperLink></li>
                                </ul>
                                <div class="text">
                                    <asp:Literal ID="Comment" runat="server" Text='<%# Eval("CommentHtml") %>'></asp:Literal>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                </ol>
                <a name="postcomment"></a>
                <div id="postcomment" runat="server">
                    <h3>
                        Leave a Comment (you must be signed in)</h3> 
                    <p id="form-info">
                        Line and paragraph breaks are automatic.<br />
                        The following markup is allowed: *This is italics*, **this is bold** and ***this
                        is underlined***<br />
                        This text: &quot;Goto Bellevue&quot; = &quot;www.bellevue.org&quot; is a hyperlink.
                    </p>
                    <hr />
                    <p>
                        <label for="comment">
                            Your Comment</label>
                        <br />
                        <asp:TextBox ID="Comments" runat="server" Columns="25" Rows="5" TextMode="MultiLine"
                            Style="width: 95%; height: 100px"></asp:TextBox></p>
                    <p>
                        Management reserves the right to edit or remove any comment.
                    </p>
                    <p>
                        <asp:Button ID="Preview" runat="server" Text="Preview" OnClick="Preview_Click" />
                        <asp:Button ID="PostCmt" runat="server" Text="Submit" OnClick="PostComment_Click" />
                    </p>
                </div>
                <ol id="PreviewArea" runat="server">
                    <li class='response alt'>
                        <ul class="meta">
                            <li class="poster">
                                <%= User.Identity.Name %>
                                wrote:</li>
                            <li class="date">
                                <%= DateTime.Now.ToString("ddd d MMM yyyy")%></li>
                            <li class="time">
                                <%= DateTime.Now.ToString("HHmm")%></li>
                        </ul>
                        <div class="text">
                            <asp:Literal ID="PreviewComment" runat="server" Text='<%= PreviewCommentHtml %>'></asp:Literal>
                        </div>
                    </li>
                </ol>
            </div>
        </div>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="FetchPostComments"
        TypeName="DiscData.BlogPostController">
        <SelectParameters>
            <asp:QueryStringParameter Name="BlogPostId" QueryStringField="id" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphSideBar" runat="server">
    <div id="extra">
        <div class="panel">
            <h4>
                Sidestep</h4>
            <ul>
                <li id="NextListItem" class="next" runat="server">Next entry:<br />
                    <asp:HyperLink ID="NextEntry" runat="server">HyperLink</asp:HyperLink></li>
                <li id="PrevListItem" class="prev" runat="server">Previous entry:<br />
                    <asp:HyperLink ID="PrevEntry" runat="server">HyperLink</asp:HyperLink></li>
            </ul>
        </div>
        <div class="panel" id="Categories">
            <h4>
                Categories</h4>
            <ul>
                <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                <asp:HyperLink ID="HyperLink1" runat="server"  style='<%# "font-size: " + Eval("Size") %>'
                    NavigateUrl='<%# "~/Blog/" + Eval("BlogName") + "/" + Eval("Category") + ".aspx" %>'
                    Text='<%# Eval("Category") %>'></asp:HyperLink>
                </ItemTemplate>
                </asp:Repeater>
                <asp:ListView ID="ListView1" runat="server">
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
