<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.BasicPersonInfo>" %>
<% if (Page.User.IsInRole("Edit"))
   { %>
<a class="displayedit" href="/Person/BasicEdit/<%=Model.PeopleId %>">Edit</a>
<% } %>
<div style="float:left">
    <table class="Design2">
        <tr><th>Goes By:</th>
            <td><%=Model.NickName %></td>
        </tr>
        <tr><th>Title:</th>
            <td><%=Model.Title %></td>
        </tr>
        <tr><th>First:</th>
            <td><%=Model.First %></td>
        </tr>
        <tr><th>Middle:</th>
            <td><%=Model.Middle %></td>
        </tr>
        <tr><th>Last:</th>
            <td><%=Model.Last %></td>
        </tr>
        <tr><th>Suffix:</th>
            <td><%=Model.Suffix %></td>
        </tr>
        <tr><th>Former:</th>
            <td><%=Model.Maiden %></td>
        </tr>
        <tr><th>Gender:</th>
            <td><%=Model.Gender %></td>
        </tr>
    </table>
</div>
<div style="float:left">
    <table class="Design2">
        <tr>
            <th>Home Phone:</th>
            <td><%=Model.HomePhone.FmtFone() %></td>
        </tr>
        <tr>
            <th>Cell Phone:</th>
            <td><%=Model.CellPhone.FmtFone()%></td>
        </tr>
        <tr>
            <th>Work Phone:</th>
            <td><%=Model.WorkPhone.FmtFone()%></td>
        </tr>
        <tr>
            <th>Email:</th>
            <td><%=Model.EmailAddress %></td>
        </tr>
        <tr>
            <th>School:</th>
            <td><%=Model.School %></td>
        </tr>
        <tr>
            <th>Grade:</th>
            <td><%=Model.Grade %></td>
        </tr>
        <tr>
            <th>Employer:</th>
            <td><%=Model.Employer %></td>
        </tr>
        <tr>
            <th>Occupation:</th>
            <td><%=Model.Occupation %></td>
        </tr>
    </table>
</div>
<div style="float:left">
    <table class="Design2">
        <tr>
            <th>Campus:</th>
            <td><%=Model.Campus %></td>
        </tr>
        <tr>
            <th>Member Status:</th>
            <td><%=Model.MemberStatus %></td>
        </tr>
        <tr>
            <th>Joined:</th>
            <td><%=Model.JoinDate.FormatDate() %></td>
        </tr>
        <tr>
            <th>Marital Status:</th>
            <td><%=Model.MaritalStatus %></td>
        </tr>
        <tr>
            <th>Spouse:</th>
            <td><%=Model.Spouse %></td>
        </tr>
        <tr>
            <th>Wedding Date:</th>
            <td><%=Model.WeddingDate.FormatDate()%></td>
        </tr>
        <tr>
            <th>Birthday:</th>
            <td><%=Model.Birthday %></td>
        </tr>
        <tr>
            <th>Deceased:</th>
            <td><%=Model.DeceasedDate.FormatDate()%></td>
        </tr>
        <tr>
            <th>Age:</th>
            <td><%=Model.Age %></td>
        </tr>
    </table>
</div>
<div style="float:left">
    <table class="Design2">
        <tr><td><%=Model.DoNotCall %></td></tr>
        <tr><td><%=Model.DoNotVisit %></td></tr>
        <tr><td><%=Model.DoNotMail %></td></tr>
    </table>
</div>
<div style="clear:both"></div>
