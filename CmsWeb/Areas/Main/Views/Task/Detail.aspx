<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskDetail>" %>
<% if (false)
   { %>
    <link href="/Content/style.css" rel="stylesheet" type="text/css" />
<% } %>
<td colspan="7"><a name="detail"></a>
    <table class="Design2" style="border: 3px solid black">
    <tr><td align="right" colspan="2"><a href="#" onclick="Deselect();return false">
        <img style="border:0" src="/Content/images/888888_11x11_icon_close.gif" /></a></td></tr>
        <tr>
   
            <th>Task</th>
            <td>
                <%=Html.Hidden("TaskId", Model.Id)%>
                <%=Model.Description%>
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
                <a id="changeowner" href="#">(transfer)</a>
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
                <a id="delegate" href="#"><%=Model.ChangeCoOwner%></a>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>Due</th>
            <td>
                <%="{0:d}".Fmt(Model.Due) %>
            </td>
        </tr>
        <tr>
            <th>Priority</th>
            <td>
                <span id="Priority" style="margin-right:3em"><%=Model.Priority%></span>
                <% if(Model.IsAnOwner) 
                   { %>
                    <a href="#" onclick="SetPriority(<%=ViewData.Model.Id%>,1);return false;"> 1 </a>
                    <a href="#" onclick="SetPriority(<%=ViewData.Model.Id%>,2);return false;"> 2 </a>
                    <a href="#" onclick="SetPriority(<%=ViewData.Model.Id%>,3);return false;"> 3 </a>
                    <a href="#" onclick="SetPriority(<%=ViewData.Model.Id%>,0);return false;">None</a>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>Status:</th>
            <td>
                <span class="edit-select" vid="Status" canedit='<%=Model.IsAnOwner%>'><%=Model.Status%></span>
                <% if (Model.CanComplete)
                   { %>
                <a href="#" onclick="SetComplete(<%=Model.Id%>);return false;" style="font-size:120%">(complete)</a>
                <% }
                   if (Model.CanAccept)
                   { %>
                <a href="#" onclick="Accept(<%=ViewData.Model.Id%>);return false;" style="font-size:120%">(accept)</a>
                <% }
                   if (Model.ShowCompleted)
                   { %>
                <span style="font-size: smaller; color: Gray" runat="server"><%="{0:f}".Fmt(Model.CompletedOn)%></span>
                <% } %>
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
        <tr style='font-size: larger'>
            <th>Regarding Person:</th>
            <td style="border: 1px solid grey">
                <% if (Model.WhoId.HasValue)
                   { %>
                <%=Html.HyperLink("/Person/Index/" + Model.WhoId, Model.Who)%>
                <% } 
                   if (Model.IsAnOwner)
                   { %>
                <a id="changeabout" href="#"><%=Model.ChangeWho %></a>
                <% }
                   if (Model.WhoId.HasValue)
                   { %>
                <%=Html.HyperLink(Model.ProspectReportLink(), "Prospect Report", new { target = "_blank" })%>
                <div>
                    <%=Html.HyperLink("http://www.google.com/maps?q=" + Model.WhoAddrCityStateZip, Model.WhoAddress, new { target = "_blank" })%>
                    | <%=Html.HyperLink("http://www.google.com/maps?f=d&saddr=2000+Appling+Rd,+Cordova,+Tennessee+38016&pw=2&daddr=" + Model.WhoAddrCityStateZip, "driving directions", new { target = "_blank" })%><br />
                    <%=Html.HyperLink("mailto:" + Model.WhoEmail, Model.WhoEmail2)%>
                    | <%=Model.WhoPhone%> 
                </div>
                <% } %>
            </td>
        </tr>
        <% if (Model.ShowLocation)
           { %>
        <tr>
            <th>Location:</th>
            <td>
                <span class=".location"><%=Model.Location%></span>
                <% if (Model.IsAnOwner)
                   { %>
                <div id='SetLocation'></div>
                <% } %>
            </td>
        </tr>
        <% } %>
        <tr>
            <th>Source Contact:</th>
            <td>
                <%=Html.HyperLink("/Contact.aspx?id=" + Model.SourceContactId, 
                    "{0:d}".Fmt(Model.SourceContact)) %>
                <% if(Model.IsAnOwner)
                   { %>
                <%=Html.HyperLink("#", Model.SourceContactChange, "SearchContacts();return false")%>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>Completed Contact:</th>
            <td>
                <%=Html.HyperLink("/Contact.aspx?id=" + Model.CompletedContactId, 
                    "{0:d}".Fmt(Model.CompletedContact)) %>
                <% if(Model.CanCompleteWithContact) 
                   { %>
                <%=Html.HyperLink("#", "(complete)", "CompleteWithContact()")%>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>Notes:<br /></th>
            <td>
                <div style="width: 30em">
                    <%=Model.FmtNotes%>
                </div>
            </td>
        </tr>
<% if(Model.IsAnOwner)
{ %>
        <tr>
            <td colspan="2" align="center">
                <input type="button" value="Edit" onclick="Edit(<%=Model.Id%>)" />
            </td>
        </tr>
<% } %>
    </table>
</td>

