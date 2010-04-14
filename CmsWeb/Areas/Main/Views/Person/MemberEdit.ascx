<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.MemberInfo>" %>
<a class="displayedit" href="/Person/MemberDisplay/<%=Model.PeopleId %>">cancel</a>
<table class="Design2">
    <tr>
        <th>Contribution Statement:</th>
        <td><%=Html.DropDownList("StatementOptionId", CMSWeb.Models.PersonPage.MemberInfo.EnvelopeOptions())%></td>
    </tr>
    <tr>
        <th>Envelope Option</th>
        <td><%=Html.DropDownList("EnvelopeOptionId", CMSWeb.Models.PersonPage.MemberInfo.EnvelopeOptions())%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th colspan="2" class="Design2Head">Decision</th>
    </tr>
    <tr>
        <th>Type:</th>
        <td><%=Html.DropDownList("DecisionTypeId", CMSWeb.Models.PersonPage.MemberInfo.DecisionCodes())%></td>
    </tr>
    <tr>
        <th>Date:</th>
        <td><%=Html.TextBox("DecisionDate", Model.DecisionDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th colspan="2" class="Design2Head">Join</th>
    </tr>
    <tr>
        <th>Type:</th>
        <td><%=Html.DropDownList("JoinTypeId", CMSWeb.Models.PersonPage.MemberInfo.JoinTypes())%></td>
    </tr>
    <tr>
        <th>Date:</th>
        <td><%=Model.JoinDate.FormatDate()%></td>
    </tr>
    <tr>
        <th>Previous Church:</th>
        <td><%=Html.TextBox("PrevChurch")%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th colspan="2" class="Design2Head">Church Membership</th>
    </tr>
    <tr>
        <th>Member Status:</th>
        <td><%=Html.DropDownList("MemberStatusId", CMSWeb.Models.PersonPage.MemberInfo.MemberStatuses())%></td>
    </tr>
    <tr>
        <th>Joined:</th>
        <td><%=Html.TextBox("JoinDate", Model.JoinDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th colspan="2" class="Design2Head">Baptism</th>
    </tr>
    <tr>
        <th>Type:</th>
        <td><%=Html.DropDownList("BaptismTypeId", CMSWeb.Models.PersonPage.MemberInfo.BaptismTypes())%></td>
    </tr>
    <tr>
        <th>Status:</th>
        <td><%=Html.DropDownList("BaptismStatusId", CMSWeb.Models.PersonPage.MemberInfo.BaptismStatuses())%></td>
    </tr>
    <tr>
        <th>Date:</th>
        <td><%=Html.TextBox("BaptismDate", Model.BaptismDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
    <tr>
        <th>Scheduled:</th>
        <td><%=Html.TextBox("BaptismSchedDate", Model.BaptismSchedDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th colspan="2" class="Design2Head">Drop</th>
    </tr>
    <tr>
        <th>Type:</th>
        <td><%=Html.DropDownList("DropTypeId", CMSWeb.Models.PersonPage.MemberInfo.DropTypes())%></td>
    </tr>
    <tr>
        <th>Date:</th>
        <td><%=Html.TextBox("DropDate", Model.DropDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
    <tr>
        <th>New Church:</th>
        <td><%=Html.TextBox("NewChurch")%></td>
    </tr>
<tr><td></td></tr>
    <tr>
        <th colspan="2" class="Design2Head">New Member Class</th>
    </tr>
    <tr>
        <th>Status:</th>
        <td><%=Html.DropDownList("NewMemberClassStatusId", CMSWeb.Models.PersonPage.MemberInfo.NewMemberClassStatuses())%></td>
    </tr>
    <tr>
        <th>Date:</th>
        <td><%=Html.TextBox("NewMemberClassDate", Model.NewMemberClassDate.FormatDate(), new { @class = "datepicker" })%></td>
    </tr>
<tr><td></td></tr>
     <tr><td></td><td><a href="/Person/MemberUpdate/<%=Model.PeopleId %>" class="submitbutton">Save Changes</a></td></tr>
</table>

