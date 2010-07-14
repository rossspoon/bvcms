<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsWeb.Models.TaskModel>" %>

<asp:Content ID="indexHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
    <%= SquishIt.Framework.Bundle.JavaScript()
        .Add("/Content/js/jquery.pagination.js")
        .Add("/Content/js/jquery.form.js")
        .Add("/Content/js/jquery.form2.js")
        .Add("/Scripts/Task.js")
        .Add("/Scripts/SearchPeople.js")
        .Render("/Content/Task_#.js")
            %>
    </div>
    <form id="form" action="/Task/List" method="get">
    <%=Html.Hidden("SortTask")%>
    <table class="modalPopup">
        <% if (Page.User.IsInRole("AdvancedTask"))
           { %>
        <tr>
            <th>
                Project:
            </th>
            <td>
                <%=Html.DropDownList("Project", Model.Projects())%>
            </td>
        </tr>
        <tr>
            <th>
                Location:
            </th>
            <td>
                <%=Html.DropDownList("Location", Model.Locations())%>
            </td>
        </tr>
        <% } %>
        <tr>
            <th>
                Status:
            </th>
            <td>
                <%=Html.DropDownList("StatusId", Model.TaskStatusCodes(), new { onchange = "RefreshList()" })%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <label for="OnwerOnly" class="Bold">
                    Owned Tasks Only:</label>
                <%=Html.CheckBox("OwnerOnly", new { onclick = "RefreshList()" })%>
            </td>
        </tr>
    </table>
    <%=Html.Hidden("CurTab", Model.MyListId())%>
    <div id="tabs">
        <ul>
            <% Html.RenderPartial("Tabs", Model); %>
        </ul>
        <% foreach (var list in Model.FetchTaskLists())
           { %>
        <div id='<%=list.Id %>' style="display:none"></div>
        <% } %>
    </div>
        <table>
            <tr>
                <td>
                    <%=Html.DropDownList("actions", ViewData.Model.ActionItems(), new { onchange = "DoAction()" })%>
                </td>
                <td>
                    New Task:
                    <input type="text" id="TaskDesc" maxlength="100" style="width: 217px" onkeypress="return AddTaskEnter(event)" />
                    <a href="javascript:AddTaskClick()">Add Task</a>
                </td>
            </tr>
        </table>
        <table id="tasks">
            <thead>
                <tr>
                    <th>
                    </th>
                    <th title="sort by priority">
                        <a href="#" class="sortable">123</a>
                    </th>
                    <th>
                        <a href="#" class="sortable">Due/Completed</a>
                    </th>
                    <th>
                        <a href="#" class="sortable">Task</a>
                    </th>
                    <th>
                        <a href="#" class="sortable">About</a>
                    </th>
                    <th>
                        Status
                    </th>
                    <th>
                        <a href="#" class="sortable">Assigned</a>
                    </th>
                </tr>
            </thead>
            <tbody>
                <% Html.RenderPartial("Rows", Model); %>
            </tbody>
        </table>
        <% Html.RenderPartial("Pager", Model.pagerModel()); %>
        <div>
            New List(tab):
            <input type="text" id="ListName" maxlength="50" style="width: 217px" onkeypress="return AddListEnter(event)" />
            <a href="#" onclick="return AddListClick()">Add List</a>
        </div>
<input type="hidden" id="Count" value='<%=Model.Count%>' />
<%=Html.Hidden("Sort",Model.Sort)%>
    </form>
    <div id="dialogbox" title="Search Contacts" style="width: 560px; overflow: scroll">
    </div>
    <div id="SearchPeopleDialog" title="Search People" style="width: 560px; overflow: scroll">
    </div>
</asp:Content>
