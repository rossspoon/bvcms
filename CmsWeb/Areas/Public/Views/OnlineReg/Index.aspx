<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.OnlineRegModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
tr.alt
{
    background-color: #ddd;
    width: 100%;
}
.blue
{
    color: blue;
}
</style>
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>    
    <script src="/Scripts/OnlineReg.js" type="text/javascript"></script>
    <h2><%=Model.Header%></h2>
    <% if (Model.IsEnded())
       { %>
    <h4 style="color:Red">Registration has ended</h4>
    <% }
       else
       { %>
    <%=Model.Instructions%>
    <form class="DisplayEdit" action="/OnlineReg/CompleteRegistration/<%=Model.qtesting %>" method="post">
    <% Html.RenderPartial("List", Model); %>
    </form>
    <% } %>
</asp:Content>
