<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskModel>" %>

<asp:Content ID="indexHead" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr.alt td
        {
            background-color: #eee;
        }
        .Bold
        {
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <script src="/Content/js/jquery.pagination.js" type="text/javascript"></script>
        <script src="/Content/js/ui.tabs.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form.js" type="text/javascript"></script>
        <script src="/Content/js/jquery.form2.js" type="text/javascript"></script>
        <script src="/Scripts/Task.js" type="text/javascript"></script>
    </div>
    <form id="form" action="/Task/List" method="get">
    <%=Html.Hidden("SortTask")%>
    <table class="modalPopup">
        <% if (Page.User.IsInRole("AdvancedTask"))
           { %>
        <tr>
            <th>Project:</th>
            <td>
                <%=Html.DropDownList("Project", ViewData.Model.Projects())%>
            </td>
        </tr>
        <tr>
            <th>Location:</th>
            <td>
                <%=Html.DropDownList("Location", ViewData.Model.Locations())%>
            </td>
        </tr>
        <% } %>
        <tr>
            <th>Status:</th>
            <td>
                <%=Html.DropDownList("StatusId", ViewData.Model.TaskStatusCodes(), new { onchange = "RefreshList()" })%>
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
    <%=Html.Hidden("CurTab", ViewData.Model.MyListId())%>
    <ul id="tabs" class="ui-tabs-nav">
        <% Html.RenderPartial("Tabs", ViewData.Model); %>
    </ul>
    <div style="clear: both" class="ui-tabs-panel">
        <table id="tasks">
            <thead>
                <tr>
                    <td colspan="7">
                        <table>
                            <tr>
                                <td>
                                    <%=Html.DropDownList("actions", ViewData.Model.ActionItems(), new { onchange = "DoAction()" })%>
                                </td>
                                <td>
                                    New Task:
                                    <input type="text" id="TaskDesc" style="width: 217px" onkeypress="return AddTaskEnter(event)" />
                                    <a href="javascript:AddTaskClick()">Add Task</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
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
                <% Html.RenderPartial("Rows", ViewData.Model); %>
            </tbody>
        </table>
        <% Html.RenderPartial("Pager", ViewData.Model.pagerModel()); %>
    </div>
    <div>
        New List(tab):
        <input type="text" id="ListName" style="width: 217px" onkeypress="return AddListEnter(event)" />
        <a href="#" onclick="return AddListClick()">Add List</a>
    </div>
    </form>
    <div id="dialogbox" title="Search Contacts" style="width: 560px; overflow: scroll">
    </div>
</asp:Content>
