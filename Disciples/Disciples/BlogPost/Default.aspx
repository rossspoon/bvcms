<%@ Page Language="C#" AutoEventWireup="True" Inherits="BlogPost_Default" Codebehind="Default.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="h" runat="server">
    <link href="../App_Themes/Default/Common2.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Default/blog.css" rel="stylesheet" type="text/css" />
    <title>BlogPost Proxy</title>
</head>
<body>
    <div id="CommonContent">
        <div id="CommonContentInner" runat="server">
            <div class="CommonContentArea">
                <h1 class="CommonTitle">
                    <asp:HyperLink ID="HyperLink1" EnableViewState="false" runat="server"><%=blog.Title%></asp:HyperLink></h1>
                <div class="CommonContent">
                    <div class="CommonDescription">
                        <asp:Label ID="Label1" runat="server" EnableViewState="false"><%=blog.Description%></asp:Label>
                    </div>
                </div>
                <div class="CommonContent">
                    <div class="BlogPostArea None">
                        <h4 class="BlogPostHeader">
                            <asp:HyperLink ID="TitleLink" runat="server"><%=blogpost.Title%></asp:HyperLink></h4>
                        <div class="BlogPostContent">
                            <%=blogpost.Post%>
                        </div>
                        <div class="BlogPostFooter">
                            Posted
                            <asp:HyperLink ID="pubdate" runat="server"><%=blogpost.EntryDate.Value.ToString("M/d/yy h:mm tt")%></asp:HyperLink>
                            by
                            <asp:HyperLink ID="poster" runat="server"><%=blogpost.PosterName%></asp:HyperLink>
                            <br />
                            <div class="em">
                                Filed under:
                                <asp:Repeater runat="server" ID="CategoryList" EnableViewState="False">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="CatLink" runat="server" NavigateUrl='<%# "~/Blog/" + blogpost.BlogCached.Name + "/" + Eval("Category") + ".asp" %>'><%# Eval("Category") %></asp:HyperLink>,
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
