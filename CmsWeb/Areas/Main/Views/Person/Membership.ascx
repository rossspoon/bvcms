<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CMSWeb.Models.PersonModel.PersonInfo>" %>
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
                    <th colspan="2" class="LightBlueBG">Decision</th>
                </tr>
                <tr>
                    <th>Type:</th>
                    <td><%=Model.DecisionType %></td>
                </tr>
                <tr>
                    <th>Date:</th>
                    <td><%=Model.DecisionDate %></td>
                </tr>
            </table>
        </td>
        <td>
        </td>
        <td valign="top">
            <table class="Design2">
                <tr>
                    <th colspan="2" class="LightBlueBG">Join</th>
                </tr>
                <tr>
                    <th>Type:</th>
                    <td><%=Model.JoinType %></td>
                </tr>
                <tr>
                    <th>Date:</th>
                    <td><%=Model.JoinDate %></td>
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
                    <th colspan="2" class="LightBlueBG">Church Membership</th>
                </tr>
                <tr>
                    <th>Member Status:</th>
                    <td><%=Model.MemberStatus %></td>
                </tr>
                <tr>
                    <th>Joined:</th>
                    <td><%=Model.JoinDate %></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top">
            <table class="Design2" width="100%">
                <tr>
                    <th colspan="2" class="LightBlueBG">Baptism</th>
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
                    <td><%=Model.BaptismDate %></td>
                </tr>
                <tr>
                    <th>Scheduled:</th>
                    <td><%=Model.BaptismSchedDate %></td>
                </tr>
            </table>
            <td>
            </td>
            <td valign="top">
                <table class="Design2" width="100%">
                    <tr>
                        <th colspan="2" class="LightBlueBG">Drop</th>
                    </tr>
                    <tr>
                        <th>Type:</th>
                        <td><%=Model.DropType %></td>
                    </tr>
                    <tr>
                        <th>Date:</th>
                        <td><%=Model.DropDate %></td>
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
                        <th colspan="2" class="LightBlueBG">Step 1 Class</th>
                    </tr>
                    <tr>
                        <th>Status:</th>
                        <td><%=Model.NewMemberClassStatus %></td>
                    </tr>
                    <tr>
                        <th>Date:</th>
                        <td><%=Model.NewMemberClassDate %></td>
                    </tr>
                </table>
            </td>
    </tr>
</table>
