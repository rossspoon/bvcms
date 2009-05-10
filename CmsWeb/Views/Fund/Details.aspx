<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<CmsData.ContributionFund>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Detail</h2>

    <fieldset>
        <legend>Fields</legend>
        <p>
            FundId:
            <%= Html.Encode(Model.FundId) %>
        </p>
        <p>
            FundName:
            <%= Html.Encode(Model.FundName) %>
        </p>
        <p>
            FundDescription:
            <%= Html.Encode(Model.FundDescription) %>
        </p>
        <p>
            FundStatusId:
            <%= Html.Encode(Model.FundStatusId) %>
        </p>
        <p>
            FundTypeId:
            <%= Html.Encode(Model.FundTypeId) %>
        </p>
        <p>
            FundPledgeFlag:
            <%= Html.Encode(Model.FundPledgeFlag) %>
        </p>
        <p>
            FundTarget:
            <%= Html.Encode(String.Format("{0:F}", Model.FundTarget)) %>
        </p>
        <p>
            FundOpenDate:
            <%= Html.Encode(String.Format("{0:g}", Model.FundOpenDate)) %>
        </p>
        <p>
            FundCloseDate:
            <%= Html.Encode(String.Format("{0:g}", Model.FundCloseDate)) %>
        </p>
        <p>
            FundAccountCode:
            <%= Html.Encode(Model.FundAccountCode) %>
        </p>
        <p>
            FundIncomeDept:
            <%= Html.Encode(Model.FundIncomeDept) %>
        </p>
        <p>
            FundIncomeAccount:
            <%= Html.Encode(Model.FundIncomeAccount) %>
        </p>
        <p>
            FundIncomeFund:
            <%= Html.Encode(Model.FundIncomeFund) %>
        </p>
        <p>
            FundCashDept:
            <%= Html.Encode(Model.FundCashDept) %>
        </p>
        <p>
            FundCashAccount:
            <%= Html.Encode(Model.FundCashAccount) %>
        </p>
        <p>
            FundCashFund:
            <%= Html.Encode(Model.FundCashFund) %>
        </p>
    </fieldset>
    <p>

        <%=Html.ActionLink("Edit", "Edit", new { id=Model.FundId }) %> |
        <%=Html.ActionLink("Delete", "Delete", new { id=Model.FundId }) %> |
        <%=Html.ActionLink("Back to List", "Index") %>
    </p>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

