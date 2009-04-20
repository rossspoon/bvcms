<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="head" runat="server">
    <title>Registration</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="/Scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function() {
        $("input#first").keypress(function(e) {
            if (e.which == 96) {
                $("#first").val("Bob");
                $("#last").val("Bellevue");
                $("#email").val("bob@bellevue.org");
                $("#birthday").val("12/25/1960");
                $("#phone").val("(901) 555-1212");
                return false;
            }
        });
    });
</script>
    <h2>Registration</h2>
    <p>
        Use the form below to allow us to find your membership record in our church system. 
        We will email you a temporary username and password to access this site in the future.
    </p>
    <%= Html.ValidationSummary() %>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Personal Information (used to verify your membership in the Church database)</legend>
                <p>
                    <label for="first">First Name:</label>
                    <%= Html.TextBox("first") %>
                    <%= Html.ValidationMessage("first") %>
                </p>
                <p>
                    <label for="last">Last Name:</label>
                    <%= Html.TextBox("last") %>
                    <%= Html.ValidationMessage("last") %>
                </p>
                <p>
                    <label for="email">Email:</label>
                    <%= Html.TextBox("email") %>
                    <%= Html.ValidationMessage("email") %>
                </p>
                <p>
                    <label for="birthday">Birthday:</label>
                    <%= Html.TextBox("birthday")%>
                    <%= Html.ValidationMessage("birthday") %>
                </p>
                <p>
                    <label for="phone">Phone:</label>
                    <%= Html.TextBox("phone")%>
                    <%= Html.ValidationMessage("phone") %>
                </p>
                <p>
                    <input type="submit" value="Submit" />
                </p>
            </fieldset>
        </div>
    <p>
        If you are having a difficult time or have questions or special requests,
        please call Bellevue's Prayer Ministry at 901-347-5750 or contact them via email at
        <a href="mailto:prayer@bellevue.org">prayer@bellevue.org</a> </p>
    <% } %>
</asp:Content>
