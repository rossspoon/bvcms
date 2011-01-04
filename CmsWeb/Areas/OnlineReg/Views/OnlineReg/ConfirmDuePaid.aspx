<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsData.Transaction>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
   <h2>Transaction Completed</h2>
    <p>
        Thank you for your payment of <%=Model.Amt.Value.ToString("N") %> for <%=Model.Description %>.<br />
        Your balance is <%=Model.Amtdue.Value.ToString("N") %>.<br/>
        You should receive a confirmation email shortly.
    </p>
</asp:Content>