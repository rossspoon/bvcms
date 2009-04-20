<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Content.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <%=ViewData["page"]%>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="HeaderLinks" runat="server">
    <% Html.RenderAction<CMSWeb.Controllers.DisplayController>(c => c.RsdLink()); %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="blogbody">
    <h2 class="title"><%=ViewData["title"]%></h2>
    <hr />
    <%=ViewData["html"]%>
    </div>
</asp:Content>
