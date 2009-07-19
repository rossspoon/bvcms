<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Recreation Registration Received</h2>
    <p>
        Thank you for registering your child.  You should receive a confirmation email shortly.
    </p>
    <a href="/Recreation/">Register another child</a>

</asp:Content>
