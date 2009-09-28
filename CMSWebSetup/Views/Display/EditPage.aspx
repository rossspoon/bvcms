<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Content.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewData["page"]%>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="/Content/js/jquery.MetaData.js" type="text/javascript"></script>
<script src="/Content/js/jquery.FCKEditor.pack.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function() { $('#html').fck({ path: '/fckeditor/' }); });
</script>
<% using (Html.BeginForm("UpdatePage", "Display", FormMethod.Post))
   { %>
<div class="blogbody">
<h2 class="title"><%=ViewData["title"]%></h2>
<hr />
<%=Html.Hidden("id")%>
<div>Title: <%=Html.TextBox("title")%></div>
<%=Html.TextArea("html")%>
<input type="submit" value="Post" />
</div>
<% } %>
</asp:Content>
