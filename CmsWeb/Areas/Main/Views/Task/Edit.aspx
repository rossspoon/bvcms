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
                    <%=Html.Hidden("TaskId", Model.Id)%>
                    <%=Html.TextBox("Description", Model.Description)%>
                </td>
            </tr>
            <tr>
                <th>Created</th>
                <td>
                    <span style="font-size: smaller; color: Gray"><%=Model.CreatedOn.ToString("f")%></span>
                </td>
            </tr>
            <tr>
                <th>Owner</th>
                <td>
                    <%=Html.HyperLink(Model.TaskEmail, Model.Owner)%>
                    <% if (Model.IsOwner) 
                       { %>
                    <%=Html.HyperLink("javascript:SearchPeople(ChangeOwnerPerson)", "(transfer)")%>
                    <% } %>
                </td>
            </tr>
            <tr>
                <th>Delegated To:</th>
                <td>
                    <% if (Model.CoOwnerId.HasValue) 
                       { %>
                    <%=Html.HyperLink("mailto:" + Model.CoOwnerEmail, Model.CoOwner) %>
                    <% } 
                       if (Model.IsOwner) 
                       { %>
                    <%=Html.HyperLink("javascript:SearchPeople(AddDelegatePerson)", Model.ChangeCoOwner)%>
                    <%=Html.CheckBox("ForceCompleteWContact") %> Complete With Contact
                    <% } %>
                </td>
            </tr>
            <tr>
                <th>Due</th>
                <td>
                    <%=Html.TextBox("Due", Model.Due, new { @class = "datepicker" })%>
                </td>
            </tr>
            <tr>
                <th>Priority</th>
                <td>
                    <%=Html.DropDownList("Priority", Model.PriorityList())%>
                </td>
            </tr>
            <tr>
                <th>Status:</th>
                <td>
                    <%=Html.DropDownList("StatusId", Model.StatusList())%>
                </td>
            </tr>
            <% if (Model.ShowLocation)
               { %>
            <tr>
                <th>Project:</th>
                <td><%=Model.Project%>
                </td>
            </tr>
            <% } %>
            <tr>
                <th>Regarding Person:</th>
                <td><%=Model.Who%></td>
            </tr>
            <% if (Model.ShowLocation)
               { %>
            <tr>
                <th>Location:</th>
                <td><%=Model.Location%></td>
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

