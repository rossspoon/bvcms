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
    
<form action="/PostBundle/BatchUpload" method="post" enctype="multipart/form-data">
    <label>Filename: <input type="file" name="file" /></label>
    <label>Contribution Date: </label> <%=Html.DatePicker("date") %>
    <input type="submit" value="Submit" />
    <%=Html.TextArea("text", "", new { rows = "10", cols = "1000", wrap = "off", style = "width:90%" }) %><br />
</form>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
