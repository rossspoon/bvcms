<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.MemberNotesInfo>" %>
<% if (Page.User.IsInRole("Edit"))
   { %>
<a class="displayedit" href="/Person/MemberNotesEdit/<%=Model.PeopleId %>">Edit</a>
<% } %>
<table>
    <tr>
        <td valign="top">
            <table class="Design2">
                <tr>
                    <th colspan="6" class="Design2Head">Letter</th>
                </tr>
                <tr>
                    <th>Status:</th>
                    <td><%=Model.LetterStatus %></td>
                    <th>Date Requested:</th>
                    <td><%=Model.LetterRequested.FormatDate() %></td>
                    <th>Date Received:</th>
                    <td><%=Model.LetterReceived.FormatDate() %></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <strong>Notes:</strong><br />
            <%=Model.LetterNotes %>
        </td>
    </tr>
</table>
