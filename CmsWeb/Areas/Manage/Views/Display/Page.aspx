<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="blogbody">
    <h2 class="title"><%=ViewData["title"]%></h2>
    <hr />
    <%=ViewData["html"]%>
    </div>
</asp:Content>
