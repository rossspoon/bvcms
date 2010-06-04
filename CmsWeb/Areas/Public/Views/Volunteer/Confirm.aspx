<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/onlinereg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteerModel>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Content/js/jquery-1.4.2.min.js" type="text/javascript"></script>    
    <script src="/Content/js/jquery.idle-timer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(document).bind("idle.idleTimer", function() {
                window.location.href = '/Volunteer/' + $('#View').val();
            });
            $.idleTimer(<%=ViewData["timeout"] %>);
        });
    </script>
    <%=Html.Hidden("View") %>

    <h2><%=Model.formtitle %></h2>
    <h4>Information Received</h4>
    <p>
        Thank you for being willing to volunteer.  
        You should receive an email with more information shortly.
    </p>
    <p>
        If you do not receive an email, it may be that we have an old or no email address for you, 
        please contact the church to provide one for us and we'll update your record.
    </p>
    <% if(User.IsInRole("Access") && ViewData["url"].IsNotNull())
       { %>
       <a href='<%=ViewData["url"] %>'>continue</a>
    <% } %>

</asp:Content>
