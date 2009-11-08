<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsData.ContributionFund>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <% using (Html.BeginForm("Update", "Fund")) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="FundId">FundId:</label>
                <%= Html.Encode(Model.FundId) %>
            </p>
            <p>
                <label for="FundName">FundName:</label>
                <%= Html.TextBox("FundName") %>
            </p>
            <p>
                <label for="FundDescription">FundDescription:</label>
                <%= Html.TextBox("FundDescription") %>
            </p>
            <p>
                <label for="FundStatusId">FundStatusId:</label>
                <%= Html.DropDownList("FundStatusId", CMSWebSetup.Controllers.FundController.GetFundStatusList()) %>
            </p>
            <p>
                <label for="FundTypeId">FundTypeId:</label>
                <%= Html.DropDownList("FundTypeId", CMSWebSetup.Controllers.FundController.GetFundTypeList()) %>
            </p>
            <p>
                <label for="FundPledgeFlag">FundPledgeFlag:</label>
                <%= Html.CheckBox("FundPledgeFlag") %>
            </p>
            <p>
                <label for="FundTarget">FundTarget:</label>
                <%= Html.TextBox("FundTarget") %>
            </p>
            <p>
                <label for="FundOpenDate">FundOpenDate:</label>
                <%= Html.TextBox("FundOpenDate") %>
            </p>
            <p>
                <label for="FundCloseDate">FundCloseDate:</label>
                <%= Html.TextBox("FundCloseDate") %>
            </p>
            <p>
                <label for="FundAccountCode">FundAccountCode:</label>
                <%= Html.TextBox("FundAccountCode") %>
            </p>
            <p>
                <label for="FundIncomeDept">FundIncomeDept:</label>
                <%= Html.TextBox("FundIncomeDept") %>
            </p>
            <p>
                <label for="FundIncomeAccount">FundIncomeAccount:</label>
                <%= Html.TextBox("FundIncomeAccount") %>
            </p>
            <p>
                <label for="FundIncomeFund">FundIncomeFund:</label>
                <%= Html.TextBox("FundIncomeFund") %>
            </p>
            <p>
                <label for="FundCashDept">FundCashDept:</label>
                <%= Html.TextBox("FundCashDept") %>
            </p>
            <p>
                <label for="FundCashAccount">FundCashAccount:</label>
                <%= Html.TextBox("FundCashAccount") %>
            </p>
            <p>
                <label for="FundCashFund">FundCashFund:</label>
                <%= Html.TextBox("FundCashFund") %>
            </p>
            <p>
                <%=Html.Hidden("FundId") %>
                <input type="submit" value="Save" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%=Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

