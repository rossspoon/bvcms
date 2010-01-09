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
                var loc = "/Register/Visit2/<%=Model.campusid %>?familyid=<%=Model.familyid %>";
                if (thisday)
                    loc += "&thisday=" + thisday;
                window.location = loc;
            });
            $('#newreg').click(function() {
                var loc = "/Register/Visit/<%=Model.campusid %>";
                if (thisday)
                    loc += "?thisday=" + thisday;
                window.location = loc;
            });
        });
    </script>

    <h2>Church Database Registration Received</h2>
    <p>
        Thank you for registering.  You should receive a confirmation email shortly.
    </p>
    <%=Html.Hidden("thisday", Model.thisday) %>
    <p><a id="addfam" href="#">Register another family member</a></p>
    <p><a id="newreg" href="#">New Registration</a></p>

</asp:Content>
