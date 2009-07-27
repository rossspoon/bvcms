<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site3.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.RecRegModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Received</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Payment Processing</h2>
    <p>
        You will now be redirected to ServiceU.com to process your credit card payment of $25.00.
        After you are finished there, you will be redirected back here to get your confirmation.
    </p>
    <form action="https://public.serviceu.com/transaction/pay.asp" method="post">
    <%=Html.Hidden("OrgID", 12263) %>
    <%=Html.Hidden("OrgAccountID", 2514) %>
    <%=Html.Hidden("Amount", 25) %>
    <%=Html.Hidden("PostbackURL", "http://" + Request.Url.Authority + "/RecReg/Confirm/" + Model.regid) %>
    <%=Html.Hidden("NameOnAccount", Model.fname.HasValue() ? Model.fname : Model.mname) %>
    <%=Html.Hidden("Address", Model.participant.PrimaryAddress) %>
    <%=Html.Hidden("City", Model.participant.PrimaryCity) %>
    <%=Html.Hidden("State", Model.participant.PrimaryState) %>
    <%=Html.Hidden("PostalCode", Model.participant.PrimaryZip.Zip5()) %>
    <%=Html.Hidden("Phone", Model.participant.Family.HomePhone.FmtFone()) %>
    <%=Html.Hidden("Email", Model.registration.Email) %>
    <%=Html.SubmitButton("Submit", "Next") %>
    </form>

</asp:Content>
