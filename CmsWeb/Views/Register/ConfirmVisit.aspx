<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function() {
            $('a:first').focus();
        });
    </script>

    <h2>Church Database Registration Received</h2>
    <p>
        Thank you for registering.  You should receive a confirmation email shortly.
    </p>
    <p><a href="/Register/Visit2/">Register another family member</a></p>
    <p><a href="/Register/Visit/<%=Session["campus"] %>">New Registration</a></p>

</asp:Content>
