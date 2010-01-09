<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Registered</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Registration Successful</h2>
    <p>
        Thank you for enrolling in Step 2.  
        You should receive an email confirmation shortly.
    </p>
    <p>
        <a href="/StepClass/Step2">Register another family member</a>
    </p>
    <a href="http://www.bellevue.org/pages/page.asp?page_id=66485" target="_top">Return to Member Development</a>

</asp:Content>
