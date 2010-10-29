<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CmsWeb.Models.PersonPage.BasicPersonInfo>" %>
<a class="displayedit" href="/Person/BasicDisplay/<%=Model.PeopleId %>">cancel</a>
    <table class="Design2 basicedit">
        <tr><th>Goes By:</th>
            <td><%=Html.TextBox("NickName") %></td>
        </tr>
        <tr><th>Title:</th>
            <td><%=Html.TextBox("Title") %></td>
        </tr>
        <tr><th>First:</th>
            <td><%=Html.TextBox("First") %></td>
        </tr>
        <tr><th>Middle:</th>
            <td><%=Html.TextBox("Middle") %></td>
        </tr>
        <tr><th>Last:</th>
            <td><%=Html.TextBox("Last") %></td>
        </tr>
        <tr><th>Suffix:</th>
            <td><%=Html.TextBox("Suffix") %></td>
        </tr>
        <tr><th>Alt Name:</th>
            <td><%=Html.TextBox("AltName") %></td>
        </tr>
        <tr><th>Former:</th>
            <td><%=Html.TextBox("Maiden") %></td>
        </tr>
        <tr><th>Gender:</th>
            <td><%=Html.DropDownList("GenderId", CmsWeb.Models.PersonPage.BasicPersonInfo.GenderCodes())%></td>
        </tr>
<tr><td></td></tr>
        <tr>
            <th>Home Phone:</th>
            <td><%=Html.TextBox("HomePhone", Model.HomePhone.FmtFone()) %></td>
        </tr>
        <tr>
            <th>Cell Phone:</th>
            <td><%=Html.TextBox("CellPhone", Model.CellPhone.FmtFone()) %></td>
        </tr>
        <tr>
            <th>Work Phone:</th>
            <td><%=Html.TextBox("WorkPhone", Model.WorkPhone.FmtFone()) %></td>
        </tr>
        <tr>
            <th>Email:</th>
            <td><%=Html.TextBox("EmailAddress") %></td>
        </tr>
        <tr>
            <th>School:</th>
            <td><%=Html.TextBox("School") %></td>
        </tr>
        <tr>
            <th>Grade:</th>
            <td><%=Html.TextBox("Grade") %></td>
        </tr>
        <tr>
            <th>Employer:</th>
            <td><%=Html.TextBox("Employer") %></td>
        </tr>
        <tr>
            <th>Occupation:</th>
            <td><%=Html.TextBox("Occupation") %></td>
        </tr>
<tr><td></td></tr>
        <tr>
            <th>Marital Status:</th>
            <td><%=Html.DropDownList("MaritalStatusId", CmsWeb.Models.PersonPage.BasicPersonInfo.MaritalStatuses())%></td>
        </tr>
        <tr>
            <th>Wedding Date:</th>
            <td><%=Html.TextBox("WeddingDate", Model.WeddingDate.FormatDate(), new { @class = "datepicker" })%></td>
        </tr>
        <tr>
            <th>Birthday:</th>
            <td><%=Html.TextBox("Birthday", Model.Birthday, new { @class = "datepicker" }) %></td>
        </tr>
<% if(Model.person.CanUserEditAll)
  { %>
        <tr>
            <th>Campus:</th>
            <td><%=Html.DropDownList("CampusId", CmsWeb.Models.PersonPage.BasicPersonInfo.Campuses())%></td>
        </tr>
    <% if(Page.User.IsInRole("Membership"))
      { %>
        <tr>
            <th>Deceased:</th>
            <td><%=Html.TextBox("DeceasedDate", Model.DeceasedDate.FormatDate(), new { @class = "datepicker" })%></td>
        </tr>
    <% } %>
<% } %>
<tr><td></td></tr>
        <tr><td>Do Not Call</td><td><%=Html.CheckBox("DoNotCallFlag") %> </td></tr>
        <tr><td>Do Not Visit</td><td><%=Html.CheckBox("DoNotVisitFlag") %> </td></tr>
        <tr><td>Do Not Mail</td><td><%=Html.CheckBox("DoNotMailFlag") %> </td></tr>
        <tr><td></td><td><a href="/Person/BasicUpdate/<%=Model.PeopleId %>" class="submitbutton">Save Changes</a></td></tr>
    </table>

