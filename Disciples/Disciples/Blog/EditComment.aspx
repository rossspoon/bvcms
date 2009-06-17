<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditComment.aspx.cs"
    Inherits="EditComment" Title="Edit Comment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="server">
    <div id="main">
        <div id="thoughts">
            <div id="comments">
                <ol id="PreviewArea" runat="server">
                    <li class='response alt'>
                        <ul class="meta">
                            <li class="poster">
                                <%= comment.PosterName %></li>
                            <li class="date">
                                <%= comment.DatePosted.ToString("ddd d MMM yyyy")%></li>
                            <li class="time">
                                <%= comment.DatePosted.ToString("HHmm") %></li>
                        </ul>
                        <div class="text">
                            <asp:Literal ID="PreviewComment" runat="server" ></asp:Literal>
                        </div>
                    </li>
                </ol>
                <div id="editcomment" runat="server">
                    <h3>
                        Edit Comment</h3>
                    <p id="form-info">
                        Line and paragraph breaks are automatic.<br />
                        The following markup is allowed: *This is italics*, **this is bold** and ***this
                        is underlined***<br />
                        This text: &quot;Goto Bellevue&quot; = &quot;www.bellevue.org&quot; is a hyperlink.
                    </p>
                    <hr>
                    <p>
                        <label for="comment">
                            Comment</label>
                        <br />
                        <asp:TextBox ID="Comments" runat="server" Columns="25" Rows="5" TextMode="MultiLine"
                            Style="width: 95%; height: 100px"></asp:TextBox></p>
                    <p>
                        Management reserves the right to edit or remove any comment.
                    </p>
                    <p>
                        <asp:Button ID="Preview" runat="server" Text="Preview" OnClick="Preview_Click" />
                        <asp:Button ID="PostCmt" runat="server" Text="Submit" OnClick="PostComment_Click" />
                        <asp:Button ID="Delete" runat="server" onclick="Delete_Click" Text="Delete" />
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
