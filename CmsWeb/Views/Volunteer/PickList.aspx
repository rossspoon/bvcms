<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.VolunteerModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Volunteer</title>
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
<script>
</script>
    <h2>Volunteer for <%=Model.Opportunity.Description %></h2>

    <% using (Html.BeginForm()) { %>
    <%=Html.Hidden("Id", Model.VolInterestId) %>
        <div>
            <fieldset>
                <table style="empty-cells:show">
                <col style="width: 13em; text-align:right" />
                <col />
                <col />
                <tr><td colspan="3">Volunteer Interests for <%=Model.person.Name %></td></tr>
                <% if (false && Model.Opportunity.ExtraQuestion.HasValue())
                   { %>
                <tr>
                    <td colspan="3"><%=Model.Opportunity.ExtraQuestion %></td>
                </tr>
                <% } %>
                <tr>
                    <td><label for="interests">Interests</label></td>
                    <td>
                    <% foreach (var i in Model.Opportunity.VolInterestCodes)
                       { %>
                       <input type="checkbox" class="ckinterest" name="interests" value="<%=i.Id %>" <%=Model.Checked(i.Id)%> /> <%=i.Description %><br />
                    <% } %>
                    </td>
                    <td><%= Html.ValidationMessage("interests") %></td>
                </tr>
                <% if (Model.Opportunity.ExtraQuestion.HasValue())
                   { %>
                <tr>
                    <td><%=Model.Opportunity.ExtraQuestion %></td>
                    <td><%=Html.TextBox("question", Model.question) %></td>
                    <td>&nbsp;</td>
                </tr>
                <% } %>
                <tr>
                    <td>&nbsp;</td><td><input type="submit" value="Submit" /><br />
                    <span style="color:Green;font-weight:bold"><%=ViewData["saved"] %></span></td>
                </tr>
                </table>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
