<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.DiscipleLifeModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>DiscipleLife Registration Received</h2>
    <p>
        Thank you for registering.<br />
        <%=Model.person.Name %> is now registered in <%=Model.organization.OrganizationName %>.  
        You should receive a confirmation email shortly.
    </p>
    <a href="/DiscipleLife/Index/<%=Model.division.Id %>">Register another person for <%=Model.division.Name %></a>

</asp:Content>
