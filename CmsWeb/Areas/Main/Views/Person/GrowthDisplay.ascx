<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.GrowthInfo>" %>
<% if (Page.User.IsInRole("Edit"))
   { %>
<a class="displayedit" href="/Person/GrowthEdit/<%=Model.PeopleId %>">Edit</a>
<% } %>
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
                    <td><input type="checkbox" <%=Model.MemberAnyChurch.Value ? "checked='checked'" : "" %> disabled="disabled" /></td>
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
</table>
