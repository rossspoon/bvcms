<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<CmsData.ContributionFund>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>ViewPage1</h2>

    <table>
        <tr>
            <th></th>
            <th>
                FundId
            </th>
            <th>
                FundName
            </th>
            <th>
                FundDescription
            </th>
            <th>
                FundStatusId
            </th>
            <th>
                FundTypeId
            </th>
            <th>
                FundPledgeFlag
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { id=item.FundId }) %> |
            </td>
            <td>
                <%= Html.Encode(item.FundId) %>
            </td>
            <td>
                <%= Html.Encode(item.FundName) %>
            </td>
            <td>
                <%= Html.Encode(item.FundDescription) %>
            </td>
            <td>
                <%= Html.Encode(item.FundStatusId) %>
            </td>
            <td>
                <%= Html.Encode(item.FundTypeId) %>
            </td>
            <td>
                <%= Html.Encode(item.FundPledgeFlag) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <% using (Html.BeginForm("Create", "Fund"))
       { %>
    <p>
        New Fund Id: <%= Html.TextBox("fundid") %>
        <input type="submit" value="Create" />
    </p>
    <% } %>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

