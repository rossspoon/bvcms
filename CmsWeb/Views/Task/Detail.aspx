<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.TaskDetail>" %>
<% if (false)
   { %>
    <link href="/Content/style.css" rel="stylesheet" type="text/css" />
<% } %>
<td colspan="7"><a name="select" /><a name="detail">
    <table class="Design2" style="border: 3px solid black">
    <tr><td align="right" colspan="2"><a href="#" onclick="Deselect();return false">
        <img style="border:0" src="/Content/images/888888_11x11_icon_close.gif" /></a></td></tr>
        <tr>
   
            <th>Task</th>
            <td>
                <%=Html.Hidden("TaskId", ViewData.Model.Id)%>
                <%=ViewData.Model.Description%>
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
                <%=Html.HyperLink("#", "(transfer)", "SearchPeople(ChangeOwnerPerson);return false")%>
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
                <%=Html.HyperLink("#", ViewData.Model.ChangeCoOwner, "SearchPeople(AddDelegatePerson);return false")%>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>Due</th>
            <td>
                <%="{0:d}".Fmt(ViewData.Model.Due) %>
            </td>
        </tr>
        <tr>
            <th>Priority</th>
            <td>
                <span id="Priority" style="margin-right:3em"><%=ViewData.Model.Priority%></span>
                <% if(ViewData.Model.IsAnOwner) 
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
                <span class="edit-select" vid="Status" canedit='<%=ViewData.Model.IsAnOwner%>'><%=ViewData.Model.Status%></span>
                <% if (ViewData.Model.CanComplete)
                   { %>
                <a href="#" onclick="SetComplete(<%=ViewData.Model.Id%>)" style="font-size:120%">(complete)</a>
                <% }
                   if (ViewData.Model.CanAccept)
                   { %>
                <a href="#" onclick="Accept(<%=ViewData.Model.Id%>)" style="font-size:120%">(accept)</a>
                <% }
                   if (ViewData.Model.ShowCompleted)
                   { %>
                <span style="font-size: smaller; color: Gray" runat="server"><%="{0:f}".Fmt(ViewData.Model.CompletedOn)%></span>
                <% } %>
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
        <tr style='font-size: larger'>
            <th>Regarding Person:</th>
            <td style="border: 1px solid grey">
                <% if (ViewData.Model.WhoId.HasValue)
                   { %>
                <%=Html.HyperLink("/Person.aspx?id=" + ViewData.Model.WhoId, ViewData.Model.Who)%>
                <% } 
                   if (ViewData.Model.IsAnOwner)
                   { %>
                <%=Html.HyperLink("#", ViewData.Model.ChangeWho, "SearchPeople(AddAboutPerson);return false")%>
                <% }
                   if (ViewData.Model.WhoId.HasValue)
                   { %>
                <%=Html.HyperLink(ViewData.Model.ProspectReportLink(), "Prospect Report", new { target = "_blank" })%>
                <div>
                    <%=Html.HyperLink("http://www.google.com/maps?q=" + ViewData.Model.WhoAddrCityStateZip, ViewData.Model.WhoAddress, new { target = "_blank" })%>
                    | <%=Html.HyperLink("http://www.google.com/maps?f=d&saddr=2000+Appling+Rd,+Cordova,+Tennessee+38016&pw=2&daddr=" + ViewData.Model.WhoAddrCityStateZip, "driving directions", new { target = "_blank" })%><br />
                    <%=Html.HyperLink("mailto:" + ViewData.Model.WhoEmail, ViewData.Model.WhoEmail2)%>
                    | <%=ViewData.Model.WhoPhone%> 
                </div>
                <% } %>
            </td>
        </tr>
        <% if (ViewData.Model.ShowLocation)
           { %>
        <tr>
            <th>Location:</th>
            <td>
                <span class=".location"><%=ViewData.Model.Location%></span>
                <% if (ViewData.Model.IsAnOwner)
                   { %>
                <div id='SetLocation'></div>
                <% } %>
            </td>
        </tr>
        <% } %>
        <tr>
            <th>Source Contact:</th>
            <td>
                <%=Html.HyperLink("/Contact.aspx?id=" + ViewData.Model.SourceContactId, 
                    "{0:d}".Fmt(ViewData.Model.SourceContact)) %>
                <% if(ViewData.Model.IsAnOwner)
                   { %>
                <%=Html.HyperLink("#", ViewData.Model.SourceContactChange, "SearchContacts();return false")%>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>Completed Contact:</th>
            <td>
                <%=Html.HyperLink("/Contact.aspx?id=" + ViewData.Model.CompletedContactId, 
                    "{0:d}".Fmt(ViewData.Model.CompletedContact)) %>
                <% if(ViewData.Model.CanCompleteWithContact) 
                   { %>
                <%=Html.HyperLink("#", "(complete)", "CompleteWithContact()")%>
                <% } %>
            </td>
        </tr>
        <tr>
            <th>Notes:<br /></th>
            <td>
                <div style="width: 30em">
                    <%=ViewData.Model.FmtNotes%>
                </div>
            </td>
        </tr>
<% if(ViewData.Model.IsAnOwner)
{ %>
        <tr>
            <td colspan="2" align="center">
                <input type="button" value="Edit" onclick="Edit(<%=ViewData.Model.Id%>)" />
            </td>
        </tr>
<% } %>
    </table></a>
</td>

