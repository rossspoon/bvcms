<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="/ckeditor/ckeditor.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function() {
        CKEDITOR.replace( 'html', {
                filebrowserUploadUrl : '/Account/CKEditorUpload/',
                filebrowserImageUploadUrl: '/Account/CKEditorUpload/'
        });
    });
</script>
<% using (Html.BeginForm("UpdateLeagueContent", "Display", FormMethod.Post))
   { %>
<div class="blogbody">
<h2 class="title"><%=ViewData["title"]%></h2>
<a href="/Setup/Recreation/">Leagues</a>
<hr />
<%=Html.Hidden("id")%>
<div>Title: <%=Html.TextBox("title")%></div>
<div><%=Html.TextArea("html", new { rows = "16", style = "width:90%" })%>
</div>
<input type="submit" value="Post" />
</div>
<% } %>
</asp:Content>
