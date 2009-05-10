<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.MetroZipModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit</h2>

    <%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm("Update", "Fund")) {%>

        <fieldset>
            <legend>Fields</legend>
            <p>
                <label for="FundId">FundId:</label>
                <%= Html.TextBox("fund.FundId", Model.fund.FundId) %>
                <%= Html.ValidationMessage("FundId", "*") %>
            </p>
            <p>
                <label for="FundName">FundName:</label>
                <%= Html.TextBox("fund.FundName", Model.fund.FundName) %>
                <%= Html.ValidationMessage("FundName", "*") %>
            </p>
            <p>
                <label for="FundDescription">FundDescription:</label>
                <%= Html.TextBox("fund.FundDescription", Model.fund.FundDescription) %>
                <%= Html.ValidationMessage("FundDescription", "*") %>
            </p>
            <p>
                <label for="FundStatusId">FundStatusId:</label>
                <%= Html.DropDownList("fund.FundStatusId", Model.FundStatuses()) %>
                <%= Html.ValidationMessage("FundStatusId", "*") %>
            </p>
            <p>
                <label for="FundTypeId">FundTypeId:</label>
                <%= Html.TextBox("FundTypeId", Model.fund.FundTypeId) %>
                <%= Html.ValidationMessage("FundTypeId", "*") %>
            </p>
            <p>
                <label for="FundPledgeFlag">FundPledgeFlag:</label>
                <%= Html.CheckBox("FundPledgeFlag", Model.fund.FundPledgeFlag) %>
                <%= Html.ValidationMessage("FundPledgeFlag", "*") %>
            </p>
            <p>
                <label for="FundTarget">FundTarget:</label>
                <%= Html.TextBox("FundTarget", String.Format("{0:F}", Model.fund.FundTarget)) %>
                <%= Html.ValidationMessage("FundTarget", "*") %>
            </p>
            <p>
                <label for="FundOpenDate">FundOpenDate:</label>
                <%= Html.TextBox("FundOpenDate", String.Format("{0:g}", Model.fund.FundOpenDate)) %>
                <%= Html.ValidationMessage("FundOpenDate", "*") %>
            </p>
            <p>
                <label for="FundCloseDate">FundCloseDate:</label>
                <%= Html.TextBox("FundCloseDate", String.Format("{0:g}", Model.fund.FundCloseDate)) %>
                <%= Html.ValidationMessage("FundCloseDate", "*") %>
            </p>
            <p>
                <label for="FundAccountCode">FundAccountCode:</label>
                <%= Html.TextBox("FundAccountCode", Model.fund.FundAccountCode) %>
                <%= Html.ValidationMessage("FundAccountCode", "*") %>
            </p>
            <p>
                <label for="FundIncomeDept">FundIncomeDept:</label>
                <%= Html.TextBox("FundIncomeDept", Model.fund.FundIncomeDept) %>
                <%= Html.ValidationMessage("FundIncomeDept", "*") %>
            </p>
            <p>
                <label for="FundIncomeAccount">FundIncomeAccount:</label>
                <%= Html.TextBox("FundIncomeAccount", Model.fund.FundIncomeAccount) %>
                <%= Html.ValidationMessage("FundIncomeAccount", "*") %>
            </p>
            <p>
                <label for="FundIncomeFund">FundIncomeFund:</label>
                <%= Html.TextBox("FundIncomeFund", Model.fund.FundIncomeFund) %>
                <%= Html.ValidationMessage("FundIncomeFund", "*") %>
            </p>
            <p>
                <label for="FundCashDept">FundCashDept:</label>
                <%= Html.TextBox("FundCashDept", Model.fund.FundCashDept) %>
                <%= Html.ValidationMessage("FundCashDept", "*") %>
            </p>
            <p>
                <label for="FundCashAccount">FundCashAccount:</label>
                <%= Html.TextBox("FundCashAccount", Model.fund.FundCashAccount) %>
                <%= Html.ValidationMessage("FundCashAccount", "*") %>
            </p>
            <p>
                <label for="FundCashFund">FundCashFund:</label>
                <%= Html.TextBox("FundCashFund", Model.fund.FundCashFund) %>
                <%= Html.ValidationMessage("FundCashFund", "*") %>
            </p>
            <p>
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

