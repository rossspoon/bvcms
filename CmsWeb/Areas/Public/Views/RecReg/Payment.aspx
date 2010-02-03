<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/bvorg.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecRegModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Recreation Registration</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Payment Processing</h2>
    <p>
        You will now be redirected to ServiceU.com to process your credit card payment of <%=Model.Amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
    </p>
    <form action="https://public.serviceu.com/transaction/pay.asp" method="post">
    <%=Html.Hidden("OrgID", Model.ServiceUOrgID) %>
    <%=Html.Hidden("OrgAccountID", Model.ServiceUOrgAccountID) %>
    <%=Html.Hidden("Amount", Model.Amount) %>
    <%=Html.Hidden("PostbackURL", Request.Url.Scheme + "://" + Request.Url.Authority + "/RecReg/Confirm/" + Model.regid) %>
    <%=Html.Hidden("NameOnAccount", Model.registration.Fname.HasValue() ? 
        Model.registration.Fname : Model.registration.Mname) %>
    <%=Html.Hidden("Address", Model.participant.PrimaryAddress) %>
    <%=Html.Hidden("City", Model.participant.PrimaryCity) %>
    <%=Html.Hidden("State", Model.participant.PrimaryState) %>
    <%=Html.Hidden("PostalCode", Model.participant.PrimaryZip.Zip5()) %>
    <%=Html.Hidden("Phone", Model.participant.Family.HomePhone.FmtFone()) %>
    <%=Html.Hidden("Email", Model.registration.Email) %>
    <%=Html.Hidden("Misc1", Model.participant.Name) %>
    <%=Html.Hidden("Misc2", Model.division.Name) %>
    <%=Html.Hidden("Misc3", Model.Amount) %>
    <%=Html.SubmitButton("Submit", "Next") %>
    </form>

</asp:Content>
