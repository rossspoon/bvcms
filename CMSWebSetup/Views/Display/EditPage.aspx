<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
<% if ((bool)ViewData["ishtml"])
   { %>
<script src="/ckeditor/ckeditor.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function() {
        CKEDITOR.replace( 'html', {
                filebrowserUploadUrl : '/Account/CKEditorUpload/',
                filebrowserImageUploadUrl: '/Account/CKEditorUpload/'
        });
    });
</script>
    $(function() {
<% } %>
<script type="text/javascript">
        $("#ishtml").click(function() {
            window.location = "/Display/EditPage/" + $('#id').val() + "?ishtml=" + $('#ishtml').attr('checked');
        });
    });
</script>
<% using (Html.BeginForm("UpdatePage", "Display", FormMethod.Post))
   { %>
<div class="blogbody">
<h2 class="title"><%=ViewData["title"]%></h2>
<hr />
<%=Html.Hidden("id")%>
<div>Title: <%=Html.TextBox("title")%> <%=Html.CheckBox("ishtml") %> Is Html</div>
<div><%=Html.TextArea("html", new { rows = "16", style = "width:90%" })%>
</div>
<input type="submit" value="Post" />
</div>
<% } %>
</asp:Content>
