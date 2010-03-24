<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteerModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Volunteer</title>
    <link rel="stylesheet" href="/content/jquery.treeview.css" type="text/css" media="screen" />
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="/content/js/jquery.treeview.min.js"></script>
    <script src="/Content/js/jquery.idle-timer.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $(document).bind("idle.idleTimer", function() {
                window.location.href = '/Volunteer/' + $('#View').val();
            });
            $.idleTimer(<%=ViewData["timeout"] %>);
            $("ul.treeview").treeview({
                control: "#treecontrol",
                animated: "fast",
                collapsed: true
            });
        });
    </script>
    <%=Html.Hidden("View") %>
    <%=Model.FormInitialize() %>
    <h4><%=Model.person.Name %></h4>
    <% using (Html.BeginForm()) { %>
    <%=Html.Hidden("pid", Model.person.PeopleId) %>
    <% var summary = Model.PrepareSummaryText();
       if(summary != "")
       { %>
       <h4>Previous Selections</h4>
        <%=summary %>
    <% } %>

        <div>
            <fieldset>

                <%=Model.formcontent %>
                <input type="submit" value="Submit" />
                <% if (User.IsInRole("Admin") && Util.UserPeopleId != Model.person.PeopleId)
                   { %>
                   <input type="checkbox" name="noemail" value="noemail" checked="checked" /> No Email Notice
                <% } %>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
