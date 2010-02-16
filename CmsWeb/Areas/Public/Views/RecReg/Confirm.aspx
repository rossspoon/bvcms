<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title><%=ViewData["orgname"] %> Event Registration</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Event Registration Received</h2>
    <p>
        Thank you for registering for the <%=ViewData["orgname"] %> event.  
        You should receive a confirmation email at <%=ViewData["email"] %> shortly.
    </p>

</asp:Content>
