<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
   <h2>Transaction Completed</h2>
    <p>
        Thank you for your Commitment to <%=ViewData["Organization"] %><br />
        You should receive a confirmation email shortly.
    </p>
</asp:Content>