<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="/ckeditor/ckeditor.js" type="text/javascript"></script>
<% using (Html.BeginForm("UpdatePage", "Display", FormMethod.Post))
   { %>
<div class="blogbody">
<h2 class="title"><%=ViewData["title"]%></h2>
<hr />
<%=Html.Hidden("id")%>
<div>Title: <%=Html.TextBox("title")%></div>
<%=Html.TextArea("html", new { @class="ckeditor" })%>
<input type="submit" value="Post" />
</div>
<% } %>
</asp:Content>
