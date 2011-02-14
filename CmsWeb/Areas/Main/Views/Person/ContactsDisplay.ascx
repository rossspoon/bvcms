<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<int>" %>
<table>
    <tr>
        <td>
            <strong>Contacts Given:</strong>
            <a href="/Person/AddContactMade/<%=Model %>" 
                confirm="Are you sure you want to add a new contact record?" 
                class="CreateAndGo">Add New</a>
        </td>
        <td>
        </td>
        <td>
            <strong>Contacts Received:</strong>
            <a href="/Person/AddContactReceived/<%=Model %>" 
                confirm="Are you sure you want to add a new contact record?" 
                class="CreateAndGo">Add New Contact</a>
            <a href="/Person/AddAboutTask/<%=Model %>" 
                confirm="Are you sure you want to add a new task about this person?" 
                class="CreateAndGo">Add New Task</a>
<%--            <div><a href="/Manage/Emails/SentTo/<%=Model %>">Received emails</a></div>
--%>        </td>
    </tr>
    <tr>
        <td style="vertical-align: top">
            <form class="gridf" action="/Person/ContactsMadeGrid/<%=Model %>">
            </form>
        </td>
        <td>
        </td>
        <td style="vertical-align: top">
            <form class="gridf" action="/Person/PendingTasksGrid/<%=Model %>">
            </form>
            <form class="gridf" action="/Person/ContactsReceivedGrid/<%=Model %>">
            </form>
        </td>
    </tr>
</table>        
