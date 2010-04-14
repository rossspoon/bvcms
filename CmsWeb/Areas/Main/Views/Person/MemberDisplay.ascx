<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonPage.MemberInfo>" %>
<% if (Page.User.IsInRole("Membership"))
   { %>
<a class="displayedit" href="/Person/MemberEdit/<%=Model.PeopleId %>">Edit</a>
<% } %>
<table>
    <tr>
        <td valign="top">
            <table class="Design2">
                <tr>
                    <th>Contribution Statement:</th>
                    <td><%=Model.StatementOption %></td>
                </tr>
            </table>
        </td>
        <td>
        </td>
        <td valign="top">
            <table class="Design2">
                <tr>
                    <th>Envelope Option</th>
                    <td><%=Model.EnvelopeOption %></td>
                </tr>
            </table>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table class="Design2">
                <tr>
                    <th colspan="2" class="Design2Head">Decision</th>
                </tr>
                <tr>
                    <th>Type:</th>
                    <td><%=Model.DecisionType %></td>
                </tr>
                <tr>
                    <th>Date:</th>
                    <td><%=Model.DecisionDate.FormatDate() %></td>
                </tr>
            </table>
        </td>
        <td>
        </td>
        <td valign="top">
            <table class="Design2">
                <tr>
                    <th colspan="2" class="Design2Head">Join</th>
                </tr>
                <tr>
                    <th>Type:</th>
                    <td><%=Model.JoinType %></td>
                </tr>
                <tr>
                    <th>Date:</th>
                    <td><%=Model.JoinDate.FormatDate()%></td>
                </tr>
                <tr>
                    <th>Previous Church:</th>
                    <td><%=Model.PrevChurch %></td>
                </tr>
            </table>
        </td>
        <td valign="top">
            <table class="Design2" width="100%">
                <tr>
                    <th colspan="2" class="Design2Head">Church Membership</th>
                </tr>
                <tr>
                    <th>Member Status:</th>
                    <td><%=Model.MemberStatus %></td>
                </tr>
                <tr>
                    <th>Joined:</th>
                    <td><%=Model.JoinDate.FormatDate() %></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table class="Design2" width="100%">
                <tr>
                    <th colspan="2" class="Design2Head">Baptism</th>
                </tr>
                <tr>
                    <th>Type:</th>
                    <td><%=Model.BaptismType %></td>
                </tr>
                <tr>
                    <th>Status:</th>
                    <td><%=Model.BaptismStatus %></td>
                </tr>
                <tr>
                    <th>Date:</th>
                    <td><%=Model.BaptismDate.FormatDate() %></td>
                </tr>
                <tr>
                    <th>Scheduled:</th>
                    <td><%=Model.BaptismSchedDate.FormatDate() %></td>
                </tr>
            </table>
            <td>
            </td>
            <td valign="top">
                <table class="Design2" width="100%">
                    <tr>
                        <th colspan="2" class="Design2Head">Drop</th>
                    </tr>
                    <tr>
                        <th>Type:</th>
                        <td><%=Model.DropType %></td>
                    </tr>
                    <tr>
                        <th>Date:</th>
                        <td><%=Model.DropDate.FormatDate() %></td>
                    </tr>
                    <tr>
                        <th>New Church:</th>
                        <td><%=Model.NewChurch %></td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table class="Design2" width="100%">
                    <tr>
                        <th colspan="2" class="Design2Head">New Member Class</th>
                    </tr>
                    <tr>
                        <th>Status:</th>
                        <td><%=Model.NewMemberClassStatus %></td>
                    </tr>
                    <tr>
                        <th>Date:</th>
                        <td><%=Model.NewMemberClassDate.FormatDate() %></td>
                    </tr>
                </table>
            </td>
    </tr>
</table>
<% 
    CmsData.Person p = DbUtil.Db.LoadPersonById(Model.PeopleId);
    Html.RenderPartial("PeopleExtrasGrid", p); 
%>
