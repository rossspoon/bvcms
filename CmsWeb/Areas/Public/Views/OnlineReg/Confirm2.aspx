<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TransactionInfo>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title><%=Model.Header %> Event Registration</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Transaction Completed</h2>
    <p>
        Thank you for your payment of <%=Model.AmountDue.ToString("c") %> for <%=Model.Header %>  
        You should receive a confirmation email at <%=Util.FirstAddress(Model.Email) %> shortly.
    </p>

</asp:Content>
