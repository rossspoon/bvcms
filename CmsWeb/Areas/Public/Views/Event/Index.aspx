<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<IList<CMSWeb.Models.EventModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
tr.alt
{
    background-color: #ddd;
    width: 100%;
}
</style>
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>    
    <script src="/Scripts/Event.js?v=5" type="text/javascript"></script>
    <h2><%=ViewData["EventName"] %></h2>
    <% if ((bool)ViewData["filled"])
       { %>
       Sorry, this event has been filled
    <% }
       else
       { %>
    <%=ViewData["Instructions"]%>
    <% 
        var d = new Dictionary<string, object>();
        d.Add("class", "DisplayEdit");
        var rv = ViewData["rv"] as RouteValueDictionary;
        using (Html.BeginForm("CompleteRegistration", "Event", rv, FormMethod.Post, d))
        {
            Html.RenderPartial("List", Model);
        }
       }
    %>
</asp:Content>
