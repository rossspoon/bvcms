<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteersModel>" %>
<html>
<head>
<script src="/Content/js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script type="text/javascript">
</script>
</head>
<body>
</body>
</html>
    <h2>Custom Report for <%=Model.Opportunity.Description %></h2>
    <div>
    <%=Model.reportcontent %>
<%=Html.Hidden("opportunityid", Model.OpportunityId) %>
    </div>
