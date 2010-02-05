<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site2.Master" Inherits="System.Web.Mvc.ViewPage<CMSWeb.Models.PaymentModel>" %>

<asp:Content ID="registerHead" ContentPlaceHolderID="TitleContent" runat="server">
	<title>Complete Registration Payment</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Payment Processing</h2>
    <p>
        You will now be redirected to ServiceU.com to process your credit card payment of <%=Model.amount.ToString("C") %>.
        After you are finished there, you will be redirected back here to get your confirmation.
    </p>
    <form action="https://public.serviceu.com/transaction/pay.asp" method="post">
    <input type="hidden" name="OrgID" value="<%=Model.ServiceUOrgID %>" />
    <%=Html.Hidden("OrgAccountID", Model.ServiceUOrgAccountID) %>
    <%=Html.Hidden("Amount", Model.amount) %>
    <%=Html.Hidden("PostbackURL", Model.postbackurl) %>
    <%=Html.Hidden("NameOnAccount", Model.name) %> 
    <%=Html.Hidden("Address", Model.address) %>
    <%=Html.Hidden("City", Model.city) %>
    <%=Html.Hidden("State", Model.state)%>
    <%=Html.Hidden("PostalCode", Model.zip)%>
    <%=Html.Hidden("Phone", Model.phone) %>
    <%=Html.Hidden("Email", Model.email) %>
    <%=Html.Hidden("Misc1", Model.name)%>
    <%=Html.Hidden("Misc2", Model.description) %>
    <%=Html.Hidden("Misc3", Model.oid) %>
    <%=Html.Hidden("Misc4", Model.amount) %>
    <%=Html.SubmitButton("Submit", "Next") %>
    </form>

</asp:Content>
