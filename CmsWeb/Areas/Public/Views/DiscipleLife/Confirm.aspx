<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.DiscipleLifeModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title><%=Model.division.Name %> Registration</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%=Model.division.Name %> Registration Received</h2>
    <p>
        Thank you for registering.<br />
        <%=Model.person.Name %> is now registered in <%=Model.organization.OrganizationName %>.  
        You should receive a confirmation email shortly.
    </p>
    <a href="/DiscipleLife/Index/<%=Model.division.Id %>">Register another person for <%=Model.division.Name %></a>

</asp:Content>
