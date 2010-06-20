<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    $(function() {
        $(".datepicker").datepicker({
            dateFormat: 'm/d/yy',
            changeMonth: true,
            changeYear: true
        });
    });
</script>
    <h2>Batch Load Bundle</h2>
    
    <%using(Html.BeginForm())
    { %>
    <%=Html.TextArea("text", ViewData["text"].ToString(), new { rows = "30", cols = "1000", wrap = "off", style = "width:90%" }) %><br />
    Contribution Date: <%=Html.DatePicker("date") %> <input type="submit" value="Submit" />
 <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
