<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.SalesModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Purchase Confirmation</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Transaction Successful</h2>
    <p>
        Thank you for purchase. <%=Model.person.Name %> has purchased <%=Model.quantity %> items of the <%=Model.Description %>.  
        You should receive a confirmation email with download instructions shortly.
    </p>

</asp:Content>
