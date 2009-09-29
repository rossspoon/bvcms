<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSRegCustom.Models.MOBSModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>MOBS Event Registration</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>MOBS Registration Received</h2>
    <p>
        Thank you for registering. <%=Model.person.Name %> has purchased <%=Model.tickets %> tickets for the MOBS event on <%=Model.meeting.MeetingDate %>.  
        You should receive a confirmation email shortly.
    </p>

</asp:Content>
