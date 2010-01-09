<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.GODisciplesModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title><%=ViewData["header"] %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Model.GroupDescription %></h2>
    <p>
        Thank you for joining GO Disciples.<br />
        You should receive a confirmation email shortly with further instructions.
        If you do not receive an email, it may be that we have an old or no email address for you, 
        please contact the church to provide one for us and we'll update your record.
    </p>

</asp:Content>
