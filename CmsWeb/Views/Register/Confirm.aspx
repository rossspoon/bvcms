<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RegisterModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function() {
            var thisday = $('#thisday').val();
            $('a:first').focus();
            $('#addfam').click(function() {
                window.location = "/Register/Form2/?familyid=<%=Model.familyid %>";
            });
        });
    </script>

    <h2>Church Database Registration Received</h2>
    <p>
        Thank you for registering.  You should receive a confirmation email shortly.
    </p>
    <p><a id="addfam" href="#">Register another family member</a></p>
    <p><a href="/Register/">New Registration</a></p>

</asp:Content>
