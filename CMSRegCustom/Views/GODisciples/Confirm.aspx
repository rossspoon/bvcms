<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSRegCustom.Models.GODisciplesModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title><%=ViewData["header"] %></title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Model.GroupDescription %></h2>
    <p>
        Thank you for joining GO Disciples.<br />
        You should receive a confirmation email shortly with further instructions.
    </p>

</asp:Content>
