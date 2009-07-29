<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecRegModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Recreation Registration Received</h2>
    <p>
        Thank you for registering <%=Model.participant.Name %> in <%=Model.division.Name %>, <%=Model.organization.OrganizationName %>.  
        You should receive a confirmation email shortly.
    </p>
    <a href="/RecReg/Index/<%=Model.division.Id %>">Register another person in this league</a>

</asp:Content>
