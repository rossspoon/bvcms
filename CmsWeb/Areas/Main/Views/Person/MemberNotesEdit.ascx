<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PersonPage.MemberNotesInfo>" %>
<a class="displayedit" href="/Person/MemberNotesDisplay/<%=Model.PeopleId %>">cancel</a>
<table class="Design2">
    <tr>
        <th colspan="2" class="Design2Head">Letter</th>
    </tr>
    <tr>
        <th>Status:</th>
        <td><%=Html.DropDownList("LetterStatusId", CmsWeb.Models.PersonPage.MemberNotesInfo.LetterStatuses())%></td>
    </tr>
    <tr>
        <th>Date Requested:</th>
        <td><%=Html.TextBox("LetterRequested", Model.LetterRequested.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
    <tr>
        <th>Date Received:</th>
        <td><%=Html.TextBox("LetterReceived", Model.LetterReceived.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th>Notes:</th>
        <td>
            <%=Html.TextArea("LetterNotes")%>
        </td>
    </tr>
<tr><td></td></tr>
     <tr><td></td><td><a href="/Person/MemberNotesUpdate/<%=Model.PeopleId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
