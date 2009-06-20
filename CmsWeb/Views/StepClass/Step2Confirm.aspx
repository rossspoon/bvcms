<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage" %>

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
    <a href="http://www.bellevue.org">Return to Bellevue.org</a>

</asp:Content>
