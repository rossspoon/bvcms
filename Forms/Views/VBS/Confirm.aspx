<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Vacation Bible School Registration Received</h2>
    <p>
        Thank you for registering your child.  You should receive a confirmation email shortly.
    </p>
    <a href="/VBS/">Register another child</a>

</asp:Content>
