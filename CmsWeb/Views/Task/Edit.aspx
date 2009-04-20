<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskDetail>" %>
<% if (false)
   { %>
    <link href="/Content/style.css" rel="stylesheet" type="text/css" />
<% } %>
    <td colspan="7"><a name="select" />
    <form id="Edit" action="/Task/Update/">
        <table class="Design2" style="border: 3px solid black">
            <tr>
                <th>Task</th>
                <td>
                    <%=Html.Hidden("TaskId", ViewData.Model.Id)%>
                    <%=Html.TextBox("Description", ViewData.Model.Description)%>
                </td>
            </tr>
            <tr>
                <th>Created</th>
                <td>
                    <span style="font-size: smaller; color: Gray"><%=ViewData.Model.CreatedOn.ToString("f")%></span>
                </td>
            </tr>
            <tr>
                <th>Owner</th>
                <td>
                    <%=Html.HyperLink(ViewData.Model.TaskEmail, ViewData.Model.Owner)%>
                    <% if (ViewData.Model.IsOwner) 
                       { %>
                    <%=Html.HyperLink("javascript:SearchPeople(ChangeOwnerPerson)", "(transfer)")%>
                    <% } %>
                </td>
            </tr>
            <tr>
                <th>Delegated To:</th>
                <td>
                    <% if (ViewData.Model.CoOwnerId.HasValue) 
                       { %>
                    <%=Html.HyperLink("mailto:" + ViewData.Model.CoOwnerEmail, ViewData.Model.CoOwner) %>
                    <% } 
                       if (ViewData.Model.IsOwner) 
                       { %>
                    <%=Html.HyperLink("javascript:SearchPeople(AddDelegatePerson)", ViewData.Model.ChangeCoOwner)%>
                    <% } %>
                </td>
            </tr>
            <tr>
                <th>Due</th>
                <td>
                    <%=Html.TextBox("Due", ViewData.Model.Due, new { @class = "datepicker" })%>
                </td>
            </tr>
            <tr>
                <th>Priority</th>
                <td>
                    <%=Html.DropDownList("Priority", ViewData.Model.PriorityList())%>
                </td>
            </tr>
            <tr>
                <th>Status:</th>
                <td>
                    <%=Html.DropDownList("StatusId", ViewData.Model.StatusList())%>
                </td>
            </tr>
            <% if (ViewData.Model.ShowLocation)
               { %>
            <tr>
                <th>Project:</th>
                <td><%=ViewData.Model.Project%>
                </td>
            </tr>
            <% } %>
            <tr>
                <th>Regarding Person:</th>
                <td><%=ViewData.Model.Who%></td>
            </tr>
            <% if (ViewData.Model.ShowLocation)
               { %>
            <tr>
                <th>Location:</th>
                <td><%=ViewData.Model.Location%></td>
            </tr>
            <% } %>
            <tr>
                <th>Notes:<br /></th>
                <td>
                    <%=Html.TextArea("Notes", new { style = "width: 30em" })%>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input type="button" value="Update" onclick="Update()" />
                    <input type="button" value="Cancel" onclick="Deselect()" />
                </td>
            </tr>
        </table>
    </form>
    </td>

