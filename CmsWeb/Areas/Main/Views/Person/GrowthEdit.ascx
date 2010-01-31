<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.GrowthInfo>" %>
<a class="displayedit" href="/Person/GrowthDisplay/<%=Model.PeopleId %>">cancel</a>
<table class="Design2">
    <tr>
        <th>How did you hear about our Church?</th>
        <td><%=Html.DropDownList("InterestPointId", CMSWeb.Models.PersonPage.GrowthInfo.InterestPoints())%></td>
    </tr>
    <tr>
        <th>Origin:</th>
        <td><%=Html.DropDownList("OriginId", CMSWeb.Models.PersonPage.GrowthInfo.Origins())%></td>
    </tr>
    <tr>
        <th>Entry Point:</th>
        <td><%=Html.DropDownList("EntryPointId", CMSWeb.Models.PersonPage.GrowthInfo.EntryPoints())%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th>Is a member of another Church?</th>
        <td><%=Html.CheckBox("MemberAnyChurch") %></td>
    </tr>
    <tr>
        <th>Prayed for Christ as Savior</th>
        <td><%=Html.CheckBox("ChristAsSavior") %></td>
    </tr>
    <tr>
        <th>Would like someone to visit</th>
        <td><%=Html.CheckBox("PleaseVisit") %></td>
    </tr>
    <tr>
        <th>Interested in joining Church</th>
        <td><%=Html.CheckBox("InterestedInJoining") %></td>
    </tr>
    <tr>
        <th>Would like to know how to become a Christian</th>
        <td><%=Html.CheckBox("SendInfo") %></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th>Comments:</th>
        <td>
            <%=Html.TextArea("Comments") %>
        </td>
    </tr>
<tr><td></td></tr>
     <tr><td></td><td><a href="/Person/GrowthUpdate/<%=Model.PeopleId %>" class="submitbutton">Save Changes</a></td></tr>
</table>
