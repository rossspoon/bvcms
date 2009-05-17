<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Forms</h2>
   <p>
        Members and friends can sign up for our various programs and events on this site.</p>
    <h3>
        Help...</h3>
    <p>
        If you are having a difficult time or have questions or special requests,
        please the appropriate ministry using the contact information on the specific page.</p>
        <a href="/VBS/">VBS Registration</a>
</asp:Content>
