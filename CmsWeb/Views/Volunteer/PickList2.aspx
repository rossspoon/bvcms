<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteerModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Volunteer</title>
    <link rel="stylesheet" href="/content/jquery.treeview.css" type="text/css" media="screen" />
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/content/js/jquery.treeview.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function() {
            $("ul.treeview").treeview({
                animated: "fast",
                collapsed: true
            });
        });
    </script>
    <h2>Volunteer for <%=Model.Opportunity.Description %></h2>

    <% using (Html.BeginForm()) { %>
    <%=Html.Hidden("Id", Model.VolInterestId) %>
        <div>
            <fieldset>
                <%=Model.formcontent %>
                <input type="submit" value="Submit" /><br />
                <span style="color:Green;font-weight:bold"><%=ViewData["saved"] %></span>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
