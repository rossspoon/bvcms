<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Church Database Registration Received</h2>
    <p>
        Thank you for registering.  You should receive a confirmation email shortly.
    </p>
    <a href="/Register/Form2/">Register another family member</a>

</asp:Content>
