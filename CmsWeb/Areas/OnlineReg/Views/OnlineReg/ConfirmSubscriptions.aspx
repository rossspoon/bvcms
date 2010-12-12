<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.ManageSubsModel>" %>
<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
   <h2>Confirmation</h2>
    <p>
        Thank you <%=Model.person.PreferredName %>, for managing your subscriptions to <%=Model.Division.Name %><br />
        You should receive a confirmation email shortly.</p>
    <p> You have subscribed to the following:</p>
    <%=Model.Summary %>
</asp:Content>