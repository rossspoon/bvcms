<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonModel.PersonInfo>" %>
<table>
    <tr>
        <td valign="top" style="border-style: groove; border-width: thin;">
            <table class="Design2">
                <tr>
                    <th>How did you hear about our Church?</th>
                    <td><%=Model.InterestPoint %></td>
                </tr>
                <tr>
                    <th>Origin:</th>
                    <td><%=Model.Origin %></td>
                </tr>
                <tr>
                    <th>Entry Point:</th>
                    <td><%=Model.EntryPoint %></td>
                </tr>
                <tr>
                    <th>Is a member of another Church?</th>
                    <td><input type="checkbox" <%=Model.MemberAnyChurch ? "checked='checked'" : "" %> disabled="disabled" /></td>
                </tr>
            </table>
        </td>
        <td>
        </td>
        <td valign="top" style="border-style: groove; border-width: thin;">
            <table class="Design2">
                <tr>
                    <td><input type="checkbox" <%=Model.ChristAsSavior ? "checked='checked'" : "" %> disabled="disabled" /></td>
                    <th>Prayed for Christ as Savior</th>
                </tr>
                <tr>
                    <td><input type="checkbox" <%=Model.PleaseVisit ? "checked='checked'" : "" %> disabled="disabled" /></td>
                    <th>Would like someone to visit</th>
                </tr>
                <tr>
                    <td><input type="checkbox" <%=Model.InterestedInJoining ? "checked='checked'" : "" %> disabled="disabled" /></td>
                   <th>Interested in joining Church</th>
                </tr>
                <tr>
                    <td><input type="checkbox" <%=Model.SendInfo ? "checked='checked'" : "" %> disabled="disabled" /></td>
                    <th>Would like to know how to become a Christian</th>
                </tr>
            </table>
        </td>
        <td>
        </td>
        <td valign="top" style="border-style: groove; border-width: thin;">
            <table class="Design2">
                <tr>
                    <td height="100%" width="100%">
                        <strong>Comments:</strong><br />
                        <%=Model.Comments %>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <strong>Contacts Given:</strong>
            <a href="/Person/AddContactMade/<%=Model.PeopleId %>" 
                confirm="Are you sure you want to add a new contact record?" 
                class="CreateAndGo">Add New</a>
        </td>
        <td>
        </td>
        <td>
            <strong>Contacts Received:</strong>
            <a href="/Person/AddContactReceived/<%=Model.PeopleId %>" 
                confirm="Are you sure you want to add a new contact record?" 
                class="CreateAndGo">Add New Contact</a>
            <a href="/Person/AddAboutTask/<%=Model.PeopleId %>" 
                confirm="Are you sure you want to add a new task about this person?" 
                class="CreateAndGo">Add New Task</a>
        </td>
    </tr>
    <tr>
        <td style="vertical-align: top">
            <form action="/Person/ContactsMadeGrid/<%=Model.PeopleId %>">
            </form>
        </td>
        <td>
        </td>
        <td style="vertical-align: top">
            <% Html.RenderPartial("PendingTasksGrid", Model.tasks); %>
            <form action="/Person/ContactsReceivedGrid/<%=Model.PeopleId %>">
            </form>
        </td>
    </tr>
</table>
