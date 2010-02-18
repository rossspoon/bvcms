<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteerCommitmentsModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script language="javascript">
        $(function() {
            $(".reminder").click(function() {
                window.location = "/Volunteers/EmailReminders/" + $(this).attr("mid");
            });
        });
    </script>

    <h2>
        Volunteer Calendar for
        <%=Model.OrgName %></h2>
    <table id="t" border="1" cellpadding="5" cellspacing="0">
        <thead>
            <tr>
                <th>
                    Week
                </th>
                <% foreach (var t in Model.times)
                   { %>
                <th>
                    <%=t %>
                </th>
                <% } %>
            </tr>
        </thead>
        <tbody>
            <% foreach (var w in Model.weeks)
               { %>
            <tr>
                <th valign="top">
                    <%=w %>
                </th>
                <% foreach (var t in Model.times)
                   {
                       var cell = Model.details.FirstOrDefault(c => c.Sunday == w && c.DayHour == t);
                %>
                <td valign="top">
                    <% if (cell != null)
                       { %>
                    <a href="#" class="reminder" mid="<%=cell.MeetingId %>">Email Reminders(<%=cell.Persons.Count() %>)</a><br />
                    <a href="/Reports/Rollsheet/?meetingid=<%=cell.MeetingId %>">Rollsheet Report</a><br />
                    <br />
                    <% foreach (var p in cell.Persons)
                       { %>
                    <a href="/Person/Index/<%=p.PeopleId %>">
                        <%=p.Name%></a><br />
                    <% } %>
                    <% } %>
                </td>
                <% } %>
            </tr>
            <% } %>
        </tbody>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
